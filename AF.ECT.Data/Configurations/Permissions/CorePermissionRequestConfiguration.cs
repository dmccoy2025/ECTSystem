using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Permissions;

/// <summary>
/// Entity Framework configuration for the CorePermissionRequest entity.
/// </summary>
public class CorePermissionRequestConfiguration : IEntityTypeConfiguration<CorePermissionRequest>
{
    /// <summary>
    /// Configures the CorePermissionRequest entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CorePermissionRequest> builder)
    {
        builder.ToTable("Core_PermissionRequest", "dbo");

        builder.HasKey(e => e.ReqId)
            .HasName("PK_Core_PermissionRequest");

        builder.Property(e => e.ReqId).HasColumnName("ReqID");
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.PermId).HasColumnName("PermID");
        builder.Property(e => e.DateReq).HasColumnName("DateReq");

        builder.HasIndex(e => e.UserId, "IX_Core_PermissionRequest_UserID");
        builder.HasIndex(e => e.PermId, "IX_Core_PermissionRequest_PermID");
        builder.HasIndex(e => e.DateReq, "IX_Core_PermissionRequest_DateReq");
    }
}
