using Docker_Compose_Generator.Services;

namespace Docker_Compose_Generator.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDockerComposeService, DockerComposeService>();

        return services;
    }
}
