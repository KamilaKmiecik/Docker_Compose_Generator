using Microsoft.AspNetCore.Mvc;

namespace Docker_Compose_Generator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
