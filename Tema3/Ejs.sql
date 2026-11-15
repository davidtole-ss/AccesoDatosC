-- 0. BASE DE DATOS
IF DB_ID('Biblioteca') IS NULL
    CREATE DATABASE Biblioteca;
GO
USE Biblioteca;
GO


-- 1. TABLA
IF OBJECT_ID('Prestamos', 'U') IS NULL
BEGIN
    CREATE TABLE Prestamos (
        IdPrestamo              INT IDENTITY(1,1) NOT NULL,
        CodigoSocio             VARCHAR(10)   NOT NULL,
        NombreSocio             NVARCHAR(100) NOT NULL,
        ISBN                    CHAR(13)      NOT NULL,
        Titulo                  NVARCHAR(150) NOT NULL,
        Autor                   NVARCHAR(100) NOT NULL,
        Genero                  NVARCHAR(20)  NOT NULL,
        FechaPrestamo           DATE          NOT NULL,
        FechaDevolucionPrevista DATE          NOT NULL,
        FechaDevolucionReal     DATE          NULL,

        CONSTRAINT PK_Prestamos PRIMARY KEY (IdPrestamo),
        CONSTRAINT CK_Prestamos_FechaPrevista
            CHECK (FechaDevolucionPrevista > FechaPrestamo)
    );
END;
GO


-- 2. IMPORTAR
BULK INSERT Prestamos
FROM 'C:\Datos\prestamos_biblioteca.txt'
WITH (
    FIELDTERMINATOR = ';',
    ROWTERMINATOR   = '\n',
    FIRSTROW        = 2,
    CODEPAGE        = '65001',
    KEEPIDENTITY
);
GO

SELECT COUNT(*)                    AS TotalPrestamos  FROM Prestamos;
SELECT IDENT_CURRENT('Prestamos')  AS UltimoId;
SELECT COUNT(*)                    AS Pendientes      FROM Prestamos WHERE FechaDevolucionReal IS NULL;
SELECT COUNT(DISTINCT CodigoSocio) AS SociosDistintos FROM Prestamos;
SELECT COUNT(DISTINCT ISBN)        AS LibrosDistintos FROM Prestamos;
SELECT Genero, COUNT(*) AS Prestamos FROM Prestamos GROUP BY Genero ORDER BY Prestamos DESC;
GO

-- TRUNCATE TABLE Prestamos;


-- 3. INDICE
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Prestamos_CodigoSocio')
    CREATE NONCLUSTERED INDEX IX_Prestamos_CodigoSocio ON Prestamos (CodigoSocio);
GO

-- CREATE UNIQUE NONCLUSTERED INDEX IX_Tabla_Columna ON Tabla (Columna);
-- DROP INDEX IX_Prestamos_CodigoSocio ON Prestamos;

SET STATISTICS IO ON;
SELECT IdPrestamo, Titulo, FechaPrestamo, FechaDevolucionPrevista, FechaDevolucionReal
FROM Prestamos
WHERE CodigoSocio = 'S2966'
ORDER BY FechaPrestamo DESC;
SET STATISTICS IO OFF;
GO


-- 4. VISTAS
GO
CREATE OR ALTER VIEW vw_PrestamosPendientes AS
    SELECT IdPrestamo, CodigoSocio, NombreSocio, Titulo, FechaPrestamo,
           FechaDevolucionPrevista,
           DATEDIFF(DAY, FechaDevolucionPrevista, GETDATE()) AS DiasVencido
    FROM Prestamos
    WHERE FechaDevolucionReal IS NULL;
GO

SELECT COUNT(*) AS LibrosFuera FROM vw_PrestamosPendientes;
SELECT COUNT(*) AS Vencidos    FROM vw_PrestamosPendientes WHERE DiasVencido > 0;
SELECT TOP (10) * FROM vw_PrestamosPendientes ORDER BY DiasVencido DESC, IdPrestamo;
GO

