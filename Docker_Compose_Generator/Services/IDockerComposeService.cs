using Docker_Compose_Generator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Docker_Compose_Generator.Services
{
    public interface IDockerComposeService
    {
        public string GenerateDockerComposeYaml(DockerComposeCreateDto model);
        public FileContentResult DownloadYaml(string yamlContent, ControllerBase controllerBase);

    }


}
