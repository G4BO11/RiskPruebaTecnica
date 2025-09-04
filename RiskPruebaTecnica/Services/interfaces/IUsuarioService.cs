using RiskPruebaTecnica.Models.Entities;

public interface IUsuarioService
{
    Task<Usuario> ValidateUserAsync(string email, string password);
    Task<Usuario> GetByEmailAsync(string email);
    Task<bool> CreateUserAsync(string nombreUsuario, string email, string password);
}