using Docker_Compose_Generator.Models;
using Microsoft.AspNetCore.Http.Connections;

namespace Docker_Compose_Generator.Domain.Entities;

public class NetworkEntity
{
    public required string Name { get; set; }
    public string? Driver { get; set; }
    public bool? Internal { get; private set; }
    public bool? Attachable { get; private set; }
    public IPAMConfigurationEntity? Ipam { get; private set; }
    public Dictionary<string, string>? DriverOptions { get; set; }

    public static NetworkEntity Create(string name, string? driver = null, Dictionary<string, string>? driverOptions = null, IPAMConfigurationEntity? ipam = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Network name cannot be empty.");

        return new NetworkEntity
        {
            Name = name,
            Driver = driver,
            DriverOptions = driverOptions,
            Ipam = ipam
        };
    }

    public Dictionary<string, object> GenerateNetworkSection()
    {
        var networkSection = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(Driver))
            networkSection["driver"] = Driver;

        if (Internal.HasValue && Internal.Value)
            networkSection["internal"] = true;

        if (Attachable.HasValue && Attachable.Value)
            networkSection["attachable"] = true;
            
        if (Ipam != null)
            networkSection["ipam"] = Ipam.GenerateIpamSection();

        if (DriverOptions != null && DriverOptions.Any(x => !string.IsNullOrEmpty(x.Key)))
            networkSection["driver_opts"] = DriverOptions;

        return networkSection;
    }
}

public class IPAMConfigurationEntity
{
    public IPAMConfigEntity Configuration { get; private set; } = new();
    public string? Driver { get; private set; }
    public string? Subnet { get; private set; }
    public string? Gateway { get; private set; }

    public static IPAMConfigurationEntity Create(string? driver = null, IPAMConfigEntity iPAM = null)
    {
        return new IPAMConfigurationEntity { Driver = driver, Configuration = iPAM };
    }   
    
    public static IPAMConfigurationEntity Create(string? subnet, string? gateway, string? driver = null)
    {
        return new IPAMConfigurationEntity { Driver = driver, Subnet = subnet, Gateway = gateway };
    }

    public Dictionary<string, object> GenerateIpamSection()
    {
        if (Configuration == null)
            throw new Exception("Add IPAM Configuration!");
        
        if (string.IsNullOrEmpty(Subnet))
            throw new Exception("Fill in the subnet");
         
        if (string.IsNullOrEmpty(Gateway))
            throw new Exception("Fill in the gateway");
        

        return new Dictionary<string, object>
        {
            { "driver", Driver ?? "default" },
            { "config", Configuration.GenerateIpamConfigSection(Subnet, Gateway) }
        };
    }
}

public class IPAMConfigEntity
{
    public string? Subnet { get; private set; }
    public string? Gateway { get; private set; }

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
    
    public Dictionary<string, object> GenerateIpamConfigSection(string subnet, string gateway)
    {
        var configSection = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(subnet)) 
            configSection["subnet"] = subnet;
        else 
            throw new Exception("Fill in the subnet in ipam configuration");


        if (!string.IsNullOrEmpty(gateway)) 
            configSection["gateway"] = gateway;
        else 
            throw new Exception("Fill in the gateway in ipam configuration");

        return configSection;
    }
}

public class EnvironmentEntity
{
    public required string Key { get; set; }
    public required string Value { get; set; }

    public static EnvironmentEntity Create(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Environment key and value cannot be empty.");

        return new EnvironmentEntity { Key = key, Value = value };
    }
}

public class RestartPolicyEntity
{
    public required string Condition { get; set; }
    public int? MaxRetries { get; set; }
    public TimeSpan? Delay { get; set; }

    public static RestartPolicyEntity Create(string condition, int? maxRetries = null, TimeSpan? delay = null)
    {
        if (string.IsNullOrWhiteSpace(condition))
            condition = string.Empty; 

       //     throw new ArgumentException("Restart policy condition cannot be empty.");

        return new RestartPolicyEntity { Condition = condition, MaxRetries = maxRetries, Delay = delay };
    }
}
