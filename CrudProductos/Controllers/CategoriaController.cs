using Microsoft.AspNetCore.Mvc;

namespace CrudProductos.Controllers
{
    public class CategoriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
