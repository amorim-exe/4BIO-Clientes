using ClienteAPI.Models;

namespace ClienteAPI.Repository
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetAllAsync();
        Task<Cliente> GetByIdAsync(int id);
        Task<Cliente> CreateAsync(Cliente client);
        Task<Cliente> UpdateAsync(Cliente client);
        Task<bool> DeleteAsync(int id);
        Task SaveAllAsync(List<Cliente> clients);
    }
}