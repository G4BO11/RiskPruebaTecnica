# Supermercado Antojitos - Sistema de Ventas

Sistema de control de ventas desarrollado para el Supermercado Antojitos, un negocio con más de 10 años en el mercado ubicado en el barrio Vivir Bueno. Soluciona el problema de manejo manual de ventas que generaba descuadres y pérdida de dinero.

## 🎯 Objetivo

Desarrollar un aplicativo que controle las ventas diarias, gestione el inventario de productos y permita identificar y registrar clientes para brindar atención personalizada en el futuro.

## 🛠️ Tecnologías Utilizadas

- **.NET 8** - Backend
- **Entity Framework Core** - ORM
- **PostgreSQL** - Base de datos
- **ASP.NET Identity** - Autenticación y autorización
- **Docker** - Contenedorización de la base de datos
- **MVC Pattern** - Arquitectura

## 🏗️ Arquitectura

```
RiskPruebaTecnica/
├── Data/
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
├── Models/
│   └── Entities/
├── Repositories/
├── Services/
├── Controllers/
├── Views/
└── Program.cs
```

## 📋 Funcionalidades Principales

### 👤 Gestión de Usuarios
- Registro e inicio de sesión para cajeros
- Autenticación con ASP.NET Identity
- Usuario administrador por defecto: `admin@supermercado.com`

### 👥 Gestión de Clientes
- Registro obligatorio de clientes para cada venta
- Datos requeridos: número de identificación, nombre, apellido, dirección, teléfono, correo
- Búsqueda por número de identificación
- Registro automático si el cliente no existe

### 🛍️ Gestión de Productos
- CRUD completo de productos
- Campos: código, nombre, valor unitario, unidades existentes
- Control de inventario automático
- Validación de stock antes de la venta

### 💰 Sistema de Ventas
- Registro de ventas con múltiples productos
- Fecha automática del sistema
- Cálculo automático de subtotales y total
- Descuento automático de inventario
- Detalle completo de cada venta

### 📊 Reportes
- Generación de reportes en Excel/PDF
- Reportes disponibles:
  - Diarios
  - Semanales
  - Mensuales
  - Anuales

## 🗄️ Modelo de Base de Datos

### Entidades Principales

- **Cliente**: Información del cliente
- **Producto**: Catálogo de productos
- **Venta**: Encabezado de venta
- **DetalleVenta**: Detalle de productos por venta
- **IdentityUser**: Usuarios del sistema (cajeros)

### Relaciones
- Cliente → Ventas (1:N)
- Usuario → Ventas (1:N)
- Venta → DetalleVenta (1:N)
- Producto → DetalleVenta (1:N)

## 🚀 Instalación y Configuración

### Prerrequisitos
- .NET 8 SDK
- Docker y Docker Compose
- IDE (Visual Studio, VS Code, Rider)

### Pasos de Instalación

1. **Clonar el repositorio**
   ```bash
   git clone [url-del-repositorio]
   cd RiskPruebaTecnica
   ```

2. **Levantar la base de datos con Docker**
   ```bash
   docker-compose up -d
   ```

3. **Configurar la cadena de conexión**
   
   En `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=supermercado_antojitos;Username=postgres;Password=postgres123"
     }
   }
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```

5. **Acceder a la aplicación**
   - URL: `https://localhost:5001`
   - Usuario admin: `admin@supermercado.com`
   - Contraseña: `Admin123!`

## 🐳 Docker

El proyecto incluye configuración Docker para PostgreSQL:

```yaml
version: '3.8'
services:  
  database:
    image: postgres:14
    restart: always
    ports:
      - '5432:5432'
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres123
      POSTGRES_DB: supermercado_antojitos
```

## 🔧 Configuración de Identity

- Contraseñas mínimo 6 caracteres
- Requiere mayúsculas y minúsculas
- Email único obligatorio
- Bloqueo después de 5 intentos fallidos
- Sesión de 30 minutos con extensión automática

## 📦 Datos de Prueba

El sistema incluye datos de prueba que se cargan automáticamente:

### Usuario Administrador
- Email: `admin@supermercado.com`
- Contraseña: `Admin123!`

### Productos de Prueba
- Arroz Diana 1kg - $3,500 (100 unidades)
- Leche Alpina 1L - $4,200 (50 unidades)
- Pan Tajado Bimbo - $2,800 (30 unidades)

## 🔄 Flujo de Trabajo de Ventas

1. **Iniciar sesión** como cajero
2. **Iniciar venta** - ingresar número de identificación del cliente
3. **Registrar cliente** (si no existe)
4. **Agregar productos** - seleccionar productos y cantidades
5. **Verificar total** - revisar detalle de la venta
6. **Confirmar venta** - procesar y actualizar inventario

## 📝 Notas de Desarrollo

- Utiliza Repository Pattern para acceso a datos
- Implementa Service Layer para lógica de negocio
- Validaciones tanto en cliente como servidor
- Manejo de errores centralizado
- Código limpio siguiendo principios SOLID

## 🤝 Contribución

Este proyecto fue desarrollado como prueba técnica para Risk. Para contribuir:

1. Fork el proyecto
2. Crear rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Crear Pull Request