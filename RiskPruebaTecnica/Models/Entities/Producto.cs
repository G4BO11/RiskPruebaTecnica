namespace RiskPruebaTecnica.Models.Entities;

public class Producto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public decimal ValorUnitario { get; set; }
    public int UnidadesExistentes { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
    
    
    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
}