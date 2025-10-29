using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Memos;

/// <summary>
/// Configuration for the <see cref="CoreMemoTemplate"/> entity.
/// </summary>
public class CoreMemoTemplateConfiguration : IEntityTypeConfiguration<CoreMemoTemplate>
{
    public void Configure(EntityTypeBuilder<CoreMemoTemplate> builder)
    {
        // Table mapping
        builder.ToTable("core_memo_template", "dbo");

        // Primary key
        builder.HasKey(e => e.TemplateId)
            .HasName("PK_core_memo_template");

        // Properties
        builder.Property(e => e.TemplateId)
            .HasColumnName("template_id");

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        builder.Property(e => e.Body)
            .IsRequired()
            .HasColumnName("body");

        builder.Property(e => e.Attachments)
            .HasColumnName("attachments");

        builder.Property(e => e.Active)
            .HasColumnName("active");

        builder.Property(e => e.SigBlock)
            .HasColumnName("sig_block");

        builder.Property(e => e.AddDate)
            .HasColumnName("add_date");

        builder.Property(e => e.AddSuspenseDate)
            .HasColumnName("add_suspense_date");

        builder.Property(e => e.DataSource)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("data_source");

        builder.Property(e => e.Compo)
            .IsRequired()
            .HasMaxLength(4)
            .HasColumnName("compo");

        builder.Property(e => e.AddSignature)
            .HasColumnName("add_signature");

        builder.Property(e => e.Phi)
            .HasColumnName("phi");

        builder.Property(e => e.Module)
            .HasColumnName("module");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasColumnName("created_date");

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("modified_by");

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime")
            .HasColumnName("modified_date");

        builder.Property(e => e.FontSize)
            .HasColumnName("font_size");

        // Indexes
        builder.HasIndex(e => e.Active, "IX_core_memo_template_active");

        builder.HasIndex(e => e.Module, "IX_core_memo_template_module");

        builder.HasIndex(e => e.Compo, "IX_core_memo_template_compo");
    }
}
