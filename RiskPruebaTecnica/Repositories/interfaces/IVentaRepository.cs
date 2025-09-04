using RiskPruebaTecnica.Models.Entities;

namespace RiskPruebaTecnica.Repositories;

public interface IVentaRepository : IGenericRepository<Venta>
{
    Task<IEnumerable<Venta>> GetVentasByFechaAsync(DateTime fecha);
    Task<IEnumerable<Venta>> GetVentasByRangoFechaAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<Venta> GetVentaWithDetallesAsync(Guid ventaId);
}