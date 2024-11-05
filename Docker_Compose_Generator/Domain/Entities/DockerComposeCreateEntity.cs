using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Docker_Compose_Generator.Domain.Entities
{
    public class DockerComposeEntity
    {
        public string Version { get; private set; }
        public List<ServiceEntity> Services { get; private set; } = new List<ServiceEntity>();
        public List<VolumeEntity> Volumes { get; private set; } = new List<VolumeEntity>();
        public List<NetworkEntity> Networks { get; private set; } = new List<NetworkEntity>();

        private DockerComposeEntity(string version)
        {
            Version = version;
        }

        public static DockerComposeEntity Create(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                throw new ArgumentException("Version cannot be empty.");

            return new DockerComposeEntity(version);
        }

        public void AddService(ServiceEntity service)
        {
            Services.Add(service);
        }

        public string ToYaml()
        {
            var composeDict = new Dictionary<string, object>
            {
                { "version", Version },
                { "services", Services.ToDictionary(s => s.Name, s => s.GenerateServiceSection()) },
                { "volumes", Volumes.ToDictionary(v => v.Name, v => v.GenerateVolumeSection()) },
                { "networks", Networks.ToDictionary(n => n.Name, n => n.GenerateNetworkSection()) }
            };

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return serializer.Serialize(composeDict);
        }
    }

}
