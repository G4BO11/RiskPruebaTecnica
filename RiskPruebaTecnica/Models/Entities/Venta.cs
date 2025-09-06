using Microsoft.AspNetCore.Identity;

namespace RiskPruebaTecnica.Models.Entities;

public class Venta
{
    public Guid Id { get; set; }
    public DateTime FechaVenta { get; set; }
    public decimal Total { get; set; }
    public string UserId { get; set; }
    public Guid ClienteId { get; set; }

    public virtual IdentityUser User { get; set; }
    public virtual Cliente Cliente { get; set; }
    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
}