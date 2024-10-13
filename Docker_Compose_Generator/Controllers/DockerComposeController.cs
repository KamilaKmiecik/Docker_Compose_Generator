using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using YamlDotNet.Core;
using Docker_Compose_Generator.Validation;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using Docker_Compose_Generator.Models;
using Docker_Compose_Generator.Services;

namespace Docker_Compose_Generator.Controllers
{
    public class DockerComposeController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IDockerComposeService _dockerComposeService;

        public DockerComposeController(IWebHostEnvironment hostEnvironment, IDockerComposeService dockerComposeService)
        {
            _hostEnvironment = hostEnvironment;
            _dockerComposeService = dockerComposeService;
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

            try
            {
                //TODO: Fix validation
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

            var allowedExtensions = new[] { ".yaml", ".yml" };
            var fileExtension = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
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

        // GET: DockerCompose/RunToCompose
        public IActionResult RunToCompose()
        {
            return View();
        }

        // POST: DockerCompose/RunToCompose
        [HttpPost]
        public IActionResult RunToCompose(string dockerRunCommand)
        {
            if (string.IsNullOrEmpty(dockerRunCommand))
            {
                ModelState.AddModelError("dockerRunCommand", "Command cannot be empty.");
                return View();
            }

            var parts = dockerRunCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts[0] != "docker" || parts[1] != "run")
            {
                ModelState.AddModelError("dockerRunCommand", "Invalid 'docker run' command.");
                return View();
            }


            //TODO: Add version control
            var composeDict = new Dictionary<string, object>
            {
                { "version", "3.8" },
                { "services", new Dictionary<string, Dictionary<string, object>>() }
            };

            var service = new Dictionary<string, object>();
            var serviceName = "service_name";

            int i = 2;
            while (i < parts.Length)
            {
                switch (parts[i])
                {
                    case "--name":
                        service["container_name"] = parts[i + 1];
                        serviceName = parts[i + 1];
                        i++;
                        break;

                    case "-d":
                        service["detach"] = true;
                        break;

                    case "-p":
                        if (!service.ContainsKey("ports"))
                        {
                            service["ports"] = new List<string>();
                        }
                        ((List<string>)service["ports"]).Add(parts[i + 1]);
                        i++;
                        break;

                    case "-v":
                        if (!service.ContainsKey("volumes"))
                        {
                            service["volumes"] = new List<string>();
                        }
                        ((List<string>)service["volumes"]).Add(parts[i + 1]);
                        i++;
                        break;

                    case "-e":
                    case "--env":
                        if (!service.ContainsKey("environment"))
                        {
                            service["environment"] = new List<string>();
                        }
                        ((List<string>)service["environment"]).Add(parts[i + 1]);
                        i++;
                        break;

                    case "--network":
                        if (!service.ContainsKey("networks"))
                        {
                            service["networks"] = new List<string>();
                        }
                        ((List<string>)service["networks"]).Add(parts[i + 1]);
                        i++;
                        break;

                    case "--restart":
                        service["restart"] = parts[i + 1];
                        i++;
                        break;

                    default:
                        if (!service.ContainsKey("image"))
                        {
                            service["image"] = parts[i];
                        }
                        break;
                }
                i++;
            }

            var servicesDict = (Dictionary<string, Dictionary<string, object>>)composeDict["services"];
            servicesDict[serviceName] = service;

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            ViewBag.YamlContent = serializer.Serialize(composeDict);
            return View("Details");
        }


        // GET: DockerCompose/CreateUsingUI
        [HttpGet]
        public IActionResult CreateUsingUI()
        {
            return View();
        }

        // POST: DockerCompose/CreateUsingUI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsingUI(DockerComposeCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var yamlContent = _dockerComposeService.GenerateDockerComposeYaml(model);

                if(string.IsNullOrEmpty(yamlContent))
                    return View(model);

                return File(System.Text.Encoding.UTF8.GetBytes(yamlContent), "application/octet-stream", "docker-compose.yml");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", $"A{ex.Message}\n {ex.StackTrace}");
                return View(model);
            }
        }


    }
}
