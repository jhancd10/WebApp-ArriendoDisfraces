-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2021-05-19 04:40:10.915

-- tables
-- Table: cliente
CREATE TABLE cliente (
    id int  NOT NULL,
    rut varchar(12)  NOT NULL,
    nombres varchar(25)  NOT NULL,
    apellidos varchar(25)  NOT NULL,
    telefono int  NOT NULL,
    CONSTRAINT cliente_pk PRIMARY KEY  (id)
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
    id int  NOT NULL,
    observacion varchar(100)  NOT NULL,
    fecha_arriendo date  NOT NULL,
    dias_arriendo int  NOT NULL,
    cliente_id int  NOT NULL,
    disfraz_id int  NOT NULL,
    tipo_pago_id int  NOT NULL,
    CONSTRAINT servicios_pk PRIMARY KEY  (id)
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

-- foreign keys
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

-- End of file.

