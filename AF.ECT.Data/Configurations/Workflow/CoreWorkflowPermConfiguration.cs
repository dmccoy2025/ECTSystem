using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowPerm"/> entity.
/// </summary>
public class CoreWorkflowPermConfiguration : IEntityTypeConfiguration<CoreWorkflowPerm>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowPerm> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_perm", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.GroupId })
            .HasName("PK_core_workflow_perm");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.CanView)
            .HasColumnName("can_view");

        builder.Property(e => e.CanCreate)
            .HasColumnName("can_create");

        // Relationships
        builder.HasOne(d => d.Group)
            .WithMany()
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_perm_core_user_group");

        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_perm_core_workflow");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_perm_workflow_id");

        builder.HasIndex(e => e.GroupId, "IX_core_workflow_perm_group_id");
    }
}
