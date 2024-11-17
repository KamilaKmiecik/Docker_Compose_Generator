using Docker_Compose_Generator.Extensions;
using Docker_Compose_Generator.Models;
using Docker_Compose_Generator.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public IActionResult CreateUsingUI()
        {
            var model = HttpContext.Session.GetObjectFromJson<DockerComposeCreateDto>("DockerComposeModel");

            if (model == null)
            {
                model = new DockerComposeCreateDto();
            }

            return View(model);
        }

        // POST: DockerCompose/CreateUsingUI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsingUI(DockerComposeCreateDto model)
        {
            HttpContext.Session.SetObjectAsJson("DockerComposeModel", model);

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
    }
}
