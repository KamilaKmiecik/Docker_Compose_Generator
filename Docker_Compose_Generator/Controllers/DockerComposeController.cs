using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using YamlDotNet.Core;
using Docker_Compose_Generator.Validation; 

namespace Docker_Compose_Generator.Controllers
{
    public class DockerComposeController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public DockerComposeController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        // GET: DockerCompose/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: DockerCompose/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DockerCompose/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string yamlContent)
        {
            if (string.IsNullOrEmpty(yamlContent))
            {
                ModelState.AddModelError("YamlContent", "Content cannot be empty.");
                return View();
            }


            //TODO: Wybieranie ścieżki z dysku
            try
            {
                ValidatorYaml.ValidateYaml(yamlContent);

                var filePath = Path.Combine(_hostEnvironment.WebRootPath, "docker-compose.yml");

                await System.IO.File.WriteAllTextAsync(filePath, yamlContent);

                return RedirectToAction(nameof(Index));
            }
            catch (YamlException ex)
            {
                ModelState.AddModelError("YamlContent", $"Invalid YAML format: {ex.Message}");
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("YamlContent", $"An error occurred: {ex.Message}");
                return View();
            }
        }

        // GET: DockerCompose/Read
        public IActionResult Read()
        {
            return View();
        }

        // POST: DockerCompose/Read
        [HttpPost]
        public async Task<IActionResult> Read(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please upload a valid Docker Compose file.");
                return View();
            }


            string yamlContent;
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                yamlContent = await stream.ReadToEndAsync();
            }

            ViewBag.YamlContent = yamlContent;
            return View("Details");
        }
    }
}
