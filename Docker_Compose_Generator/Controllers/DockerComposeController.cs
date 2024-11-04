using Microsoft.AspNetCore.Mvc;
using Docker_Compose_Generator.Models;
using Docker_Compose_Generator.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Docker_Compose_Generator.Controllers
{
    public class DockerComposeController : Controller
    {
        private readonly IDockerComposeService _dockerComposeService;

        public DockerComposeController(IDockerComposeService dockerComposeService)
        {
            _dockerComposeService = dockerComposeService;
        }

        public IActionResult Create()
        {
            // Inicjalizujemy pusty obiekt DockerComposeCreateDto
            var model = new DockerComposeCreateDto();
            return View(model);
        }

        public IActionResult NetworkListPartial(List<NetworkDTO> model)
        {
            return PartialView("_NetworkListPartial", model ?? new List<NetworkDTO>());
        }

        public IActionResult ServiceListPartial(List<ServiceDto> model)
        {
            return PartialView("_ServiceListPartial", model ?? new List<ServiceDto>());
        }

        public IActionResult VolumeListPartial(List<VolumeDTO> model)
        {
            return PartialView("_VolumeListPartial", model ?? new List<VolumeDTO>());
        }


        [HttpPost]
        public IActionResult Create(DockerComposeCreateDto model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Success");
            }
            return View(model);
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

                return File(System.Text.Encoding.UTF8.GetBytes(yamlContent), "application/octet-stream", "docker-compose.yml");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult GetServicePartial(int index)
        {
            var model = new ServiceDto() { Name = "", Image = ""}; 
            ViewData["index"] = index;
            return PartialView("_ServicePartial", model);
        }

        [HttpGet]
        public IActionResult GetNetworkPartial(int index)
        {
            var model = new NetworkDTO() { Name = "" };
            ViewData["index"] = index;
            return PartialView("_NetworkPartial", model);
        }

        [HttpGet]
        public IActionResult GetVolumePartial(int index)
        {
            var model = new VolumeDTO() { Name = "" }; 
            ViewData["index"] = index;
            return PartialView("_VolumePartial", model);
        }


        public IActionResult GetServicePartial(int serviceIndex, ServiceDto portDto)
        {
            ViewData["serviceIndex"] = serviceIndex; // Przekazanie indeksu usługi do widoku
            return PartialView("AddPortPartial", portDto);
        }


        public IActionResult GetVolumePartial(VolumeDTO volumeDto)
        {
            return PartialView("AddVolumePartial", volumeDto);
        }


        public IActionResult GetNetworkPartial(NetworkDTO networkDto)
        {
            return PartialView("AddNetworkPartial", networkDto);
        }

        [HttpGet]
        public IActionResult GetPartial(string type, int index)
        {
            ViewData["Index"] = index;

            return type switch
            {
                "service" => PartialView("_ServicePartial", new ServiceDto { Name = "", Image = "" }),
                "network" => PartialView("_NetworkPartial", new NetworkDTO { Name = "" }),
                "volume" => PartialView("_VolumePartial", new VolumeDTO { Name = "" }),
                _ => BadRequest()
            };
        }

    }
}
