# BaseDeDatosSQL

# Gestor de Conexiones a Bases de Datos

Este es un sistema gestor de base de datos en C# para Windows Forms que permite conectarse a diferentes motores de bases de datos, incluyendo:

- **SQL Server**
- **MySQL**
- **PostgreSQL**
- **Oracle**
- **Firebird**

## 📌 Características
- Conexión a múltiples bases de datos.
- Exploración de servidores y bases de datos disponibles.
- Configuración manual y automática de conexiones.
- Soporte para conexiones remotas y locales.

## 🚀 Instalación
1. Clona este repositorio:
   ```bash
   git clone https://github.com/tuusuario/tu-repositorio.git
   ```
2. Abre el proyecto en **Visual Studio**.
3. Instala los paquetes NuGet necesarios:
   ```bash
   Install-Package MySql.Data
   Install-Package Npgsql
   Install-Package System.Data.SqlClient
   Install-Package Oracle.ManagedDataAccess
   Install-Package FirebirdSql.Data.FirebirdClient
   ```
4. Compila y ejecuta el proyecto.

## ⚙️ Configuración de Conexión
Dependiendo del motor de base de datos, la conexión se configura de la siguiente manera:

### 🔹 **SQL Server** (Local y Remoto)
```csharp
string conexion = "Server=SERVIDOR,1433;Database=master;User Id=USUARIO;Password=CONTRASEÑA;";
```
Si es una instancia con nombre, usa:
```csharp
string conexion = "Server=SERVIDOR\INSTANCIA;Database=master;User Id=USUARIO;Password=CONTRASEÑA;";
```

### 🔹 **MySQL**
```csharp
string conexion = "Server=SERVIDOR;User ID=USUARIO;Password=CONTRASEÑA;Database=INFORMACIÓN_SCHEMA;";
```

### 🔹 **PostgreSQL**
```csharp
string conexion = "Host=SERVIDOR;Username=USUARIO;Password=CONTRASEÑA;Database=postgres;";
```

### 🔹 **Oracle**
```csharp
string conexion = "Data Source=SERVIDOR:1521/NOMBRE_DB;User Id=USUARIO;Password=CONTRASEÑA;";
```

### 🔹 **Firebird**
```csharp
string conexion = "DataSource=SERVIDOR;Database=RUTA_BD.FDB;User=USUARIO;Password=CONTRASEÑA;Charset=UTF8;";
```

## 🛠 Tecnologías Utilizadas
- **C# (Windows Forms)**
- **ADO.NET** para manejo de bases de datos
- **Paquetes NuGet** para conexión con cada SGBD

## 📜 Licencia
Este proyecto está bajo la licencia [MIT](LICENSE).

