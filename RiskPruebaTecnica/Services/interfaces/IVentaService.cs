public interface IVentaService
{
    Task<VentaDto> GetByIdAsync(Guid id);
    Task<IEnumerable<VentaDto>> GetAllAsync();
    Task<IEnumerable<VentaDto>> GetVentasByFechaAsync(DateTime fecha);
    Task<VentaDto> CrearVentaAsync(Guid usuarioId, Guid clienteId, List<DetalleVentaRequest> detalles);
    Task<decimal> CalcularTotalVentaAsync(List<DetalleVentaRequest> detalles);
}