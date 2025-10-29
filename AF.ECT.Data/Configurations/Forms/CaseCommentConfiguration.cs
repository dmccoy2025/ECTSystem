using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CaseComment"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the case_comment table,
/// which stores comments and notes added to cases throughout their lifecycle. Comments provide audit
/// trail and collaboration capabilities for case workers.
/// </remarks>
public class CaseCommentConfiguration : IEntityTypeConfiguration<CaseComment>
{
    /// <summary>
    /// Configures the CaseComment entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CaseComment> builder)
    {
        // Table mapping
        builder.ToTable("case_comment", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_case_comment");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Lodid)
            .HasColumnName("lodid");

        builder.Property(e => e.Comments)
            .IsRequired()
            .HasColumnName("comments");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()")
            .HasColumnName("created_date");

        builder.Property(e => e.Deleted)
            .HasColumnName("deleted");

        builder.Property(e => e.ModuleId)
            .HasColumnName("module_id");

        builder.Property(e => e.CommentType)
            .HasColumnName("comment_type");

        // Indexes
        builder.HasIndex(e => e.Lodid, "IX_case_comment_lodid");

        builder.HasIndex(e => e.CreatedBy, "IX_case_comment_created_by");

        builder.HasIndex(e => e.CreatedDate, "IX_case_comment_created_date");

        builder.HasIndex(e => e.Deleted)
            .HasDatabaseName("IX_case_comment_deleted")
            .HasFilter("deleted = 0");

        builder.HasIndex(e => new { e.ModuleId, e.Lodid }, "IX_case_comment_module_lodid");
    }
}
