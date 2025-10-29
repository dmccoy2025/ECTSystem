using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowView"/> entity.
/// </summary>
public class CoreWorkflowViewConfiguration : IEntityTypeConfiguration<CoreWorkflowView>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowView> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_view", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.PageId })
            .HasName("PK_core_workflow_view");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.PageId)
            .HasColumnName("page_id");

        // Relationships
        builder.HasOne(d => d.Page)
            .WithMany()
            .HasForeignKey(d => d.PageId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_view_core_page");

        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_view_core_workflow");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_view_workflow_id");

        builder.HasIndex(e => e.PageId, "IX_core_workflow_view_page_id");
    }
}
