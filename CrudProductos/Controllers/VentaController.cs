using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudProductos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public VentaController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Validaciones
        private async Task<IActionResult?> ValidarVenta(Venta venta)
        {
            //validar no sea venta nula
            if (venta == null)
            {
                return BadRequest("La venta no puede ser nula");
            }
            //validar cantidad no sea vacia
            if (venta.Cantidad == 0)
            {
                return BadRequest("La cantidad no puede ser vacia");
            }
            //validar si un producto existe
            var productoExistente = await _dbContext.Producto.AnyAsync
            (p => p.IdProducto == venta.ProductoId);
            if (!productoExistente)
            {
                return BadRequest("El producto no existe");
            }
            return null;
        }
        //Get all Ventas
        [HttpGet]
        public async Task<IActionResult> GetVenta()
        {
            return Ok(await _dbContext.Venta.ToListAsync());
        }
        //create Venta
        [HttpPost]
        public async Task<IActionResult> CreateVenta(Venta venta)
        {
            var error = await ValidarVenta(venta);
            if (error != null)
            {
                return error;
            }
            await _dbContext.Venta.AddAsync(venta);
            await _dbContext.SaveChangesAsync();
            return Ok(venta);
        }
        //update Venta
        [HttpPut]
        public async Task<IActionResult> UpdateVenta(Venta venta)
        {
            var error = await ValidarVenta(venta);
            if (error != null)
            {
                return error;
            }
            _dbContext.Venta.Update(venta);
            await _dbContext.SaveChangesAsync();
            return Ok(venta);
        }
        //delete Venta
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            var venta = await _dbContext.Venta.FirstOrDefaultAsync(v => v.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }
            _dbContext.Venta.Remove(venta);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
