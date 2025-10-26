using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Permissions;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CorePermission"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_permission table,
/// which stores the master list of available permissions in the ECT system. Permissions are assigned to
/// user groups via CoreGroupPermission and control access to features, forms, workflows, and data scopes.
/// </remarks>
public class CorePermissionConfiguration : IEntityTypeConfiguration<CorePermission>
{
    /// <summary>
    /// Configures the CorePermission entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CorePermission> builder)
    {
        // Table mapping
        builder.ToTable("core_permission", "dbo");

        // Primary key
        builder.HasKey(e => e.PermId)
            .HasName("PK__core_per__97EE940F9F51B7D8");

        // Property configurations
        builder.Property(e => e.PermId)
            .HasColumnName("perm_id");

        builder.Property(e => e.PermName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("perm_name");

        builder.Property(e => e.PermDesc)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("perm_desc");

        builder.Property(e => e.Exclude)
            .HasColumnName("exclude");

        // Indexes
        builder.HasIndex(e => e.PermName)
            .IsUnique()
            .HasDatabaseName("UQ_core_permission_perm_name");

        builder.HasIndex(e => e.Exclude)
            .HasDatabaseName("IX_core_permission_exclude")
            .HasFilter("exclude = 0");
    }
}
