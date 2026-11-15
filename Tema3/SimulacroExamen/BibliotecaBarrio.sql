IF DB_ID('BibliotecaBarrio') IS NULL
    CREATE DATABASE BibliotecaBarrio;
GO
USE BibliotecaBarrio;
GO

-- A1. TABLAS
DROP TABLE IF EXISTS Prestamos;
DROP TABLE IF EXISTS Libros;
GO

CREATE TABLE Libros (
    IdLibro     INT           NOT NULL,
    Titulo      NVARCHAR(150) NOT NULL,
    Autor       NVARCHAR(100) NOT NULL,
    Genero      NVARCHAR(20)  NOT NULL,
    Ejemplares  INT           NOT NULL,
    CONSTRAINT PK_Libros            PRIMARY KEY (IdLibro),
    CONSTRAINT CK_Libros_Genero     CHECK (Genero IN (N'Novela', N'Ensayo', N'Infantil', N'Cómic')),
    CONSTRAINT CK_Libros_Ejemplares CHECK (Ejemplares > 0)
);

CREATE TABLE Prestamos (
    IdPrestamo     INT IDENTITY(1,1) NOT NULL,
    IdLibro        INT           NOT NULL,
    NombreLector   NVARCHAR(100) NOT NULL,
    EsSocio        BIT           NOT NULL,
    FechaPrestamo  DATE          NOT NULL,
    Dias           INT           NOT NULL,
    Fianza         DECIMAL(6,2)  NOT NULL,
    Estado         NVARCHAR(10)  NOT NULL CONSTRAINT DF_Prestamos_Estado DEFAULT N'Prestado',
    CONSTRAINT PK_Prestamos        PRIMARY KEY (IdPrestamo),
    CONSTRAINT FK_Prestamos_Libros FOREIGN KEY (IdLibro) REFERENCES Libros (IdLibro),
    CONSTRAINT CK_Prestamos_Dias   CHECK (Dias IN (7, 14, 21)),
    CONSTRAINT CK_Prestamos_Fianza CHECK (Fianza >= 0),
    CONSTRAINT CK_Prestamos_Estado CHECK (Estado IN (N'Prestado', N'Devuelto', N'Perdido'))
);
GO

INSERT INTO Libros (IdLibro, Titulo, Autor, Genero, Ejemplares) VALUES
    (1, N'Cien años de soledad',      N'Gabriel García Márquez',   N'Novela',   3),
    (2, N'El principito',             N'Antoine de Saint-Exupéry', N'Infantil', 2),
    (3, N'Sapiens',                   N'Yuval Noah Harari',        N'Ensayo',   2),
    (4, N'Astérix el galo',           N'René Goscinny',            N'Cómic',    2),
    (5, N'La sombra del viento',      N'Carlos Ruiz Zafón',        N'Novela',   3),
    (6, N'Breve historia del tiempo', N'Stephen Hawking',          N'Ensayo',   1);
GO

-- IdLibro no es IDENTITY porque el catálogo es fijo y los Id ya vienen dados;
-- los préstamos se crean continuamente, por eso IdPrestamo sí lo es.

-- A2. INDICE
-- Acelera la búsqueda más frecuente del mostrador (los préstamos de un lector):
-- evita recorrer la tabla entera, a cambio de ocupar espacio y ralentizar las escrituras.
CREATE NONCLUSTERED INDEX IX_Prestamos_NombreLector ON Prestamos (NombreLector);
GO

-- A3. VISTA
CREATE OR ALTER VIEW vw_ResumenLibros AS
    SELECT l.IdLibro, l.Titulo, l.Ejemplares,
           COUNT(p.IdPrestamo)                                               AS TotalPrestamos,
           COUNT(CASE WHEN p.Estado = N'Prestado' THEN 1 END)                AS PrestadosAhora,
           l.Ejemplares - COUNT(CASE WHEN p.Estado = N'Prestado' THEN 1 END) AS Disponibles,
           ISNULL(SUM(p.Fianza), 0)                                          AS FianzasCobradas
    FROM Libros l
    LEFT JOIN Prestamos p ON p.IdLibro = l.IdLibro
    GROUP BY l.IdLibro, l.Titulo, l.Ejemplares;
GO

-- A4. PROCEDIMIENTO
CREATE OR ALTER PROCEDURE usp_EjemplaresDisponibles
    @IdLibro     INT,
    @Disponibles INT OUTPUT
AS
BEGIN
    SET @Disponibles = NULL;

    IF NOT EXISTS (SELECT 1 FROM Libros WHERE IdLibro = @IdLibro)
        RETURN;

    DECLARE @Ejemplares INT, @Prestados INT;
    SELECT @Ejemplares = Ejemplares FROM Libros WHERE IdLibro = @IdLibro;
    SELECT @Prestados = COUNT(*) FROM Prestamos WHERE IdLibro = @IdLibro AND Estado = N'Prestado';

    SET @Disponibles = @Ejemplares - @Prestados;
END;
GO

SELECT * FROM vw_ResumenLibros ORDER BY IdLibro;

DECLARE @d INT;
EXEC usp_EjemplaresDisponibles @IdLibro = 1, @Disponibles = @d OUTPUT;
SELECT @d AS DisponiblesLibro1;
GO
