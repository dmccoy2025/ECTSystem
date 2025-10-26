using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Memos;

/// <summary>
/// Entity Framework configuration for the CoreMemoContents2 entity.
/// </summary>
public class CoreMemoContents2Configuration : IEntityTypeConfiguration<CoreMemoContents2>
{
    /// <summary>
    /// Configures the CoreMemoContents2 entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreMemoContents2> builder)
    {
        builder.ToTable("Core_MemoContents2", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_MemoContents2");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.MemoId).HasColumnName("MemoID");
        builder.Property(e => e.Body).HasColumnName("Body");
        builder.Property(e => e.SigBlock).HasColumnName("SigBlock");
        builder.Property(e => e.SuspenseDate)
            .HasMaxLength(50)
            .HasColumnName("SuspenseDate");
        builder.Property(e => e.MemoDate)
            .HasMaxLength(50)
            .HasColumnName("MemoDate");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.Attachments).HasColumnName("Attachments");

        builder.HasIndex(e => e.MemoId, "IX_Core_MemoContents2_MemoID");
        builder.HasIndex(e => e.CreatedBy, "IX_Core_MemoContents2_CreatedBy");
    }
}
