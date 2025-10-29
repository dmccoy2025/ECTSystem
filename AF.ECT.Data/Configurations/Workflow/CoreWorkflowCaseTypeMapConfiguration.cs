using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowCaseTypeMap"/> entity.
/// </summary>
public class CoreWorkflowCaseTypeMapConfiguration : IEntityTypeConfiguration<CoreWorkflowCaseTypeMap>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowCaseTypeMap> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_case_type_map", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.CaseTypeId })
            .HasName("PK_core_workflow_case_type_map");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.CaseTypeId)
            .HasColumnName("case_type_id");

        // Relationships
        builder.HasOne(d => d.CaseType)
            .WithMany()
            .HasForeignKey(d => d.CaseTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_case_type_map_core_case_type");

        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_workflow_case_type_map_core_workflow");

        // Indexes
        builder.HasIndex(e => e.WorkflowId, "IX_core_workflow_case_type_map_workflow_id");

        builder.HasIndex(e => e.CaseTypeId, "IX_core_workflow_case_type_map_case_type_id");
    }
}
