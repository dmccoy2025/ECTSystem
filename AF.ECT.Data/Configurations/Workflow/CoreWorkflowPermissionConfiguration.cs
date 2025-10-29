using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowPermission"/> entity.
/// </summary>
public class CoreWorkflowPermissionConfiguration : IEntityTypeConfiguration<CoreWorkflowPermission>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowPermission> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_permission", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.PermId })
            .HasName("PK_core_workflow_permission");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.PermId)
            .HasColumnName("perm_id");

        // Relationships
        builder.HasOne(d => d.Perm)
            .WithMany()
            .HasForeignKey(d => d.PermId)
            .HasConstraintName("FK_core_workflow_permission_core_permission");

        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .HasConstraintName("FK_core_workflow_permission_core_workflow");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_permission_workflow_id");

        builder.HasIndex(e => e.PermId, "IX_core_workflow_permission_perm_id");
    }
}