CREATE OR ALTER VIEW vw_EstadisticasGenero AS
    SELECT Genero,
           COUNT(*)                   AS TotalPrestamos,
           COUNT(FechaDevolucionReal) AS Devueltos,
           COUNT(CASE WHEN FechaDevolucionReal > FechaDevolucionPrevista THEN 1 END) AS DevueltosConRetraso,
           CAST(COUNT(CASE WHEN FechaDevolucionReal > FechaDevolucionPrevista THEN 1 END) * 100.0
                / COUNT(FechaDevolucionReal) AS DECIMAL(5,1)) AS PorcentajeRetraso
    FROM Prestamos
    GROUP BY Genero;
GO

SELECT * FROM vw_EstadisticasGenero ORDER BY PorcentajeRetraso DESC;
GO


-- 5. PROCEDIMIENTOS
GO
CREATE OR ALTER PROCEDURE usp_PrestamosSocio
    @CodigoSocio VARCHAR(10)
AS
BEGIN
    SELECT IdPrestamo, Titulo, FechaPrestamo, FechaDevolucionPrevista, FechaDevolucionReal,
           CASE
               WHEN FechaDevolucionReal IS NULL                   THEN 'Pendiente'
               WHEN FechaDevolucionReal > FechaDevolucionPrevista THEN 'Devuelto con retraso'
               ELSE 'Devuelto'
           END AS Estado
    FROM Prestamos
    WHERE CodigoSocio = @CodigoSocio
    ORDER BY FechaPrestamo DESC;
END;
GO

EXEC usp_PrestamosSocio @CodigoSocio = 'S2966';
GO

CREATE OR ALTER PROCEDURE usp_RegistrarPrestamo
    @CodigoSocio VARCHAR(10),
    @ISBN        CHAR(13)
AS
BEGIN
    DECLARE @Nombre NVARCHAR(100);
    SELECT TOP (1) @Nombre = NombreSocio FROM Prestamos WHERE CodigoSocio = @CodigoSocio;
    IF @Nombre IS NULL
    BEGIN
        SELECT CONCAT('ERROR: el socio ', @CodigoSocio, ' no existe') AS Mensaje;
        RETURN;
    END;

    DECLARE @Titulo NVARCHAR(150), @Autor NVARCHAR(100), @Genero NVARCHAR(20);
    SELECT TOP (1) @Titulo = Titulo, @Autor = Autor, @Genero = Genero
    FROM Prestamos WHERE ISBN = @ISBN;
    IF @Titulo IS NULL
    BEGIN
        SELECT CONCAT('ERROR: el libro ', @ISBN, ' no existe') AS Mensaje;
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM vw_PrestamosPendientes
               WHERE CodigoSocio = @CodigoSocio AND DiasVencido > 0)
    BEGIN
        SELECT 'DENEGADO: el socio tiene libros vencidos sin devolver' AS Mensaje;
        RETURN;
    END;

    DECLARE @EnCasa INT;
    SELECT @EnCasa = COUNT(*) FROM vw_PrestamosPendientes WHERE CodigoSocio = @CodigoSocio;
    IF @EnCasa >= 3
    BEGIN
        SELECT 'DENEGADO: el socio ya tiene 3 libros en prestamo' AS Mensaje;
        RETURN;
    END;

    INSERT INTO Prestamos (CodigoSocio, NombreSocio, ISBN, Titulo, Autor, Genero,
                           FechaPrestamo, FechaDevolucionPrevista)
    VALUES (@CodigoSocio, @Nombre, @ISBN, @Titulo, @Autor, @Genero,
            GETDATE(), DATEADD(DAY, 15, GETDATE()));

    SELECT CONCAT('OK: prestamo ', SCOPE_IDENTITY(), ' registrado. Devolver antes del ',
                  CONVERT(NVARCHAR(10), DATEADD(DAY, 15, GETDATE()), 103)) AS Mensaje;
END;
GO

