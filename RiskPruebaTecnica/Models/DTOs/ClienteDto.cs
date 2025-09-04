public class ClienteDto
{
    public Guid Id { get; set; }
    public string NumeroIdentificacion { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string NombreCompleto => $"{Nombre} {Apellido}";
}