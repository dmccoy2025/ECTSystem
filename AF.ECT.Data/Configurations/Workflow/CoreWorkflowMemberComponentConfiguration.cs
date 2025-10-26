using Microsoft.EntityFrameworkCore;
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
        builder.ToTable("core_workflow_member_component");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.ComponentId })
            .HasName("PK_core_workflow_member_component");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.ComponentId)
            .HasColumnName("component_id");

        // Indexes
        builder.HasIndex(e => e.WorkflowId)
            .HasDatabaseName("IX_core_workflow_member_component_workflow_id");

        builder.HasIndex(e => e.ComponentId)
            .HasDatabaseName("IX_core_workflow_member_component_component_id");
    }
}
