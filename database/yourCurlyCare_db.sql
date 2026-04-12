SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

CREATE DATABASE IF NOT EXISTS `yourCurlyCare_db` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci;
USE `yourCurlyCare_db`;

DROP TABLE IF EXISTS `cuestionario`;
CREATE TABLE IF NOT EXISTS `cuestionario` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `n_pregunta` int(11) DEFAULT NULL,
  `fecha_creacion` datetime NOT NULL,
  `estado` enum('Borrador','Entregado','Sin_contestar') NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

INSERT INTO `cuestionario` (`id`, `n_pregunta`, `fecha_creacion`, `estado`) VALUES
(1, 1, '2026-04-07 16:50:19', 'Sin_contestar');

DROP TABLE IF EXISTS `pregunta`;
CREATE TABLE IF NOT EXISTS `pregunta` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_cuestionario` int(11) NOT NULL,
  `enunciado` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

INSERT INTO `pregunta` (`id`, `id_cuestionario`, `enunciado`) VALUES
(1, 1, '¿Cuánto volumen tienes en la raíz?'),
(2, 1, '¿Cuánto tiempo te dura el rizo?'),
(3, 1, '¿Cómo dirías que es tu cuero cabelludo?'),
(4, 1, '¿Se te encrespa a menudo el cabello?'),
(5, 1, '¿Cómo es el brillo de tu cabello?'),
(6, 1, '¿Cómo absorve los productos tu cabello?'),
(7, 1, '¿Tienes algún tinte o tratamiento en el pelo?'),
(8, 1, '¿Qué nivel de porosidad tiene tu pelo?'),
(9, 1, 'Sensación general'),
(10, 1, '¿Cómo queda el cabello el día después de lavarlo?'),
(11, 1, '¿Qué tipo de rizo tienes?');

DROP TABLE IF EXISTS `productoEscaner`;
CREATE TABLE IF NOT EXISTS `productoEscaner` (
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
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

INSERT INTO `productoEscaner` (`id`, `codigo_barras`, `nombre`, `marca`, `ingredientes`, `alcohol`, `silicona`, `sulfato`, `es_apto`) VALUES
(1, '8402001007903', 'Espuma Ondas Curly', 'deliplus', 'aqua, isobutane, polyquaternium-4, propane, propylene glycol, xylitylglucoside, laureth-4, parfum, sodium benzoate, anhydroxylitol, maltitol, potassium sorbate, cocamidopropyl betaine, disodium edta, panthenol, xylitol, citric acid, butane, pelvetia canaliculata extract, limonene, phenoxyethanol, linalool, citronellol, genariol, ethylhexylgrycerin', 0, 0, 0, 1),
(3, '8700216166577', 'Champú Hidrate Coconut scent', 'Herbal Essences', 'Aqua, Sodium Laureth Sulfate, Sodium Citrate, Cocamidopropyl Betaine, Sodium Xylenesulfonate, Sodium Chloride, Parfum, Sodium Benzoate, Glycerin, Citric Acid, Stearyl Alcohol, Sodium Salicylate, Guar Hydrorypropyiltrimonium Chloride, Cetyl Alcohol, Trihydroxystearin, Sodium Hydroxide Hexamethylindanopyran, Polyquatemium-6, Tetrasodium EDTA, Linalool, Linalyl Acetate, Citrus Aurantium Peel Oil Histidine, Coumarin, Limonene, Vanillin, Terpineol, Cocos Nucifera Fruit Juice', 0, 0, 1, 1),
(4, '8480000226327', 'Curl Perfect Sérum oil', 'deliplus', 'COCO-CAPRYLATE/CAPRATE, PERSEA GRATISSIMA OIL, MACADAMIA TERNIFOLIA SEED OIL, DIBUTYL ADIPATE, ARGANIA SPINOSA KERNEL OIL, GLYCINE SOJA OIL, PARFUM, TOCOPHEROL, BETA-SITOSTEROL, SQUALENE, LIMONENE, GERANIOL.', 0, 0, 0, 1),
(8, '8426827014303', 'Gel fijador fuerte', 'drn', ' Aqua (Water), Glycerin, Carbomer, Polysorbate 20, Oleo EUropaea Fruit Oil, Propylene Glycol, Aloe Ferox Leaf Extract, Hydrolyzed Wheat Gluten, Hydrolyzed Corn Protein, Hydrolyzed Rice Protein, Zea Mays Starch, Polyquaternium-7, Guar Hydroxypropyltrimonium Chloride Gluconolactone, Phenethyl Alcohol, Caprylyl Glycol, PEG-40 Hydrogenated Castor Oil, PPG-1-PEG-9 Lauryl Glycol Ether, Calcium Gluconate Ethyhexylglycerin, Calcium Titanium Borosilicate, Titanium Dioxide, Ti Oxide, Acrylates/Palmeth-25 Acrylate Copolymer PVP, Tetrasodium Glutamate Diacetate, Sodium Hydroxide, Phenoxyethanol, Sodium Benzoate, Potasium Sorbate, CI 19140, CI 42051, Citrus Aurantium Bergamia Oil, Citrus Limon Peel Oil, Hexamethylindanopyran, Limenene, Linalool, Linalyl Acetate, Pinene, Parfum (Fragrance).', 0, 0, 0, 1),
(9, '8436097097098', 'Keratine Liquide', 'Byphasse', ' Aqua (Water), Alcohol Denat., Phenoxyethanol PEG-40 Hydrogenated Castor Oil, Chlorphenesin, Polyquaternium-6, Parfum (Fragrance), Hydrolyzed Keratin, Polyquaternium-11, Glycerin, Citric Acid Potassium Sorbate, Ethyhexyalycerin Amyl Cinnamal, Linalool, Hexamethylindanopyran, Benzyl Salicylate, Citrus Limon Peel Oil Limonene, Hydroxycitronellal, CI 15985 (FD&C Yellow No. 6).', 0, 0, 0, 1),
(10, '8710919150315', 'Acondicionador de Argán', 'inecto', 'Aqua, Cetearyl Alcohol, Argania Spinosa Kernel Oil, Cetrimonium Choride, Panthenol Parfum, Phenoxyethanol, Glycerin, Benzoic Acid Dehydroacetic Acid, Citric Acid, Pantolactone, Tetramethyl Acetyloctahydronaphthalenes, Sodium Hydroxide, Hexyl Cinnanal.', 0, 0, 0, 1),
(11, '5012251010399', 'Acondicionador Tea Tree', 'Beauty formulas', 'Aqua (Water), Cetearyl Alcohol, Cetrimonium Chloride, Cetyl Alcohol, Hydrolysed Collagen, Melaleuca Alternifolia (Tea Tree) Leaf Oil, Disodium EDTA, Citric Acid, Propylene Glycol, Magnesium Nitrate, Magnesium Chloride, Triethylene Glycol, Benzyl Alcohol, Methylchloroisothiazolinone, Methylisothiazolinone, CI 42090, CI 19140.', 0, 0, 0, 1),
(12, '8420651167006', 'Champú hidratante rizos perfectos', 'Herbal', 'Aqua (Water), Sodium laureth sulfate, Cocamidopropyl betaine, Sodium chloride, Cocamide DEA, Vitis Vinifera (Grape) Seed Extract, Hydrolized silk, Pyrus malus (Apple) fruit extract, Prunus persica (Peach) fruit extract, Rubus ideaus (Raspberry) fruit extract, Carica papaya (Papaya) fruit extract, Fragaria ananassa (Strawberry) fruit extract, Cucumis sativus (Cucumber) fruit extract, Actinidia chinensis (Kiwi) fruit extract, Polyquaternium-10, Polyquaternium-7, Glycol distearate, Laureth-4, Cocamidopropylamine oxide, Parfum, Citric acid, Glycerin, Ethylhexyl methoxycinnamate, Lactic acid, Sodium benzoate, Magnesium nitrate, Magnesium chloride, Potassium sorbate, Imidazolidinyl urea, BHT, Methylchloroisothiazolinone, Methylisothiazolinone,', 0, 0, 1, 1),
(13, '8402001027185', 'Champú Anticaída-Fortificánte', 'deliplus', 'AQUA, SODIUM LAURETH SULFATE, SODIUM LAUROAMPHOACETATE, COCAMIDOPROPYL BETAINE, SALICYLIC ACID, PANAX GINSENG ROOT EXTRACT, GLYCERIN, BIOTIN, MAGNESIUM ASCORBYL PHOSPHATE, HYOROLYZED CORN PROTEIN, ADENOSINE, CARNITINE, UREA, PARFUM, CITRIC ACID, DECYL GLUCOSIDE, PENTYLENE GLYCOL, POLYQUATERNIUM-10, SODIUM CHLORIDE, PROPYLENE GLYCOL, TETRASODIUM GLUTAMATE DIACETATE, SODIUM CITRATE, SODIUM HYDROXIDE, DISODIUM PHOSPHATE, TETRAMETHYL ACETYLOCTAHYORONAPHTHALENES, HEXAMETHYLINDANOPYRAN, LIMONENE, HEXYL CINNAMAL, CITRUS LIMON PEEL OIL, BENZYL ALCOHOL, SODIUM BENZOATE, POTASSIUM SORBATE.', 0, 0, 1, 1),
(14, '8700216419826', 'Acondicionador Pro-V  repara & protege', 'Pantene ', 'Aqua, Stearyl Alcohol, Behentrimonium Methosulfate, Bis-Aminopropyl Dimethicone, Cetyl Alcohol, Isopropyl Alcohol, Parfum, Benzyl Alcohol, Oleic Acid, Sodium Benzoate. Polysorbate 20, Hexamethylindanopyran, Disodium EDTA, Panthenol, Dimethyl Phenethyl Acetate, Histidine, Citric Acid, Hexyl Cinnamal, Tetramethy Acetyloctahydronaphthalenes, Vanillin, Hydroxycitronellal Limonene, Rose Ketones', 1, 1, 0, 0),
(17, '8480000468970', 'Ondas Surfing', 'deliplus', 'AQUA, ALCOHOL DENAT., PROPANEDIOL, POLYQUATERNIUM-11, SODIUM CHLORIDE, POLYSORBATE 20, SEPIOLITE EXTRACT, GLYCERIN, CITRIC ACID, TETRASODIUM GLUTAMATE DIACETATE, ALPHA-ISOMETHYL IONONE, CITRONELLOL, HEXYL CINNAMAL, LINALOOL, PARFUM', 1, 0, 0, 0),
(18, '8411135006621', 'Champú suave Hidratación y cuidado natural', 'Giorgi', 'Aqua (Water), Sodium Lauroyl Methyl Isethionate, Cocamidopropyl Betaine, Glycerin, Parfum (Frangance), Persea Gratissima (Avocado) Fruit Extract, Hydrolyzed Wheat Potein, Hydrolyzed Soy Protein, Hydrolyzed Corn Protein, Coco Glucoside, Glyceryl Stearate, Sodium Methyl Oleoyl Taurate, Guar Hydroxypropyltrimonium Chloride, Sorbitol, PEG-150 Distearate, Sodium Chloride, Sodium Benzoate, Glyceryl Oleate, Glycol Distearate, Citric Acid, Tetrasodium Glutamate Diacetate,  Potassium Sorbate, Citronellol, Hexyl Cinnamal, Linalool.', 0, 0, 0, 1);

DROP TABLE IF EXISTS `registroEscaner`;
CREATE TABLE IF NOT EXISTS `registroEscaner` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_productoE` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `fecha_escaner` datetime NOT NULL,
  `respuesta` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

