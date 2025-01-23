using Microsoft.AspNetCore.Mvc;

namespace CrudProductos.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
