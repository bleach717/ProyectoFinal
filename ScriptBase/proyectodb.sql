-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 02-12-2025 a las 02:07:30
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `proyectodb`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `activo`
--

CREATE TABLE `activo` (
  `id_activo` int(11) NOT NULL,
  `id_tipo` int(11) NOT NULL,
  `detalle` varchar(200) DEFAULT NULL,
  `Marca` varchar(100) DEFAULT NULL,
  `Modelo` varchar(100) DEFAULT NULL,
  `Serie` varchar(100) DEFAULT NULL,
  `Anio` int(11) DEFAULT NULL,
  `estado` varchar(50) DEFAULT NULL,
  `costo` decimal(8,2) DEFAULT NULL,
  `ubicacion` varchar(100) DEFAULT NULL,
  `disponible` tinyint(1) DEFAULT 1,
  `fecha_registro` date DEFAULT curdate(),
  `propietario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `activo`
--

INSERT INTO `activo` (`id_activo`, `id_tipo`, `detalle`, `Marca`, `Modelo`, `Serie`, `Anio`, `estado`, `costo`, `ubicacion`, `disponible`, `fecha_registro`, `propietario`) VALUES
(1, 1, 'Laptop', 'sd', 'ds', 'sd', 2025, 'BUENO', 650.00, 'Bodega', 1, '2025-11-22', 1),
(2, 1, 'Laptop HP i7 - 16GB RAM', NULL, NULL, NULL, NULL, 'REGULAR', 900.00, 'Oficina 2', 1, '2025-11-22', 2),
(3, 2, 'Mouse inalámbrico Logitech', NULL, NULL, NULL, NULL, 'BUENO', 25.00, 'Oficina 1', 1, '2025-11-22', 3),
(4, 2, 'Mouse Genius USB', NULL, NULL, NULL, NULL, 'MALO', 10.00, 'Bodega', 0, '2025-11-22', 3),
(5, 3, 'Impresora Epson L3150', NULL, NULL, NULL, NULL, 'BUENO', 250.00, 'Recepción', 1, '2025-11-22', 4),
(6, 3, 'Impresora HP LaserJet 1020', NULL, NULL, NULL, NULL, 'REGULAR', 180.00, 'Oficina 3', 1, '2025-11-22', 1),
(7, 4, 'Proyector Epson X05+', NULL, NULL, NULL, NULL, 'BUENO', 500.00, 'Sala de reuniones', 1, '2025-11-22', 5),
(8, 4, 'Proyector BenQ MH550', NULL, NULL, NULL, NULL, 'REGULAR', 450.00, 'Bodega', 1, '2025-11-22', 2),
(9, 5, 'Escritorio madera 120cm', NULL, NULL, NULL, NULL, 'BUENO', 120.00, 'Oficina 4', 1, '2025-11-22', 4),
(10, 5, 'Escritorio metálico 140cm', NULL, NULL, NULL, NULL, 'REGULAR', 140.00, 'Oficina 1', 1, '2025-11-22', 2),
(16, 7, 'Camara', 'sonu', 'ad25d', 'dfd552', 2025, 'nuevo', 20.30, 'Oficina', 1, '2025-12-02', 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_activo`
--

CREATE TABLE `tipo_activo` (
  `id_tipo` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `descripcion` varchar(200) DEFAULT NULL,
  `ruta_imagen` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo_activo`
--

INSERT INTO `tipo_activo` (`id_tipo`, `nombre`, `descripcion`, `ruta_imagen`) VALUES
(1, 'Laptop', 'Equipo portátil de oficina', 'img/computador.png'),
(2, 'Mouse', 'Dispositivo periférico', 'img/mouse.png'),
(3, 'Impresora', 'Impresora multifunción', 'img/impresora.png'),
(4, 'Proyector', 'Proyector de presentaciones', 'img/proyector.png'),
(5, 'Escritorio', 'Mueble de oficina', 'img/escritorio.png'),
(6, 'Mesa Madera', 'Mesa Madera Caoba 32', '{\"error\":false,\"mensaje\":\"Imagen subida correctamente\",\"url\":\"http:\\/\\/127.0.0.1\\/wsproyecto\\/imagenes_tipo\\/img_6927eb3e284617.51069623.jpg\"}'),
(7, 's', 's', 'img_6927ee93bbd130.16991909.jpg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `transferencia`
--

CREATE TABLE `transferencia` (
  `id_transferencia` int(11) NOT NULL,
  `id_activo` int(11) NOT NULL,
  `id_usuario_origen` int(11) NOT NULL,
  `id_usuario_destino` int(11) NOT NULL,
  `fecha_transferencia` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `transferencia`
--

INSERT INTO `transferencia` (`id_transferencia`, `id_activo`, `id_usuario_origen`, `id_usuario_destino`, `fecha_transferencia`) VALUES
(1, 1, 1, 2, '2025-11-22 21:52:06'),
(2, 2, 2, 3, '2025-11-22 21:52:06'),
(3, 3, 3, 4, '2025-11-22 21:52:06'),
(4, 4, 3, 1, '2025-11-22 21:52:06'),
(5, 5, 4, 5, '2025-11-22 21:52:06'),
(6, 6, 1, 3, '2025-11-22 21:52:06'),
(7, 7, 5, 2, '2025-11-22 21:52:06'),
(8, 8, 2, 4, '2025-11-22 21:52:06'),
(9, 9, 4, 1, '2025-11-22 21:52:06'),
(10, 10, 2, 5, '2025-11-22 21:52:06'),
(11, 16, 1, 3, '2025-12-01 20:06:27');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id_usuario` int(11) NOT NULL,
  `correo` varchar(100) NOT NULL,
  `contrasena` varchar(200) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `cedula` varchar(10) NOT NULL,
  `rol` enum('ADMIN','USER') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id_usuario`, `correo`, `contrasena`, `nombre`, `apellido`, `cedula`, `rol`) VALUES
(1, 'admin@empresa.com', '123456', 'Carlos', 'Mendoza', '0102030405', 'ADMIN'),
(2, 'maria@empresa.com', '123456', 'María', 'Gómez', '0912233445', 'USER'),
(3, 'juan@empresa.com', '123456', 'Juan', 'Torres', '1103344556', 'USER'),
(4, 'sofia@empresa.com', '123456', 'Sofía', 'Lopez', '0921122334', 'USER'),
(5, 'pedro@empresa.com', '123456', 'Pedro', 'Vera', '1009988776', 'USER');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `activo`
--
ALTER TABLE `activo`
  ADD PRIMARY KEY (`id_activo`),
  ADD KEY `id_tipo` (`id_tipo`),
  ADD KEY `propietario` (`propietario`);

--
-- Indices de la tabla `tipo_activo`
--
ALTER TABLE `tipo_activo`
  ADD PRIMARY KEY (`id_tipo`);

--
-- Indices de la tabla `transferencia`
--
ALTER TABLE `transferencia`
  ADD PRIMARY KEY (`id_transferencia`),
  ADD KEY `id_activo` (`id_activo`),
  ADD KEY `id_usuario_origen` (`id_usuario_origen`),
  ADD KEY `id_usuario_destino` (`id_usuario_destino`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id_usuario`),
  ADD UNIQUE KEY `correo` (`correo`),
  ADD UNIQUE KEY `cedula` (`cedula`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `activo`
--
ALTER TABLE `activo`
  MODIFY `id_activo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `tipo_activo`
--
ALTER TABLE `tipo_activo`
  MODIFY `id_tipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `transferencia`
--
ALTER TABLE `transferencia`
  MODIFY `id_transferencia` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `activo`
--
ALTER TABLE `activo`
  ADD CONSTRAINT `activo_ibfk_1` FOREIGN KEY (`id_tipo`) REFERENCES `tipo_activo` (`id_tipo`) ON UPDATE CASCADE,
  ADD CONSTRAINT `activo_ibfk_2` FOREIGN KEY (`propietario`) REFERENCES `usuario` (`id_usuario`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `transferencia`
--
ALTER TABLE `transferencia`
  ADD CONSTRAINT `transferencia_ibfk_1` FOREIGN KEY (`id_activo`) REFERENCES `activo` (`id_activo`) ON UPDATE CASCADE,
  ADD CONSTRAINT `transferencia_ibfk_2` FOREIGN KEY (`id_usuario_origen`) REFERENCES `usuario` (`id_usuario`) ON UPDATE CASCADE,
  ADD CONSTRAINT `transferencia_ibfk_3` FOREIGN KEY (`id_usuario_destino`) REFERENCES `usuario` (`id_usuario`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
