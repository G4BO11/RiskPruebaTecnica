using RiskPruebaTecnica.Models.Entities;

namespace RiskPruebaTecnica.Repositories;

public interface IClienteRepository : IGenericRepository<Cliente>
{
    Task<Cliente> GetByNumeroIdentificacionAsync(string numeroIdentificacion);
    Task<bool> ExistsByNumeroIdentificacionAsync(string numeroIdentificacion);
}