using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowRwoaReason"/> entity.
/// </summary>
public class CoreWorkflowRwoaReasonConfiguration : IEntityTypeConfiguration<CoreWorkflowRwoaReason>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowRwoaReason> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_rwoa_reason", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.RwoaId })
            .HasName("PK_core_workflow_rwoa_reason");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.RwoaId)
            .HasColumnName("rwoa_id");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_rwoa_reason_workflow_id");

        builder.HasIndex(e => e.RwoaId, "IX_core_workflow_rwoa_reason_rwoa_id");
    }
}
