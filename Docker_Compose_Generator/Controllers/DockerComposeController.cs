using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Docker_Compose_Generator.Data;
using Docker_Compose_Generator.Models;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Docker_Compose_Generator.Controllers
{
    public class DockerComposeController : Controller
    {
        private readonly DockerComposeContext _context;

        public DockerComposeController(DockerComposeContext context)
        {
            _context = context;
        }

        // GET: DockerCompose
        public async Task<IActionResult> Index()
        {
            var configurations = await _context.ComposeConfigurations
                .Include(c => c.ServiceModels)
                .Include(c => c.Networks)
                .ToListAsync();
            return View(configurations);
        }

        // GET: DockerCompose/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DockerCompose/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DockerComposeModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: DockerCompose/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var configuration = await _context.ComposeConfigurations
                .Include(c => c.ServiceModels)
                .Include(c => c.Networks)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (configuration == null)
            {
                return NotFound();
            }

            return View(configuration);
        }

        // GET: DockerCompose/Generate/5
        public async Task<IActionResult> Generate(int id)
        {
            var configuration = await _context.ComposeConfigurations
                .Include(c => c.ServiceModels)
                .Include(c => c.Networks)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (configuration == null)
            {
                return NotFound();
            }

            var content = GenerateDockerComposeContent(configuration);
            var fileName = "docker-compose.yml";
            var mimeType = "application/x-yaml";
            var bytes = Encoding.UTF8.GetBytes(content);

            return File(bytes, mimeType, fileName);
        }

        private string GenerateDockerComposeContent(DockerComposeModel model)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"version: '3.8'");
            sb.AppendLine("services:");

            foreach (var service in model.ServiceModels)
            {
                sb.AppendLine($"  {service.ServiceName}:");
                sb.AppendLine($"    image: {service.Image}");
                if (service.Port.HasValue)
                {
                    sb.AppendLine($"    ports:");
                    sb.AppendLine($"      - \"{service.Port}\"");
                }
                if (!string.IsNullOrEmpty(service.EnvironmentVariables))
                {
                    sb.AppendLine($"    environment:");
                    sb.AppendLine($"      - {service.EnvironmentVariables}");
                }
                if (!string.IsNullOrEmpty(service.Volumes))
                {
                    sb.AppendLine($"    volumes:");
                    sb.AppendLine($"      - {service.Volumes}");
                }
            }

            if (model.Networks.Any())
            {
                sb.AppendLine("networks:");
                foreach (var network in model.Networks)
                {
                    sb.AppendLine($"  {network.Name}:");
                }
            }

            return sb.ToString();
        }
    }
}
