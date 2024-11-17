using Xunit;
using Moq;
using Docker_Compose_Generator.Controllers;
using Docker_Compose_Generator.Models;
using Docker_Compose_Generator.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class DockerComposeControllerTests
{
    private readonly Mock<IDockerComposeService> _dockerComposeServiceMock;
    private readonly DockerComposeController _controller;

    public DockerComposeControllerTests()
    {
        _dockerComposeServiceMock = new Mock<IDockerComposeService>();
        _controller = new DockerComposeController(_dockerComposeServiceMock.Object);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldReturnFile_WhenModelHasServices()
    {
        // Arrange
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO> { new ServiceDTO { Name = "web", Image = "nginx" } }
        };
        var yamlContent = "version: '3'\nservices:\n  web:\n    image: nginx";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(yamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("docker-compose.yml", fileResult.FileDownloadName);
        Assert.Contains("web", System.Text.Encoding.UTF8.GetString(fileResult.FileContents));
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldHandleEmptyModelCorrectly()
    {
        // Arrange
        var model = new DockerComposeCreateDto();
        var yamlContent = "version: '3'\nservices: []";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(yamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("docker-compose.yml", fileResult.FileDownloadName);
        Assert.Contains("services: []", System.Text.Encoding.UTF8.GetString(fileResult.FileContents));
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldHandleInvalidDockerComposeCreateDto()
    {
        // Arrange
        var model = new DockerComposeCreateDto();
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Throws(new ArgumentException("Invalid data"));

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
        Assert.Equal("Invalid data", _controller.TempData["ErrorMessage"]);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldHandleServiceException()
    {
        // Arrange
        var model = new DockerComposeCreateDto();
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Throws(new Exception("Service error"));

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
        Assert.Equal("Service error", _controller.TempData["ErrorMessage"]);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldReturnFile_WhenMultipleNetworksExist()
    {
        // Arrange
        var model = new DockerComposeCreateDto
        {
            Networks = new List<NetworkDTO>
        {
            new NetworkDTO { Name = "frontend" },
            new NetworkDTO { Name = "backend" }
        }
        };
        var yamlContent = "version: '3'\nnetworks:\n  frontend:\n  backend:\n";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(yamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("docker-compose.yml", fileResult.FileDownloadName);
        Assert.Contains("frontend", System.Text.Encoding.UTF8.GetString(fileResult.FileContents));
        Assert.Contains("backend", System.Text.Encoding.UTF8.GetString(fileResult.FileContents));
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldHandleLargeModelData()
    {
        // Arrange
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO> { new ServiceDTO { Name = "web", Image = "nginx" } },
            Networks = new List<NetworkDTO> { new NetworkDTO { Name = "net1" } },
            Volumes = new List<VolumeDTO> { new VolumeDTO { Name = "vol1" } }
        };
        var yamlContent = "version: '3'\nservices:\n  web:\n    image: nginx\nnetworks:\n  net1:\nvolumes:\n  vol1:\n";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(yamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("docker-compose.yml", fileResult.FileDownloadName);
        Assert.Contains("web", System.Text.Encoding.UTF8.GetString(fileResult.FileContents));
        Assert.Contains("net1", System.Text.Encoding.UTF8.GetString(fileResult.FileContents));
        Assert.Contains("vol1", System.Text.Encoding.UTF8.GetString(fileResult.FileContents));
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldReturnViewWhenModelIsNull()
    {
        // Arrange
        DockerComposeCreateDto model = null;

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.Model);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldGenerateDockerComposeV3_8_WhenValidModel()
    {
        // Arrange
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO>
        {
            new ServiceDTO { Name = "web", Image = "nginx" }
        }
        };
        var expectedYamlContent = "version: '3.8'\nservices:\n  web:\n    image: nginx";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(expectedYamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        var yamlResult = System.Text.Encoding.UTF8.GetString(fileResult.FileContents);
        Assert.Contains("version: '3.8'", yamlResult);
        Assert.Contains("web", yamlResult);
        Assert.Contains("nginx", yamlResult);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldGenerateDockerComposeV3_8_WithMultipleServices()
    {
        // Arrange
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO>
        {
            new ServiceDTO { Name = "web", Image = "nginx" },
            new ServiceDTO { Name = "db", Image = "postgres" }
        }
        };
        var expectedYamlContent = "version: '3.8'\nservices:\n  web:\n    image: nginx\n  db:\n    image: postgres";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(expectedYamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        var yamlResult = System.Text.Encoding.UTF8.GetString(fileResult.FileContents);
        Assert.Contains("version: '3.8'", yamlResult);
        Assert.Contains("web", yamlResult);
        Assert.Contains("nginx", yamlResult);
        Assert.Contains("db", yamlResult);
        Assert.Contains("postgres", yamlResult);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldGenerateDockerComposeV3_8_WithNetworks()
    {
        // Arrange
        var model = new DockerComposeCreateDto
        {
            Networks = new List<NetworkDTO>
        {
            new NetworkDTO { Name = "frontend" },
            new NetworkDTO { Name = "backend" }
        }
        };
        var expectedYamlContent = "version: '3.8'\networks:\n  frontend:\n  backend:\n";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(expectedYamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        var yamlResult = System.Text.Encoding.UTF8.GetString(fileResult.FileContents);
        Assert.Contains("version: '3.8'", yamlResult);
        Assert.Contains("frontend", yamlResult);
        Assert.Contains("backend", yamlResult);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldGenerateDockerComposeV3_8_WithVolumes()
    {
        // Arrange
        var model = new DockerComposeCreateDto
        {
            Volumes = new List<VolumeDTO>
        {
            new VolumeDTO { Name = "vol1" },
            new VolumeDTO { Name = "vol2" }
        }
        };
        var expectedYamlContent = "version: '3.8'\nvolumes:\n  vol1:\n  vol2:\n";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(expectedYamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        var yamlResult = System.Text.Encoding.UTF8.GetString(fileResult.FileContents);
        Assert.Contains("version: '3.8'", yamlResult);
        Assert.Contains("vol1", yamlResult);
        Assert.Contains("vol2", yamlResult);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldGenerateDockerComposeV3_8_WithServicePorts()
    {
        // Arrange
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO>
        {
            new ServiceDTO { Name = "web", Image = "nginx" }
        }
        };
        var expectedYamlContent = "version: '3.8'\nservices:\n  web:\n    image: nginx\n    ports:\n      - '80:80'";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(expectedYamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        var yamlResult = System.Text.Encoding.UTF8.GetString(fileResult.FileContents);
        Assert.Contains("version: '3.8'", yamlResult);
        Assert.Contains("web", yamlResult);
        Assert.Contains("nginx", yamlResult);
        Assert.Contains("80:80", yamlResult);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldGenerateDockerComposeV3_8_WithEnvironmentVariables()
    {
        // Arrange
        var model = new DockerComposeCreateDto
        {
            Services = new List<ServiceDTO>
        {
            new ServiceDTO
            {
                Name = "web",
                Image = "nginx",
            }
        }
        };
        var expectedYamlContent = "version: '3.8'\nservices:\n  web:\n    image: nginx\n    environment:\n      - ENV_VAR=production\n      - DEBUG=true";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(expectedYamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        var yamlResult = System.Text.Encoding.UTF8.GetString(fileResult.FileContents);
        Assert.Contains("version: '3.8'", yamlResult);
        Assert.Contains("web", yamlResult);
        Assert.Contains("nginx", yamlResult);
        Assert.Contains("ENV_VAR=production", yamlResult);
        Assert.Contains("DEBUG=true", yamlResult);
    }

    [Fact]
    public async Task CreateUsingUI_Get_ShouldReturnViewWithModel()
    {
        // Arrange
        var expectedModel = new DockerComposeCreateDto();

        // Act
        var result =  _controller.CreateUsingUI();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(expectedModel, viewResult.Model);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldReturnFile_WhenModelIsValid()
    {
        // Arrange
        var model = new DockerComposeCreateDto();
        var yamlContent = "version: '3'";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(yamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/octet-stream", fileResult.ContentType);
        Assert.Equal("docker-compose.yml", fileResult.FileDownloadName);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldReturnView_WhenModelIsInvalid()
    {
        // Arrange
        var model = new DockerComposeCreateDto();
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Throws(new System.Exception("Test Exception"));

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(model, viewResult.Model);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldSetTempDataOnFailure()
    {
        // Arrange
        var model = new DockerComposeCreateDto();
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Throws(new System.Exception("Test Exception"));

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        Assert.Equal(model, _controller.TempData["DockerConfig"]);
        Assert.Equal("Test Exception", _controller.TempData["ErrorMessage"]);
    }

    [Fact]
    public async Task CreateUsingUI_Post_ShouldReturnFile_WhenYAMLGenerationSucceeds()
    {
        // Arrange
        var model = new DockerComposeCreateDto();
        var yamlContent = "version: '3'";
        _dockerComposeServiceMock.Setup(service => service.GenerateDockerComposeYaml(It.IsAny<DockerComposeCreateDto>())).Returns(yamlContent);

        // Act
        var result = await _controller.CreateUsingUI(model);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("docker-compose.yml", fileResult.FileDownloadName);
    }
}
