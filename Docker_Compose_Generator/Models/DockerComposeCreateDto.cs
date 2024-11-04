﻿namespace Docker_Compose_Generator.Models;

public class DockerComposeCreateDto
{
    public string Version => "3.8";
    public List<ServiceDto>? Services { get; set; } = new List<ServiceDto>();
    public List<VolumeDTO>? Volumes { get; set; } = new List<VolumeDTO>();
    public List<NetworkDTO>? Networks { get; set; } = new List<NetworkDTO>();

    public DockerComposeCreateDto()
    {

    }
}
