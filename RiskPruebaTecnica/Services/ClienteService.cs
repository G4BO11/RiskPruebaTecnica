using RiskPruebaTecnica.Models.Entities;
using RiskPruebaTecnica.Repositories;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<ClienteDto> GetByIdAsync(Guid id)
    {
        var cliente = await _clienteRepository.GetByIdAsync(id);
        return cliente == null ? null : MapToDto(cliente);
    }

    public async Task<ClienteDto> GetByNumeroIdentificacionAsync(string numeroIdentificacion)
    {
        var cliente = await _clienteRepository.GetByNumeroIdentificacionAsync(numeroIdentificacion);
        return cliente == null ? null : MapToDto(cliente);
    }

    public async Task<IEnumerable<ClienteDto>> GetAllAsync()
    {
        var clientes = await _clienteRepository.GetAllAsync();
        return clientes.Select(MapToDto);
    }

    public async Task<ClienteDto> CreateAsync(ClienteDto clienteDto)
    {
        // Validar que no existe
        if (await _clienteRepository.ExistsByNumeroIdentificacionAsync(clienteDto.NumeroIdentificacion))
        {
            throw new InvalidOperationException($"Ya existe un cliente con identificación {clienteDto.NumeroIdentificacion}");
        }

        var cliente = new Cliente
        {
            NumeroIdentificacion = clienteDto.NumeroIdentificacion,
            Nombre = clienteDto.Nombre,
            Apellido = clienteDto.Apellido,
            Direccion = clienteDto.Direccion,
            Telefono = clienteDto.Telefono,
            Email = clienteDto.Email,
            FechaRegistro = DateTime.UtcNow
        };

        await _clienteRepository.AddAsync(cliente);
        return MapToDto(cliente);
    }

    public async Task UpdateAsync(ClienteDto clienteDto)
    {
        var cliente = await _clienteRepository.GetByIdAsync(clienteDto.Id);
        if (cliente == null)
            throw new KeyNotFoundException($"Cliente con ID {clienteDto.Id} no encontrado");

        cliente.NumeroIdentificacion = clienteDto.NumeroIdentificacion;
        cliente.Nombre = clienteDto.Nombre;
        cliente.Apellido = clienteDto.Apellido;
        cliente.Direccion = clienteDto.Direccion;
        cliente.Telefono = clienteDto.Telefono;
        cliente.Email = clienteDto.Email;

        await _clienteRepository.UpdateAsync(cliente);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _clienteRepository.DeleteAsync(id);
    }

    public async Task<bool> ExistsByNumeroIdentificacionAsync(string numeroIdentificacion)
    {
        return await _clienteRepository.ExistsByNumeroIdentificacionAsync(numeroIdentificacion);
    }

    private static ClienteDto MapToDto(Cliente cliente)
    {
        return new ClienteDto
        {
            Id = cliente.Id,
            NumeroIdentificacion = cliente.NumeroIdentificacion,
            Nombre = cliente.Nombre,
            Apellido = cliente.Apellido,
            Direccion = cliente.Direccion,
            Telefono = cliente.Telefono,
            Email = cliente.Email
        };
    }
}