using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudProductos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public CategoriaController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Validaciones
        private async Task<IActionResult?> ValidarCategoria(Categoria categoria)
        {
            //validar categoria con el mismo nombre
            var categoriaExistente = await _dbContext.Categoria.AnyAsync
            (c => c.Nombre == categoria.Nombre);
            if (categoriaExistente)
            {
                return BadRequest("La categoria ya existe");
            }
            //validar no sea categoria nula
            if (categoria == null)
            {
                return BadRequest("La categoria no puede ser nula");
            }
            //validar nombre no sea vacio
            if (string.IsNullOrEmpty(categoria.Nombre))
            {
                return BadRequest("El nombre no puede ser vacio");
            }
            return null;
        }
        //Get all Categorias
        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            return Ok(await _dbContext.Categoria.ToListAsync());
        }
        //create Categoria
        [HttpPost]
        public async Task<IActionResult> CreateCategoria(Categoria categoria)
        {
            var error = await ValidarCategoria(categoria);
            if (error != null)
            {
                return error;
            }
            await _dbContext.Categoria.AddAsync(categoria);
            await _dbContext.SaveChangesAsync();
            return Ok(categoria);
        }
        //update Categoria
        [HttpPut]
        public async Task<IActionResult> UpdateCategoria(Categoria categoria)
        {
            var error = await ValidarCategoria(categoria);
            if (error != null)
            {
                return error;
            }
            _dbContext.Categoria.Update(categoria);
            await _dbContext.SaveChangesAsync();
            return Ok(categoria);
        }
        //delete Categoria
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _dbContext.Categoria.FirstOrDefaultAsync(c => c.IdCategoria == id);
            if (categoria == null)
            {
                return NotFound();
            }
            //validar si tiene productos asignados
            var productosAsignados = await _dbContext.Producto.AnyAsync
                (p => p.CategoriaId == categoria.IdCategoria);
            if (productosAsignados)
            {
                return BadRequest("La categoria tiene productos asignados");
            }
            _dbContext.Categoria.Remove(categoria);
            await _dbContext.SaveChangesAsync();
            return Ok(categoria);
        }
    }
}
