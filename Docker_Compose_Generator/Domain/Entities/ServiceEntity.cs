using System.ComponentModel.DataAnnotations;

namespace Docker_Compose_Generator.Domain.Entities;

public class ServiceEntity 
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public List<PortEntity> Ports { get; private set; } = new();
    public List<VolumeEntity> Volumes { get; private set; } = new();
    public List<EnvironmentEntity> Environment { get; private set; } = new();
    public List<NetworkEntity> Networks { get; private set; } = new();
    public RestartPolicyEntity? RestartPolicy { get; set; }

    public static ServiceEntity Create(
        string name,
        string image,
        List<PortEntity>? ports = null,
        List<VolumeEntity>? volumes = null,
        List<EnvironmentEntity>? environment = null,
        List<NetworkEntity>? networks = null,
        RestartPolicyEntity? restartPolicy = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Service name cannot be empty");
        if (string.IsNullOrWhiteSpace(image))
            throw new ArgumentException("Service image cannot be empty");

        return new ServiceEntity
        {
            Name = name,
            Image = image,
            Ports = ports ?? new List<PortEntity>(),
            Volumes = volumes ?? new List<VolumeEntity>(),
            Environment = environment ?? new List<EnvironmentEntity>(),
            Networks = networks ?? new List<NetworkEntity>(),
            RestartPolicy = restartPolicy
        };
    }

    public Dictionary<string, object> GenerateServiceSection()
    {
        var serviceSection = new Dictionary<string, object>
        {
            { "image", Image }
        };

        if (Ports.Any())
            serviceSection["ports"] = Ports.Select(p => p.ToString()).ToList();

        if (Environment.Any())
            serviceSection["environment"] = Environment.ToDictionary(e => e.Key, e => e.Value);

        if (Volumes.Any())
            serviceSection["volumes"] = Volumes.Select(v => v.GenerateVolumeSection()).ToList();

        if (Networks.Any())
            serviceSection["networks"] = Networks.Select(n => n.Name).ToList();

        if (RestartPolicy != null)
            serviceSection["restart"] = RestartPolicy.Condition;

        return serviceSection;
    }
}


public class PortEntity : IValidatableObject
{
    public required int HostPort { get; set; }
    public required int ContainerPort { get; set; }
    public string? Protocol { get; private set; } = "tcp";

    public static PortEntity Create(int hostPort, int containerPort, string? protocol = "tcp")
    {
        if (hostPort < 0 || containerPort < 0)
            throw new ArgumentOutOfRangeException("Ports must be non-negative");

        return new PortEntity { HostPort = hostPort, ContainerPort = containerPort, Protocol = protocol };
    }

    public override string ToString()
    {
        return $"{HostPort}:{ContainerPort}/{Protocol}";
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (HostPort < 0)
            yield return new ValidationResult("Host port must be non-negative", new[] { nameof(HostPort) });

        if (ContainerPort < 0)
            yield return new ValidationResult("Container port must be non-negative", new[] { nameof(ContainerPort) });
    }
}
