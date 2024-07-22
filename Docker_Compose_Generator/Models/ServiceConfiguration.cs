namespace Docker_Compose_Generator.Models;

public class ServiceConfiguration
{
    public string Image { get; set; }
    public string[] Ports { get; set; }
    public string[] Environment { get; set; }
    public string[] Volumes { get; set; }
}
