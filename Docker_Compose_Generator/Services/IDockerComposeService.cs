using Docker_Compose_Generator.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Microsoft.Extensions.Hosting;

namespace Docker_Compose_Generator.Services
{
    public interface IDockerComposeService
    {
        string GenerateDockerComposeYaml(DockerComposeCreateDto model);
      
    }


}
