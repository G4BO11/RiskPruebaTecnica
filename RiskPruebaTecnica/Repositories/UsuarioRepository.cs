using Microsoft.EntityFrameworkCore;
using RiskPruebaTecnica.Data;
using RiskPruebaTecnica.Models.Entities;
using RiskPruebaTecnica.Repositories;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Usuario> GetByEmailAsync(string email)
    {
        var usuario = await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email && u.Activo);

        if (usuario == null)
            throw new InvalidOperationException($"Usuario with Email '{email}' not found.");

        return usuario;
    }

    public async Task<Usuario> ValidateUserAsync(string email, string password)
    {
        // Por ahora validación simple, después hashearemos passwords
        var usuario = await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password && u.Activo);

        if (usuario == null)
            throw new InvalidOperationException($"Usuario with Email '{email}' not found.");

        return usuario;
    }
}