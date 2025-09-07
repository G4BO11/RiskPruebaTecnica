using System.ComponentModel.DataAnnotations;

public class ClienteDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "El número de identificación es requerido")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "La identificación debe tener entre 6 y 20 caracteres")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "La identificación solo debe contener números")]
    [Display(Name = "Número de Identificación")]
    public string NumeroIdentificacion { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo debe contener letras")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo debe contener letras")]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; }

    [Required(ErrorMessage = "La dirección es requerida")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 200 caracteres")]
    [Display(Name = "Dirección")]
    public string Direccion { get; set; }

    [Required(ErrorMessage = "El teléfono es requerido")]
    [Phone(ErrorMessage = "El formato del teléfono no es válido")]
    [RegularExpression(@"^[0-9]{7,10}$", ErrorMessage = "El teléfono debe tener entre 7 y 10 dígitos")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
    [Display(Name = "Correo Electrónico")]
    public string Email { get; set; }

    public string NombreCompleto => $"{Nombre} {Apellido}";
}