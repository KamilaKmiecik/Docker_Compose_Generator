namespace Docker_Compose_Generator.Models;

public class ServiceDto
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public List<string>? Ports { get; set; } = new List<string>();
    public List<string>? Volumes { get; set; } = new List<string>();
    public List<string>? Environment { get; set; } = new List<string>();
    public List<string>? Networks { get; set; } = new List<string>();
    public string? RestartPolicy { get; set; }
}
