public interface IClienteService
{
    Task<ClienteDto> GetByIdAsync(Guid id);
    Task<ClienteDto> GetByNumeroIdentificacionAsync(string numeroIdentificacion);
    Task<IEnumerable<ClienteDto>> GetAllAsync();
    Task<ClienteDto> CreateAsync(ClienteDto clienteDto);
    Task UpdateAsync(ClienteDto clienteDto);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByNumeroIdentificacionAsync(string numeroIdentificacion);
}