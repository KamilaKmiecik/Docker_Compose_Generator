namespace Docker_Compose_Generator.Models;

public class ServiceDTO
{
    public required string Name { get; set; }

    public required string Image { get; set; }

    public List<Port>? Ports { get; set; } = new List<Port>();
    public List<Volume>? Volumes { get; set; } = new List<Volume>();

    public List<Environment>? Environment { get; set; } = new List<Environment>();

    public List<Network>? Networks { get; set; } = new List<Network>();

    public RestartPolicy? RestartPolicy { get; set; }


    public ServiceDTO()
    {

    }
}

public class Port
{
    public required int HostPort { get; set; }
    public required int ContainerPort { get; set; }
    public string? Protocol { get; set; } = "tcp";
}


public class Volume : VolumeDTO
{
    public Volume()
    {

    }
    public new required string Source { get; set; }
    public new required string Target { get; set; }
    public new string? AccessMode { get; set; } = "rw";
}

public class Network : NetworkDTO
{
    public string? Alias { get; set; }
}


public class Environment
{
    public required string Key { get; set; }
    public required string Value { get; set; }
}


public class RestartPolicy
{
    // "no", "always", "on-failure", "unless-stopped"
    public required string Condition { get; set; }

    // tylko dla "on-failure"
    public int? MaxRetries { get; set; }
    public TimeSpan? Delay { get; set; }
}

