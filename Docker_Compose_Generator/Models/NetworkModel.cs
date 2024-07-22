namespace Docker_Compose_Generator.Models;

public class NetworkModel
{
    public int Id { get; set; } 

    public string Name { get; set; }

    public int ComposeConfigurationId { get; set; }
    public DockerComposeModel DockerComposeModel { get; set; }
}
