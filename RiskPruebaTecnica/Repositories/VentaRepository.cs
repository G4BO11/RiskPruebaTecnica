using Microsoft.EntityFrameworkCore;
using RiskPruebaTecnica.Data;
using RiskPruebaTecnica.Models.Entities;
using RiskPruebaTecnica.Repositories;
public class VentaRepository : GenericRepository<Venta>, IVentaRepository
{
    public VentaRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Venta>> GetVentasByFechaAsync(DateTime fecha)
    {
        return await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.User)
            .Where(v => v.FechaVenta.Date == fecha.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Venta>> GetVentasByRangoFechaAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.User)
            .Where(v => v.FechaVenta.Date >= fechaInicio.Date && v.FechaVenta.Date <= fechaFin.Date)
            .ToListAsync();
    }

    public async Task<Venta> GetVentaWithDetallesAsync(Guid ventaId)
    {
        var venta = await _dbSet
            .Include(v => v.Cliente)
            .Include(v => v.User)
            .Include(v => v.DetallesVenta)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(v => v.Id == ventaId);

        if (venta == null)
            throw new InvalidOperationException("Venta not found.");

        return venta;
    }
}
