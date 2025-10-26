using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupWorkflowAction"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_workflow_action table,
/// which defines the available actions that can be performed on workflow states (Approve, Reject, Return,
/// Forward, etc.). Workflow actions determine what transitions are possible between workflow states.
/// </remarks>
public class CoreLkupWorkflowActionConfiguration : IEntityTypeConfiguration<CoreLkupWorkflowAction>
{
    /// <summary>
    /// Configures the CoreLkupWorkflowAction entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupWorkflowAction> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_workflow_action", "dbo");

        // Primary key
        builder.HasKey(e => e.Type)
            .HasName("PK_core_lkup_workflow_action");

        // Property configurations
        builder.Property(e => e.Type)
            .HasColumnName("type");

        builder.Property(e => e.Text)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("text");

        // Indexes
        builder.HasIndex(e => e.Text)
            .IsUnique()
            .HasDatabaseName("UQ_core_lkup_workflow_action_text");
    }
}
