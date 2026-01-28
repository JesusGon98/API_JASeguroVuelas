using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using API_JASeguroVuelas.Models;

namespace API_JASeguroVuelas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IMongoClient _mongoClient;
        private readonly MongoDBSettings _mongoSettings;

        public HealthController(
            ILogger<HealthController> logger,
            IMongoClient mongoClient,
            Microsoft.Extensions.Options.IOptions<MongoDBSettings> mongoSettings)
        {
            _logger = logger;
            _mongoClient = mongoClient;
            _mongoSettings = mongoSettings.Value;
        }

        /// <summary>
        /// Endpoint para verificar el estado de la API
        /// </summary>
        /// <returns>Estado de salud de la API</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            _logger.LogInformation("Health check realizado");
            
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                service = "JA Seguro que Vuelas API",
                version = "1.0.0"
            });
        }

        /// <summary>
        /// Endpoint para probar la conexión a MongoDB
        /// </summary>
        /// <returns>Estado de la conexión a MongoDB</returns>
        [HttpGet("mongodb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> TestMongoDB()
        {
            try
            {
                _logger.LogInformation("Probando conexión a MongoDB...");
                
                // Intentar listar las bases de datos (prueba de conexión)
                await _mongoClient.ListDatabaseNamesAsync();
                
                // Intentar acceder a la base de datos específica
                var database = _mongoClient.GetDatabase(_mongoSettings.DatabaseName);
                await database.ListCollectionNamesAsync();
                
                _logger.LogInformation("Conexión a MongoDB exitosa");
                
                return Ok(new
                {
                    status = "connected",
                    message = "Conexión a MongoDB exitosa",
                    database = _mongoSettings.DatabaseName,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (MongoAuthenticationException ex)
            {
                _logger.LogError(ex, "Error de autenticación con MongoDB");
                return StatusCode(503, new
                {
                    status = "authentication_failed",
                    message = "Error de autenticación con MongoDB",
                    error = ex.Message,
                    details = "Verifica que el usuario y contraseña sean correctos en appsettings.json"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al conectar con MongoDB");
                return StatusCode(503, new
                {
                    status = "connection_failed",
                    message = "Error al conectar con MongoDB",
                    error = ex.Message,
                    connectionString = !string.IsNullOrEmpty(_mongoSettings.ConnectionString) 
                        ? _mongoSettings.ConnectionString.Replace(_mongoSettings.ConnectionString.Split('@')[0].Split(':')[2], "***") 
                        : "No configurada"
                });
            }
        }
    }
}
