using Docker_Compose_Generator.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Docker_Compose_Generator.Services
{
    public interface IDockerComposeService
    {
        public string GenerateDockerComposeYaml(DockerComposeCreateDto model);
        public FileContentResult DownloadYaml(string yamlContent, ControllerBase controllerBase);

    }


}
