-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2021-05-19 04:40:10.915

-- tables
-- Table: cliente
CREATE TABLE cliente (
    id int IDENTITY(1,1) PRIMARY KEY,
    rut varchar(12)  NOT NULL,
    nombres varchar(25)  NOT NULL,
    apellidos varchar(25)  NOT NULL,
    telefono int  NOT NULL
);

-- Table: disfraz
CREATE TABLE disfraz (
    id int  NOT NULL,
    codigo varchar(10)  NOT NULL,
    nombre varchar(20)  NOT NULL,
    descripcion varchar(100)  NOT NULL,
    cantidad int  NOT NULL,
    tipo_disfraz_id int  NOT NULL,
    CONSTRAINT disfraz_pk PRIMARY KEY  (id)
);

-- Table: servicios
CREATE TABLE servicios (
    id int IDENTITY(1,1) PRIMARY KEY,
    observacion varchar(100)  NOT NULL,
    fecha_arriendo date  NOT NULL,
    dias_arriendo int  NOT NULL,
    cliente_id int  NOT NULL,
    disfraz_id int  NOT NULL,
    tipo_pago_id int  NOT NULL
);

-- Table: tipo_disfraz
CREATE TABLE tipo_disfraz (
    id int  NOT NULL,
    codigo varchar(10)  NOT NULL,
    nombre varchar(20)  NOT NULL,
    CONSTRAINT tipo_disfraz_pk PRIMARY KEY  (id)
);

-- Table: tipo_pago
CREATE TABLE tipo_pago (
    id int  NOT NULL,
    codigo varchar(10)  NOT NULL,
    nombre varchar(20)  NOT NULL,
    descripcion varchar(50)  NOT NULL,
    CONSTRAINT tipo_pago_pk PRIMARY KEY  (id)
);

-- Table: usuario
CREATE TABLE usuario (
    id int  NOT NULL,
    username varchar(20)  NOT NULL,
    password varchar(20)  NOT NULL,
    fecha_creacion date  NOT NULL,
    CONSTRAINT usuario_pk PRIMARY KEY  (id)
);

-- Foreign Keys

-- Reference: disfraz_tipo_disfraz (table: disfraz)
ALTER TABLE disfraz ADD CONSTRAINT disfraz_tipo_disfraz
    FOREIGN KEY (tipo_disfraz_id)
    REFERENCES tipo_disfraz (id);

-- Reference: servicios_cliente (table: servicios)
ALTER TABLE servicios ADD CONSTRAINT servicios_cliente
    FOREIGN KEY (cliente_id)
    REFERENCES cliente (id);

-- Reference: servicios_disfraz (table: servicios)
ALTER TABLE servicios ADD CONSTRAINT servicios_disfraz
    FOREIGN KEY (disfraz_id)
    REFERENCES disfraz (id);

-- Reference: servicios_tipo_pago (table: servicios)
ALTER TABLE servicios ADD CONSTRAINT servicios_tipo_pago
    FOREIGN KEY (tipo_pago_id)
    REFERENCES tipo_pago (id);

-- Structure

--User Admin
INSERT INTO[dbo].[usuario] (id, username, password, fecha_creacion)
VALUES(1, 'admin', 'admin', CONVERT(DATETIME,'08-10-21 10:34:09 PM',1));

--Tipo Disfraces
INSERT INTO[dbo].[tipo_disfraz]
VALUES 
(1, 'N', 'Nuevo'),
(2, 'S', 'Segunda Mano'),
(3, 'D', 'Defectuoso'),
(4, 'A', 'Americana');

--Tipo Pago
INSERT INTO[dbo].[tipo_pago]
VALUES 
(1, 'E', 'Efectivo', ''),
(3, 'TC', 'Tarjeta Credito', ''), 
(2, 'TD', 'Tarjeta Debito', '');

--Disfraces
INSERT INTO[dbo].[disfraz]
VALUES 
(1, 'SPD', 'Spiderman', '', 5, 2),
(2, 'DN', 'Dinosaurio', '', 5, 4),
(3, 'M', 'Momia', '', 3, 3),
(4, 'J', 'Jefe: Squid Game', '', 10, 1);


-- Alter Structure: New Functions

-- Table: cliente
ALTER TABLE [dbo].[cliente]
ADD total_servicios INT NOT NULL;

ALTER TABLE [dbo].[cliente]
ADD tipo_cliente_id INT NOT NULL;

-- Table: tipo_cliente
CREATE TABLE tipo_cliente (
    id int  NOT NULL,
    codigo varchar(10)  NOT NULL,
    nombre varchar(20)  NOT NULL,
    arriendos_permitidos INT NOT NULL,
    CONSTRAINT tipo_cliente_pk PRIMARY KEY  (id)
);

INSERT INTO[dbo].[tipo_cliente]
VALUES 
(1, 'N', 'Normal', 1),
(2, 'P', 'Premium', 3),
(3, 'E', 'Empresarial', 10);

-- Reference: cliente_tipo_cliente (table: cliente)
ALTER TABLE cliente ADD CONSTRAINT cliente_tipo_cliente
    FOREIGN KEY (tipo_cliente_id)
    REFERENCES tipo_cliente (id);

-- Table: servicios
ALTER TABLE [dbo].[servicios]
ADD cantidad INT NOT NULL;

ALTER TABLE [dbo].[servicios]
ADD estado BIT NOT NULL;

-- End of file.