using System.Text.Json;
using ClienteAPI.Models;

namespace ClienteAPI.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _filePath;
        private List<Cliente> _clientes;
        private static readonly SemaphoreSlim _locker = new(1,1);
        private readonly JsonSerializerOptions _opts = new(JsonSerializerDefaults.Web) { WriteIndented = true };

        public ClienteRepository()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Clientes.json");
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");

            var json = File.ReadAllText(_filePath);
            _clientes = JsonSerializer.Deserialize<List<Cliente>>(json, _opts) ?? new List<Cliente>();
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            await _locker.WaitAsync();
            try
            {
                return _clientes.ToList();
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            await _locker.WaitAsync();
            try
            {
                return _clientes.FirstOrDefault(c => c.Id == id) ?? throw new InvalidOperationException();
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            await _locker.WaitAsync();
            try
            {
                cliente.Id = _clientes.Any() ? _clientes.Max(c => c.Id) + 1 : 1;
                AssignNestedIds(cliente);
                _clientes.Add(cliente);
                await SaveAsync();
                return cliente;
            }
            finally
            {
                _locker.Release();
            }
        }
        
        private async Task SaveAsync()
        {
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(_clientes, _opts));
        }

        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            await _locker.WaitAsync();
            try
            {
                var list = await GetAllAsync();
                var idx = list.FindIndex(x => x.Id == cliente.Id);
                if (idx == -1) 
                    return null;
                
                AssignNestedIds(cliente, list[idx]);
                list[idx] = cliente;
                await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(list, _opts));
                return cliente;
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _locker.WaitAsync();
            try
            {
                var list = await GetAllAsync();
                var removed = list.RemoveAll(x => x.Id == id) > 0;
                await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(list, _opts));
                return removed;
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task SaveAllAsync(List<Cliente> clientes)
        {
            await _locker.WaitAsync();
            try
            {
                await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(clientes, _opts));
            }
            finally
            {
                _locker.Release();
            }
        }

        private static void AssignNestedIds(Cliente cliente, Cliente existing = null)
        {
            cliente.Contatos ??= [];
            cliente.Enderecos ??= [];

            int nextContactId = 1;
            if (existing?.Contatos?.Any() == true)
                nextContactId = existing.Contatos.Max(c => c.Id) + 1;

            foreach (var c in cliente.Contatos)
            {
                if (c.Id == 0) c.Id = nextContactId++;
            }

            int nextAddrId = 1;
            if (existing?.Enderecos?.Any() == true)
                nextAddrId = existing.Enderecos.Max(a => a.Id) + 1;

            foreach (var a in cliente.Enderecos)
            {
                if (a.Id == 0) a.Id = nextAddrId++;
            }
        }

    }
}
