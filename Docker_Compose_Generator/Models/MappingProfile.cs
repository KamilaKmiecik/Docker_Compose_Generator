using AutoMapper;
using Docker_Compose_Generator.Domain.Entities;
using Docker_Compose_Generator.Models;
using Environment = Docker_Compose_Generator.Models.Environment;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DockerComposeCreateDto, DockerComposeEntity>()
            .ConstructUsing(src => DockerComposeEntity.Create(src.Version));

        CreateMap<ServiceDTO, ServiceEntity>()
            .ConstructUsing(src => ServiceEntity.Create(src.Name, src.Image));

        CreateMap<VolumeDTO, VolumeEntity>()
            .ConstructUsing(src => VolumeEntity.Create(src.Driver, src.Driver, src.External, src.ReadOnly));

        CreateMap<NetworkDTO, NetworkEntity>()
            .ConstructUsing(src => NetworkEntity.Create(src.Name, src.Driver, src.Internal, src.Attachable));

        CreateMap<Port, PortEntity>()
            .ConstructUsing(src => PortEntity.Create(src.HostPort, src.ContainerPort, src.Protocol));

        CreateMap<Environment, EnvironmentEntity>()
            .ConstructUsing(src => EnvironmentEntity.Create(src.Key, src.Value));

        CreateMap<RestartPolicy, RestartPolicyEntity>()
            .ConstructUsing(src => RestartPolicyEntity.Create(src.Condition, src.MaxRetries, src.Delay));
    }
}

