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


        //!!!BARDZO WAŻNE - ŁADOWANIE FORMULARZA BEZ TEGO NIE DZIAŁA!!!!

        // GET: DockerCompose/CreateUsingUI
        [HttpGet]
        public IActionResult CreateUsingUI()
        {
            return View();
        }

        // POST: DockerCompose/CreateUsingUI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsingUI(
            string version,
            string image,
            string ports,
            string volumes,
            string environment,
            string networks,
            string restartPolicy
        )
        {
            if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(image))
            {
                ModelState.AddModelError("Validation", "Version and Image are required.");
                return View();
            }

            var composeDict = new Dictionary<string, object>
            {
                { "version", version },
                { "services", new Dictionary<string, Dictionary<string, object>>() }
            };

            var serviceDict = new Dictionary<string, object>
            {
                { "image", image }
            };

            //TEST DO DZIAŁANIA
            //TO DO - DODAĆ DYNAMICZNE DODAWANIE GUZIKIEM, UI NIE MA SENSU JAK KTOŚ NADAL MUSI SIĘ TYLE SAMO NAKOMBINOWAĆ :) !!!
            if (!string.IsNullOrEmpty(ports))
            {
                serviceDict["ports"] = ports.Split(',').Select(p => p.Trim()).ToList();
            }

            if (!string.IsNullOrEmpty(volumes))
            {
                serviceDict["volumes"] = volumes.Split(',').Select(v => v.Trim()).ToList();
            }

            if (!string.IsNullOrEmpty(environment))
            {
                serviceDict["environment"] = environment.Split(',').Select(e => e.Trim()).ToList();
            }

            if (!string.IsNullOrEmpty(networks))
            {
                serviceDict["networks"] = networks.Split(',').Select(n => n.Trim()).ToList();
            }

            if (!string.IsNullOrEmpty(restartPolicy))
            {
                serviceDict["restart"] = restartPolicy;
            }

            var servicesDict = (Dictionary<string, Dictionary<string, object>>)composeDict["services"];
            servicesDict["my_service"] = serviceDict; 

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yamlContent = serializer.Serialize(composeDict);
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, "docker-compose.yml");

            await System.IO.File.WriteAllTextAsync(filePath, yamlContent);

            return RedirectToAction(nameof(Index));
        }
    }
}
