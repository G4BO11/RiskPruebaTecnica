using RiskPruebaTecnica.Models.Entities;
using RiskPruebaTecnica.Repositories;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario> ValidateUserAsync(string email, string password)
    {
        return await _usuarioRepository.ValidateUserAsync(email, password);
    }

    public async Task<Usuario> GetByEmailAsync(string email)
    {
        return await _usuarioRepository.GetByEmailAsync(email);
    }

    public async Task<bool> CreateUserAsync(string nombreUsuario, string email, string password)
    {
        // Verificar si ya existe
        var existeUsuario = await _usuarioRepository.GetByEmailAsync(email);
        if (existeUsuario != null)
            return false;

        var usuario = new Usuario
        {
            NombreUsuario = nombreUsuario,
            Email = email,
            PasswordHash = password, // En producción esto se hashearía
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        await _usuarioRepository.AddAsync(usuario);
        return true;
    }
}