using Microsoft.AspNetCore.Mvc;

namespace CrudProductos.Controllers
{
    public class ProductoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
