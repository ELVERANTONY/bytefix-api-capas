-- ============================================================
-- ByteFix - Taller de soporte técnico
-- Laboratorio 13 - API en capas con DTOs y AutoMapper
-- Motor: SQL Server 2022 (Database First)
-- ============================================================
-- Ejecutar con:
--   sqlcmd -S localhost,1433 -U sa -P "TU_CLAVE" -C -i db/ByteFixDB.sql
-- ============================================================

IF DB_ID('ByteFixDB') IS NULL
BEGIN
    CREATE DATABASE ByteFixDB;
END
GO

USE ByteFixDB;
GO

-- Elimina las tablas en orden hijo -> padre
IF OBJECT_ID('dbo.Reparaciones', 'U') IS NOT NULL DROP TABLE dbo.Reparaciones;
IF OBJECT_ID('dbo.Tecnicos', 'U') IS NOT NULL DROP TABLE dbo.Tecnicos;
GO

-- ------------------------------------------------------------
-- Tabla padre: Tecnicos
-- ------------------------------------------------------------
CREATE TABLE dbo.Tecnicos (
    TecnicoId     INT           IDENTITY(1,1) NOT NULL,
    Documento     VARCHAR(15)   NOT NULL,
    Nombre        VARCHAR(80)   NOT NULL,
    Apellido      VARCHAR(80)   NOT NULL,
    Email         VARCHAR(120)  NOT NULL,
    Telefono      VARCHAR(20)   NULL,
    Especialidad  VARCHAR(60)   NOT NULL,
    Activo        BIT           NOT NULL CONSTRAINT DF_Tecnicos_Activo DEFAULT (1),
    FechaIngreso  DATETIME2(0)  NOT NULL CONSTRAINT DF_Tecnicos_FechaIngreso DEFAULT (SYSDATETIME()),
    CONSTRAINT PK_Tecnicos PRIMARY KEY CLUSTERED (TecnicoId),
    CONSTRAINT UQ_Tecnicos_Documento UNIQUE (Documento),
    CONSTRAINT UQ_Tecnicos_Email UNIQUE (Email)
);
GO

-- ------------------------------------------------------------
-- Tabla hija: Reparaciones (1:N con Tecnicos)
-- ------------------------------------------------------------
CREATE TABLE dbo.Reparaciones (
    ReparacionId   INT           IDENTITY(1,1) NOT NULL,
    TecnicoId      INT           NOT NULL,
    Serie          VARCHAR(30)   NOT NULL,
    Equipo         VARCHAR(100)  NOT NULL,
    Marca          VARCHAR(50)   NOT NULL,
    Modelo         VARCHAR(50)   NULL,
    FallaReportada VARCHAR(200)  NOT NULL,
    Prioridad      VARCHAR(10)   NOT NULL CONSTRAINT DF_Reparaciones_Prioridad DEFAULT ('Media'),
    Estado         VARCHAR(15)   NOT NULL CONSTRAINT DF_Reparaciones_Estado DEFAULT ('Pendiente'),
    Costo          DECIMAL(10,2) NOT NULL CONSTRAINT DF_Reparaciones_Costo DEFAULT (0),
    FechaIngreso   DATETIME2(0)  NOT NULL CONSTRAINT DF_Reparaciones_FechaIngreso DEFAULT (SYSDATETIME()),
    FechaEntrega   DATETIME2(0)  NULL,
    CONSTRAINT PK_Reparaciones PRIMARY KEY CLUSTERED (ReparacionId),
    CONSTRAINT UQ_Reparaciones_Serie UNIQUE (Serie),
    CONSTRAINT CK_Reparaciones_Prioridad CHECK (Prioridad IN ('Baja', 'Media', 'Alta')),
    CONSTRAINT CK_Reparaciones_Estado CHECK (Estado IN ('Pendiente', 'EnProceso', 'Entregado')),
    CONSTRAINT CK_Reparaciones_Costo CHECK (Costo >= 0),
    CONSTRAINT FK_Reparaciones_Tecnicos
        FOREIGN KEY (TecnicoId) REFERENCES dbo.Tecnicos (TecnicoId)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
);
GO

CREATE INDEX IX_Reparaciones_TecnicoId ON dbo.Reparaciones (TecnicoId);
CREATE INDEX IX_Reparaciones_Estado ON dbo.Reparaciones (Estado);
GO

-- ------------------------------------------------------------
-- Datos de ejemplo: 5 técnicos (uno inactivo)
-- ------------------------------------------------------------
SET IDENTITY_INSERT dbo.Tecnicos ON;

