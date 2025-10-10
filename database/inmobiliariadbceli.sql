-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 10-10-2025 a las 15:24:45
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

CREATE DATABASE IF NOT EXISTS InmobiliariaDBCeli
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_general_ci;

USE InmobiliariaDBCeli;



SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliariadbceli`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `Id` int(11) NOT NULL,
  `IdPropiedad` int(11) NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFin` date NOT NULL,
  `MontoMensual` decimal(10,2) NOT NULL,
  `Deposito` decimal(10,2) DEFAULT NULL,
  `Cuotas` int(11) NOT NULL,
  `RegistradoPor` varchar(100) DEFAULT NULL,
  `DadoDeBaja` tinyint(1) NOT NULL DEFAULT 0,
  `FechaBaja` date DEFAULT NULL,
  `UsuarioBaja` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`Id`, `IdPropiedad`, `IdInquilino`, `FechaInicio`, `FechaFin`, `MontoMensual`, `Deposito`, `Cuotas`, `RegistradoPor`, `DadoDeBaja`, `FechaBaja`, `UsuarioBaja`) VALUES
(1, 17, 10, '2025-10-10', '2026-02-10', 1000000.00, 350000.00, 4, 'admin@inmobiliaria.com', 0, NULL, NULL),
(2, 7, 3, '2025-10-24', '2025-12-26', 500000.00, 300000.00, 2, 'test@inmobiliaria.com', 0, NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id` int(11) NOT NULL,
  `DNI` varchar(15) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Telefono` varchar(25) DEFAULT NULL,
  `Direccion` varchar(150) DEFAULT NULL,
  `FechaAlta` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id`, `DNI`, `Apellido`, `Nombre`, `Email`, `Telefono`, `Direccion`, `FechaAlta`) VALUES
(1, '40856231', 'Martínez', 'Federico', 'federico.martinez@mail.com', '2665123456', 'San Juan 845', '2025-10-10 10:13:47'),
(2, '41987456', 'Suárez', 'Camila', 'camila.suarez@mail.com', '2665234567', 'Av. Lafinur 1120', '2025-10-10 10:13:47'),
(3, '42896325', 'Acosta', 'Juan Pablo', 'juanp.acosta@mail.com', '2665345678', 'Mitre 765', '2025-10-10 10:13:47'),
(4, '43896547', 'Luna', 'Mariana', 'mariana.luna@mail.com', '2665456789', 'Chacabuco 233', '2025-10-10 10:13:47'),
(5, '44987521', 'Sosa', 'Ezequiel', 'ezequiel.sosa@mail.com', '2665567890', 'Bolívar 965', '2025-10-10 10:13:47'),
(6, '45986314', 'Benítez', 'Florencia', 'florencia.benitez@mail.com', '2665678901', 'Riobamba 440', '2025-10-10 10:13:47'),
(7, '46895213', 'Ramírez', 'Tomás', 'tomas.ramirez@mail.com', '2665789012', 'Pringles 980', '2025-10-10 10:13:47'),
(8, '47895236', 'Vega', 'Julieta', 'julieta.vega@mail.com', '2665890123', 'Colón 1220', '2025-10-10 10:13:47'),
(9, '48895674', 'Domínguez', 'Nicolás', 'nicolas.dominguez@mail.com', '2665901234', 'Av. España 340', '2025-10-10 10:13:47'),
(10, '49896521', 'Carrizo', 'Agustina', 'agustina.carrizo@mail.com', '2665012345', 'Rivadavia 760', '2025-10-10 10:13:47');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(11) NOT NULL,
  `IdContrato` int(11) NOT NULL,
  `NumeroPago` int(11) NOT NULL,
  `FechaPago` date NOT NULL,
  `Detalle` varchar(200) DEFAULT NULL,
  `Importe` decimal(10,2) NOT NULL,
  `Pagado` tinyint(1) DEFAULT 0,
  `FechaRegistro` datetime DEFAULT current_timestamp(),
  `UsuarioRegistro` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`Id`, `IdContrato`, `NumeroPago`, `FechaPago`, `Detalle`, `Importe`, `Pagado`, `FechaRegistro`, `UsuarioRegistro`) VALUES
(1, 1, 1, '2025-10-10', 'Primer Pago', 1000000.00, 1, '2025-10-10 10:21:45', NULL),
(2, 2, 1, '2025-10-10', 'inicio', 500000.00, 1, '2025-10-10 10:24:03', NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propiedades`
--

CREATE TABLE `propiedades` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(150) NOT NULL,
  `Tipo` varchar(50) NOT NULL,
  `Uso` varchar(50) NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `IdPropietario` int(11) NOT NULL,
  `Disponible` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propiedades`
--

INSERT INTO `propiedades` (`Id`, `Direccion`, `Tipo`, `Uso`, `Ambientes`, `Precio`, `IdPropietario`, `Disponible`) VALUES
(1, 'Av. Illia 1020', 'Departamento', 'Residencial', 2, 95000.00, 1, 1),
(2, 'Belgrano 456', 'Casa', 'Residencial', 4, 130000.00, 2, 1),
(3, 'Colón 789', 'Local', 'Comercial', 1, 210000.00, 3, 1),
(4, 'San Martín 321', 'Departamento', 'Residencial', 3, 110000.00, 4, 1),
(5, 'Junín 654', 'Casa', 'Residencial', 5, 145000.00, 5, 1),
(6, 'Pringles 987', 'Depósito', 'Comercial', 1, 195000.00, 6, 1),
(7, 'Bolívar 1122', 'Departamento', 'Residencial', 2, 88000.00, 7, 1),
(8, 'Lafinur 334', 'Casa', 'Residencial', 3, 125000.00, 8, 0),
(9, 'Mitre 556', 'Local', 'Comercial', 1, 205000.00, 9, 1),
(10, 'Rivadavia 778', 'Casa', 'Residencial', 4, 132000.00, 10, 1),
(11, 'Lavalle 990', 'Departamento', 'Residencial', 1, 72000.00, 11, 1),
(12, 'Italia 123', 'Departamento', 'Residencial', 3, 98000.00, 12, 1),
(13, 'Buenos Aires 456', 'Casa', 'Residencial', 4, 140000.00, 13, 0),
(14, 'Constitución 789', 'Local', 'Comercial', 1, 215000.00, 14, 1),
(15, 'Justo Daract 147', 'Casa', 'Residencial', 5, 150000.00, 15, 1),
(16, 'Europa 369', 'Departamento', 'Residencial', 2, 91000.00, 16, 1),
(17, 'España 258', 'Casa', 'Residencial', 3, 118000.00, 17, 1),
(18, 'Luján 951', 'Depósito', 'Comercial', 1, 200000.00, 18, 0),
(19, 'Riobamba 753', 'Casa', 'Residencial', 4, 135000.00, 19, 1),
(20, 'Italia 159', 'Departamento', 'Residencial', 2, 89000.00, 20, 1),
(21, 'San Juan 450', 'Casa', 'Residencial', 5, 148000.00, 2, 1),
(22, 'Falucho 875', 'Departamento', 'Residencial', 1, 70000.00, 4, 1),
(23, 'Rawson 234', 'Local', 'Comercial', 1, 190000.00, 5, 1),
(24, 'Chacabuco 910', 'Casa', 'Residencial', 3, 120000.00, 8, 0),
(25, 'Tucumán 455', 'Departamento', 'Residencial', 2, 95000.00, 10, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `Id` int(11) NOT NULL,
  `DNI` varchar(15) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Telefono` varchar(25) DEFAULT NULL,
  `Direccion` varchar(150) DEFAULT NULL,
  `FechaAlta` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`Id`, `DNI`, `Apellido`, `Nombre`, `Email`, `Telefono`, `Direccion`, `FechaAlta`) VALUES
