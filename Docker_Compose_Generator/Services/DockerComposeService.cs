using AutoMapper;
using Docker_Compose_Generator.Domain.Entities;
using Docker_Compose_Generator.Models;
using Microsoft.AspNetCore.Mvc;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Docker_Compose_Generator.Services
{
    public class DockerComposeService : IDockerComposeService
    {
        private readonly IMapper _mapper;

        private const string VersionKey = "version";
        private const string ServicesKey = "services";
        private const string VolumesKey = "volumes";
        private const string NetworksKey = "networks";
        private const string ImageKey = "image";
        private const string PortsKey = "ports";
        private const string VolumesKeyInService = "volumes";
        private const string EnvironmentKey = "environment";
        private const string NetworksKeyInService = "networks";
        private const string RestartKey = "restart";
        private const string DriverKey = "driver";
        private const string DriverOptionsKey = "driver_opts";
        private const string ExternalKey = "external";
        private const string LabelsKey = "labels";
        private const string ReadOnlyKey = "read_only";
        private const string InternalKey = "internal";
        private const string AttachableKey = "attachable";
        private const string IpamKey = "ipam";

        public DockerComposeService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string GenerateDockerComposeYaml(DockerComposeCreateDto model)
        {
            var volumeEntities = _mapper.Map<List<VolumeEntity>>(model.Volumes);
            var networkEntities = _mapper.Map<List<NetworkEntity>>(model.Networks);
            var serviceEntities = _mapper.Map<List<ServiceEntity>>(model.Services);

            var composeDict = new Dictionary<string, object>
            {
                { VersionKey, model.Version },
                { ServicesKey, new Dictionary<string, object>() },
                { VolumesKey, GenerateVolumesSection(volumeEntities) },
                { NetworksKey, GenerateNetworksSection(networkEntities) }
            };

            var servicesDict = (Dictionary<string, object>)composeDict[ServicesKey];
            foreach (var service in serviceEntities)
            {
                servicesDict[service.Name] = GenerateServiceSection(service);
            }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return serializer.Serialize(composeDict);
        }

        private Dictionary<string, object> GenerateServiceSection(ServiceEntity service)
        {
            var serviceDict = new Dictionary<string, object>
            {
                { ImageKey, service.Image },
                { PortsKey, GeneratePortsSection(service.Ports) },
                { VolumesKeyInService, GenerateServiceVolumesSection(service.Volumes) },
                { EnvironmentKey, GenerateEnvironmentSection(service.Environment) },
                { NetworksKeyInService, GenerateServiceNetworksSection(service.Networks) },
            };

            return serviceDict;
        }

        private List<string> GeneratePortsSection(List<PortEntity>? ports)
        {
            return ports?.Select(p => $"{p.HostPort}:{p.ContainerPort}/{p.Protocol}").ToList() ?? new List<string>();
        }

        private List<string> GenerateServiceVolumesSection(List<VolumeEntity>? volumes)
        {
            return volumes?.Select(v => $"{v.Source}:{v.Target}:{v.AccessMode}").ToList() ?? new List<string>();
        }

        private Dictionary<string, string> GenerateEnvironmentSection(List<EnvironmentEntity>? environment)
        {
            return environment?.ToDictionary(env => env.Key, env => env.Value) ?? new Dictionary<string, string>();
        }

        private List<string> GenerateServiceNetworksSection(List<NetworkEntity>? networks)
        {
            return networks?.Select(n => n.Name).ToList() ?? new List<string>();
        }

        private string GenerateRestartPolicySection(RestartPolicy? restartPolicy)
        {
            if (restartPolicy == null) return string.Empty;
            var policy = restartPolicy.Condition;
            if (restartPolicy.MaxRetries.HasValue) policy += $" (max_retries: {restartPolicy.MaxRetries})";
            if (restartPolicy.Delay.HasValue) policy += $" (delay: {restartPolicy.Delay.Value})";
            return policy;
        }

        private Dictionary<string, object> GenerateVolumesSection(List<VolumeEntity>? volumes)
        {
            var volumesDict = new Dictionary<string, object>();
            foreach (var volume in volumes ?? new List<VolumeEntity>())
            {
                volumesDict[volume.Name] = new Dictionary<string, object>
                {
                    { DriverKey, volume.Driver },
                    { DriverOptionsKey, volume.DriverOptions ?? new Dictionary<string, string>() },
                    { ExternalKey, volume.External ?? false },
                    { LabelsKey, volume.Labels ?? new Dictionary<string, string>() },
                    { ReadOnlyKey, volume.ReadOnly ?? false }
                };
            }
            return volumesDict;
        }

        private Dictionary<string, object> GenerateNetworksSection(List<NetworkEntity>? networks)
        {
            var networksDict = new Dictionary<string, object>();
            foreach (var network in networks ?? new List<NetworkEntity>())
            {
                networksDict[network.Name] = new Dictionary<string, object>
                {
                    { DriverKey, network.Driver },
                    { InternalKey, network.Internal ?? false },
                    { AttachableKey, network.Attachable ?? false },
                    { DriverOptionsKey, network.DriverOptions ?? new Dictionary<string, string>() },
                    { IpamKey, network.Ipam }
                };
            }
            return networksDict;
        }

        public FileContentResult DownloadYaml(string yamlContent, ControllerBase controllerBase)
        {
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(yamlContent);
            return controllerBase.File(fileBytes, "application/octet-stream", "docker-compose.yml");
        }
    }
}
