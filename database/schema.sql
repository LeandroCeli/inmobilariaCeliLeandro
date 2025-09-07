-- Crear la base de datos
CREATE DATABASE IF NOT EXISTS InmobiliariaDB
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_general_ci;

USE InmobiliariaDB;

-- ======================================
-- Tabla: Propietarios
-- ======================================
CREATE TABLE IF NOT EXISTS Propietarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DNI VARCHAR(15) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Email VARCHAR(100),
    Telefono VARCHAR(25),
    Direccion VARCHAR(150),
    FechaAlta DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- ======================================
-- Tabla: Inquilinos
-- ======================================
CREATE TABLE IF NOT EXISTS Inquilinos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DNI VARCHAR(15) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Email VARCHAR(100),
    Telefono VARCHAR(25),
    Direccion VARCHAR(150),
    FechaAlta DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);


-- ======================================
-- Datos de prueba (opcional)
-- ======================================
INSERT INTO Propietarios (DNI, Apellido, Nombre, Email, Telefono, Direccion)
VALUES
('30123456', 'Pérez', 'Juan', 'juan.perez@example.com', '2664123456', 'Av. Siempre Viva 123'),
('28987654', 'Gómez', 'María', 'maria.gomez@example.com', '2664789123', 'Calle Falsa 456');

INSERT INTO Inquilinos (DNI, Apellido, Nombre, Email, Telefono, Direccion)
VALUES
('40123456', 'Rodríguez', 'Ana', 'ana.rodriguez@example.com', '2664333444', 'Boulevard Mitre 789'),
('41987654', 'López', 'Carlos', 'carlos.lopez@example.com', '2664777888', 'Pasaje San Martín 321');



/* contratos y propietarios*/

USE InmobiliariaDB;

-- ======================================
-- Tabla: Propiedades
-- ======================================
DROP TABLE IF EXISTS Propiedades;
CREATE TABLE Propiedades (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Direccion VARCHAR(150) NOT NULL,
    Tipo VARCHAR(50) NOT NULL,      -- Casa, Departamento, Local, etc.
    Uso VARCHAR(50) NOT NULL,       -- Comercial, Residencial, etc.
    Ambientes INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    IdPropietario INT NOT NULL,
    FOREIGN KEY (IdPropietario) REFERENCES Propietarios(Id)
);

-- ======================================
-- Tabla: Contratos
-- ======================================
DROP TABLE IF EXISTS Contratos;
CREATE TABLE Contratos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    IdPropiedad INT NOT NULL,
    IdInquilino INT NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    MontoMensual DECIMAL(10,2) NOT NULL,
    Deposito DECIMAL(10,2),
    FOREIGN KEY (IdPropiedad) REFERENCES Propiedades(Id),
    FOREIGN KEY (IdInquilino) REFERENCES Inquilinos(Id)
);

-- ======================================
-- Datos iniciales (Propiedades de prueba)
-- ======================================
INSERT INTO Propiedades (Direccion, Tipo, Uso, Ambientes, Precio, IdPropietario)
VALUES
('Av. Libertador 123', 'Departamento', 'Residencial', 3, 55000.00, 1),
('Calle San Martín 456', 'Casa', 'Residencial', 4, 75000.00, 2);

-- ======================================
-- Datos iniciales (Contratos de prueba)
-- ======================================
INSERT INTO Contratos (IdPropiedad, IdInquilino, FechaInicio, FechaFin, MontoMensual, Deposito)
VALUES
(1, 1, '2025-01-01', '2025-12-31', 55000.00, 110000.00),
(2, 2, '2025-02-01', '2026-01-31', 75000.00, 150000.00);
