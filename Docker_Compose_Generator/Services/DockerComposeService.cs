using Docker_Compose_Generator.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace Docker_Compose_Generator.Services
{
    public class DockerComposeService : IDockerComposeService
    {
        public string GenerateDockerComposeYaml(DockerComposeCreateDto model)
        {
            var composeDict = new Dictionary<string, object>
            {
                { "version", model.Version },
                { "services", new Dictionary<string, Dictionary<string, object>>() }
            };

            var servicesDict = (Dictionary<string, Dictionary<string, object>>)composeDict["services"];

            foreach (var service in model.Services)
            {
                var serviceDict = new Dictionary<string, object>
                {
                    { "image", service.Image }
                };

                AddOptionalProperty(serviceDict, "ports", service.Ports);
                AddOptionalProperty(serviceDict, "volumes", service.Volumes);
                AddOptionalProperty(serviceDict, "environment", service.Environment);
                AddOptionalProperty(serviceDict, "networks", service.Networks);
                AddOptionalProperty(serviceDict, "restart", service.RestartPolicy);

                servicesDict[service.Name] = serviceDict;
            }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return serializer.Serialize(composeDict);
        }

        private void AddOptionalProperty(Dictionary<string, object> serviceDict, string key, List<string>? values)
        {
            if (values != null && values.Any())
            {
                serviceDict[key] = values;
            }
        }

        private void AddOptionalProperty(Dictionary<string, object> serviceDict, string key, string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                serviceDict[key] = value;
            }
        }

    }
}
