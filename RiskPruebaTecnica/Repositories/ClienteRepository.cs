// Repositories/ClienteRepository.cs
using Microsoft.EntityFrameworkCore;
using RiskPruebaTecnica.Data;
using RiskPruebaTecnica.Models.Entities;
using RiskPruebaTecnica.Repositories;

public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Cliente> GetByNumeroIdentificacionAsync(string numeroIdentificacion)
    {
        var cliente = await _dbSet
            .FirstOrDefaultAsync(c => c.NumeroIdentificacion == numeroIdentificacion);

        if (cliente == null)
            throw new InvalidOperationException($"Cliente with NumeroIdentificacion '{numeroIdentificacion}' not found.");

        return cliente;
    }

    public async Task<bool> ExistsByNumeroIdentificacionAsync(string numeroIdentificacion)
    {
        return await _dbSet
            .AnyAsync(c => c.NumeroIdentificacion == numeroIdentificacion);
    }
}

