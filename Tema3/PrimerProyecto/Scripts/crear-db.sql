-- Crea la base de datos y la tabla del CRUD (§3.6.1).
-- Se puede ejecutar varias veces: solo crea lo que todavía no existe.
IF DB_ID('AccesoDatos') IS NULL
    CREATE DATABASE AccesoDatos;
GO

USE AccesoDatos;
GO

IF OBJECT_ID('Personas') IS NULL
BEGIN
    CREATE TABLE Personas (
        Id      INT IDENTITY(1,1) PRIMARY KEY,
        Nombre  NVARCHAR(100) NOT NULL,
        Email   NVARCHAR(150) NOT NULL,
        Edad    INT CHECK (Edad >= 0),
        Activo  BIT NOT NULL DEFAULT 1
    );

    CREATE UNIQUE NONCLUSTERED INDEX IX_Personas_Email ON Personas (Email);

    INSERT INTO Personas (Nombre, Email, Edad) VALUES
        (N'Ana López',  N'ana@correo.com',   22),
        (N'Luis Pérez', N'luis@correo.com',  17),
        (N'Marta Ruiz', N'marta@correo.com', 35);
END;
GO