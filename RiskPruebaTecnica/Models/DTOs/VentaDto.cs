public class VentaDto
{
    public Guid Id { get; set; }
    public DateTime FechaVenta { get; set; }
    public decimal Total { get; set; }
    public string? ClienteNombre { get; set; }
    public string? UsuarioNombre { get; set; }
    public List<DetalleVentaDto> Detalles { get; set; } = new();
}