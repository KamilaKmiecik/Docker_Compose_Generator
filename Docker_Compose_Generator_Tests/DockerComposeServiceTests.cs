
using Xunit;
using Docker_Compose_Generator.Services;
using Docker_Compose_Generator.Models;
using System.Collections.Generic;
using System;
using Docker_Compose_Generator.Controllers;
using Microsoft.AspNetCore.Mvc;

public class DockerComposeServiceTests
{
    // Testy jednostkowe dla modeli
    [Fact]
    public void Should_Add_Service_To_DockerComposeCreateDto()
    {
        var dockerCompose = new DockerComposeCreateDto();
        var service = new ServiceDTO { Name = "test-service", Image = "test-image" };

        dockerCompose.Services.Add(service);

        Assert.Single(dockerCompose.Services);
        Assert.Equal("test-service", dockerCompose.Services[0].Name);
    }

    [Fact]
    public void Should_Add_Network_To_DockerComposeCreateDto()
    {
        var dockerCompose = new DockerComposeCreateDto();
        var network = new NetworkDTO { Name = "test-network" };

        dockerCompose.Networks.Add(network);

        Assert.Single(dockerCompose.Networks);
        Assert.Equal("test-network", dockerCompose.Networks[0].Name);
    }

    [Fact]
    public void Should_Validate_Empty_Volume_List()
    {
        var dockerCompose = new DockerComposeCreateDto();

        Assert.Empty(dockerCompose.Volumes);
    }

    [Fact]
    public void Should_Create_Service_With_Valid_Name()
    {
        var service = new ServiceDTO { Name = "web", Image = "im" };

        Assert.Equal("web", service.Name);
    }

    [Fact]
    public void Should_Not_Allow_Empty_Service_Name()
    {
        var service = new ServiceDTO { Name = "", Image = "" };

        Assert.True(string.IsNullOrWhiteSpace(service.Name));
    }

    [Fact]
    public void Should_Add_Driver_To_Network()
    {
        var network = new NetworkDTO { Name = "test-network", Driver = "bridge" };

        Assert.Equal("bridge", network.Driver);
    }

    [Fact]
    public void Should_Validate_Volume_With_Correct_Name()
    {
        var volume = new VolumeDTO { Name = "volume1" };

        Assert.Equal("volume1", volume.Name);
    }

    [Fact]
    public void Should_Not_Allow_Empty_Volume_Name()
    {
        var volume = new VolumeDTO { Name = "" };

        Assert.True(string.IsNullOrWhiteSpace(volume.Name));
    }

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
        Assert.Contains("version: 3.8", yaml);
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
