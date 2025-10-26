using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreAssociatedCase"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_associated_case table,
/// which tracks relationships between cases across different workflows. Enables linking related investigations,
/// follow-up cases, and cross-referenced matters.
/// </remarks>
public class CoreAssociatedCaseConfiguration : IEntityTypeConfiguration<CoreAssociatedCase>
{
    /// <summary>
    /// Configures the CoreAssociatedCase entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreAssociatedCase> builder)
    {
        // Table mapping
        builder.ToTable("core_associated_case");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.RefId, e.AssociatedWorkflow, e.AssociatedRefId })
            .HasName("PK_core_associated_case");

        // Property configurations
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.RefId)
            .HasColumnName("ref_id");

        builder.Property(e => e.AssociatedWorkflow)
            .HasColumnName("associated_workflow");

        builder.Property(e => e.AssociatedRefId)
            .HasColumnName("associated_ref_id");

        builder.Property(e => e.AssociatedCaseId)
            .HasMaxLength(100)
            .HasColumnName("associated_case_id");

        // Indexes
        builder.HasIndex(e => new { e.WorkflowId, e.RefId })
            .HasDatabaseName("IX_core_associated_case_workflow_ref");

        builder.HasIndex(e => new { e.AssociatedWorkflow, e.AssociatedRefId })
            .HasDatabaseName("IX_core_associated_case_associated");

        builder.HasIndex(e => e.AssociatedCaseId)
            .HasDatabaseName("IX_core_associated_case_case_id");
    }
}
