using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowMemberComponent"/> entity.
/// </summary>
public class CoreWorkflowMemberComponentConfiguration : IEntityTypeConfiguration<CoreWorkflowMemberComponent>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowMemberComponent> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_member_component", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.ComponentId })
            .HasName("PK_core_workflow_member_component");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.ComponentId)
            .HasColumnName("component_id");

        // Relationships
        builder.HasOne(d => d.Component)
            .WithMany()
            .HasForeignKey(d => d.ComponentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_member_component_core_lkup_component");

        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_member_component_core_workflow");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_member_component_workflow_id");

        builder.HasIndex(e => e.ComponentId, "IX_core_workflow_member_component_component_id");
    }
}
