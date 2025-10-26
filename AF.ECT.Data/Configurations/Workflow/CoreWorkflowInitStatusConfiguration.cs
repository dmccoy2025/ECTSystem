using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowInitStatus"/> entity.
/// </summary>
public class CoreWorkflowInitStatusConfiguration : IEntityTypeConfiguration<CoreWorkflowInitStatus>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowInitStatus> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_init_status", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.GroupId, e.StatusId })
            .HasName("PK_core_workflow_init_status");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.StatusId)
            .HasColumnName("status_id");

        // Indexes
        builder.HasIndex(e => e.WorkflowId)
            .HasDatabaseName("IX_core_workflow_init_status_workflow_id");

        builder.HasIndex(e => e.GroupId)
            .HasDatabaseName("IX_core_workflow_init_status_group_id");

        builder.HasIndex(e => e.StatusId)
            .HasDatabaseName("IX_core_workflow_init_status_status_id");
    }
}
