namespace Docker_Compose_Generator.Models;

public class DockerComposeCreateDto
{
    public required string Version { get; set; }
    public List<ServiceDto>? Services { get; set; } = new List<ServiceDto>();
    public List<VolumeDTO>? Volumes { get; set; } = new List<VolumeDTO>();
    public List<NetworkDTO>? Networks { get; set; } = new List<NetworkDTO>();
}
