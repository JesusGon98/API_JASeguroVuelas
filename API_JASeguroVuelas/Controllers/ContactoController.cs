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
                    Mensaje = request.Mensaje,
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

        /// <summary>
        /// Actualiza una solicitud de contacto existente
        /// </summary>
        /// <param name="id">ID del contacto</param>
        /// <param name="request">Datos actualizados del contacto</param>
        /// <returns>Contacto actualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(string id, [FromBody] ContactoRequest request)
        {
            try
            {
                _logger.LogInformation("Actualizando contacto con ID: {Id}", id);

                var existing = await _contactoService.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound(new { message = $"No se encontró un contacto con ID {id}" });
                }

                // Validación básica
                if (string.IsNullOrWhiteSpace(request.Nombre) ||
                    string.IsNullOrWhiteSpace(request.Correo) ||
                    string.IsNullOrWhiteSpace(request.Telefono) ||
                    string.IsNullOrWhiteSpace(request.Origen) ||
                    string.IsNullOrWhiteSpace(request.Destino))
                {
                    return BadRequest(new { message = "Todos los campos son requeridos" });
                }

                existing.Nombre = request.Nombre;
                existing.Correo = request.Correo;
                existing.Telefono = request.Telefono;
                existing.Origen = request.Origen;
                existing.Destino = request.Destino;
                existing.Mensaje = request.Mensaje;
                existing.FechaActualizacion = DateTime.UtcNow;

                await _contactoService.UpdateAsync(id, existing);

                _logger.LogInformation("Contacto actualizado exitosamente con ID: {Id}", id);
                return Ok(existing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar contacto");
                return StatusCode(500, new 
                { 
                    message = "Error al actualizar el contacto",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Elimina una solicitud de contacto
        /// </summary>
        /// <param name="id">ID del contacto</param>
        /// <returns>Sin contenido</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                _logger.LogInformation("Eliminando contacto con ID: {Id}", id);

                var existing = await _contactoService.GetByIdAsync(id);
                if (existing == null)
                {
                    return NotFound(new { message = $"No se encontró un contacto con ID {id}" });
                }

                await _contactoService.DeleteAsync(id);

                _logger.LogInformation("Contacto eliminado exitosamente con ID: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar contacto");
                return StatusCode(500, new 
                { 
                    message = "Error al eliminar el contacto",
                    error = ex.Message
                });
            }
        }
    }
}
