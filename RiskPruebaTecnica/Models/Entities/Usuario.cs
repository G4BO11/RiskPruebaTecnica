namespace RiskPruebaTecnica.Models.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public string NombreUsuario { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
    
    public virtual ICollection<Venta> Ventas { get; set; }
}