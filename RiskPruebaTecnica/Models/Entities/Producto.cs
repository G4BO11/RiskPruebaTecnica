namespace RiskPruebaTecnica.Models.Entities;

public class Producto
{
    public Guid Id { get; set; }
    public required string Codigo { get; set; }
    public required string Nombre { get; set; }
    public required decimal ValorUnitario { get; set; }
    public required int UnidadesExistentes { get; set; }
    public required DateTime FechaCreacion { get; set; }
    public required bool Activo { get; set; }

    public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
}