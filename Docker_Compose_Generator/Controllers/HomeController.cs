using Microsoft.AspNetCore.Mvc;
using Docker_Compose_Generator.Models;
using YamlDotNet.Serialization;
using Docker_Compose_Generator.Models;

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
