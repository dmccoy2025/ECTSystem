using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Memos;

/// <summary>
/// Entity Framework configuration for the CoreMemos2 entity.
/// </summary>
public class CoreMemos2Configuration : IEntityTypeConfiguration<CoreMemos2>
{
    /// <summary>
    /// Configures the CoreMemos2 entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreMemos2> builder)
    {
        builder.ToTable("Core_Memos2", "dbo");

        builder.HasKey(e => e.MemoId)
            .HasName("PK_Core_Memos2");

        builder.Property(e => e.MemoId).HasColumnName("MemoID");
        builder.Property(e => e.RefId).HasColumnName("RefID");
        builder.Property(e => e.ModuleId).HasColumnName("ModuleID");
        builder.Property(e => e.TemplateId).HasColumnName("TemplateID");
        builder.Property(e => e.Deleted).HasColumnName("Deleted");
        builder.Property(e => e.LetterHead).HasColumnName("LetterHead");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");

        builder.HasIndex(e => e.RefId, "IX_Core_Memos2_RefID");
        builder.HasIndex(e => e.TemplateId, "IX_Core_Memos2_TemplateID");
    }
}