INSERT INTO `registroEscaner` (`id`, `id_productoE`, `id_usuario`, `fecha_escaner`, `respuesta`) VALUES
(1, 2, 0, '2026-04-02 12:27:56', '1'),
(2, 2, 0, '2026-04-02 13:32:57', '1'),
(3, 2, 0, '2026-04-02 13:33:15', '1'),
(4, 2, 0, '2026-04-02 18:51:20', '1'),
(5, 3, 0, '2026-04-02 21:09:54', '1'),
(6, 4, 0, '2026-04-02 21:38:42', '1'),
(7, 5, 0, '2026-04-02 22:54:42', '1'),
(8, 6, 0, '2026-04-04 18:02:13', '0'),
(9, 7, 0, '2026-04-04 18:13:54', '0'),
(10, 8, 0, '2026-04-04 20:20:21', '1'),
(11, 9, 0, '2026-04-04 20:33:29', '1'),
(12, 10, 0, '2026-04-04 20:49:33', '1'),
(13, 11, 0, '2026-04-04 20:57:32', '1'),
(14, 12, 0, '2026-04-05 13:21:28', '1'),
(15, 13, 0, '2026-04-05 17:38:17', '1'),
(16, 14, 0, '2026-04-05 18:16:13', '0'),
(17, 15, 0, '2026-04-05 18:30:16', '1'),
(18, 16, 0, '2026-04-05 18:31:50', '1'),
(19, 17, 0, '2026-04-06 19:31:13', '1'),
(20, 18, 0, '2026-04-11 11:16:28', '1');

