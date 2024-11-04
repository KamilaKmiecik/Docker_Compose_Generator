using Xunit;
using Moq;
using AutoMapper;
using FluentAssertions;
using Docker_Compose_Generator.Models;
using Docker_Compose_Generator.Services;
using Docker_Compose_Generator.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public void DownloadYaml_ShouldReturnFileContentResult()
        {
            // Arrange
            var yamlContent = "version: '3.8'";
            var controllerMock = new Mock<ControllerBase>();
            var expectedFileName = "docker-compose.yml";
            var expectedContentType = "application/octet-stream";

            // Act
            var result = _service.DownloadYaml(yamlContent, controllerMock.Object);

            // Assert
            result.Should().BeOfType<FileContentResult>();
            result.FileDownloadName.Should().Be(expectedFileName);
            result.ContentType.Should().Be(expectedContentType);
            result.FileContents.Should().BeEquivalentTo(System.Text.Encoding.UTF8.GetBytes(yamlContent));
        }
    }
}
