// using System.ComponentModel.DataAnnotations;
namespace RiskPruebaTecnica.Models.Entities;

public class Cliente
{
    public Guid Id { get; set; }
    public required string NumeroIdentificacion { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public required string Direccion { get; set; }
    public required string Telefono { get; set; }
    public required string Email { get; set; }
    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Venta> Ventas { get; set; }
}