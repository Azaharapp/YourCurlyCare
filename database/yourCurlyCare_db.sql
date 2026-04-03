SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;


CREATE OR REPLACE TABLE `cuestionario` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `n_pregunta` int(11) DEFAULT NULL,
  `fecha_creacion` datetime NOT NULL,
  `estado` enum('Borrador','Entregado','Sin_contestar') NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

CREATE OR REPLACE TABLE `pregunta` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_cuestionario` int(11) NOT NULL,
  `enunciado` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

CREATE OR REPLACE TABLE `productoEscaner` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `codigo_barras` varchar(255) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `marca` varchar(255) NOT NULL DEFAULT '0',
  `ingredientes` text NOT NULL DEFAULT '0',
  `alcohol` tinyint(1) NOT NULL,
  `silicona` tinyint(1) NOT NULL,
  `sulfato` tinyint(1) NOT NULL,
  `es_apto` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

INSERT INTO `productoEscaner` (`id`, `codigo_barras`, `nombre`, `marca`, `ingredientes`, `alcohol`, `silicona`, `sulfato`, `es_apto`) VALUES
(1, '8402001007903', 'Espuma Ondas Curly', 'deliplus', 'aqua, isobutane, polyquaternium-4, propane, propylene glycol, xylitylglucoside, laureth-4, parfum, sodium benzoate, anhydroxylitol, maltitol, potassium sorbate, cocamidopropyl betaine, disodium edta, panthenol, xylitol, citric acid, butane, pelvetia canaliculata extract, limonene, phenoxyethanol, linalool, citronellol, genariol, ethylhexylgrycerin', 0, 0, 0, 1),
(3, '8700216166577', 'Champú Hidrate Coconut scent', 'Herbal Essences', 'Aqua, Sodium Laureth Sulfate, Sodium Citrate, Cocamidopropyl Betaine, Sodium Xylenesulfonate, Sodium Chloride, Parfum, Sodium Benzoate, Glycerin, Citric Acid, Stearyl Alcohol, Sodium Salicylate, Guar Hydrorypropyiltrimonium Chloride, Cetyl Alcohol, Trihydroxystearin, Sodium Hydroxide Hexamethylindanopyran, Polyquatemium-6, Tetrasodium EDTA, Linalool, Linalyl Acetate, Citrus Aurantium Peel Oil Histidine, Coumarin, Limonene, Vanillin, Terpineol, Cocos Nucifera Fruit Juice', 0, 0, 1, 1),
(4, '8480000226327', 'Curl Perfect Sérum oil', 'deliplus', 'COCO-CAPRYLATE/CAPRATE, PERSEA GRATISSIMA OIL, MACADAMIA TERNIFOLIA SEED OIL, DIBUTYL ADIPATE, ARGANIA SPINOSA KERNEL OIL, GLYCINE SOJA OIL, PARFUM, TOCOPHEROL, BETA-SITOSTEROL, SQUALENE, LIMONENE, GERANIOL.', 0, 0, 0, 1);

CREATE OR REPLACE TABLE `registroEscaner` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_productoE` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `fecha_escaner` datetime NOT NULL,
  `respuesta` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

INSERT INTO `registroEscaner` (`id`, `id_productoE`, `id_usuario`, `fecha_escaner`, `respuesta`) VALUES
(1, 2, 0, '2026-04-02 12:27:56', '1'),
(2, 2, 0, '2026-04-02 13:32:57', '1'),
(3, 2, 0, '2026-04-02 13:33:15', '1'),
(4, 2, 0, '2026-04-02 18:51:20', '1'),
(5, 3, 0, '2026-04-02 21:09:54', '1'),
(6, 4, 0, '2026-04-02 21:38:42', '1'),
(7, 5, 0, '2026-04-02 22:54:42', '1');

CREATE OR REPLACE TABLE `respuesta` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_usuario` int(11) NOT NULL,
  `id_cuestionario` int(11) NOT NULL,
  `id_pregunta` int(11) NOT NULL,
  `opcion` enum('a','b','c') NOT NULL,
  `fecha_realizacion` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

CREATE OR REPLACE TABLE`resultado` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_cuestionario` int(11) NOT NULL,
  `resultado_final` varchar(255) DEFAULT NULL,
  `fecha_realizacion` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

CREATE OR REPLACE TABLE `usuario` (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) DEFAULT NULL,
  `username` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL,
  `fecha_registro` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `username` (`username`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

INSERT INTO `usuario` (`id`, `nombre`, `apellido`, `username`, `email`, `password`, `fecha_registro`) VALUES
(1, 'Lola', 'Martinez', 'lolamar', 'lolamar@ejemplo.com', '123456', '2026-03-05 20:13:27'),
(2, 'string', 'string', 'string', 'jii@ejemplo.com', '$2a$11$QJbtVbizLMt7.jtW5fLPCexEqmoWd3bgGPCnVH.GwOzk2WeEwcdtC', '2026-03-08 20:24:47'),
(3, 'hola', 'holita', 'piparrita', 'yeah@example.com', '$2a$11$E1jXYZ2.fdmBwC5jOK/nm.TI2fyPL7qlgF7TnIbt26dElwPt0Ij9K', '2026-03-09 18:31:25'),
(4, 'Susanita', 'Tiene Un', 'ratonChiquitin', 'quecome@chocolate.y', '$2a$11$mpy5Ozzv.TzQrjXxbddx2OULtOO5I0ZqaMHM.A2cu6oFqAO5pNtT6', '2026-03-12 22:12:41'),
(7, 'ricitos', 'de', 'oro', 'delbueno@muy.bueno', '$2a$11$QnHL9OJ7hxoo8kkDBMU1Bewxaq7561BIleuUBDvZOx8ONIgO6LODi', '2026-03-15 13:14:04'),
(8, 'bob', 'esponja', 'patricio', 'estrella@calamardo.gary', '$2a$11$ZrEqjAIs3mfGD.NJ.cK0iufSaKY/QTku4iqGEeTRY8HMBMcUWFyhm', '2026-03-25 19:20:05'),
(9, 'hola', 'hola', 'hola', 'hola@ejemplo.com', '$2a$11$75KJgu9VJjTH6QLISpsM.eHY.riT9OBmzirFjOj21KiRT3p9A1Ut6', '2026-03-28 10:15:01'),
(10, 'toc toc', 'quien es', 'soy yo', 'yoquien@yo.jaj', '$2a$11$Eb/9Hu8cPn8dUiacpVLoauplsvPhRcWKaw0QLa77tjVTRPkEbn9CK', '2026-03-28 19:43:13');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
