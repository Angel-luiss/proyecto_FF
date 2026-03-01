using Microsoft.AspNetCore.Mvc;
using Productos.Api.Models;
using Productos.Api.Services;

namespace Productos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductosController(IProductoService service)
        {
            _service = service;
        }

        // ✅ GET lista
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos(
            [FromQuery] string? codigo,
            [FromQuery] string? nombre,
            [FromQuery] bool? activo)
        {
            var productos = await _service.GetAll(codigo, nombre, activo);
            return Ok(productos);
        }

        // ✅ GET por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _service.GetById(id);

            if (producto == null)
                return NotFound("Producto no encontrado.");

            return Ok(producto);
        }

        // ✅ POST crear
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            var result = await _service.Create(producto);

            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(
                nameof(GetProducto),
                new { id = result.Producto!.Id },
                result.Producto);
        }

        // ✅ PUT editar
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            var result = await _service.Update(id, producto);

            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent();
        }

        // ✅ DELETE soft delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var result = await _service.SoftDelete(id);

            if (!result.Success)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}