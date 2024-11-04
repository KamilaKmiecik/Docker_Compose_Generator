namespace Docker_Compose_Generator.Domain.Entities;

  public class NetworkEntity
{
    public required string Name { get; set; }
    public string? Driver { get; private set; }
    public bool? Internal { get; private set; }
    public bool? Attachable { get; private set; }
    public IPAMConfigurationEntity? Ipam { get; private set; }
    public Dictionary<string, string>? DriverOptions { get; private set; }

    private NetworkEntity() { }

    public static NetworkEntity Create(string name, string? driver = null, bool? internalNetwork = false, bool? attachable = false)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Network name cannot be empty");

        return new NetworkEntity { Name = name, Driver = driver, Internal = internalNetwork, Attachable = attachable };
    }

    public Dictionary<string, object> GenerateNetworkSection()
    {
        var networkSection = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(Driver))
        {
            networkSection["driver"] = Driver;
        }

        if (Internal.HasValue && Internal.Value)
        {
            networkSection["internal"] = true;
        }

        if (Attachable.HasValue && Attachable.Value)
        {
            networkSection["attachable"] = true;
        }

        if (Ipam != null)
        {
            networkSection["ipam"] = Ipam.GenerateIpamSection();
        }

        if (DriverOptions != null && DriverOptions.Any())
        {
            networkSection["driver_opts"] = DriverOptions;
        }

        return networkSection;
    }
}

public class EnvironmentEntity
{
    public required string Key { get; set; }
    public required string Value { get; set; }

    private EnvironmentEntity() { }

    public static EnvironmentEntity Create(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Environment key cannot be empty");
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Environment value cannot be empty");

        return new EnvironmentEntity { Key = key, Value = value };
    }
}

public class RestartPolicyEntity
{
    public required string Condition { get; set; }
    public int? MaxRetries { get; private set; }
    public TimeSpan? Delay { get; private set; }

    private RestartPolicyEntity() { }

    public static RestartPolicyEntity Create(string condition, int? maxRetries = null, TimeSpan? delay = null)
    {
        if (string.IsNullOrWhiteSpace(condition)) throw new ArgumentException("Condition cannot be empty");

        return new RestartPolicyEntity { Condition = condition, MaxRetries = maxRetries, Delay = delay };
    }
}

public class IPAMConfigurationEntity
{
    public List<IPAMConfigEntity> Configurations { get; private set; } = new List<IPAMConfigEntity>();
    public string? Driver { get; private set; }

    private IPAMConfigurationEntity() { }

    public static IPAMConfigurationEntity Create(string? driver = null)
    {
        return new IPAMConfigurationEntity { Driver = driver };
    }

    public Dictionary<string, object> GenerateIpamSection()
    {
        var ipamSection = new Dictionary<string, object>
            {
                { "driver", Driver ?? "default" },
                { "config", Configurations.Select(c => c.GenerateIpamConfigSection()).ToList() }
            };

        return ipamSection;
    }
}

public class IPAMConfigEntity
{
    public string? Subnet { get; private set; }
    public string? Gateway { get; private set; }

    private IPAMConfigEntity() { }

    public static IPAMConfigEntity Create(string? subnet = null, string? gateway = null)
    {
        return new IPAMConfigEntity { Subnet = subnet, Gateway = gateway };
    }

    public Dictionary<string, object> GenerateIpamConfigSection()
    {
        var configSection = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(Subnet)) configSection["subnet"] = Subnet;
        if (!string.IsNullOrEmpty(Gateway)) configSection["gateway"] = Gateway;
        return configSection;
    }
}
