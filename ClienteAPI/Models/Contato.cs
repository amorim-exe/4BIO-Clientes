namespace ClienteAPI.Models;

public class Contato
{
    public int Id { get; set; }
    
    public required string Tipo { get; set; } 
    
    public int DDD { get; set; }
    
    public decimal Telefone { get; set; }
}