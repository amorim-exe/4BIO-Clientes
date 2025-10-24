using ClienteAPI.Models;
using ClienteAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace ClienteAPI.Controllers
{
    [ApiController]
    [Route("cliente")]
    public class ClienteController(IClienteService service) : ControllerBase
    {
        [HttpGet("listar")]
        public async Task<IActionResult> List([FromQuery] string? nome = null, [FromQuery] string? email = null,
            [FromQuery] string? cpf = null)
        {
            var list = await service.ListAsync(nome, email, cpf);
            return Ok(list);
        }

        [HttpPost("criar")]
        public async Task<IActionResult> Create([FromBody] Cliente client)
        {
            try
            {
                var created = await service.CreateAsync(client);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var client = await service.GetByIdAsync(id);
            if (client == null) 
                return NotFound();
            
            return Ok(client);
        }

        [HttpPut("atualizar/{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Cliente client)
        {
            try
            {
                var updated = await service.UpdateAsync(id, client);
                if (updated == null) 
                    return NotFound();
                
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("remover/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var removed = await service.DeleteAsync(id);
            if (!removed) 
                return NotFound();
            
            return NoContent();
        }

        [HttpPost("{clientId:int}/contato")]
        public async Task<IActionResult> AddOrUpdateContact([FromRoute] int clientId, [FromBody] Contato contact)
        {
            try
            {
                var c = await service.AddOrUpdateContactAsync(clientId, contact);
                if (c == null) 
                    return NotFound();
                
                return Ok(c);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{clientId:int}/contato/{contactId:int}")]
        public async Task<IActionResult> RemoveContact([FromRoute] int clientId, [FromRoute] int contactId)
        {
            var removed = await service.RemoveContactAsync(clientId, contactId);
            if (!removed) 
                return NotFound();
            
            return NoContent();
        }

        [HttpPost("{clientId:int}/endereco")]
        public async Task<IActionResult> AddOrUpdateAddress([FromRoute] int clientId, [FromBody] Endereco address)
        {
            try
            {
                var a = await service.AddOrUpdateAddressAsync(clientId, address);
                return Ok(a);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{clientId:int}/endereco/{addressId:int}")]
        public async Task<IActionResult> RemoveAddress([FromRoute] int clientId, [FromRoute] int addressId)
        {
            var removed = await service.RemoveAddressAsync(clientId, addressId);
            if (!removed) 
                return NotFound();
            
            return NoContent();
        }
    }
}