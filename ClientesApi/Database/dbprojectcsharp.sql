CREATE DATABASE DemoClientes
GO

USE DemoClientes
GO
CREATE TABLE clientes
(
    id          INT IDENTITY(1,1) PRIMARY KEY,
    apellidos   NVARCHAR(70)    NOT NULL,
    nombres     NVARCHAR(70)    NOT NULL,
    telefono    NVARCHAR(9)     NOT NULL,
    correo      NVARCHAR(150)   NOT NULL,
    dni         CHAR(8)         NOT NULL,
    direccion   NVARCHAR(150)   NOT NULL,
    CONSTRAINT uk_dni_cli UNIQUE (dni),
    CONSTRAINT ck_dni_cli CHECK (dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
)
GO

INSERT INTO clientes 
    (apellidos, nombres, telefono, correo, dni, direccion) VALUES
    ('Torres Peña', 'María', '956111222', 'maria@gmail.com', '11221122', 'Chincha Alta'),
    ('Carbajal Sotelo', 'Rocío', '956222333', 'rocio@gmail.com', '33334444', 'Pueblo Nuevo')
GO
CREATE TABLE Productos
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(100) NOT NULL,
    precio DECIMAL(7,2) NOT NULL,
    garantia TINYINT NOT NULL,
    numserie NVARCHAR(40) NULL 
)
GO

INSERT INTO productos
    (nombre, descripcion, precio, garantia, numserie) VALUES
    ('Teclado', 'Logitech negro USB', 75, 12, 'ABC-12345'),
    ('Mouse', 'Genius USB', 40, 6, NULL)
GO

CREATE TABLE proveedores
(
    id          INT IDENTITY(1,1) PRIMARY KEY,
    razon_social NVARCHAR(120) NOT NULL,
    ruc         CHAR(11) NOT NULL,
    telefono    NVARCHAR(9) NOT NULL,
    correo      NVARCHAR(150) NOT NULL,
    direccion   NVARCHAR(150) NOT NULL,
    CONSTRAINT uk_ruc_pro UNIQUE (ruc),
    CONSTRAINT ck_ruc_pro CHECK (
        ruc NOT LIKE '%[^0-9]%'
        AND LEN(ruc) = 11
    )
)
GO

INSERT INTO proveedores
    (razon_social, ruc, telefono, correo, direccion)
VALUES
    ('Tecnología Perú SAC', '20123456789', '956333444',
     'ventas@tecnologiaperu.com', 'Lima'),

    ('Distribuidora del Sur SAC', '20456789123', '956444555',
     'contacto@distribuidorasur.com', 'Chincha Alta'),

    ('Importaciones Digitales EIRL', '20678912345', '956555666',
     'ventas@importacionesdigitales.com', 'Ica')
GO