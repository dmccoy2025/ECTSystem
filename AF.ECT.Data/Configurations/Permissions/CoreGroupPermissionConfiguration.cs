using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Permissions;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreGroupPermission"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_group_permission table,
/// which serves as the many-to-many relationship table between user groups and permissions. This table
/// defines which permissions are granted to which groups, controlling feature access across the ECT system.
/// </remarks>
public class CoreGroupPermissionConfiguration : IEntityTypeConfiguration<CoreGroupPermission>
{
    /// <summary>
    /// Configures the CoreGroupPermission entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreGroupPermission> builder)
    {
        // Table mapping
        builder.ToTable("core_group_permission", "dbo");

        // Composite key
        builder.HasKey(e => new { e.GroupId, e.PermId })
            .HasName("PK_core_group_permission");

        // Property configurations
        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.PermId)
            .HasColumnName("perm_id");

        // Relationships
        builder.HasOne(d => d.Group)
            .WithMany()
            .HasForeignKey(d => d.GroupId)
            .HasConstraintName("FK_core_group_permission_core_user_group");

        builder.HasOne(d => d.Perm)
            .WithMany()
            .HasForeignKey(d => d.PermId)
            .HasConstraintName("FK_core_group_permission_core_permission");

        // Indexes
        builder.HasIndex(e => e.GroupId, "IX_core_group_permission_group_id");

        builder.HasIndex(e => e.PermId, "IX_core_group_permission_perm_id");
    }
}
