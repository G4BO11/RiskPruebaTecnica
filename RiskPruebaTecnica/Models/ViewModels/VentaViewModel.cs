public class VentaViewModel
{
    public VentaRequest VentaRequest { get; set; } = new();
    public List<ProductoDto> ProductosDisponibles { get; set; } = new();
    public ClienteDto ClienteSeleccionado { get; set; }
    public decimal TotalVenta { get; set; }
}