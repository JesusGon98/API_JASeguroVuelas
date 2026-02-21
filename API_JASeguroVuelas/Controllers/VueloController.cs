using Microsoft.AspNetCore.Mvc;
using API_JASeguroVuelas.Models;
using API_JASeguroVuelas.Services;

namespace API_JASeguroVuelas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VueloController : ControllerBase
    {
        private readonly ILogger<VueloController> _logger;
        private readonly VueloService _vueloService;

        public VueloController(
            ILogger<VueloController> logger,
            VueloService vueloService)
        {
            _logger = logger;
            _vueloService = vueloService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var list = await _vueloService.GetAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _vueloService.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { message = $"No se encontró el vuelo con ID {id}" });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] VueloRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Origen) ||
                string.IsNullOrWhiteSpace(request.Destino) ||
                string.IsNullOrWhiteSpace(request.Hora) ||
                string.IsNullOrWhiteSpace(request.Aerolinea) ||
                string.IsNullOrWhiteSpace(request.Estado))
            {
                return BadRequest(new { message = "Origen, Destino, Hora, Aerolinea y Estado son requeridos" });
            }

            var vuelo = new Vuelo
            {
                Origen = request.Origen,
                Destino = request.Destino,
                Fecha = request.Fecha,
                Hora = request.Hora,
                Aerolinea = request.Aerolinea,
                Precio = request.Precio,
                Estado = request.Estado,
                Imagen = request.Imagen,
                FechaCreacion = DateTime.UtcNow
            };
            await _vueloService.CreateAsync(vuelo);
            return CreatedAtAction(nameof(GetById), new { id = vuelo.Id }, vuelo);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(string id, [FromBody] VueloRequest request)
        {
            var existing = await _vueloService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No se encontró el vuelo con ID {id}" });

            existing.Origen = request.Origen;
            existing.Destino = request.Destino;
            existing.Fecha = request.Fecha;
            existing.Hora = request.Hora;
            existing.Aerolinea = request.Aerolinea;
            existing.Precio = request.Precio;
            existing.Estado = request.Estado;
            existing.Imagen = request.Imagen;
            await _vueloService.UpdateAsync(id, existing);
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _vueloService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No se encontró el vuelo con ID {id}" });
            await _vueloService.DeleteAsync(id);
            return NoContent();
        }
    }
}
