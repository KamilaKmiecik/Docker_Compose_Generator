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

        public string GenerateDockerComposeYaml(DockerComposeCreateDto model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Input model cannot be null.");

            if (string.IsNullOrWhiteSpace(model.Version))
                throw new ArgumentException("Version cannot be null or empty.", nameof(model.Version));

            var dockerCompose = DockerComposeEntity.Create(model.Version);

            if (model.Services?.Any() == true)
            {
                foreach (var serviceDto in model.Services)
                {
                    if (string.IsNullOrWhiteSpace(serviceDto.Name))
                        throw new ArgumentException("Service name cannot be null or empty.");
                    if (string.IsNullOrWhiteSpace(serviceDto.Image))
                        throw new ArgumentException($"Service '{serviceDto.Name}' must have a valid image.");

                    var ports = serviceDto.Ports?.Select(p =>
                        PortEntity.Create(p.HostPort, p.ContainerPort, p.Protocol)
                    ).ToList() ?? new List<PortEntity>();

                    var volumes = serviceDto.Volumes?.Select(v =>
                        VolumeEntity.Create(
                            v.Name ?? throw new ArgumentNullException(nameof(v.Name)),
                            v.Target ?? string.Empty, 
                            v.Source ?? string.Empty, 
                            v.AccessMode ?? string.Empty
                        )
                    ).ToList() ?? new List<VolumeEntity>();

                    var environment = serviceDto.Environment?.Select(e =>
                        EnvironmentEntity.Create(e.Key ?? string.Empty, e.Value ?? string.Empty)
                    ).ToList() ?? new List<EnvironmentEntity>();

                    var networks = serviceDto.Networks?.Select(n =>
                        NetworkEntity.Create(
                            n.Name ?? throw new ArgumentNullException(nameof(n.Name))//,
                          //  n.Driver,
                          //  n.DriverOptions
                        )
                    ).ToList() ?? new List<NetworkEntity>();

                    var restartPolicy = serviceDto.RestartPolicy != null
                        ? RestartPolicyEntity.Create(
                            serviceDto.RestartPolicy.Condition,
                            serviceDto.RestartPolicy.MaxRetries,
                            serviceDto.RestartPolicy.Delay
                        )
                        : null;

                    var service = ServiceEntity.Create(
                        serviceDto.Name,
                        serviceDto.Image,
                        ports,
                        volumes,
                        environment,
                        networks,
                        restartPolicy
                    );

                    dockerCompose.AddService(service);
                }
            }

            if (model.Volumes?.Any() == true)
            {
                foreach (var volumeDto in model.Volumes)
                {
                    if (string.IsNullOrWhiteSpace(volumeDto.Name))
                        throw new ArgumentException("Volume name cannot be null or empty.");

                    var volume = VolumeEntity.Create(
                        volumeDto.Name,
                        volumeDto.Target ?? string.Empty, 
                        volumeDto.Source ?? string.Empty, 
                        volumeDto.AccessMode ?? string.Empty,
                        volumeDto.Driver,
                        volumeDto.DriverOptions,
                        volumeDto.Labels,
                        volumeDto.External ?? false,
                        volumeDto.ReadOnly ?? false
                    );

                    dockerCompose.Volumes.Add(volume);
                }
            }

            if (model.Networks?.Any() == true)
            {
                foreach (var networkDto in model.Networks)
                {
                    if (string.IsNullOrWhiteSpace(networkDto.Name))
                        throw new ArgumentException("Network name cannot be null or empty.");

                    
                    var ipam = networkDto.Ipam != null
                       ? IPAMConfigurationEntity.Create(
                           networkDto.Ipam.Configuration.Subnet,
                           networkDto.Ipam.Configuration.Gateway,
                           networkDto.Ipam.Driver
                       )
                       : null;

                    var network = NetworkEntity.Create(
                        networkDto.Name,
                        networkDto.Driver,
                        networkDto.DriverOptions,
                        ipam
                    );

                    dockerCompose.Networks.Add(network);
                }
            }

            return dockerCompose.ToYaml();
        }


        public FileContentResult DownloadYaml(string yamlContent, ControllerBase controllerBase)
        {
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(yamlContent);
            return controllerBase.File(fileBytes, "application/octet-stream", "docker-compose.yml");
        }
    }
}
