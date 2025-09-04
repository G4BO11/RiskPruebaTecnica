using RiskPruebaTecnica.Models.Entities;

namespace RiskPruebaTecnica.Repositories;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario> GetByEmailAsync(string email);
    Task<Usuario> ValidateUserAsync(string email, string password);
}