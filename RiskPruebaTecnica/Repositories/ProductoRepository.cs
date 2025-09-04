using Microsoft.EntityFrameworkCore;
using RiskPruebaTecnica.Data;
using RiskPruebaTecnica.Models.Entities;
using RiskPruebaTecnica.Repositories;
public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
{
    public ProductoRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Producto> GetByCodigoAsync(string codigo)
    {
        var producto = await _dbSet
            .FirstOrDefaultAsync(p => p.Codigo == codigo && p.Activo);
        if (producto == null)
        {
            throw new InvalidOperationException($"No se encontró un producto activo con el código '{codigo}'.");
        }
        return producto;
    }

    public async Task<bool> ExistsByCodigoAsync(string codigo)
    {
        return await _dbSet
            .AnyAsync(p => p.Codigo == codigo);
    }

    public async Task<bool> HasStockAsync(Guid productoId, int cantidad)
    {
        var producto = await GetByIdAsync(productoId);
        return producto != null && producto.UnidadesExistentes >= cantidad;
    }

    public async Task UpdateStockAsync(Guid productoId, int nuevaCantidad)
    {
        var producto = await GetByIdAsync(productoId);
        if (producto != null)
        {
            producto.UnidadesExistentes = nuevaCantidad;
            await UpdateAsync(producto);
        }
    }
}