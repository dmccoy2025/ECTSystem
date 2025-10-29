using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Memos;

/// <summary>
/// Configuration for the <see cref="CoreMemoLetterhead"/> entity.
/// </summary>
public class CoreMemoLetterheadConfiguration : IEntityTypeConfiguration<CoreMemoLetterhead>
{
    public void Configure(EntityTypeBuilder<CoreMemoLetterhead> builder)
    {
        // Table mapping
        builder.ToTable("core_memo_letterhead", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_memo_letterhead");

        // Properties
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        builder.Property(e => e.Version)
            .HasColumnName("version");

        builder.Property(e => e.Compo)
            .IsRequired()
            .HasMaxLength(4)
            .HasColumnName("compo");

        builder.Property(e => e.DateCreated)
            .HasColumnType("datetime")
            .HasColumnName("date_created");

        builder.Property(e => e.LogoImageLeft)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("logo_image_left");

        builder.Property(e => e.HeaderTitle)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("header_title");

        builder.Property(e => e.HeaderTitleSize)
            .HasColumnName("header_title_size");

        builder.Property(e => e.HeaderSubtitle)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("header_subtitle");

        builder.Property(e => e.HeaderSubtitleSize)
            .HasColumnName("header_subtitle_size");

        builder.Property(e => e.Font)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("font");

        builder.Property(e => e.FontSize)
            .HasColumnName("font_size");

        builder.Property(e => e.HeaderFontColor)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("header_font_color");

        builder.Property(e => e.LogoImageRight)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("logo_image_right");

        builder.Property(e => e.FooterImageCenter)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("footer_image_center");

        builder.Property(e => e.EffectiveDate)
            .HasColumnType("datetime")
            .HasColumnName("effective_date");

        // Indexes
        builder.HasIndex(e => e.Compo, "IX_core_memo_letterhead_compo");

        builder.HasIndex(e => e.EffectiveDate, "IX_core_memo_letterhead_effective_date");
    }
}
