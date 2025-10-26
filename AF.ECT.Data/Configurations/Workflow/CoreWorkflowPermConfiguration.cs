using Microsoft.EntityFrameworkCore;
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

        // Indexes
        builder.HasIndex(e => e.WorkflowId)
            .HasDatabaseName("IX_core_workflow_perm_workflow_id");

        builder.HasIndex(e => e.GroupId)
            .HasDatabaseName("IX_core_workflow_perm_group_id");
    }
}
