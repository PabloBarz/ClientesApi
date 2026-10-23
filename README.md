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
```


# API REST - C# (.NET 10) -> MS SQL SERVER

## Clientes
* GET
* POST
* PUT
* DELETE (Protegido - Solo Admin)

## Proteger API REST -> Usuario
JWT (Json Web Token)
* Crear usuario (contraseña encriptada con BCrypt)
* Iniciar sesión (login) -> TOKEN (autorización)
* Consumir API REST

---

## *** Test ***

### 1. GET - Listar Clientes (Sin Autorización)
* **URL:** `https://localhost:7292/api/clientes`
* **Método:** GET
* **Resultado:** 401 Unauthorized / Sin autorización

### 2. POST - Registro de Usuario
* **URL:** `https://localhost:7292/api/auth/registrar`
* **Método:** POST
* **Body (JSON):**
```json
{
  "username": "´Pablo",
  "password": "clave1234"
}
```
* **Resultado:** 
```json
{ "mensaje": "Usuario registrado correctamente" }
```

### 3. POST - Iniciar Sesión (Login)
* **URL:** `https://localhost:7292/api/auth/login`
* **Método:** POST
* **Body (JSON):**
```json
{
  "username": "Pablo",
  "password": "clave1234"
}
```
* **Resultado:** Recibe el JWT Token junto a su fecha de expiración.
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expira": "2026-10-23T15:41:05.9780016Z"
}
```

### 4. Asignación de Rol en Base de Datos (Admin)
Para ejecutar acciones administrativas como la eliminación, se actualiza el rol del usuario directamente en la base de datos de `Usuario` a `Admin`:
```sql
UPDATE Usuario SET rol = 'Admin' WHERE id = 2
GO
```

### 5. DELETE - Eliminar Cliente por ID (Con Autorización Admin)
* **URL:** `https://localhost:7292/api/clientes/1`
* **Método:** DELETE
* **Headers:** `Authorization: Bearer <TOKEN_DE_LOGIN>`
* **Resultado:** 204 No Content (Cliente eliminado con éxito)
