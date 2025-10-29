using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Permissions;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CorePageAccess"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_page_access table,
/// which controls page-level access permissions based on user groups, workflows, and workflow statuses.
/// </remarks>
public class CorePageAccessConfiguration : IEntityTypeConfiguration<CorePageAccess>
{
    /// <summary>
    /// Configures the CorePageAccess entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CorePageAccess> builder)
    {
        // Table mapping
        builder.ToTable("core_page_access", "dbo");

        // Primary key
        builder.HasKey(e => e.MapId)
            .HasName("PK_core_page_access");

        // Property configurations
        builder.Property(e => e.MapId)
            .HasColumnName("map_id");

        builder.Property(e => e.StatusId)
            .HasColumnName("status_id");

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.Access)
            .HasColumnName("access");

        builder.Property(e => e.PageId)
            .HasColumnName("page_id");

        // Foreign key relationships
        builder.HasOne(d => d.Group)
            .WithMany(p => p.CorePageAccesses)
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_page_access_group");

        // Indexes
        builder.HasIndex(e => new { e.GroupId, e.WorkflowId, e.StatusId, e.PageId }, "IX_core_page_access_composite");

        builder.HasIndex(e => e.PageId, "IX_core_page_access_page_id");
    }
}
