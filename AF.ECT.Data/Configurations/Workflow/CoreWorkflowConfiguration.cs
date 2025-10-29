using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreWorkflow"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema and relationships for the core_workflow table,
/// which represents workflow definitions in the ECT system. Workflows define the sequence
/// of statuses and transitions that cases follow from initiation to completion. Different
/// workflows can be configured for formal vs. informal investigations, different components
/// (Active, Reserve, Guard), and different case types (LOD, RLOD, etc.).
/// </remarks>
public class CoreWorkflowConfiguration : IEntityTypeConfiguration<CoreWorkflow>
{
    /// <summary>
    /// Configures the entity of type <see cref="CoreWorkflow"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreWorkflow> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow", "dbo");

        // Primary key
        builder.HasKey(e => e.WorkflowId)
            .HasName("PK_core_workflow");

        // Properties configuration
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.ModuleId)
            .HasColumnName("module_id");

        builder.Property(e => e.Compo)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("compo");

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("title");

        builder.Property(e => e.Formal)
            .HasColumnName("formal");

        builder.Property(e => e.Active)
            .HasColumnName("active");

        builder.Property(e => e.InitialStatus)
            .HasColumnName("initial_status");

        // Relationships
        builder.HasOne(d => d.InitialStatusNavigation)
            .WithMany(p => p.CoreWorkflows)
            .HasForeignKey(d => d.InitialStatus)
            .HasConstraintName("FK_core_workflow_core_work_status");

        builder.HasOne(d => d.Module)
            .WithMany(p => p.CoreWorkflows)
            .HasForeignKey(d => d.ModuleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_core_lkup_module");

        // Indexes
        builder.HasIndex(e => new { e.ModuleId, e.Compo, e.Formal })
            .HasDatabaseName("IX_core_workflow_module_compo_formal")
            .IsUnique();

        builder.HasIndex(e => e.Active, "IX_core_workflow_active");
        
        builder.HasIndex(e => e.InitialStatus, "IX_core_workflow_initial_status");
        
        builder.HasIndex(e => e.Title, "IX_core_workflow_title");
    }
}
