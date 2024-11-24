using Docker_Compose_Generator.Extensions;
using Docker_Compose_Generator.Models;
using Docker_Compose_Generator.Services;
using Microsoft.AspNetCore.Http;
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


        private DockerComposeCreateDto GetCurrentModel()
        {
            var model = HttpContext.Session.GetObjectFromJson<DockerComposeCreateDto>("DockerComposeModel");
            return model ?? new DockerComposeCreateDto(); 
        }

        private void SaveCurrentModel(DockerComposeCreateDto model)
        {
            HttpContext.Session.SetObjectAsJson("DockerComposeModel", model);
        }

        // GET: DockerCompose/CreateUsingUI
        public IActionResult CreateUsingUI()
        {
            var model = GetCurrentModel();
            return View(model);
        }

        // POST: DockerCompose/CreateUsingUI
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsingUI(DockerComposeCreateDto model)
        {
            SaveCurrentModel(model);

            HttpContext.Session.SetObjectAsJson("DockerComposeModel", model);

            try
            {
                var yamlContent = _dockerComposeService.GenerateDockerComposeYaml(model);

                return File(System.Text.Encoding.UTF8.GetBytes(yamlContent), "application/octet-stream", "docker-compose.yml");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("Error", $"An error occurred: {ex.InnerException.Message}");
                    TempData["ErrorMessage"] = ex.InnerException.Message;
                }
                else
                {
                    ModelState.AddModelError("Error", $"An error occurred: {ex.Message}");
                    TempData["ErrorMessage"] = ex.Message;
                }

            

                return View(model);
            }
          
        }

        // Dynamiczne widoki częściowe
        [HttpGet]
        public IActionResult GetServicePartial(int index)
        {
            var model = new ServiceDTO { Name = "", Image = "" };
            ViewData["index"] = index;
            return PartialView("_ServicePartial", model);
        }

        [HttpGet]
        public IActionResult GetNetworkPartial(int index)
        {
            var model = new NetworkDTO { Name = "" };
            ViewData["index"] = index;
            return PartialView("_NetworkPartial", model);
        }

        [HttpGet]
        public IActionResult GetVolumePartial(int index)
        {
            var model = new VolumeDTO { Name = "" };
            ViewData["index"] = index;
            return PartialView("_VolumePartial", model);
        }

        // Dodawanie dynamiczne elementów
        [HttpPost]
        public IActionResult AddService([FromBody] ServiceDTO newService)
        {
            if (newService == null || string.IsNullOrWhiteSpace(newService.Name))
            {
                return Json(new { success = false, message = "Invalid service configuration." });
            }

            var model = GetCurrentModel();

            if(model.Services != null)
                model.Services.Add(newService);

            SaveCurrentModel(model);

            return Json(new { success = true, message = "Service added successfully.", newService });
        }

        [HttpPost]
        public IActionResult AddNetwork([FromBody] NetworkDTO newNetwork)
        {
            if (newNetwork == null || string.IsNullOrWhiteSpace(newNetwork.Name))
            {
                return Json(new { success = false, message = "Invalid network configuration." });
            }

            var model = GetCurrentModel();
            
            if(model.Networks != null)
            model.Networks.Add(newNetwork);
            SaveCurrentModel(model);

            return Json(new { success = true, message = "Network added successfully.", newNetwork });
        }

        [HttpPost]
        public IActionResult AddVolume([FromBody] VolumeDTO newVolume)
        {
            if (newVolume == null || string.IsNullOrWhiteSpace(newVolume.Name))
            {
                return Json(new { success = false, message = "Invalid volume configuration." });
            }

            var model = GetCurrentModel();

            if(model.Volumes != null)
                model.Volumes.Add(newVolume);
            SaveCurrentModel(model);

            return Json(new { success = true, message = "Volume added successfully.", newVolume });
        }

        [HttpPost]
        public IActionResult RemoveService(int index)
        {
            var model = GetCurrentModel();

            if (model.Services != null && index >= 0 && index < model.Services.Count)
            {
                model.Services.RemoveAt(index);
                SaveCurrentModel(model);
            }

            return Json(new { success = true, message = "Service removed successfully." });
        }

        [HttpPost]
        public IActionResult RemoveNetwork(int index)
        {
            var model = GetCurrentModel();

            if (model.Networks != null && index >= 0 && index < model.Networks.Count)
            {
                model.Networks.RemoveAt(index);
                SaveCurrentModel(model);
            }

            return Json(new { success = true, message = "Network removed successfully." });
        }

        [HttpPost]
        public IActionResult RemoveVolume(int index)
        {
            var model = GetCurrentModel();

            if (model.Volumes != null && index >= 0 && index < model.Volumes.Count)
            {
                model.Volumes.RemoveAt(index);
                SaveCurrentModel(model);
            }

            return Json(new { success = true, message = "Volume removed successfully." });
        }

    }
}
