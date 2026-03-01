using Microsoft.EntityFrameworkCore;
using Productos.Api.Data;
using Productos.Api.Models;

namespace Productos.Api.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAll(string? codigo, string? nombre, bool? activo)
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

        public async Task<Producto?> GetById(int id)
        {
            return await _context.Productos
                .Where(p => p.Id == id && p.Activo)
                .FirstOrDefaultAsync();
        }

        public async Task<(bool Success, string? Error, Producto? Producto)> Create(Producto producto)
        {
            if (await _context.Productos.AnyAsync(p => p.Codigo == producto.Codigo))
                return (false, "El código ya existe.", null);

            producto.CreatedAt = DateTime.Now;
            producto.Activo = true;

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return (true, null, producto);
        }

        public async Task<(bool Success, string? Error)> Update(int id, Producto producto)
        {
            var existing = await _context.Productos.FindAsync(id);

            if (existing == null || !existing.Activo)
                return (false, "Producto no encontrado.");

            if (await _context.Productos.AnyAsync(p => p.Codigo == producto.Codigo && p.Id != id))
                return (false, "El código ya existe.");

            existing.Codigo = producto.Codigo;
            existing.Nombre = producto.Nombre;
            existing.Precio = producto.Precio;
            existing.Stock = producto.Stock;
            existing.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, string? Error)> SoftDelete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return (false, "Producto no encontrado.");

            if (!producto.Activo)
                return (false, "El producto ya está inactivo.");

            producto.Activo = false;
            producto.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}