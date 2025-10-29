using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="ChildCaseComment"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the child_case_comment table,
/// which stores threaded replies to parent dialogue comments. Enables nested discussion threads within cases.
/// </remarks>
public class ChildCaseCommentConfiguration : IEntityTypeConfiguration<ChildCaseComment>
{
    /// <summary>
    /// Configures the ChildCaseComment entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<ChildCaseComment> builder)
    {
        // Table mapping
        builder.ToTable("child_case_comment", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_child_case_comment");

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

        builder.Property(e => e.ParentCommentId)
            .HasColumnName("parent_comment_id");

        builder.Property(e => e.Role)
            .HasMaxLength(50)
            .HasColumnName("role");

        // Relationships
        builder.HasOne(d => d.ParentComment)
            .WithMany(p => p.ChildCaseComments)
            .HasForeignKey(d => d.ParentCommentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_child_case_comment_case_dialogue_comment");

        // Indexes
        builder.HasIndex(e => e.ParentCommentId, "IX_child_case_comment_parent_comment_id");

        builder.HasIndex(e => e.Lodid, "IX_child_case_comment_lodid");

        builder.HasIndex(e => e.CreatedDate, "IX_child_case_comment_created_date");

        builder.HasIndex(e => e.Deleted)
            .HasDatabaseName("IX_child_case_comment_deleted")
            .HasFilter("deleted = 0");
    }
}
