namespace Docker_Compose_Generator.Models;

public class VolumeDTO
{
    public required string Name { get; set; }

    public string? Driver { get; set; }

    public Dictionary<string, string>? DriverOptions { get; set; }

    public Dictionary<string, string>? Labels { get; set; }

    public bool? External { get; set; }

    public bool? ReadOnly { get; set; }

    public string? Source { get; set; } 

    public string? Target { get; set; } 

    public string? AccessMode { get; set; } 

    public VolumeDTO() { }
}
