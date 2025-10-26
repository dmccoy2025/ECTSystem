using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowCertificationStampMap"/> entity.
/// </summary>
public class CoreWorkflowCertificationStampMapConfiguration : IEntityTypeConfiguration<CoreWorkflowCertificationStampMap>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowCertificationStampMap> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_certification_stamp_map", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.WorkflowId, e.CertStampId })
            .HasName("PK_core_workflow_certification_stamp_map");

        // Properties
        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.CertStampId)
            .HasColumnName("cert_stamp_id");

        // Indexes
        builder.HasIndex(e => e.WorkflowId)
            .HasDatabaseName("IX_core_workflow_certification_stamp_map_workflow_id");

        builder.HasIndex(e => e.CertStampId)
            .HasDatabaseName("IX_core_workflow_certification_stamp_map_cert_stamp_id");
    }
}