CREATE OR ALTER PROCEDURE usp_RegistrarDevolucion
    @IdPrestamo INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Prestamos WHERE IdPrestamo = @IdPrestamo)
    BEGIN
        SELECT CONCAT('ERROR: el prestamo ', @IdPrestamo, ' no existe') AS Mensaje;
        RETURN;
    END;

    DECLARE @FechaReal DATE, @FechaPrevista DATE;
    SELECT @FechaReal = FechaDevolucionReal, @FechaPrevista = FechaDevolucionPrevista
    FROM Prestamos WHERE IdPrestamo = @IdPrestamo;

    IF @FechaReal IS NOT NULL
    BEGIN
        SELECT CONCAT('ERROR: el prestamo ya se devolvio el ', CONVERT(NVARCHAR(10), @FechaReal, 103)) AS Mensaje;
        RETURN;
    END;

    UPDATE Prestamos SET FechaDevolucionReal = GETDATE()
    WHERE IdPrestamo = @IdPrestamo;

    DECLARE @Retraso INT = DATEDIFF(DAY, @FechaPrevista, GETDATE());
    IF @Retraso > 0
        SELECT CONCAT('Devuelto con ', @Retraso, ' dias de retraso') AS Mensaje;
    ELSE
        SELECT 'Devuelto en plazo. Gracias!' AS Mensaje;
END;
GO

-- EXEC usp_RegistrarPrestamo 'S0003', '9788457144799';
-- EXEC usp_RegistrarDevolucion 40065;

-- DELETE FROM Prestamos WHERE IdPrestamo > 60000;
-- UPDATE Prestamos SET FechaDevolucionReal = NULL WHERE IdPrestamo = 40065;
-- DBCC CHECKIDENT ('Prestamos', RESEED, 60000);
GO


-- 6. TABLAS RELACIONADAS
USE AccesoDatos;
GO

IF OBJECT_ID('Alumnos', 'U') IS NULL
BEGIN
    CREATE TABLE Alumnos (
        Id      INT IDENTITY(1,1) NOT NULL,
        Nombre  NVARCHAR(100)     NOT NULL,
        Email   NVARCHAR(150)     NOT NULL,
        Edad    INT               NULL,
        Activo  BIT               NOT NULL CONSTRAINT DF_Alumnos_Activo DEFAULT 1,
        CONSTRAINT PK_Alumnos       PRIMARY KEY (Id),
        CONSTRAINT UQ_Alumnos_Email UNIQUE (Email),
        CONSTRAINT CK_Alumnos_Edad  CHECK (Edad >= 16)
    );
END;

IF OBJECT_ID('Cursos', 'U') IS NULL
BEGIN
    CREATE TABLE Cursos (
        Id      INT IDENTITY(1,1) NOT NULL,
        Titulo  NVARCHAR(100)     NOT NULL,
        Precio  DECIMAL(10,2)     NOT NULL,
        Plazas  INT               NOT NULL,
        CONSTRAINT PK_Cursos        PRIMARY KEY (Id),
        CONSTRAINT CK_Cursos_Precio CHECK (Precio >= 0),
        CONSTRAINT CK_Cursos_Plazas CHECK (Plazas > 0)
    );
END;

IF OBJECT_ID('Matriculas', 'U') IS NULL
BEGIN
    CREATE TABLE Matriculas (
        Id        INT IDENTITY(1,1) NOT NULL,
        AlumnoId  INT               NOT NULL,
        CursoId   INT               NOT NULL,
        Fecha     DATE              NOT NULL CONSTRAINT DF_Matriculas_Fecha DEFAULT GETDATE(),
        CONSTRAINT PK_Matriculas             PRIMARY KEY (Id),
        CONSTRAINT FK_Matriculas_Alumnos     FOREIGN KEY (AlumnoId) REFERENCES Alumnos (Id),
        CONSTRAINT FK_Matriculas_Cursos      FOREIGN KEY (CursoId)  REFERENCES Cursos (Id),
        CONSTRAINT UQ_Matriculas_AlumnoCurso UNIQUE (AlumnoId, CursoId)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM Alumnos)
    INSERT INTO Alumnos (Nombre, Email, Edad) VALUES
        (N'Ana Lopez',  N'ana@academia.com',   20),
        (N'Luis Perez', N'luis@academia.com',  17),
        (N'Marta Ruiz', N'marta@academia.com', 25);

