using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Memos;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreMemo"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_memo table,
/// which stores memoranda attached to cases for official correspondence and documentation.
/// </remarks>
public class CoreMemoConfiguration : IEntityTypeConfiguration<CoreMemo>
{
    /// <summary>
    /// Configures the CoreMemo entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreMemo> builder)
    {
        // Table mapping
        builder.ToTable("core_memo", "dbo");

        // Primary key
        builder.HasKey(e => e.MemoId)
            .HasName("PK_core_memo");

        // Property configurations
        builder.Property(e => e.MemoId)
            .HasColumnName("memo_id");

        builder.Property(e => e.RefId)
            .HasColumnName("ref_id");

        builder.Property(e => e.TemplateId)
            .HasColumnName("template_id");

        builder.Property(e => e.Deleted)
            .HasColumnName("deleted");

        builder.Property(e => e.LetterHead)
            .HasColumnName("letter_head");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("(getdate())");

        // Foreign key relationships
        builder.HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.CoreMemos)
            .HasForeignKey(d => d.CreatedBy)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_memo_created_by");

        builder.HasOne(d => d.LetterHeadNavigation)
            .WithMany(p => p.CoreMemos)
            .HasForeignKey(d => d.LetterHead)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_memo_letter_head");

        builder.HasOne(d => d.Template)
            .WithMany(p => p.CoreMemos)
            .HasForeignKey(d => d.TemplateId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_memo_template");

        // Indexes
        builder.HasIndex(e => e.RefId, "IX_core_memo_ref_id");

        builder.HasIndex(e => new { e.RefId, e.Deleted })
            .HasDatabaseName("IX_core_memo_ref_deleted")
            .HasFilter("[deleted] = 0");

        builder.HasIndex(e => e.CreatedBy, "IX_core_memo_created_by");

        builder.HasIndex(e => e.CreatedDate, "IX_core_memo_created_date");
    }
}
