namespace Docker_Compose_Generator.Domain.Entities;



public class PortEntity
{
    public required int HostPort { get; set; }
    public required int ContainerPort { get; set; }
    public string? Protocol { get; private set; }

    public PortEntity() { }

    public static PortEntity Create(int hostPort, int containerPort, string? protocol = "tcp")
    {
        if (hostPort < 0 || containerPort < 0)
            throw new ArgumentOutOfRangeException("Ports must be non-negative");

        return new PortEntity { HostPort = hostPort, ContainerPort = containerPort, Protocol = protocol };
    }
}

public class ServiceEntity
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public List<PortEntity> Ports { get; private set; } = new List<PortEntity>();
    public List<VolumeEntity> Volumes { get; private set; } = new List<VolumeEntity>();
    public List<EnvironmentEntity> Environment { get; set; } = new List<EnvironmentEntity>();
    public List<NetworkEntity> Networks { get; private set; } = new List<NetworkEntity>();
    public RestartPolicyEntity? RestartPolicy { get; set; }

    public ServiceEntity() { }

    public static ServiceEntity Create(string name, string image)
    {
        if (string.IsNullOrWhiteSpace(name))  throw new ArgumentException("Service name cannot be empty");
        if (string.IsNullOrWhiteSpace(image))  throw new ArgumentException("Service image cannot be empty");

        return new ServiceEntity { Name = name, Image = image };
    }

    public Dictionary<string, object> GenerateServiceSection()
    {
        var serviceSection = new Dictionary<string, object>
            {
                { "image", Image }
            };

        if (Ports.Any())
        {
            serviceSection["ports"] = Ports.Select(p => p.ToString()).ToList();
        }

        if (Environment.Any())
        {
            serviceSection["environment"] = Environment.ToDictionary(e => e.Key, e => e.Value);
        }

        if (Volumes.Any())
        {
            serviceSection["volumes"] = Volumes.Select(v => v.GenerateVolumeSection()).ToList();
        }

        if (Networks.Any())
        {
            serviceSection["networks"] = Networks.Select(n => n.Name).ToList();
        }

        if (RestartPolicy != null)
        {
            serviceSection["restart"] = RestartPolicy.Condition;
        }

        return serviceSection;
    }


}

