using Microsoft.AspNetCore.Mvc;
using API_JASeguroVuelas.Models;
using API_JASeguroVuelas.Services;
using MongoDB.Driver;

namespace API_JASeguroVuelas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactoController : ControllerBase
    {
        private readonly ILogger<ContactoController> _logger;
        private readonly ContactoService _contactoService;

        public ContactoController(
            ILogger<ContactoController> logger,
            ContactoService contactoService)
        {
            _logger = logger;
            _contactoService = contactoService;
        }

        /// <summary>
        /// Obtiene todas las solicitudes de contacto registradas
        /// </summary>
        /// <returns>Lista de contactos</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Obteniendo lista de contactos");
            
            var contactos = await _contactoService.GetAsync();
            return Ok(contactos);
        }

        /// <summary>
        /// Obtiene una solicitud de contacto por su ID
        /// </summary>
        /// <param name="id">ID del contacto</param>
        /// <returns>Contacto encontrado</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogInformation("Buscando contacto con ID: {Id}", id);
            
            var contacto = await _contactoService.GetByIdAsync(id);
            
            if (contacto == null)
            {
                return NotFound(new { message = $"No se encontró un contacto con ID {id}" });
            }
            
            return Ok(contacto);
        }

        /// <summary>
        /// Crea una nueva solicitud de contacto
        /// </summary>
        /// <param name="request">Datos del contacto</param>
        /// <returns>Contacto creado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] ContactoRequest request)
        {
            try
            {
                _logger.LogInformation("Creando nuevo contacto: {Nombre}", request.Nombre);

                // Validación básica
                if (string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.Correo) ||
                    string.IsNullOrWhiteSpace(request.Telefono) ||
                    string.IsNullOrWhiteSpace(request.Origen) ||
                    string.IsNullOrWhiteSpace(request.Destino))
                {
                    return BadRequest(new { message = "Todos los campos son requeridos" });
                }

                // Crear nuevo contacto
                var ahora = DateTime.UtcNow;
                var nuevoContacto = new Contacto
                {
                    Nombre = request.Nombre,
                    Correo = request.Correo,
                    Telefono = request.Telefono,
                    Origen = request.Origen,
                    Destino = request.Destino,
                    FechaCreacion = ahora,
                    FechaActualizacion = ahora
                };

                await _contactoService.CreateAsync(nuevoContacto);

                _logger.LogInformation("Contacto creado exitosamente con ID: {Id}", nuevoContacto.Id);

                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = nuevoContacto.Id }, 
                    nuevoContacto
                );
            }
            catch (MongoAuthenticationException ex)
            {
                _logger.LogError(ex, "Error de autenticación con MongoDB al crear contacto");
                return StatusCode(500, new 
                { 
                    message = "Error de autenticación con la base de datos",
                    error = "Verifica las credenciales en appsettings.json. Usuario o contraseña incorrectos."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear contacto");
                return StatusCode(500, new 
                { 
                    message = "Error al crear el contacto",
                    error = ex.Message
                });
            }
        }
    }
}
