namespace Docker_Compose_Generator.Models;

public class DockerComposeCreateDto
{
    public required string Version { get; set; }
    public required List<ServiceDto> Services { get; set; } = new List<ServiceDto>();
}
