using CrudProductos;
using CrudProductos.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CrudProductos.Tests
{
    public class ProductoControllerTests
    {
        private AppDBContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            return new AppDBContext(options);
        }

        [Fact]
        public async Task GetProducto_ReturnsAllProductos()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Producto.Add(new Producto { IdProducto = 1, Nombre = "Producto1", Precio = 100, CategoriaId = 1 });
            dbContext.Producto.Add(new Producto { IdProducto = 2, Nombre = "Producto2", Precio = 200, CategoriaId = 1 });
            await dbContext.SaveChangesAsync();

            var controller = new ProductoController(dbContext);

            // Act
            var result = await controller.GetProducto();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var productos = Assert.IsType<List<Producto>>(okResult.Value);
            Assert.Equal(2, productos.Count);
        }

        [Fact]
        public async Task CreateProducto_AddsNewProducto()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            dbContext.Categoria.Add(new Categoria { IdCategoria = 1, Nombre = "Categoria1" });
            await dbContext.SaveChangesAsync();

            var controller = new ProductoController(dbContext);
            var nuevoProducto = new Producto { IdProducto = 1, Nombre = "Producto1", Precio = 100, CategoriaId = 1 };

            // Act
            var result = await controller.CreateProducto(nuevoProducto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var producto = Assert.IsType<Producto>(okResult.Value);
            Assert.Equal("Producto1", producto.Nombre);
        }
    }
}
