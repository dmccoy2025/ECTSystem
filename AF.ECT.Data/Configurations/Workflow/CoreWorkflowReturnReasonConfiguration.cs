using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowReturnReason"/> entity.
/// </summary>
public class CoreWorkflowReturnReasonConfiguration : IEntityTypeConfiguration<CoreWorkflowReturnReason>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowReturnReason> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_return_reason", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.ReturnId })
            .HasName("PK_core_workflow_return_reason");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.ReturnId)
            .HasColumnName("return_id");

        // Relationships
        builder.HasOne(d => d.Return)
            .WithMany()
            .HasForeignKey(d => d.ReturnId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_return_reason_return");

        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_return_reason_core_workflow");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_return_reason_workflow_id");

        builder.HasIndex(e => e.ReturnId, "IX_core_workflow_return_reason_return_id");
    }
}
