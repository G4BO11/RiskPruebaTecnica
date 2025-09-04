using RiskPruebaTecnica.Models.Entities;

namespace RiskPruebaTecnica.Repositories;

public interface IProductoRepository : IGenericRepository<Producto>
{
    Task<Producto> GetByCodigoAsync(string codigo);
    Task<bool> ExistsByCodigoAsync(string codigo);
    Task<bool> HasStockAsync(Guid productoId, int cantidad);
    Task UpdateStockAsync(Guid productoId, int nuevaCantidad);
}