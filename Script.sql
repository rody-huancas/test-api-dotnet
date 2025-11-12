CREATE DATABASE DBCrud;
GO

USE DBCrud;
GO

CREATE TABLE Departamento (
    IdDepartamento INT IDENTITY PRIMARY KEY,
    Nombre         VARCHAR(50)
);
GO

CREATE TABLE Empleado (
    IdEmpleado     INT IDENTITY PRIMARY KEY,
    NombreCompleto VARCHAR(50),
    IdDepartamento INT REFERENCES Departamento (IdDepartamento),
    Sueldo         DECIMAL(10,2),
    FechaContrato  DATE
);
GO

INSERT INTO Departamento (Nombre) VALUES ('Administracion'), ('Marketing');

INSERT INTO Empleado (NombreCompleto, IdDepartamento, Sueldo, FechaContrato) VALUES ('Maria Mendez', 1, 4500, '2024-01-12');

SELECT * FROM Departamento;
SELECT * FROM Empleado;
GO

CREATE PROCEDURE sp_listaEmpleados
AS
BEGIN
    SELECT
        e.IdEmpleado,
        e.NombreCompleto,
        e.Sueldo,
        CONVERT(CHAR(10), e.FechaContrato, 103) AS FechaContrato,
        d.IdDepartamento,
        d.Nombre
    FROM Empleado e
    INNER JOIN Departamento d
        ON e.IdDepartamento = d.IdDepartamento;
END;
GO

CREATE PROCEDURE sp_crearEmpleado
(
    @NombreCompleto VARCHAR(50),
    @IdDepartamento INT,
    @Sueldo         DECIMAL(10,2),
    @FechaContrato  VARCHAR(10)
)
AS
BEGIN
    SET DATEFORMAT dmy;

    INSERT INTO Empleado (
        NombreCompleto,
        IdDepartamento,
        Sueldo,
        FechaContrato
    ) VALUES (
        @NombreCompleto,
        @IdDepartamento,
        @Sueldo,
        CONVERT(DATE, @FechaContrato)
    );
END;
GO

CREATE PROCEDURE sp_editarEmpleado
(
    @IdEmpleado     INT,
    @NombreCompleto VARCHAR(50),
    @IdDepartamento INT,
    @Sueldo         DECIMAL(10,2),
    @FechaContrato  VARCHAR(10)
)
AS
BEGIN
    SET DATEFORMAT dmy;

    UPDATE Empleado
    SET
        NombreCompleto = @NombreCompleto,
        IdDepartamento = @IdDepartamento,
        Sueldo = @Sueldo,
        FechaContrato = CONVERT(DATE, @FechaContrato)
    WHERE IdEmpleado = @IdEmpleado;
END;
GO

CREATE PROCEDURE sp_eliminarEmpleado
(
    @IdEmpleado INT
)
AS
BEGIN
    DELETE FROM Empleado
    WHERE IdEmpleado = @IdEmpleado;
END;
GO