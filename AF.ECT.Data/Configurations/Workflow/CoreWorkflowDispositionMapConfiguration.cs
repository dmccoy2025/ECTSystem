using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowDispositionMap"/> entity.
/// </summary>
public class CoreWorkflowDispositionMapConfiguration : IEntityTypeConfiguration<CoreWorkflowDispositionMap>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowDispositionMap> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_disposition_map", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.DispositionId })
            .HasName("PK_core_workflow_disposition_map");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.DispositionId)
            .HasColumnName("disposition_id");

        // Relationships
        builder.HasOne(d => d.Disposition)
            .WithMany()
            .HasForeignKey(d => d.DispositionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_disposition_map_core_lkup_disposition");

        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_disposition_map_core_workflow");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_disposition_map_workflow_id");

        builder.HasIndex(e => e.DispositionId, "IX_core_workflow_disposition_map_disposition_id");
    }
}
