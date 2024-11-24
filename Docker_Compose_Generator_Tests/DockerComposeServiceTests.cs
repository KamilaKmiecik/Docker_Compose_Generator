
using Xunit;
using Docker_Compose_Generator.Services;
using Docker_Compose_Generator.Models;
using System.Collections.Generic;
using System;

public class DockerComposeServiceTests
{
    private readonly DockerComposeService _service = new();

    [Fact]
    public void ThrowsArgumentException_WhenServiceNameIsEmpty()
    {
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO>
            {
                new ServiceDTO { Name = "", Image = "nginx" }
            }
        };
        Assert.Throws<ArgumentException>(() => _service.GenerateDockerComposeYaml(model));
    }

    [Fact]
    public void ThrowsArgumentException_WhenVolumeNameIsEmpty()
    {
        var model = new DockerComposeCreateDto
        {
            Volumes = new List<VolumeDTO>
            {
                new VolumeDTO { Name = "" }
            }
        };
        Assert.Throws<ArgumentException>(() => _service.GenerateDockerComposeYaml(model));
    }

    [Fact]
    public void ThrowsArgumentException_WhenNetworkNameIsEmpty()
    {
        var model = new DockerComposeCreateDto
        {
            Networks = new List<NetworkDTO>
            {
                new NetworkDTO { Name = "" }
            }
        };
        Assert.Throws<ArgumentException>(() => _service.GenerateDockerComposeYaml(model));
    }

    // Generation Tests
    [Fact]
    public void GeneratesValidYaml_WithSingleService()
    {
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO>
            {
                new ServiceDTO
                {
                    Name = "web",
                    Image = "nginx",
                    Ports = new List<Port>
                    {
                        new Port { HostPort = 80, ContainerPort = 80, Protocol = "tcp" }
                    }
                }
            }
        };

        var yaml = _service.GenerateDockerComposeYaml(model);
        Assert.Contains("web:", yaml);
        Assert.Contains("image: nginx", yaml);
        Assert.Contains("ports:", yaml);
        Assert.Contains("- 80:80", yaml);
    }

    [Fact]
    public void GeneratesValidYaml_WithEmptyServicesVolumesAndNetworks()
    {
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO>(),
            Volumes = new List<VolumeDTO>(),
            Networks = new List<NetworkDTO>()
        };

        var yaml = _service.GenerateDockerComposeYaml(model);
        Assert.Contains("version: '3.8'", yaml);
        Assert.DoesNotContain("services:", yaml);
        Assert.DoesNotContain("volumes:", yaml);
        Assert.DoesNotContain("networks:", yaml);
    }

    [Fact]
    public void GeneratesValidYaml_WithServiceMissingOptionalFields()
    {
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO>
            {
                new ServiceDTO
                {
                    Name = "app",
                    Image = "node"
                }
            }
        };

        var yaml = _service.GenerateDockerComposeYaml(model);
        Assert.Contains("app:", yaml);
        Assert.Contains("image: node", yaml);
        Assert.DoesNotContain("ports:", yaml);
        Assert.DoesNotContain("volumes:", yaml);
        Assert.DoesNotContain("networks:", yaml);
    }

    [Fact]
    public void GeneratesValidYaml_WithMultipleServices()
    {
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO>
            {
                new ServiceDTO
                {
                    Name = "web",
                    Image = "nginx",
                    Ports = new List<Port>
                    {
                        new Port { HostPort = 80, ContainerPort = 80, Protocol = "tcp" }
                    }
                },
                new ServiceDTO
                {
                    Name = "app",
                    Image = "node"
                }
            }
        };

        var yaml = _service.GenerateDockerComposeYaml(model);
        Assert.Contains("web:", yaml);
        Assert.Contains("image: nginx", yaml);
        Assert.Contains("app:", yaml);
        Assert.Contains("image: node", yaml);
    }
}