INSERT INTO dbo.Tecnicos
    (TecnicoId, Documento, Nombre, Apellido, Email, Telefono, Especialidad, Activo, FechaIngreso)
VALUES
    (1, '45678912', 'Luis',   'Mendoza',  'luis.mendoza@bytefix.pe',   '987111222', 'Hardware y laptops',       1, '2023-02-15 09:00:00'),
    (2, '41234567', 'Ana',    'Rojas',    'ana.rojas@bytefix.pe',      '987333444', 'Redes y servidores',       1, '2023-06-01 09:00:00'),
    (3, '40123456', 'Marco',  'Díaz',     'marco.diaz@bytefix.pe',     '987555666', 'Software y sistemas',      1, '2024-01-10 09:00:00'),
    (4, '43567890', 'Sofía',  'Vargas',   'sofia.vargas@bytefix.pe',   '987777888', 'Impresoras y periféricos', 1, '2024-08-20 09:00:00'),
    (5, '39876543', 'Pedro',  'Ccahuana', 'pedro.ccahuana@bytefix.pe', NULL,        'Recuperación de datos',    0, '2022-11-05 09:00:00');

SET IDENTITY_INSERT dbo.Tecnicos OFF;
GO

-- ------------------------------------------------------------
-- Datos de ejemplo: 10 reparaciones
-- ------------------------------------------------------------
SET IDENTITY_INSERT dbo.Reparaciones ON;

INSERT INTO dbo.Reparaciones
    (ReparacionId, TecnicoId, Serie, Equipo, Marca, Modelo, FallaReportada, Prioridad, Estado, Costo, FechaIngreso, FechaEntrega)
VALUES
    (1,  1, 'BF-2026-001', 'Laptop',    'Lenovo',    'ThinkPad E14', 'No enciende y el cargador no carga',        'Alta',  'Pendiente',  180.00, '2026-10-28 10:15:00', NULL),
    (2,  2, 'BF-2026-002', 'Servidor',  'Dell',      'PowerEdge T40', 'No responde en la red local',               'Alta',  'EnProceso',  450.00, '2026-10-29 11:30:00', NULL),
    (3,  3, 'BF-2026-003', 'PC Escritorio', 'HP',    'ProDesk 400',   'Pantalla azul al iniciar Windows',          'Media', 'EnProceso',  220.00, '2026-10-30 09:05:00', NULL),
    (4,  4, 'BF-2026-004', 'Impresora',  'Epson',    'L3250',         'Imprime con líneas y no escanea',           'Baja',  'Entregado',   95.00, '2026-10-20 08:45:00', '2026-10-22 16:20:00'),
    (5,  1, 'BF-2026-005', 'Laptop',     'Asus',     'VivoBook 15',   'Teclado derrama líquido y no responde',     'Alta',  'Entregado',  310.00, '2026-10-15 14:10:00', '2026-10-18 12:00:00'),
    (6,  3, 'BF-2026-006', 'PC Escritorio', 'Armada', NULL,           'Reinicia solo después de 10 minutos',       'Media', 'Pendiente',  150.00, '2026-11-02 10:00:00', NULL),
    (7,  2, 'BF-2026-007', 'Switch',     'TP-Link',   'TL-SG1024',     'Puertos 12 y 13 sin enlace',                'Baja',  'Pendiente',   80.00, '2026-11-03 15:25:00', NULL),
    (8,  4, 'BF-2026-008', 'Monitor',    'Samsung',   'LS24R350',      'Enciende pero no da imagen',                'Media', 'EnProceso',  170.00, '2026-11-04 09:40:00', NULL),
    (9,  1, 'BF-2026-009', 'Laptop',     'Apple',     'MacBook Air M1', 'Batería se agota en 20 minutos',           'Alta',  'Pendiente',  390.00, '2026-11-05 11:50:00', NULL),
    (10, 3, 'BF-2026-010', 'PC Escritorio', 'Lenovo', 'ThinkCentre M70', 'No reconoce el disco duro',               'Media', 'Entregado',  260.00, '2026-10-25 13:15:00', '2026-10-27 17:30:00');

SET IDENTITY_INSERT dbo.Reparaciones OFF;
GO

-- ------------------------------------------------------------
-- Verificación
-- ------------------------------------------------------------
SELECT 'Tecnicos' AS Tabla, COUNT(*) AS Registros FROM dbo.Tecnicos
UNION ALL
SELECT 'Reparaciones', COUNT(*) FROM dbo.Reparaciones;
GO
