// Services/ProductoService.cs
using RiskPruebaTecnica.Models.Entities;
using RiskPruebaTecnica.Repositories;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;
    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<ProductoDto> GetByIdAsync(Guid id)
    {
        var producto = await _productoRepository.GetByIdAsync(id);
        return producto == null ? null : MapToDto(producto);
    }

    public async Task<ProductoDto> GetByCodigoAsync(string codigo)
    {
        var producto = await _productoRepository.GetByCodigoAsync(codigo);
        return producto == null ? null : MapToDto(producto);
    }

    public async Task<IEnumerable<ProductoDto>> GetAllAsync()
    {
        var productos = await _productoRepository.GetAllAsync();
        return productos.Where(p => p.Activo).Select(MapToDto);
    }

    public async Task<ProductoDto> CreateAsync(ProductoDto productoDto)
    {
        // Validar código único
        if (await _productoRepository.ExistsByCodigoAsync(productoDto.Codigo))
        {
            throw new InvalidOperationException($"Ya existe un producto con código {productoDto.Codigo}");
        }

        var producto = new Producto
        {
            Codigo = productoDto.Codigo,
            Nombre = productoDto.Nombre,
            ValorUnitario = productoDto.ValorUnitario,
            UnidadesExistentes = productoDto.UnidadesExistentes,
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        await _productoRepository.AddAsync(producto);
        return MapToDto(producto);
    }

    public async Task UpdateAsync(ProductoDto productoDto)
    {
        var producto = await _productoRepository.GetByIdAsync(productoDto.Id);
        if (producto == null)
            throw new KeyNotFoundException($"Producto con ID {productoDto.Id} no encontrado");

        producto.Codigo = productoDto.Codigo;
        producto.Nombre = productoDto.Nombre;
        producto.ValorUnitario = productoDto.ValorUnitario;
        producto.UnidadesExistentes = productoDto.UnidadesExistentes;

        await _productoRepository.UpdateAsync(producto);
    }

    public async Task DeleteAsync(Guid id)
    {
        var producto = await _productoRepository.GetByIdAsync(id);
        if (producto != null)
        {
            producto.Activo = false; // Borrado lógico
            await _productoRepository.UpdateAsync(producto);
        }
    }

    public async Task<bool> ValidarStockAsync(Guid productoId, int cantidad)
    {
        return await _productoRepository.HasStockAsync(productoId, cantidad);
    }

    public async Task ActualizarStockAsync(Guid productoId, int cantidadVendida)
    {
        var producto = await _productoRepository.GetByIdAsync(productoId);
        if (producto == null)
            throw new KeyNotFoundException($"Producto con ID {productoId} no encontrado");

        if (producto.UnidadesExistentes < cantidadVendida)
            throw new InvalidOperationException($"Stock insuficiente. Disponible: {producto.UnidadesExistentes}, Solicitado: {cantidadVendida}");

        var nuevoStock = producto.UnidadesExistentes - cantidadVendida;
        await _productoRepository.UpdateStockAsync(productoId, nuevoStock);
    }

    private static ProductoDto MapToDto(Producto producto)
    {
        return new ProductoDto
        {
            Id = producto.Id,
            Codigo = producto.Codigo,
            Nombre = producto.Nombre,
            ValorUnitario = producto.ValorUnitario,
            UnidadesExistentes = producto.UnidadesExistentes
        };
    }
}