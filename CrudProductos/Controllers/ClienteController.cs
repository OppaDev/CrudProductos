using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudProductos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public ClienteController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Validaciones 
        private async Task<IActionResult?> ValidarCliente(Cliente cliente)
        {
            //validar cliente con el mismo nombre y apellido
            var clienteExistente = await _dbContext.Cliente.AnyAsync
            (c => c.Nombre == cliente.Nombre && c.Apellido == cliente.Apellido);
            if (clienteExistente)
            {
                return BadRequest("El cliente ya existe");
            }
            //validar no sea cliente nulo
            if (cliente == null)
            {
                return BadRequest("El cliente no puede ser nulo");
            }
            //validar nombre o apellido no sean vacios
            if (string.IsNullOrEmpty(cliente.Nombre) || string.IsNullOrEmpty(cliente.Apellido))
            {
                return BadRequest("El nombre y apellido no pueden ser vacios");
            }
            return null;
        }
        //Get all Clientes 
        [HttpGet]
        public async Task<IActionResult> GetCliente()
        {
            return Ok(await _dbContext.Cliente.ToListAsync());
        }
        //create Cliente
        [HttpPost]
        public async Task<IActionResult> CreateCliente(Cliente cliente)
        {
            var error = await ValidarCliente(cliente);
            if (error != null)
            {
                return error;
            }
            await _dbContext.Cliente.AddAsync(cliente);
            await _dbContext.SaveChangesAsync();
            return Ok(cliente);
        }
        //update Cliente
        [HttpPut]
        public async Task<IActionResult> UpdateCliente(Cliente cliente)
        {
            var error = await ValidarCliente(cliente);
            if (error != null)
            {
                return error;
            }
            _dbContext.Cliente.Update(cliente);
            await _dbContext.SaveChangesAsync();
            return Ok(cliente);
        }
        //delete Cliente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _dbContext.Cliente.FirstOrDefaultAsync(c => c.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }
            _dbContext.Cliente.Remove(cliente);
            await _dbContext.SaveChangesAsync();
            return Ok(cliente);
        }
    }
}
