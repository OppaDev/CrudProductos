using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudProductos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public ProductoController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Validaciones
        private async Task<IActionResult?> ValidarProducto(Producto producto)
        {
            //validar si una categoria existe
            var categoriaExistente = await _dbContext.Categoria.AnyAsync
            (c => c.IdCategoria == producto.CategoriaId);
            if (!categoriaExistente)
            {
                return BadRequest("La categoria no existe");
            }
            //validar producto con el mismo nombre
            var productoExistente = await _dbContext.Producto.AnyAsync
            (p => p.Nombre == producto.Nombre);
            if (productoExistente)
            {
                return BadRequest("El producto ya existe");
            }
            //validar no sea producto nulo
            if (producto == null)
            {
                return BadRequest("El producto no puede ser nulo");
            }
            //validar nombre no sea vacio
            if (string.IsNullOrEmpty(producto.Nombre))
            {
                return BadRequest("El nombre no puede ser vacio");
            }
            //validar precio positivo
            if (producto.Precio <= 0)
            {
                return BadRequest("El precio debe ser positivo");
            }
            return null;
        }
        //Get all Productos
        [HttpGet]
        public async Task<IActionResult> GetProducto()
        {
            return Ok(await _dbContext.Producto.ToListAsync());
        }
        //create Producto
        [HttpPost]
        public async Task<IActionResult> CreateProducto(Producto producto)
        {
            var error = await ValidarProducto(producto);
            if (error != null)
            {
                return error;
            }
            await _dbContext.Producto.AddAsync(producto);
            await _dbContext.SaveChangesAsync();
            return Ok(producto);
        }
        //update Producto
        [HttpPut]
        public async Task<IActionResult> UpdateProducto(Producto producto)
        {
            var error = await ValidarProducto(producto);
            if (error != null)
            {
                return error;
            }
            _dbContext.Producto.Update(producto);
            await _dbContext.SaveChangesAsync();
            return Ok(producto);
        }
        //delete Producto
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _dbContext.Producto.FirstOrDefaultAsync(p => p.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }
            //validar si tiene una categoria asignada
            var categoriaExistente = await _dbContext.Categoria.AnyAsync
            (c => c.IdCategoria == producto.CategoriaId);
            if (categoriaExistente)
            {
                return BadRequest("Tiene una categoria asignada");
            }
            _dbContext.Producto.Remove(producto);
            await _dbContext.SaveChangesAsync();
            return Ok(producto);
        }
    }
}
