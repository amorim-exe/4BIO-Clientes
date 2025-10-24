using ClienteAPI.Models;
using ClienteAPI.Repository;
using ClienteAPI.Utils;

namespace ClienteAPI.Service
{
    public class ClienteService(IClienteRepository repo) : IClienteService
    {
        public async Task<List<Cliente>> ListAsync(string? nome = null, string? email = null, string? cpf = null)
        {
            var all = await repo.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(nome))
                all = all.Where(c => c.Nome?.Contains(nome, StringComparison.OrdinalIgnoreCase) == true).ToList();
            if (!string.IsNullOrWhiteSpace(email))
                all = all.Where(c => string.Equals(c.Email, email, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrWhiteSpace(cpf))
                all = all.Where(c => string.Equals(c.CPF, cpf, StringComparison.OrdinalIgnoreCase)).ToList();
            return all;
        }

        public async Task<Cliente> CreateAsync(Cliente client)
        {
            ValidateClient(client, isUpdate: false);
            return await repo.CreateAsync(client);
        }

        public async Task<Cliente> UpdateAsync(int id, Cliente client)
        {
            var existing = await repo.GetByIdAsync(id);
            if (existing == null) 
                return null;
            
            client.Id = id;
            ValidateClient(client, isUpdate: true);
            return await repo.UpdateAsync(client);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await repo.DeleteAsync(id);
        }

        public async Task<Contato> AddOrUpdateContactAsync(int clientId, Contato contact)
        {
            var client = await repo.GetByIdAsync(clientId);
            if (client == null) 
                return null;
            
            if (string.IsNullOrWhiteSpace(contact.Tipo)) 
                throw new ArgumentException("Tipo inválido");
            
            if (contact.Id == 0)
            {
                var nextId = client.Contatos.Any() ? client.Contatos.Max(c => c.Id) + 1 : 1;
                contact.Id = nextId;
                client.Contatos.Add(contact);
            }
            else
            {
                var idx = client.Contatos.FindIndex(c => c.Id == contact.Id);
                if (idx == -1) client.Contatos.Add(contact);
                else client.Contatos[idx] = contact;
            }
            await repo.UpdateAsync(client);
            return contact;
        }

        public async Task<bool> RemoveContactAsync(int clientId, int contactId)
        {
            var client = await repo.GetByIdAsync(clientId);
            if (client == null) 
                return false;
            
            var removed = client.Contatos.RemoveAll(c => c.Id == contactId) > 0;
            await repo.UpdateAsync(client);
            return removed;
        }

        public async Task<Endereco> AddOrUpdateAddressAsync(int clientId, Endereco address)
        {
            var client = await repo.GetByIdAsync(clientId);
            if (client == null) 
                return null;
            
            if (!Validadores.IsValidCep(address.CEP)) 
                throw new ArgumentException("CEP inválido");
            
            if (address.Id == 0)
            {
                var nextId = client.Enderecos.Any() ? client.Enderecos.Max(a => a.Id) + 1 : 1;
                address.Id = nextId;
                client.Enderecos.Add(address);
            }
            else
            {
                var idx = client.Enderecos.FindIndex(a => a.Id == address.Id);
                if (idx == -1) client.Enderecos.Add(address);
                else client.Enderecos[idx] = address;
            }
            await repo.UpdateAsync(client);
            return address;
        }

        public async Task<bool> RemoveAddressAsync(int clientId, int addressId)
        {
            var client = await repo.GetByIdAsync(clientId);
            if (client == null) 
                return false;
            
            var removed = client.Enderecos.RemoveAll(a => a.Id == addressId) > 0;
            await repo.UpdateAsync(client);
            return removed;
        }

        public async Task<Cliente> GetByIdAsync(int id) => await repo.GetByIdAsync(id);

        private static void ValidateClient(Cliente client, bool isUpdate)
        {
            if (client == null) 
                throw new ArgumentNullException(nameof(client));
            
            if (string.IsNullOrWhiteSpace(client.Nome)) 
                throw new ArgumentException("Nome é obrigatório");
            
            if (!Validadores.IsValidEmail(client.Email)) 
                throw new ArgumentException("Email inválido");
            
            if (!Validadores.IsValidCpf(client.CPF)) 
                throw new ArgumentException("CPF inválido");
            
            if (!Validadores.IsValidRg(client.RG)) 
                throw new ArgumentException("RG inválido");

            if (client.Contatos.Any(c => string.IsNullOrWhiteSpace(c.Tipo)))
            {
                throw new ArgumentException("Tipo de contato inválido");
            }

            if (client.Enderecos.Any(a => !Validadores.IsValidCep(a.CEP)))
            {
                throw new ArgumentException("CEP inválido em endereço");
            }
        }
    }
}