IF NOT EXISTS (SELECT 1 FROM Cursos)
    INSERT INTO Cursos (Titulo, Precio, Plazas) VALUES
        (N'SQL Server desde cero', 120.00, 3),
        (N'C# avanzado',           200.00, 2);

IF NOT EXISTS (SELECT 1 FROM Matriculas)
    INSERT INTO Matriculas (AlumnoId, CursoId) VALUES (1, 1), (2, 1), (1, 2);
GO

-- INSERT INTO Matriculas (AlumnoId, CursoId) VALUES (99, 1);
-- INSERT INTO Matriculas (AlumnoId, CursoId) VALUES (1, 1);


-- 7. ALTER TABLE
ALTER TABLE Alumnos ADD Telefono NVARCHAR(20) NULL;
GO
ALTER TABLE Alumnos ALTER COLUMN Telefono NVARCHAR(30) NULL;
ALTER TABLE Alumnos ADD CONSTRAINT CK_Alumnos_Telefono CHECK (LEN(Telefono) >= 9);
ALTER TABLE Alumnos DROP CONSTRAINT CK_Alumnos_Telefono;
ALTER TABLE Alumnos DROP COLUMN Telefono;
GO


-- 8. VISTA CON JOIN
GO
CREATE OR ALTER VIEW vw_CursosConAlumnos AS
    SELECT c.Id, c.Titulo, c.Plazas,
           COUNT(m.Id)            AS Matriculados,
           c.Plazas - COUNT(m.Id) AS PlazasLibres
    FROM Cursos c
    LEFT JOIN Matriculas m ON m.CursoId = c.Id
    GROUP BY c.Id, c.Titulo, c.Plazas;
GO

SELECT * FROM vw_CursosConAlumnos ORDER BY Id;

SELECT m.Id, a.Nombre AS Alumno, c.Titulo AS Curso, CONVERT(NVARCHAR(10), m.Fecha, 103) AS Fecha
FROM Matriculas m
INNER JOIN Alumnos a ON a.Id = m.AlumnoId
INNER JOIN Cursos  c ON c.Id = m.CursoId
ORDER BY m.Id;
GO


-- 9. PROCEDIMIENTO CON OUTPUT
GO
CREATE OR ALTER PROCEDURE usp_MatricularAlumno
    @AlumnoId INT,
    @CursoId  INT,
    @NuevoId  INT OUTPUT
AS
BEGIN
    SET @NuevoId = NULL;

    IF NOT EXISTS (SELECT 1 FROM Alumnos WHERE Id = @AlumnoId)
    BEGIN
        SELECT 'ERROR: el alumno no existe' AS Mensaje;
        RETURN;
    END;

    IF NOT EXISTS (SELECT 1 FROM Cursos WHERE Id = @CursoId)
    BEGIN
        SELECT 'ERROR: el curso no existe' AS Mensaje;
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM Matriculas WHERE AlumnoId = @AlumnoId AND CursoId = @CursoId)
    BEGIN
        SELECT 'ERROR: ya esta matriculado' AS Mensaje;
        RETURN;
    END;

    DECLARE @Plazas INT, @Matriculados INT;
    SELECT @Plazas = Plazas FROM Cursos WHERE Id = @CursoId;
    SELECT @Matriculados = COUNT(*) FROM Matriculas WHERE CursoId = @CursoId;
    IF @Matriculados >= @Plazas
    BEGIN
        SELECT 'ERROR: no quedan plazas' AS Mensaje;
        RETURN;
    END;

    INSERT INTO Matriculas (AlumnoId, CursoId) VALUES (@AlumnoId, @CursoId);
    SET @NuevoId = SCOPE_IDENTITY();

    SELECT CONCAT('OK: matricula ', @NuevoId, ' registrada') AS Mensaje;
END;
GO

DECLARE @id INT;
EXEC usp_MatricularAlumno @AlumnoId = 3, @CursoId = 2, @NuevoId = @id OUTPUT;
SELECT @id AS NuevoId;
GO



-- 11. BORRAR
-- UPDATE Alumnos SET Activo = 0 WHERE Id = 3;
-- DELETE FROM Alumnos WHERE Id = 3;
-- TRUNCATE TABLE Tabla;
