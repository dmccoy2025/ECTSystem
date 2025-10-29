using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowCompletedByGroupMap"/> entity.
/// </summary>
public class CoreWorkflowCompletedByGroupMapConfiguration : IEntityTypeConfiguration<CoreWorkflowCompletedByGroupMap>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowCompletedByGroupMap> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_completed_by_group_map", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.CompletedById })
            .HasName("PK_core_workflow_completed_by_group_map");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.CompletedById)
            .HasColumnName("completed_by_id");

        // Relationships
        builder.HasOne(d => d.CompletedBy)
            .WithMany()
            .HasForeignKey(d => d.CompletedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_completed_by_group_map_core_completed_by_group");

        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_completed_by_group_map_core_workflow");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_completed_by_group_map_workflow_id");

        builder.HasIndex(e => e.CompletedById, "IX_core_workflow_completed_by_group_map_completed_by_id");
    }
}
