namespace RiskPruebaTecnica.Models.Entities;

public class Venta
{
    public Guid Id { get; set; }
    public DateTime FechaVenta { get; set; }
    public decimal Total { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid ClienteId { get; set; }
    
    public virtual Usuario Usuario { get; set; }
    public virtual Cliente Cliente { get; set; }
    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
}