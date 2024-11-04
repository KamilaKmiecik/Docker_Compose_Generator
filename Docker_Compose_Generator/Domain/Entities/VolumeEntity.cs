namespace Docker_Compose_Generator.Domain.Entities
{

    public class VolumeEntity
    {
        public required string Name { get; set; }
        public string? Driver { get; private set; }
        public string Target { get; set; }
        public Dictionary<string, string>? DriverOptions { get; private set; }
        public Dictionary<string, string>? Labels { get; private set; }
        public bool? External { get; private set; }
        public bool? ReadOnly { get; private set; }
        public string AccessMode { get; set; }
        public string Source { get; set; }
        private VolumeEntity() { }

        public static VolumeEntity Create(string name, string? driver = null, bool? external = false, bool? readOnly = false, string? target = null)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Volume name cannot be empty");

            return new VolumeEntity { Name = name, Driver = driver, External = external, ReadOnly = readOnly };
        }

        public Dictionary<string, object> GenerateVolumeSection()
        {
            var volumeSection = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(Driver))
            {
                volumeSection["driver"] = Driver;
            }

            if (DriverOptions != null && DriverOptions.Any())
            {
                volumeSection["driver_opts"] = DriverOptions;
            }

            if (Labels != null && Labels.Any())
            {
                volumeSection["labels"] = Labels;
            }

            if (External.HasValue && External.Value)
            {
                volumeSection["external"] = true;
            }

            return volumeSection;
        }
    }
}
