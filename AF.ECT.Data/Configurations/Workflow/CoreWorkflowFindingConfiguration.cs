using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowFinding"/> entity.
/// </summary>
public class CoreWorkflowFindingConfiguration : IEntityTypeConfiguration<CoreWorkflowFinding>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowFinding> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_finding", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.FindingId })
            .HasName("PK_core_workflow_finding");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.FindingId)
            .HasColumnName("finding_id");

        builder.Property(e => e.SortOrder)
            .HasColumnName("sort_order");

        // Indexes
        builder.HasIndex(e => e.WorkflowId)
            .HasDatabaseName("IX_core_workflow_finding_workflow_id");

        builder.HasIndex(e => e.FindingId)
            .HasDatabaseName("IX_core_workflow_finding_finding_id");

        builder.HasIndex(e => e.SortOrder)
            .HasDatabaseName("IX_core_workflow_finding_sort_order");
    }
}
