public class VentaRequest
{
    public Guid ClienteId { get; set; }
    public string NumeroIdentificacionCliente { get; set; }
    public List<DetalleVentaRequest> Detalles { get; set; } = new();
}