using AutoMapper;
using Docker_Compose_Generator.Domain.Entities;
using Docker_Compose_Generator.Models;
using Environment = Docker_Compose_Generator.Models.Environment;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapowanie głównego DTO `DockerComposeCreateDto` na encję `DockerComposeEntity`
        CreateMap<DockerComposeCreateDto, DockerComposeEntity>()
            .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version))
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services))
            .ForMember(dest => dest.Volumes, opt => opt.MapFrom(src => src.Volumes))
            .ForMember(dest => dest.Networks, opt => opt.MapFrom(src => src.Networks));

        // Mapowanie `ServiceDTO` na `ServiceEntity`
        CreateMap<ServiceDTO, ServiceEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Ports, opt => opt.MapFrom(src => src.Ports))
            .ForMember(dest => dest.Volumes, opt => opt.MapFrom(src => src.Volumes))
            .ForMember(dest => dest.Environment, opt => opt.MapFrom(src => src.Environment))
            .ForMember(dest => dest.Networks, opt => opt.MapFrom(src => src.Networks))
            .ForMember(dest => dest.RestartPolicy, opt => opt.MapFrom(src => src.RestartPolicy));

        // Mapowanie `VolumeDTO` na `VolumeEntity`
        CreateMap<VolumeDTO, VolumeEntity>()
            .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver))
            .ForMember(dest => dest.DriverOptions, opt => opt.MapFrom(src => src.DriverOptions))
            .ForMember(dest => dest.Labels, opt => opt.MapFrom(src => src.Labels))
            .ForMember(dest => dest.External, opt => opt.MapFrom(src => src.External))
            .ForMember(dest => dest.ReadOnly, opt => opt.MapFrom(src => src.ReadOnly));

        CreateMap<RestartPolicy, RestartPolicyEntity>()
            .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition))
            .ForMember(dest => dest.MaxRetries, opt => opt.MapFrom(src => src.MaxRetries))
            .ForMember(dest => dest.Delay, opt => opt.MapFrom(src => src.Delay));

        // Mapowanie `NetworkDTO` na `NetworkEntity`
        CreateMap<NetworkDTO, NetworkEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver))
            .ForMember(dest => dest.Internal, opt => opt.MapFrom(src => src.Internal))
            .ForMember(dest => dest.Attachable, opt => opt.MapFrom(src => src.Attachable))
            .ForMember(dest => dest.DriverOptions, opt => opt.MapFrom(src => src.DriverOptions))
            .ForMember(dest => dest.Ipam, opt => opt.MapFrom(src => src.Ipam));

        // Mapowanie `Port` na `PortEntity`
        CreateMap<Port, PortEntity>()
            .ForMember(dest => dest.HostPort, opt => opt.MapFrom(src => src.HostPort))
            .ForMember(dest => dest.ContainerPort, opt => opt.MapFrom(src => src.ContainerPort))
            .ForMember(dest => dest.Protocol, opt => opt.MapFrom(src => src.Protocol));

        // Mapowanie `Environment` na `EnvironmentEntity`
        CreateMap<Environment, EnvironmentEntity>()
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Key))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));

        // Mapowanie `RestartPolicy` na `RestartPolicyEntity`
        CreateMap<RestartPolicy, RestartPolicyEntity>()
            .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition))
            .ForMember(dest => dest.Delay, opt => opt.MapFrom(src => src.Delay))
            .ForMember(dest => dest.MaxRetries, opt => opt.MapFrom(src => src.MaxRetries));
    }
}
