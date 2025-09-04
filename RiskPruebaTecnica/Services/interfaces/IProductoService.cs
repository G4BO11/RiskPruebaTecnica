public interface IProductoService
{
    Task<ProductoDto> GetByIdAsync(Guid id);
    Task<ProductoDto> GetByCodigoAsync(string codigo);
    Task<IEnumerable<ProductoDto>> GetAllAsync();
    Task<ProductoDto> CreateAsync(ProductoDto productoDto);
    Task UpdateAsync(ProductoDto productoDto);
    Task DeleteAsync(Guid id);
    Task<bool> ValidarStockAsync(Guid productoId, int cantidad);
    Task ActualizarStockAsync(Guid productoId, int cantidadVendida);
}