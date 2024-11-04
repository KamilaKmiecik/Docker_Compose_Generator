using Docker_Compose_Generator.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Docker_Compose_Generator.Services
{
    public class DockerComposeService : IDockerComposeService
    {
        public string GenerateDockerComposeYaml(DockerComposeCreateDto model)
        {
            var composeDict = new Dictionary<string, object>
            {
                { "version", model.Version },
                { "services", new Dictionary<string, object>() },
            //    { "volumes", GenerateVolumesSection(model.Volumes) },
            //    { "networks", GenerateNetworksSection(model.Networks) }
            };

            var servicesDict = (Dictionary<string, object>)composeDict["services"];

            if(model.Services != null && model.Services.Count > 0)
                foreach (var service in model.Services)
                {
                    servicesDict[service.Name] = GenerateServiceSection(service);
                }

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            return serializer.Serialize(composeDict);
        }

        private Dictionary<string, object> GenerateServiceSection(ServiceDto service)
        {
            var serviceDict = new Dictionary<string, object>
            {
                { "image", service.Image },
                { "ports", GeneratePortsSection(service.Ports) },
                { "volumes", GenerateServiceVolumesSection(service.Volumes) },
                { "environment", GenerateEnvironmentSection(service.Environment) },
                { "networks", GenerateServiceNetworksSection(service.Networks) },
                { "restart", GenerateRestartPolicySection(service.RestartPolicy) }
            };

            return serviceDict
                .Where(kvp => kvp.Value != null && !(kvp.Value is System.Collections.IList list))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private List<string> GeneratePortsSection(List<Port>? ports)
        {
            return ports?.Select(p => $"{p.HostPort}:{p.ContainerPort}/{p.Protocol}").ToList() ?? new List<string>();
        }

        private List<string> GenerateServiceVolumesSection(List<Volume>? volumes)
        {
            return volumes?.Select(v => $"{v.Source}:{v.Target}:{v.AccessMode}").ToList() ?? new List<string>();
        }

        private Dictionary<string, string> GenerateEnvironmentSection(List<Models.Environment>? environment)
        {
            return environment?.ToDictionary(env => env.Key, env => env.Value) ?? new Dictionary<string, string>();
        }

        private List<string> GenerateServiceNetworksSection(List<Network>? networks)
        {
            return networks?.Select(n => n.Name).ToList() ?? new List<string>();
        }

        private string GenerateRestartPolicySection(RestartPolicy? restartPolicy)
        {
            if (restartPolicy == null)
                return string.Empty;

            var policy = restartPolicy.Condition;
            if (restartPolicy.MaxRetries.HasValue)
            {
                policy += $" (max_retries: {restartPolicy.MaxRetries})";
            }
            if (restartPolicy.Delay.HasValue)
            {
                policy += $" (delay: {restartPolicy.Delay.Value})";
            }

            return policy;
        }

        private Dictionary<string, object> GenerateVolumesSection(List<VolumeDTO>? volumes)
        {
            var volumesDict = new Dictionary<string, object>();
            foreach (var volume in volumes ?? new List<VolumeDTO>())
            {
                volumesDict[volume.Name] = new Dictionary<string, object>
        {
            { "driver", volume.Driver },
            { "driver_opts", volume.DriverOptions ?? new Dictionary<string, string>() },
            { "external", volume.External ?? false },
            { "labels", volume.Labels ?? new Dictionary<string, string>() },
            { "read_only", volume.ReadOnly ?? false }
        };
            }
            return volumesDict;
        }

        private Dictionary<string, object> GenerateNetworksSection(List<NetworkDTO>? networks)
        {
            var networksDict = new Dictionary<string, object>();
            foreach (var network in networks ?? new List<NetworkDTO>())
            {
                networksDict[network.Name] = new Dictionary<string, object>
        {
            { "driver", network.Driver },
            { "internal", network.Internal ?? false },
            { "attachable", network.Attachable ?? false },
            { "driver_opts", network.DriverOptions ?? new Dictionary<string, string>() },
            { "ipam", network.Ipam }
        };
            }
            return networksDict;
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

        public FileContentResult DownloadYaml(string yamlContent, ControllerBase controllerBase)
        {
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(yamlContent);

            var fileName = "docker-compose.yml";

            return controllerBase.File(fileBytes, "application/octet-stream", fileName);
        }

    }
}