(1, '20345891', 'López', 'Carlos', 'carlos.lopez@mail.com', '2664123456', 'Av. Illia 1234', '2025-10-10 10:08:02'),
(2, '21458932', 'González', 'María', 'maria.gonzalez@mail.com', '2664234567', 'Belgrano 456', '2025-10-10 10:08:02'),
(3, '22567483', 'Pérez', 'Jorge', 'jorge.perez@mail.com', '2664345678', 'Colón 789', '2025-10-10 10:08:02'),
(4, '23658941', 'Torres', 'Ana', 'ana.torres@mail.com', '2664456789', 'San Martín 321', '2025-10-10 10:08:02'),
(5, '24789562', 'Fernández', 'Luis', 'luis.fernandez@mail.com', '2664567890', 'Junín 654', '2025-10-10 10:08:02'),
(6, '25896347', 'Díaz', 'Patricia', 'patricia.diaz@mail.com', '2664678901', 'Pringles 987', '2025-10-10 10:08:02'),
(7, '26987412', 'Morales', 'Ricardo', 'ricardo.morales@mail.com', '2664789012', 'Bolívar 1122', '2025-10-10 10:08:02'),
(8, '27896543', 'Herrera', 'Sofía', 'sofia.herrera@mail.com', '2664890123', 'Lafinur 334', '2025-10-10 10:08:02'),
(9, '28365479', 'Castillo', 'Héctor', 'hector.castillo@mail.com', '2664901234', 'Mitre 556', '2025-10-10 10:08:02'),
(10, '29456871', 'Ruiz', 'Gabriela', 'gabriela.ruiz@mail.com', '2664012345', 'Rivadavia 778', '2025-10-10 10:08:02'),
(11, '30478569', 'Giménez', 'Fernando', 'fernando.gimenez@mail.com', '2664123457', 'Lavalle 990', '2025-10-10 10:08:02'),
(12, '31589647', 'Cabrera', 'Laura', 'laura.cabrera@mail.com', '2664234568', 'Italia 123', '2025-10-10 10:08:02'),
(13, '32698754', 'Romero', 'Diego', 'diego.romero@mail.com', '2664345679', 'Buenos Aires 456', '2025-10-10 10:08:02'),
(14, '33785649', 'Rojas', 'Lucía', 'lucia.rojas@mail.com', '2664456780', 'Constitución 789', '2025-10-10 10:08:02'),
(15, '34896521', 'Navarro', 'Martín', 'martin.navarro@mail.com', '2664567891', 'Justo Daract 147', '2025-10-10 10:08:02'),
(16, '35987412', 'Vega', 'Elena', 'elena.vega@mail.com', '2664678902', 'Europa 369', '2025-10-10 10:08:02'),
(17, '36895473', 'Castro', 'Pablo', 'pablo.castro@mail.com', '2664789013', 'España 258', '2025-10-10 10:08:02'),
(18, '37896541', 'Molina', 'Verónica', 'veronica.molina@mail.com', '2664890124', 'Luján 951', '2025-10-10 10:08:02'),
(19, '38965472', 'Soto', 'Andrés', 'andres.soto@mail.com', '2664901235', 'Riobamba 753', '2025-10-10 10:08:02'),
(20, '39876541', 'Medina', 'Claudia', 'claudia.medina@mail.com', '2664012346', 'Italia 159', '2025-10-10 10:08:02');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `Id` int(11) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `NombreCompleto` varchar(100) DEFAULT NULL,
  `Rol` varchar(20) NOT NULL,
  `FotoPerfil` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id`, `Email`, `PasswordHash`, `NombreCompleto`, `Rol`, `FotoPerfil`) VALUES
(3, 'admin@inmobiliaria.com', '$2a$11$PZy/IqArdg1PcvLLyIqQu.JIuJAbvGxHJzPLKlUo38SSW03qe2U12', 'Administrador Inicial', 'Administrador', NULL),
(7, 'test@inmobiliaria.com', '$2a$11$UBKA19BVnbuWsLr3In0mGeMIxItaz2UJSQE3cPJOtdDj6t3lsb4Oe', 'test', 'Empleado', NULL);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IdPropiedad` (`IdPropiedad`),
  ADD KEY `IdInquilino` (`IdInquilino`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IdContrato` (`IdContrato`);

--
-- Indices de la tabla `propiedades`
--
ALTER TABLE `propiedades`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IdPropietario` (`IdPropietario`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `propiedades`
--
ALTER TABLE `propiedades`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `contratos_ibfk_1` FOREIGN KEY (`IdPropiedad`) REFERENCES `propiedades` (`Id`),
  ADD CONSTRAINT `contratos_ibfk_2` FOREIGN KEY (`IdInquilino`) REFERENCES `inquilinos` (`Id`);

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `pagos_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contratos` (`Id`);

--
-- Filtros para la tabla `propiedades`
--
ALTER TABLE `propiedades`
  ADD CONSTRAINT `propiedades_ibfk_1` FOREIGN KEY (`IdPropietario`) REFERENCES `propietarios` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
