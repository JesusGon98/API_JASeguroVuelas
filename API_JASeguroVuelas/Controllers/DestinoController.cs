using Microsoft.AspNetCore.Mvc;
using API_JASeguroVuelas.Models;
using API_JASeguroVuelas.Services;

namespace API_JASeguroVuelas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DestinoController : ControllerBase
    {
        private readonly ILogger<DestinoController> _logger;
        private readonly DestinoService _destinoService;

        public DestinoController(
            ILogger<DestinoController> logger,
            DestinoService destinoService)
        {
            _logger = logger;
            _destinoService = destinoService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var list = await _destinoService.GetAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _destinoService.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { message = $"No se encontró el destino con ID {id}" });
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] DestinoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre) || string.IsNullOrWhiteSpace(request.Descripcion))
                return BadRequest(new { message = "Nombre y Descripcion son requeridos" });

            var destino = new Destino
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                MejorEpoca = request.MejorEpoca,
                PrecioMin = request.PrecioMin,
                PrecioMax = request.PrecioMax,
                Etiqueta = request.Etiqueta,
                Foto = request.Foto,
                FechaCreacion = DateTime.UtcNow
            };
            await _destinoService.CreateAsync(destino);
            return CreatedAtAction(nameof(GetById), new { id = destino.Id }, destino);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(string id, [FromBody] DestinoRequest request)
        {
            var existing = await _destinoService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No se encontró el destino con ID {id}" });

            existing.Nombre = request.Nombre;
            existing.Descripcion = request.Descripcion;
            existing.MejorEpoca = request.MejorEpoca;
            existing.PrecioMin = request.PrecioMin;
            existing.PrecioMax = request.PrecioMax;
            existing.Etiqueta = request.Etiqueta;
            existing.Foto = request.Foto;
            await _destinoService.UpdateAsync(id, existing);
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _destinoService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"No se encontró el destino con ID {id}" });
            await _destinoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
