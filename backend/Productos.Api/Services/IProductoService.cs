using Productos.Api.Models;

namespace Productos.Api.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> GetAll(string? codigo, string? nombre, bool? activo);
        Task<Producto?> GetById(int id);
        Task<(bool Success, string? Error, Producto? Producto)> Create(Producto producto);
        Task<(bool Success, string? Error)> Update(int id, Producto producto);
        Task<(bool Success, string? Error)> SoftDelete(int id);
    }
}