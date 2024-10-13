namespace Docker_Compose_Generator.Models;

public class DockerComposeCreateDto
{
    public required string Version { get; set; }
    public required List<ServiceDto> Services { get; set; } = new List<ServiceDto>();
    public required List<VolumeDTO> Volumes { get; set; } = new List<VolumeDTO>();
    public required List<NetworkDTO> Networks { get; set; } = new List<NetworkDTO>();
}
