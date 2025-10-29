using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowCancelReason"/> entity.
/// </summary>
public class CoreWorkflowCancelReasonConfiguration : IEntityTypeConfiguration<CoreWorkflowCancelReason>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowCancelReason> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_cancel_reason", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.CancelReasonId })
            .HasName("PK_core_workflow_cancel_reason");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.CancelReasonId)
            .HasColumnName("cancel_reason_id");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_cancel_reason_workflow_id");

        builder.HasIndex(e => e.CancelReasonId, "IX_core_workflow_cancel_reason_cancel_reason_id");
    }
}
