
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Docker_Compose_Generator.Services;
using Newtonsoft.Json;

namespace Docker_Compose_Generator.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDockerComposeService, DockerComposeService>();

        return services;
    }
}

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            session.SetString(key, json); // Zapisz jako string w sesji
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }
    }


