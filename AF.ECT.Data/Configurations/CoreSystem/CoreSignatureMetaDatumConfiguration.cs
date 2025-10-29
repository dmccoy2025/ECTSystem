using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreSignatureMetaDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_signature_meta_data table,
/// which stores metadata about digital signatures applied to cases. Tracks who signed, when, in what capacity,
/// and at what workflow status. Critical for audit trail and legal compliance.
/// </remarks>
public class CoreSignatureMetaDatumConfiguration : IEntityTypeConfiguration<CoreSignatureMetaDatum>
{
    /// <summary>
    /// Configures the CoreSignatureMetaDatum entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreSignatureMetaDatum> builder)
    {
        // Table mapping
        builder.ToTable("core_signature_meta_data", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_signature_meta_data");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.RefId)
            .HasColumnName("ref_id");

        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.WorkStatus)
            .HasColumnName("work_status");

        builder.Property(e => e.UserGroup)
            .HasColumnName("user_group");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.Date)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()")
            .HasColumnName("date");

        builder.Property(e => e.NameAndRank)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name_and_rank");

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .HasColumnName("title");

        // Relationships
        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_signature_meta_data_core_user");

        builder.HasOne(d => d.UserGroupNavigation)
            .WithMany()
            .HasForeignKey(d => d.UserGroup)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_signature_meta_data_core_user_group");

        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_signature_meta_data_core_workflow");

        builder.HasOne(d => d.WorkStatusNavigation)
            .WithMany()
            .HasForeignKey(d => d.WorkStatus)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_signature_meta_data_core_work_status");

        // Indexes
        builder.HasIndex(e => new { e.WorkflowId, e.RefId }, "IX_core_signature_meta_data_workflow_ref");

        builder.HasIndex(e => e.UserId, "IX_core_signature_meta_data_user_id");

        builder.HasIndex(e => e.Date, "IX_core_signature_meta_data_date");

        builder.HasIndex(e => new { e.WorkflowId, e.RefId, e.WorkStatus, e.UserGroup })
            .IsUnique()
            .HasDatabaseName("UQ_core_signature_meta_data_complete");
    }
}
