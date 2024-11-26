using Microsoft.AspNetCore.Mvc.Formatters;

namespace Docker_Compose_Generator.Domain.Entities
{

    public class VolumeEntity
    {
        public required string Name { get; set; }
        public string? Driver { get; set; }
        public string Target { get; set; }
        public Dictionary<string, string>? DriverOptions { get; private set; }
        public Dictionary<string, string>? Labels { get; private set; }
        public bool? External { get; private set; }
        public bool? ReadOnly { get; private set; }
        public string AccessMode { get; set; }
        public string Source { get; set; }
        public VolumeEntity() { }

        public static VolumeEntity Create(string name, string target, string source, string accessMode, string? driver = null, Dictionary<string, string>? driverOptions = null, Dictionary<string, string>? labels = null, bool? external = false, bool? readOnly = false) 
        { 
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Volume name cannot be empty");

            if (string.IsNullOrWhiteSpace(target))
                target = string.Empty;
            //throw new ArgumentException("Target cannot be empty");
            if (string.IsNullOrWhiteSpace(source)) 
                source = string.Empty;
            //throw new ArgumentException("Source cannot be empty"); 

            if (string.IsNullOrWhiteSpace(accessMode))
                accessMode = string.Empty;
            //throw new ArgumentException("AccessMode cannot be empty"); 

            return new VolumeEntity { Name = name, Target = target, Source = source, AccessMode = accessMode, Driver = driver, DriverOptions = driverOptions, Labels = labels, External = external, ReadOnly = readOnly }; }

        public Dictionary<string, object> GenerateVolumeSection()
        {
            var volumeSection = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(Source))
            {
                volumeSection["source"] = Source;
            }

            if (!string.IsNullOrEmpty(Driver))
            {
                volumeSection["driver"] = Driver;
            }
                
            if (!string.IsNullOrEmpty(Target))
            {
                volumeSection["target"] = Target;
            }

            if (DriverOptions != null && DriverOptions.Any(x => !string.IsNullOrEmpty(x.Key)))
            {
                volumeSection["driver_opts"] = DriverOptions;
            }

            if (Labels != null && Labels.Any(x => !string.IsNullOrEmpty(x.Key)))
            {
                volumeSection["labels"] = Labels;
            }

            if (External.HasValue && External.Value)
            {
                volumeSection["external"] = true;
            }

            if (ReadOnly.HasValue && ReadOnly.Value)
            {
                volumeSection["readonly"] = true;
            }
            
            return volumeSection;
        }
            
        public Dictionary<string, object> GenerateVolumeSectionService()
        {
            var volumeSection = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(Name))
            {
                volumeSection["type"] = Name;
            }

            if (!string.IsNullOrEmpty(Source))
            {
                volumeSection["source"] = Source;
            }

            if (!string.IsNullOrEmpty(Target))
            {
                volumeSection["target"] = Target;
            }

            return volumeSection;
        }
    }
}
