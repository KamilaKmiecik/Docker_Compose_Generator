namespace Docker_Compose_Generator.Models;

public class NetworkDTO
{
    public required string Name { get; set; }

    public string? Driver { get; set; }

    public bool? Internal { get; set; }

    public bool? Attachable { get; set; }

    public IPAMConfiguration? Ipam { get; set; }

    public Dictionary<string, string>? DriverOptions { get; set; }


    public NetworkDTO()
    {

    }
}

public class IPAMConfiguration
{
    public IPAMConfig Configuration { get; set; }

    public string? Driver { get; set; }
}

public class IPAMConfig
{
    public string? Subnet { get; set; }
    public string? Gateway { get; set; }
}

