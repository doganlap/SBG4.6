using Volo.Abp.Domain.Entities.Auditing;

namespace AbpSaasPlatform.Tenants;

public class TenantFile : FullAuditedAggregateRoot<long>
{
    public string FileId { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string? OriginalFileName { get; set; }
    
    public string FileType { get; set; } = null!; // document, image, video, audio, other
    public string MimeType { get; set; } = null!;
    public long FileSizeBytes { get; set; }
    
    public string StorageType { get; set; } = "local"; // local, s3, azure, gcs
    public string StoragePath { get; set; } = null!;
    public string? StorageUrl { get; set; }
    
    // Organization
    public string? Folder { get; set; }
    public string? Tags { get; set; } // JSON array
    
    // Access
    public string AccessLevel { get; set; } = "private"; // public, private, shared
    public string? SharedWith { get; set; } // JSON array of user IDs
    
    // Metadata
    public string? Metadata { get; set; } // JSON
    public string? ThumbnailUrl { get; set; }
    
    // Uploader
    public Guid? UploadedBy { get; set; }
    public TenantUser? UploadedByUser { get; set; }
}
