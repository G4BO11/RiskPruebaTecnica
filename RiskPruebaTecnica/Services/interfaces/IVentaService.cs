public interface IVentaService
{
    Task<VentaDto> GetByIdAsync(Guid id);
    Task<IEnumerable<VentaDto>> GetAllAsync();
    Task<IEnumerable<VentaDto>> GetVentasByFechaAsync(DateTime fecha);
    Task<VentaDto> CrearVentaAsync(string usuarioId, Guid clienteId, List<DetalleVentaRequest> detalles);
    Task<decimal> CalcularTotalVentaAsync(List<DetalleVentaRequest> detalles);
    Task<IEnumerable<VentaDto>> GetVentasInRange(DateTime startDate, DateTime endDate);
}