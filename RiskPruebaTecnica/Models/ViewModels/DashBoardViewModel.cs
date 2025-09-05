public class DashboardViewModel
{
    public int VentasHoy { get; set; }
    public decimal TotalVentasHoy { get; set; }
    public int ProductosBajoStock { get; set; }
    public List<VentaDto> UltimasVentas { get; set; } = new();
}