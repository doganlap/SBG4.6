using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Tenants;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class TenantFileConfiguration : IEntityTypeConfiguration<TenantFile>
{
    public void Configure(EntityTypeBuilder<TenantFile> builder)
    {
        builder.ToTable("tenant_files");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FileId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.FileId).IsUnique();
        builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.OriginalFileName).HasMaxLength(255);
        builder.Property(x => x.FileType).HasMaxLength(50).IsRequired();
        builder.Property(x => x.MimeType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.StorageType).HasMaxLength(50).HasDefaultValue("local");
        builder.Property(x => x.StoragePath).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.StorageUrl).HasMaxLength(1000);
        builder.Property(x => x.Folder).HasMaxLength(500);
        builder.Property(x => x.AccessLevel).HasMaxLength(50).HasDefaultValue("private");
        builder.Property(x => x.ThumbnailUrl).HasMaxLength(1000);
        builder.HasOne(x => x.UploadedByUser)
            .WithMany()
            .HasForeignKey(x => x.UploadedBy)
            .OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(x => x.FileId);
        builder.HasIndex(x => x.FileType);
    }
}

public static class TenantFileConfigurationExtensions
{
    public static void ConfigureTenantFile(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new TenantFileConfiguration());
    }
}
