using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Memos;

/// <summary>
/// Configuration for the <see cref="CoreMemoContent"/> entity.
/// </summary>
public class CoreMemoContentConfiguration : IEntityTypeConfiguration<CoreMemoContent>
{
    public void Configure(EntityTypeBuilder<CoreMemoContent> builder)
    {
        // Table mapping
        builder.ToTable("core_memo_content", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_memo_content");

        // Properties
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.MemoId)
            .HasColumnName("memo_id");

        builder.Property(e => e.Body)
            .IsRequired()
            .HasColumnName("body");

        builder.Property(e => e.SigBlock)
            .HasColumnName("sig_block");

        builder.Property(e => e.SuspenseDate)
            .HasMaxLength(50)
            .HasColumnName("suspense_date");

        builder.Property(e => e.MemoDate)
            .HasMaxLength(50)
            .HasColumnName("memo_date");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasColumnName("created_date");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.Attachments)
            .HasColumnName("attachments");

        // Indexes
        builder.HasIndex(e => e.MemoId, "IX_core_memo_content_memo_id");

        builder.HasIndex(e => e.CreatedDate, "IX_core_memo_content_created_date");
    }
}
