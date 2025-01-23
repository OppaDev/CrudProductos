using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CrudProductos
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<Cliente> Cliente { get; set; }

    }
    public class Producto
    {
        [Key]
        public  required int IdProducto { get; set; }        
        public required string Nombre { get; set; }        
        public string? Descripcion { get; set; }
        [JsonRequired]
        public decimal Precio { get; set; } 
        public int? Cantidad_stock { get; set; }
        public required int CategoriaId { get; set; }
    }
    public class Categoria
    {
        [Key]
        public required int IdCategoria { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
    public class Venta
    {
        [Key]
        public required int IdVenta { get; set; }
        public required int ProductoId { get; set; }     
        public required int Cantidad { get; set; }
        public required DateTime Fecha_venta { get; set; }
        public required decimal Total { get; set; }
    }
    public class Cliente
    {
        [Key]
        public required int IdCliente { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}
