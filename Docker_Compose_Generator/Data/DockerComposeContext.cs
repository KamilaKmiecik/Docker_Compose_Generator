using Docker_Compose_Generator.Models;
using Microsoft.EntityFrameworkCore;

namespace Docker_Compose_Generator.Data;

public class DockerComposeContext : DbContext
{
    public DbSet<DockerComposeModel> ComposeConfigurations { get; set; }
    public DbSet<ServiceModel> Services { get; set; }
    public DbSet<NetworkModel> Networks { get; set; }

    public DockerComposeContext(DbContextOptions<DockerComposeContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfiguracja relacji między tabelami, kluczy obcych, ograniczeń itp.
        modelBuilder.Entity<DockerComposeModel>()
            .HasMany(c => c.ServiceModels)
            .WithOne(s => s.DockerComposeModel)
            .HasForeignKey(s => s.ComposeConfigurationId);

        modelBuilder.Entity<DockerComposeModel>()
            .HasMany(c => c.Networks)
            .WithOne(n => n.DockerComposeModel)
            .HasForeignKey(n => n.ComposeConfigurationId);

        // Możesz również dodać konfigurowanie innych właściwości, jeśli jest to wymagane
    }
}
