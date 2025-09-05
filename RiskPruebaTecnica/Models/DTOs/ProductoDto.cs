public class ProductoDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public decimal ValorUnitario { get; set; }
    public int UnidadesExistentes { get; set; }
}