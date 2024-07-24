using System.Collections.Generic;

namespace Docker_Compose_Generator.Models;

public class DockerComposeModel
{
    public int Id { get; set; } 
    public Dictionary<string, ServiceConfiguration> Services { get; set; }

    public ICollection<ServiceModel> ServiceModels { get; set; }
    public ICollection<NetworkModel> Networks { get; set; }

    public DockerComposeModel()
    {
        Services = new Dictionary<string, ServiceConfiguration>();
        ServiceModels = new List<ServiceModel>();
        Networks = new List<NetworkModel>();
    }
}
