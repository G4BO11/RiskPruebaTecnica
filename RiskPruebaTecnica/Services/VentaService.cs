using RiskPruebaTecnica.Models.Entities;
using RiskPruebaTecnica.Repositories;

public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IProductoService _productoService;

    public VentaService(
        IVentaRepository ventaRepository,
        IProductoRepository productoRepository,
        IProductoService productoService)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
        _productoService = productoService;
    }

    public async Task<VentaDto> GetByIdAsync(Guid id)
    {
        var venta = await _ventaRepository.GetVentaWithDetallesAsync(id);
        return venta == null ? null : MapToDto(venta);
    }

    public async Task<IEnumerable<VentaDto>> GetAllAsync()
    {
        var ventas = await _ventaRepository.GetAllAsync();
        return ventas.Select(MapToDto);
    }

    public async Task<IEnumerable<VentaDto>> GetVentasByFechaAsync(DateTime fecha)
    {
        var ventas = await _ventaRepository.GetVentasByFechaAsync(fecha);
        return ventas.Select(MapToDto);
    }

    public async Task<VentaDto> CrearVentaAsync(string usuarioId, Guid clienteId, List<DetalleVentaRequest> detalles)
    {
        // !Convertir este proceso en una trasacción si el repositorio lo soporta
        if (!detalles.Any())
            throw new InvalidOperationException("La venta debe tener al menos un producto");

        foreach (var detalle in detalles)
        {
            var tieneStock = await _productoService.ValidarStockAsync(detalle.ProductoId, detalle.Cantidad);
            if (!tieneStock)
            {
                var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                throw new InvalidOperationException($"Stock insuficiente para {producto?.Nombre}. Disponible: {producto?.UnidadesExistentes}");
            }
        }

        var total = await CalcularTotalVentaAsync(detalles);

        var venta = new Venta
        {
            UserId = usuarioId,
            ClienteId = clienteId,
            FechaVenta = DateTime.UtcNow,
            Total = total,
            DetallesVenta = new List<DetalleVenta>()
        };

        foreach (var detalleRequest in detalles)
        {
            var producto = await _productoRepository.GetByIdAsync(detalleRequest.ProductoId);
            var subtotal = producto.ValorUnitario * detalleRequest.Cantidad;

            var detalleVenta = new DetalleVenta
            {
                VentaId = venta.Id,
                ProductoId = detalleRequest.ProductoId,
                Cantidad = detalleRequest.Cantidad,
                PrecioUnitario = producto.ValorUnitario,
                Subtotal = subtotal
            };

            venta.DetallesVenta.Add(detalleVenta);
        }

        await _ventaRepository.AddAsync(venta);

        foreach (var detalle in detalles)
        {
            await _productoService.ActualizarStockAsync(detalle.ProductoId, detalle.Cantidad);
        }

        return await GetByIdAsync(venta.Id);
    }

    public async Task<decimal> CalcularTotalVentaAsync(List<DetalleVentaRequest> detalles)
    {
        decimal total = 0;

        foreach (var detalle in detalles)
        {
            var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
            if (producto != null)
            {
                total += producto.ValorUnitario * detalle.Cantidad;
            }
        }

        return total;
    }

    private static VentaDto MapToDto(Venta venta)
    {
        return new VentaDto
        {
            Id = venta.Id,
            FechaVenta = venta.FechaVenta,
            Total = venta.Total,
            ClienteNombre = venta.Cliente != null ? $"{venta.Cliente.Nombre} {venta.Cliente.Apellido}" : "",
            UsuarioNombre = venta.User?.UserName ?? "",
            Detalles = venta.DetallesVenta?.Select(d => new DetalleVentaDto
            {
                ProductoNombre = d.Producto?.Nombre ?? "",
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList() ?? new List<DetalleVentaDto>()
        };
    }

    public async Task<IEnumerable<VentaDto>> GetVentasInRange(DateTime startDate, DateTime endDate)
    {
        var ventas = await _ventaRepository.GetAllAsync();
        var filteredVentas = ventas.Where(v => v.FechaVenta >= startDate && v.FechaVenta <= endDate);
        return filteredVentas.Select(MapToDto);
    }
}