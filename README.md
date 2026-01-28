# JA Seguro que Vuelas – API

## Descripción general
**JA Seguro que Vuelas – API** es una API REST desarrollada en **ASP.NET Core** como parte de un proyecto escolar.  
El objetivo de este proyecto es simular el funcionamiento del backend de una **agencia de viajes digital**, permitiendo gestionar solicitudes de contacto de usuarios interesados en opciones de vuelo.

La API funciona como intermediaria entre una página web y una base de datos MongoDB, procesando la información enviada por los usuarios y almacenándola de manera estructurada para su posterior consulta o seguimiento.

---

## Objetivo del proyecto
El proyecto busca aplicar conocimientos de:
- Desarrollo de APIs REST
- Arquitectura cliente-servidor
- Manejo de bases de datos NoSQL (MongoDB)
- Separación de responsabilidades (Controllers, Services, Models)
- Buenas prácticas en el desarrollo backend

Todo esto enfocado a un caso de uso realista dentro del contexto de una agencia de viajes.

---

## Funcionalidades principales
- Recepción de solicitudes de contacto desde una página web
- Registro de datos del usuario (nombre, correo, teléfono, origen y destino del viaje)
- Almacenamiento de la información en MongoDB Atlas
- Comunicación mediante endpoints REST
- Manejo de errores y respuestas HTTP adecuadas
- Health check de la API y conexión a MongoDB

---

## Tecnologías utilizadas
- **ASP.NET Core 8.0**
- **C#**
- **MongoDB Atlas** (Base de datos NoSQL en la nube)
- **MongoDB.Driver** (Driver oficial de MongoDB para .NET)
- **Swagger (OpenAPI)** para documentación y pruebas de la API

---

## Arquitectura del sistema
El sistema sigue una arquitectura básica cliente-servidor:

- **Frontend**: Página web que consume la API
- **Backend**: API REST en ASP.NET Core
- **Base de datos**: MongoDB Atlas (NoSQL) para la persistencia de datos

---

## Base de datos
La información se almacena en **MongoDB Atlas**, una base de datos NoSQL en la nube.  
El acceso a los datos se realiza mediante el **MongoDB Driver para .NET**, lo que permite:
- Escalabilidad horizontal
- Flexibilidad en el esquema de datos
- Almacenamiento de documentos JSON
- Consultas eficientes y rápidas

---

## Configuración de rutas y controladores

### A) Explicación de la estructura

El proyecto sigue una arquitectura basada en controladores de ASP.NET Core, donde se aplica una separación clara de responsabilidades:

#### Estructura de carpetas

```
API_JASeguroVuelas/
├── Controllers/          # Controladores que manejan las rutas y lógica de endpoints
│   ├── HealthController.cs
│   └── ContactoController.cs
├── Models/               # Modelos de datos (DTOs y entidades)
│   ├── Contacto.cs
│   └── MongoDBSettings.cs
├── Services/             # Servicios de lógica de negocio
│   └── ContactoService.cs
├── Program.cs            # Configuración principal de la aplicación
└── appsettings.json      # Configuración de la aplicación (incluye connection string de MongoDB)
```

#### Responsabilidades de cada componente

