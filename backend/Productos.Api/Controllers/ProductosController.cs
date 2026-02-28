using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Productos.Api.Data;
using Productos.Api.Models;

namespace Productos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/productos
        // Soporta filtros opcionales: codigo, nombre, activo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos(
            string? codigo,
            string? nombre,
            bool? activo)
        {
            var query = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(codigo))
                query = query.Where(p => p.Codigo.Contains(codigo));

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre));

            if (activo.HasValue)
                query = query.Where(p => p.Activo == activo.Value);

            return await query.ToListAsync();
        }

        // GET: api/productos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Where(p => p.Id == id && p.Activo)
                .FirstOrDefaultAsync();

            if (producto == null)
                return NotFound();

            return producto;
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            producto.CreatedAt = DateTime.Now;
            producto.Activo = true;

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        // PUT: api/productos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id)
                return BadRequest("El id del body no coincide con el de la URL.");

            var existing = await _context.Productos.FindAsync(id);

            if (existing == null || !existing.Activo)
                return NotFound();

            existing.Codigo = producto.Codigo;
            existing.Nombre = producto.Nombre;
            existing.Precio = producto.Precio;
            existing.Stock = producto.Stock;
            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/productos/{id}
        // Soft delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            if (!producto.Activo)
                return BadRequest("El producto ya está inactivo.");

            producto.Activo = false;
            producto.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}