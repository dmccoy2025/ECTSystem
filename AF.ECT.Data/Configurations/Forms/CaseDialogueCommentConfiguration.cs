using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CaseDialogueComment"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the case_dialogue_comment table,
/// which stores parent-level dialogue comments that can have threaded child replies. Supports collaborative
/// discussion threads within cases.
/// </remarks>
public class CaseDialogueCommentConfiguration : IEntityTypeConfiguration<CaseDialogueComment>
{
    /// <summary>
    /// Configures the CaseDialogueComment entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CaseDialogueComment> builder)
    {
        // Table mapping
        builder.ToTable("case_dialogue_comment", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_case_dialogue_comment");

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

        builder.Property(e => e.Role)
            .HasMaxLength(50)
            .HasColumnName("role");

        // Indexes
        builder.HasIndex(e => e.Lodid, "IX_case_dialogue_comment_lodid");

        builder.HasIndex(e => e.CreatedDate, "IX_case_dialogue_comment_created_date");

        builder.HasIndex(e => e.Deleted)
            .HasDatabaseName("IX_case_dialogue_comment_deleted")
            .HasFilter("deleted = 0");
        
        builder.HasIndex(e => e.CreatedBy, "IX_case_dialogue_comment_created_by");
        
        builder.HasIndex(e => e.ModuleId, "IX_case_dialogue_comment_module_id");
        
        builder.HasIndex(e => e.CommentType, "IX_case_dialogue_comment_comment_type");
    }
}
