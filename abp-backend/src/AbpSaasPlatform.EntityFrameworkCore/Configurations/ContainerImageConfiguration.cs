using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class ContainerImageConfiguration : IEntityTypeConfiguration<ContainerImage>
{
    public void Configure(EntityTypeBuilder<ContainerImage> builder)
    {
        builder.ToTable("container_images");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ImageId).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.ImageId).IsUnique();
        builder.Property(x => x.ImageName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.ImageTag).HasMaxLength(100).IsRequired();
        builder.Property(x => x.FullImageUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.RegistryUrl).HasMaxLength(255);
        builder.Property(x => x.RegistryType).HasMaxLength(20).HasDefaultValue("dockerhub");
        builder.Property(x => x.ImageType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.ModuleId);
        builder.Property(x => x.ErpnextVersion).HasMaxLength(20);
        builder.Property(x => x.FrappeVersion).HasMaxLength(20);
        builder.Property(x => x.ImageSizeMb);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.IsLatest).HasDefaultValue(false);
        builder.Property(x => x.BuildDate);
        builder.Property(x => x.CommitHash).HasMaxLength(40);
        builder.HasOne(x => x.Module)
            .WithMany()
            .HasForeignKey(x => x.ModuleId)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => new { x.ImageName, x.ImageTag });
        builder.HasIndex(x => x.ImageType);
        builder.HasIndex(x => x.ModuleId);
    }
}

public static class ContainerImageConfigurationExtensions
{
    public static void ConfigureContainerImage(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ContainerImageConfiguration());
    }
}
