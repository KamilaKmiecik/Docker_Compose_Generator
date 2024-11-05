using Docker_Compose_Generator.Models;
using Docker_Compose_Generator.Services;
using Microsoft.AspNetCore.Mvc;

namespace Docker_Compose_Generator.Controllers
{
    public class DockerComposeController : Controller
    {
        private readonly IDockerComposeService _dockerComposeService;

        public DockerComposeController(IDockerComposeService dockerComposeService)
        {
            _dockerComposeService = dockerComposeService;
        }

        // GET: DockerCompose/CreateUsingUI
        [HttpGet]
        public IActionResult CreateUsingUI()
        {
            var model = new DockerComposeCreateDto(); // Nowy model do przekazania do widoku
            return View(model);
        }

        // POST: DockerCompose/CreateUsingUI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsingUI(DockerComposeCreateDto model)
        {
                //if (!ModelState.IsValid)
                //{
                //    return View(model); 
                //}

            try
            {
                var yamlContent = _dockerComposeService.GenerateDockerComposeYaml(model);

                return File(System.Text.Encoding.UTF8.GetBytes(yamlContent), "application/octet-stream", "docker-compose.yml");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.InnerException.Message;
                // ModelState.AddModelError("Error", $"An error occurred: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult GetServicePartial(int index)
        {
            var model = new ServiceDTO() { Name = "", Image = "" };
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


        public IActionResult GetServicePartial(int serviceIndex, ServiceDTO portDto)
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


    }
}
