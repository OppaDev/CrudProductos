using Microsoft.AspNetCore.Mvc;

namespace CrudProductos.Controllers
{
    public class VentaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