**Controllers/**
- **Responsabilidad**: Manejar las peticiones HTTP entrantes, validar datos, invocar lógica de negocio y retornar respuestas HTTP apropiadas.
- **Por qué separar**: Los controladores actúan como intermediarios entre las rutas HTTP y la lógica de negocio. Esta separación permite:
  - Reutilizar lógica de negocio en diferentes contextos
  - Facilitar pruebas unitarias
  - Mantener código organizado y mantenible
  - Seguir el patrón MVC (Model-View-Controller)

**Models/**
- **Responsabilidad**: Definir las estructuras de datos que se utilizan en la API (entidades, DTOs, requests/responses).
- **Por qué separar**: Los modelos representan el contrato de datos de la API. Separarlos permite:
  - Centralizar definiciones de datos
  - Facilitar validaciones
  - Mejorar la documentación automática con Swagger

**Program.cs**
- **Responsabilidad**: Configurar servicios, middleware y registrar rutas de controladores.
- **Por qué centralizar**: ASP.NET Core utiliza el patrón de configuración centralizada donde `Program.cs` es el punto de entrada que:
  - Registra servicios en el contenedor de dependencias
  - Configura el pipeline de middleware
  - Mapea automáticamente los controladores a rutas

### B) Paso a paso técnico

#### 1. Creación de modelos de datos

Primero se definen los modelos que representarán los datos:

**Archivo**: `Models/Contacto.cs`

```csharp
public class Contacto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string Origen { get; set; }
    public string Destino { get; set; }
    public DateTime FechaCreacion { get; set; }
}

public class ContactoRequest
{
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string Origen { get; set; }
    public string Destino { get; set; }
}
```

**Explicación**: 
- `Contacto`: Representa la entidad completa con ID y fecha de creación
- `ContactoRequest`: DTO (Data Transfer Object) para recibir datos del cliente sin campos internos

#### 2. Creación de controladores

**Archivo**: `Controllers/HealthController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
}
```

**Explicación**:
- `[ApiController]`: Atributo que habilita comportamientos de API (validación automática, binding de modelos)
- `[Route("api/[controller]")]`: Define la ruta base. `[controller]` se reemplaza automáticamente por el nombre del controlador sin "Controller"
  - Resultado: `/api/health`
- `[HttpGet]`: Especifica que este método responde a peticiones GET
- `ControllerBase`: Clase base para controladores de API (sin soporte de vistas)

**Archivo**: `Controllers/ContactoController.cs`

```csharp
[ApiController]
[Route("api/[controller]")]
public class ContactoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() { ... }

    [HttpGet("{id}")]
    public IActionResult GetById(int id) { ... }

    [HttpPost]
    public IActionResult Post([FromBody] ContactoRequest request) { ... }
}
```

**Explicación**:
- `[HttpGet]`: Ruta `/api/contacto` - Obtiene todos los contactos
- `[HttpGet("{id}")]`: Ruta `/api/contacto/{id}` - Obtiene un contacto específico
  - `{id}` es un parámetro de ruta que se mapea al parámetro del método
- `[HttpPost]`: Ruta `/api/contacto` - Crea un nuevo contacto
  - `[FromBody]`: Indica que los datos vienen en el cuerpo de la petición (JSON)

#### 3. Registro de controladores en Program.cs

**Archivo**: `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de controladores
builder.Services.AddControllers();

var app = builder.Build();

// Configurar pipeline HTTP
app.UseHttpsRedirection();
app.UseAuthorization();

// Mapear controladores a rutas
app.MapControllers();

app.Run();
```

**Explicación paso a paso**:

1. **`builder.Services.AddControllers()`**: 
   - Registra los servicios necesarios para el sistema de controladores
   - Habilita el descubrimiento automático de controladores
   - Configura el formateo JSON por defecto

2. **`app.MapControllers()`**:
   - Escanea todos los controladores en el proyecto
   - Registra automáticamente las rutas definidas con `[Route]` y `[HttpGet]`, `[HttpPost]`, etc.
   - Conecta las rutas HTTP con los métodos de los controladores

**Flujo de una petición**:
```
Cliente → HTTP GET /api/health 
       → ASP.NET Core Routing
       → HealthController.Get()
       → Retorna respuesta JSON
       → Cliente recibe respuesta
```

### C) Endpoints implementados

#### 1. Health Check

**Método HTTP**: `GET`  
**Ruta**: `/api/health`  
**Qué hace**: Verifica el estado de salud de la API, útil para monitoreo y verificación de disponibilidad.

**Ejemplo de respuesta JSON**:
```json
{
  "status": "healthy",
  "timestamp": "2024-01-15T10:30:00Z",
  "service": "JA Seguro que Vuelas API",
  "version": "1.0.0"
}
```

**Código HTTP**: `200 OK`

---

#### 2. Obtener todos los contactos

**Método HTTP**: `GET`  
**Ruta**: `/api/contacto`  
**Qué hace**: Retorna una lista con todas las solicitudes de contacto registradas en el sistema.

**Ejemplo de respuesta JSON**:
```json
[
  {
    "id": "69798dbb637ba9b81c7a4b87",
    "nombre": "Juan Pérez",
    "correo": "juan@example.com",
    "telefono": "555-1234",
    "origen": "Ciudad de México",
    "destino": "Cancún",
    "fechaCreacion": "2024-01-15T10:00:00Z"
  },
  {
    "id": "69798dbb637ba9b81c7a4b88",
    "nombre": "María García",
    "correo": "maria@example.com",
    "telefono": "555-5678",
    "origen": "Guadalajara",
    "destino": "Los Cabos",
    "fechaCreacion": "2024-01-15T11:00:00Z"
  }
]
```

**Código HTTP**: `200 OK`

---

#### 3. Obtener contacto por ID

**Método HTTP**: `GET`  
**Ruta**: `/api/contacto/{id}`  
**Qué hace**: Retorna la información de un contacto específico identificado por su ID.

**Parámetros**:
- `id` (en la ruta): ID del contacto (ObjectId de MongoDB como string)

**Ejemplo de petición**: `GET /api/contacto/69798dbb637ba9b81c7a4b87`

**Ejemplo de respuesta JSON** (éxito):
```json
{
  "id": "69798dbb637ba9b81c7a4b87",
  "nombre": "Juan Pérez",
  "correo": "juan@example.com",
  "telefono": "555-1234",
  "origen": "Ciudad de México",
  "destino": "Cancún",
  "fechaCreacion": "2024-01-15T10:00:00Z"
}
```

**Código HTTP**: `200 OK` (éxito) o `404 Not Found` (no encontrado)

**Ejemplo de respuesta JSON** (error):
```json
{
  "message": "No se encontró un contacto con ID 999"
}
```

---

#### 4. Crear nuevo contacto

**Método HTTP**: `POST`  
**Ruta**: `/api/contacto`  
**Qué hace**: Crea una nueva solicitud de contacto con los datos proporcionados por el usuario.

**Cuerpo de la petición** (JSON):
```json
{
  "nombre": "Ana López",
  "correo": "ana@example.com",
  "telefono": "555-9012",
  "origen": "Monterrey",
  "destino": "Puerto Vallarta"
}
```

**Ejemplo de respuesta JSON**:
```json
{
  "id": "69798dbb637ba9b81c7a4b87",
  "nombre": "Ana López",
  "correo": "ana@example.com",
  "telefono": "555-9012",
  "origen": "Monterrey",
  "destino": "Puerto Vallarta",
  "fechaCreacion": "2024-01-15T12:00:00Z"
}
```

**Código HTTP**: `201 Created` (éxito) o `400 Bad Request` (datos inválidos)

**Headers de respuesta**:
- `Location`: `/api/contacto/3` (URL del recurso creado)

**Ejemplo de respuesta JSON** (error de validación):
```json
{
  "message": "Todos los campos son requeridos"
}
```

### D) Cómo probarlos

#### Requisitos previos

- .NET 8.0 SDK instalado
- Editor de código (Visual Studio, VS Code, Rider, etc.)
- Navegador web o herramienta para hacer peticiones HTTP (Postman, curl, etc.)

#### Paso 1: Levantar el servidor

Abre una terminal en la raíz del proyecto y ejecuta:

```bash
cd API_JASeguroVuelas
dotnet run
```

**Salida esperada**:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5019
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

**URL base**: `http://localhost:5019` (o la que se muestre en tu terminal)

#### Paso 2: Acceder a Swagger UI (Recomendado)

Una vez que el servidor esté corriendo, abre tu navegador y ve a:

```
http://localhost:5019/swagger
```

Swagger UI te permite:
- Ver todos los endpoints disponibles
- Probar cada endpoint directamente desde el navegador
- Ver la estructura de las respuestas esperadas
- Enviar peticiones con datos de ejemplo

#### Paso 3: Probar endpoints manualmente

##### Opción A: Usando el navegador

**Health Check**:
```
http://localhost:5019/api/health
```

**Obtener todos los contactos**:
```
http://localhost:5019/api/contacto
```

**Obtener contacto por ID**:
```
http://localhost:5019/api/contacto/69798dbb637ba9b81c7a4b87
```

##### Opción B: Usando curl (Terminal/PowerShell)

**Health Check**:
```bash
curl http://localhost:5019/api/health
```

**Obtener todos los contactos**:
```bash
curl http://localhost:5019/api/contacto
```

**Obtener contacto por ID**:
```bash
curl http://localhost:5019/api/contacto/69798dbb637ba9b81c7a4b87
```

**Crear nuevo contacto**:
```bash
curl -X POST http://localhost:5019/api/contacto \
  -H "Content-Type: application/json" \
  -d "{\"nombre\":\"Ana López\",\"correo\":\"ana@example.com\",\"telefono\":\"555-9012\",\"origen\":\"Monterrey\",\"destino\":\"Puerto Vallarta\"}"
```

**En PowerShell** (Windows):
```powershell
curl.exe -X POST http://localhost:5019/api/contacto `
  -H "Content-Type: application/json" `
  -d '{\"nombre\":\"Ana López\",\"correo\":\"ana@example.com\",\"telefono\":\"555-9012\",\"origen\":\"Monterrey\",\"destino\":\"Puerto Vallarta\"}'
```

##### Opción C: Usando Postman

1. Abre Postman
2. Crea una nueva petición
3. Selecciona el método HTTP (GET o POST)
4. Ingresa la URL: `http://localhost:5019/api/contacto`
5. Para POST, ve a la pestaña "Body", selecciona "raw" y "JSON", luego pega:
```json
{
  "nombre": "Ana López",
  "correo": "ana@example.com",
  "telefono": "555-9012",
  "origen": "Monterrey",
  "destino": "Puerto Vallarta"
}
```
6. Haz clic en "Send"

#### Paso 4: Verificar respuestas

Todas las respuestas deben ser JSON válido. Ejemplo de respuesta exitosa:

```json
{
  "status": "healthy",
  "timestamp": "2024-01-15T10:30:00Z",
  "service": "JA Seguro que Vuelas API",
  "version": "1.0.0"
}
```

---

## Notas técnicas adicionales

### Convenciones REST aplicadas

- **Nombres de rutas en plural**: `/api/contacto` (aunque en español se mantiene singular por claridad)
- **Métodos HTTP semánticos**: GET para lectura, POST para creación
- **Códigos HTTP apropiados**: 200 (éxito), 201 (creado), 400 (error cliente), 404 (no encontrado)
- **Respuestas JSON consistentes**: Todas las respuestas siguen el mismo formato

### Separación de responsabilidades lograda

✅ **Rutas**: Definidas mediante atributos `[Route]` y `[HttpGet]`, `[HttpPost]` en controladores  
✅ **Lógica**: Implementada en métodos de controladores, separada de la configuración de rutas  
✅ **Modelos**: Definidos en carpeta `Models/`, separados de la lógica de controladores  
✅ **Configuración**: Centralizada en `Program.cs`, separada de la lógica de negocio

### Próximos pasos

- Validaciones más robustas
- Manejo de errores global
- Autenticación y autorización
- Implementación de PUT y DELETE para contactos
- Paginación en las consultas
- Filtros y búsqueda avanzada

