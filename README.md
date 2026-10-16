# ClientesApi

Este documento contiene el script de base de datos en SQL Server para el proyecto **ClientesApi**, incluyendo la creación de la base de datos, tablas, restricciones y datos iniciales de prueba.

## 1. Creación de la Base de Datos

```sql
CREATE DATABASE DemoClientes
GO

USE DemoClientes
GO
```

## 2. Tabla de Clientes

La tabla `clientes` almacena la información personal de los clientes, validando que el DNI tenga exactamente 8 dígitos y sea único.

```sql
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
```

## 3. Tabla de Productos

La tabla `productos` almacena los detalles de los productos disponibles, incluyendo precio, garantía y opcionalmente un número de serie.

```sql
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
```

## 4. Consultas de Prueba

```sql
SELECT * FROM productos;
GO

SELECT * FROM clientes;
GO