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

        [HttpPost]
        public IActionResult GenerateComposeFile([FromBody] ServiceModel[] services)
        {
            if (services == null || services.Length == 0)
            {
                return BadRequest("Brak usług do wygenerowania pliku docker-compose.");
            }

            try
            {
                // Tworzenie obiektu docker-compose na podstawie przekazanych usług
                var dockerCompose = new DockerComposeModel();

                foreach (var service in services)
                {
                    var serviceConfig = new ServiceConfiguration
                    {
                        Image = service.Image,
                        Ports = service.Port.HasValue ? new[] { $"{service.Port}:80" } : null,
                        Environment = service.EnvironmentVariables?.Split(','),
                        Volumes = service.Volumes?.Split(',')
                    };

                    dockerCompose.Services.Add(service.ServiceName, serviceConfig);
                }

                // Serializacja obiektu docker-compose do formatu YAML
                var serializer = new SerializerBuilder().Build();
                var yaml = serializer.Serialize(dockerCompose);

                // Tutaj możesz zapisać plik YAML do docelowego katalogu

                return Ok(yaml); // Zwróć plik YAML jako odpowiedź HTTP
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Wystąpił błąd podczas generowania pliku docker-compose: {ex.Message}");
            }
        }
    }
}
