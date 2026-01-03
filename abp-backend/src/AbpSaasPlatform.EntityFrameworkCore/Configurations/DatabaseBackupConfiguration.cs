using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AbpSaasPlatform.Containers;

namespace AbpSaasPlatform.EntityFrameworkCore.Configurations;

public class DatabaseBackupConfiguration : IEntityTypeConfiguration<DatabaseBackup>
{
    public void Configure(EntityTypeBuilder<DatabaseBackup> builder)
    {
        builder.ToTable("database_backups");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BackupId).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.BackupId).IsUnique();
        builder.Property(x => x.DatabaseId).IsRequired();
        builder.Property(x => x.BackupType).HasMaxLength(50).HasDefaultValue("full");
        builder.Property(x => x.StorageType).HasMaxLength(50).HasDefaultValue("local");
        builder.Property(x => x.StoragePath).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.StorageUrl).HasMaxLength(1000);
        builder.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("pending");
        builder.Property(x => x.BackupDate).IsRequired();
        builder.HasOne(x => x.Database)
            .WithMany()
            .HasForeignKey(x => x.DatabaseId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.DatabaseId);
        builder.HasIndex(x => x.BackupDate);
        builder.HasIndex(x => x.BackupId);
    }
}

public static class DatabaseBackupConfigurationExtensions
{
    public static void ConfigureDatabaseBackup(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new DatabaseBackupConfiguration());
    }
}