DROP TABLE IF EXISTS `respuesta`;
CREATE TABLE IF NOT EXISTS `respuesta` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_usuario` int(11) NOT NULL,
  `id_cuestionario` int(11) NOT NULL,
  `id_pregunta` int(11) NOT NULL,
  `opcion` enum('a','b','c') NOT NULL,
  `fecha_realizacion` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

DROP TABLE IF EXISTS `resultado`;
CREATE TABLE IF NOT EXISTS `resultado` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `id_cuestionario` int(11) NOT NULL,
  `resultado_final` varchar(255) DEFAULT NULL,
  `fecha_realizacion` datetime NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE IF NOT EXISTS `usuario` (
  `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) DEFAULT NULL,
  `username` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL,
  `fecha_registro` datetime NOT NULL,
  `email_confirmado` tinyint(1) NOT NULL DEFAULT 0,
  `codigo_verificacion` varchar(6) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `username` (`username`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

INSERT INTO `usuario` (`id`, `nombre`, `apellido`, `username`, `email`, `password`, `fecha_registro`, `email_confirmado`, `codigo_verificacion`) VALUES
(1, 'Lola', 'Martinez', 'lolamar', 'lolamar@ejemplo.com', '123456', '2026-03-05 20:13:27', 0, ''),
(2, 'string', 'string', 'string', 'jii@ejemplo.com', '$2a$11$QJbtVbizLMt7.jtW5fLPCexEqmoWd3bgGPCnVH.GwOzk2WeEwcdtC', '2026-03-08 20:24:47', 0, ''),
(3, 'hola', 'holita', 'piparrita', 'yeah@example.com', '$2a$11$E1jXYZ2.fdmBwC5jOK/nm.TI2fyPL7qlgF7TnIbt26dElwPt0Ij9K', '2026-03-09 18:31:25', 0, ''),
(4, 'Susanita', 'Tiene Un', 'ratonChiquitin', 'quecome@chocolate.y', '$2a$11$mpy5Ozzv.TzQrjXxbddx2OULtOO5I0ZqaMHM.A2cu6oFqAO5pNtT6', '2026-03-12 22:12:41', 0, ''),
(7, 'ricitos', 'de', 'oro', 'delbueno@muy.bueno', '$2a$11$QnHL9OJ7hxoo8kkDBMU1Bewxaq7561BIleuUBDvZOx8ONIgO6LODi', '2026-03-15 13:14:04', 0, ''),
(8, 'bob', 'esponja', 'patricio', 'estrella@calamardo.gary', '$2a$11$ZrEqjAIs3mfGD.NJ.cK0iufSaKY/QTku4iqGEeTRY8HMBMcUWFyhm', '2026-03-25 19:20:05', 0, ''),
(9, 'hola', 'hola', 'hola', 'hola@ejemplo.com', '$2a$11$75KJgu9VJjTH6QLISpsM.eHY.riT9OBmzirFjOj21KiRT3p9A1Ut6', '2026-03-28 10:15:01', 0, ''),
(10, 'toc toc', 'quien es', 'soy yo', 'yoquien@yo.jaj', '$2a$11$Eb/9Hu8cPn8dUiacpVLoauplsvPhRcWKaw0QLa77tjVTRPkEbn9CK', '2026-03-28 19:43:13', 0, ''),
(11, 'PRUEBA', 'probando', 'prueba1', 'prueba@numero.uno', '$2a$11$rqPM360q1koM4lQXtgScT.wqg6L149wu4h4zVE9bMaMrCfriv1duS', '2026-04-09 20:40:07', 0, '749283'),
(29, 'Azahara', 'Puerto', 'azaharap.14', 'al.azahara.puerto.pulido@iesportada.org', '$2a$11$ur/5A83haV230YxNlTM1I.kZ4NXJYcyIb22eIBFX9qSbQDXB/H01W', '2026-04-10 09:52:32', 1, NULL),
(30, 'Otra ', 'Cuenta de', 'Azahara', 'azaharapuertop2@gmail.com', '$2a$11$yYkC.0gJ3NlPV89RTiGIN.orPnT6H4dnGMAQp6kapoQVGjF6xXR0K', '2026-04-10 10:13:56', 1, '522490');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
