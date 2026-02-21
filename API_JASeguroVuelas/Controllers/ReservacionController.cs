using Microsoft.AspNetCore.Mvc;
using API_JASeguroVuelas.Models;
using API_JASeguroVuelas.Services;

namespace API_JASeguroVuelas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservacionController : ControllerBase
    {
        private readonly ILogger<ReservacionController> _logger;
        private readonly ReservacionService _reservacionService;

        public ReservacionController(
            ILogger<ReservacionController> logger,
            ReservacionService reservacionService)
        {
            _logger = logger;
            _reservacionService = reservacionService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var list = await _reservacionService.GetAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _reservacionService.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { message = $"No se encontró la reservación con ID {id}" });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ReservacionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Codigo) ||
                string.IsNullOrWhiteSpace(request.Origen) ||
                string.IsNullOrWhiteSpace(request.Destino) ||
                string.IsNullOrWhiteSpace(request.Estado))
            {
                return BadRequest(new { message = "Codigo, Origen, Destino y Estado son requeridos" });
            }
            if (request.Pasajeros < 1)
                return BadRequest(new { message = "Pasajeros debe ser al menos 1" });

            var ahora = DateTime.UtcNow;
            var reservacion = new Reservacion
            {
                Codigo = request.Codigo,
                Origen = request.Origen,
                Destino = request.Destino,
                Fecha = request.Fecha,
                Pasajeros = request.Pasajeros,
                Estado = request.Estado,
                FechaCreacion = ahora,
                VueloId = request.VueloId,
                UsuarioId = request.UsuarioId
            };
            await _reservacionService.CreateAsync(reservacion);
            return CreatedAtAction(nameof(GetById), new { id = reservacion.Id }, reservacion);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(string id, [FromBody] ReservacionRequest request)
        {
            var existing = await _reservacionService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No se encontró la reservación con ID {id}" });

            existing.Codigo = request.Codigo;
            existing.Origen = request.Origen;
            existing.Destino = request.Destino;
            existing.Fecha = request.Fecha;
            existing.Pasajeros = request.Pasajeros;
            existing.Estado = request.Estado;
            existing.VueloId = request.VueloId;
            existing.UsuarioId = request.UsuarioId;
            await _reservacionService.UpdateAsync(id, existing);
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _reservacionService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No se encontró la reservación con ID {id}" });
            await _reservacionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
