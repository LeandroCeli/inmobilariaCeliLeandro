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
