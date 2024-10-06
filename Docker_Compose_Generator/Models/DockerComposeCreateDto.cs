namespace Docker_Compose_Generator.Models;

public class DockerComposeCreateDto
{
    public required string Version { get; set; }

    public required string Image { get; set; }

    public string? Ports { get; set; } 
    public string? Volumes { get; set; }  
    public string? Environment { get; set; }  
    public string? Networks { get; set; }  
    public string? RestartPolicy { get; set; }  

}


