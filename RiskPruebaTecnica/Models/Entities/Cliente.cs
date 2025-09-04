namespace RiskPruebaTecnica.Models.Entities;

public class Cliente
{
    public Guid Id { get; set; }
    public string NumeroIdentificacion { get; set; } // Campo clave para búsqueda
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public DateTime FechaRegistro { get; set; }
    
    public virtual ICollection<Venta> Ventas { get; set; }
}