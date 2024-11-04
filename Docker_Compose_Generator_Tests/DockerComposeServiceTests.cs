using Xunit;
using Moq;
using AutoMapper;
using FluentAssertions;
using Docker_Compose_Generator.Models;
using Docker_Compose_Generator.Services;
using Docker_Compose_Generator.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Environment = Docker_Compose_Generator.Models.Environment;

namespace Docker_Compose_Generator_Tests
{
    public class DockerComposeServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly DockerComposeService _service;

        public DockerComposeServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _service = new DockerComposeService(_mapperMock.Object);
        }

        [Fact]
        public void GenerateDockerComposeYaml_ShouldGenerateYamlCorrectly()
        {
            // Arrange
            var dto = new DockerComposeCreateDto
            {
                //Version = "3.8",
                Services = new List<ServiceDTO>
                {
                    new ServiceDTO { Name = "web", Image = "nginx:latest" }
                },
                Volumes = new List<VolumeDTO>(),
                Networks = new List<NetworkDTO>()
            };

            var serviceEntities = new List<ServiceEntity>
            {
                new ServiceEntity { Name = "web", Image = "nginx:latest" }
            };

            _mapperMock.Setup(m => m.Map<List<ServiceEntity>>(dto.Services)).Returns(serviceEntities);

            // Act
            var result = _service.GenerateDockerComposeYaml(dto);

            // Assert
            result.Should().Contain("version: 3.8");
            result.Should().Contain("services:");
            result.Should().Contain("web:");
            result.Should().Contain("image: nginx:latest");
        }


        [Fact]
        public void GenerateDockerComposeYaml_ShouldGenerateYamlWithVolumes()
        {
            // Arrange
            var dto = new DockerComposeCreateDto
            {
                Services = new List<ServiceDTO>
                {
                    new ServiceDTO { Name = "web", Image = "nginx:latest" }
                },
                Volumes = new List<VolumeDTO>
                {
                    new VolumeDTO { Name = "my-volume", Driver = "local" }
                },
                Networks = new List<NetworkDTO>()
            };

            var serviceEntities = new List<ServiceEntity>
            {
                new ServiceEntity { Name = "web", Image = "nginx:latest" }
            };

            var volumeEntities = new List<VolumeEntity>
            {
                new VolumeEntity { Name = "my-volume", Driver = "local" }
            };

            _mapperMock.Setup(m => m.Map<List<ServiceEntity>>(dto.Services)).Returns(serviceEntities);
            _mapperMock.Setup(m => m.Map<List<VolumeEntity>>(dto.Volumes)).Returns(volumeEntities);

            // Act
            var result = _service.GenerateDockerComposeYaml(dto);

            // Assert
            result.Should().Contain("volumes:");
            result.Should().Contain("my-volume:");
            result.Should().Contain("driver: local");
        }

        [Fact]
        public void GenerateDockerComposeYaml_ShouldGenerateYamlWithNetworks()
        {
            // Arrange
            var dto = new DockerComposeCreateDto
            {
                Services = new List<ServiceDTO>
                {
                    new ServiceDTO { Name = "web", Image = "nginx:latest" }
                },
                Volumes = new List<VolumeDTO>(),
                Networks = new List<NetworkDTO>
                {
                    new NetworkDTO { Name = "my-network", Driver = "bridge" }
                }
            };

            var serviceEntities = new List<ServiceEntity>
            {
                new ServiceEntity { Name = "web", Image = "nginx:latest" }
            };

            var networkEntities = new List<NetworkEntity>
            {
                new NetworkEntity { Name = "my-network", Driver = "bridge" }
            };

            _mapperMock.Setup(m => m.Map<List<ServiceEntity>>(dto.Services)).Returns(serviceEntities);
            _mapperMock.Setup(m => m.Map<List<NetworkEntity>>(dto.Networks)).Returns(networkEntities);

            // Act
            var result = _service.GenerateDockerComposeYaml(dto);

            // Assert
            result.Should().Contain("networks:");
            result.Should().Contain("my-network:");
            result.Should().Contain("driver: bridge");
        }


        [Fact]
        public void GenerateDockerComposeYaml_ShouldGenerateYamlWithEnvironmentVariables()
        {
            // Arrange
            var dto = new DockerComposeCreateDto
            {
                Services = new List<ServiceDTO>
                {
                    new ServiceDTO
                    {
                        Name = "web",
                        Image = "nginx:latest",
                        Environment = new List<Docker_Compose_Generator.Models.Environment>
                        {
                            new Environment { Key = "ENV_VAR", Value = "value" }
                        }
                    }
                },
                Volumes = new List<VolumeDTO>(),
                Networks = new List<NetworkDTO>()
            };

            var serviceEntities = new List<ServiceEntity>
            {
                new ServiceEntity
                {
                    Name = "web",
                    Image = "nginx:latest",
                    Environment = new List<EnvironmentEntity>
                    {
                        new EnvironmentEntity { Key = "ENV_VAR", Value = "value" }
                    }
                }
            };

            _mapperMock.Setup(m => m.Map<List<ServiceEntity>>(dto.Services)).Returns(serviceEntities);

            // Act
            var result = _service.GenerateDockerComposeYaml(dto);

            // Assert
            result.Should().Contain("environment:");
            result.Should().Contain("ENV_VAR: value");
        }

        //    [Fact]
        //    public void GenerateDockerComposeYaml_ShouldIncludeRestartPolicy()
        //    {
        //        // Arrange
        //        var dto = new DockerComposeCreateDto
        //        {
        //            Services = new List<ServiceDTO>
        //            {
        //                new ServiceDTO
        //                {
        //                    Name = "web",
        //                    Image = "nginx:latest",
        //                    RestartPolicy = new RestartPolicy
        //                    {
        //                        Condition = "always",
        //                        MaxRetries = 5,
        //                        Delay = new TimeSpan(1)
        //                    }
        //                }
        //            },
        //            Volumes = new List<VolumeDTO>(),
        //            Networks = new List<NetworkDTO>()
        //        };

        //        var serviceEntities = new List<ServiceEntity>
        //        {
        //            new ServiceEntity
        //            {
        //                Name = "web",
        //                Image = "nginx:latest",
        //                RestartPolicy = new RestartPolicyEntity
        //                {
        //                    Condition = "always",
        //                    MaxRetries = 5,
        //                    Delay = new TimeSpan(10)
        //                }
        //            }
        //        };

        //        _mapperMock.Setup(m => m.Map<List<ServiceEntity>>(dto.Services)).Returns(serviceEntities);

        //        // Act
        //        var result = _service.GenerateDockerComposeYaml(dto);

        //        // Assert
        //        result.Should().Contain("restart: always (max_retries: 5) (delay: 10)");
        //    }

        //}


    }
}