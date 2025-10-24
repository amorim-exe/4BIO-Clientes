using ClienteAPI.Models;

namespace ClienteAPI.Service;

public interface IClienteService
{
    Task<List<Cliente>> ListAsync(string? nome = null, string? email = null, string? cpf = null);
    Task<Cliente> CreateAsync(Cliente cliente);
    Task<Cliente> UpdateAsync(int id, Cliente cliente);
    Task<bool> DeleteAsync(int id);
    Task<Contato> AddOrUpdateContactAsync(int clienteId, Contato contact);
    Task<bool> RemoveContactAsync(int clienteId, int contactId);
    Task<Endereco> AddOrUpdateAddressAsync(int clienteId, Endereco address);
    Task<bool> RemoveAddressAsync(int clienteId, int addressId);
    Task<Cliente> GetByIdAsync(int id);
}