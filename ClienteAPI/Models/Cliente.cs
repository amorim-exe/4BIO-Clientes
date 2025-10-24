using System.ComponentModel.DataAnnotations;

namespace ClienteAPI.Models;

public class Cliente
{
    public int Id { get; set; }
    
    public required string Nome { get; set; }
    
    [EmailAddress] 
    public required string Email { get; set; }
    
    public required string CPF { get; set; }
    
    public required string RG { get; set; }
    
    public List<Contato> Contatos { get; set; } = [];
    
    public List<Endereco> Enderecos { get; set; } = [];
}