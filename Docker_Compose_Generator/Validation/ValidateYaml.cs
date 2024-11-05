using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Docker_Compose_Generator.Validation;

public static class ValidatorYaml
{
    public static void ValidateYaml(string yamlContent)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        using (var reader = new StringReader(yamlContent))
        {
            try
            {
                var yamlObject = deserializer.Deserialize(reader);


                //TODO:  walidacja jest do rozszerzenia 
            }
            catch (YamlException ex)
            {
                throw new YamlException("The YAML content is not valid.", ex);
            }
        }
    }
}
