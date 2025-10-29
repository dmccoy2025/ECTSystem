using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreUserPermission"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_user_permission table,
/// which grants individual permissions directly to users, bypassing group-based permissions.
/// </remarks>
public class CoreUserPermissionConfiguration : IEntityTypeConfiguration<CoreUserPermission>
{
    /// <summary>
    /// Configures the CoreUserPermission entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreUserPermission> builder)
    {
        // Table mapping
        builder.ToTable("core_user_permission", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.UserId, e.PermId })
            .HasName("PK_core_user_permission");

        // Property configurations
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.PermId)
            .HasColumnName("perm_id");

        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("status");

        // Indexes
        builder.HasIndex(e => e.UserId, "IX_core_user_permission_user_id");

        builder.HasIndex(e => e.PermId, "IX_core_user_permission_perm_id");

        builder.HasIndex(e => e.Status, "IX_core_user_permission_status");
    }
}
