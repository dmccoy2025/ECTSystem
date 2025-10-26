using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the PhSection entity.
/// Configures Print Health form sections that organize fields into hierarchical layouts
/// with support for column layouts, page breaks, and nested subsections.
/// </summary>
public class PhSectionConfiguration : IEntityTypeConfiguration<PhSection>
{
    /// <summary>
    /// Configures the PhSection entity with primary key, properties for hierarchical
    /// organization, display settings, and relationships to form fields and values.
    /// </summary>
    /// <param name="builder">The entity type builder for PhSection.</param>
    public void Configure(EntityTypeBuilder<PhSection> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__PH_SECTI__3214EC2729091925");

        builder.ToTable("PH_SECTION", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");
        builder.Property(e => e.FieldColumns).HasColumnName("FIELD_COLUMNS");
        builder.Property(e => e.IsTopLevel).HasColumnName("IS_TOP_LEVEL");
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("NAME");
        builder.Property(e => e.PageBreak).HasColumnName("PAGE_BREAK");
        builder.Property(e => e.ParentId).HasColumnName("PARENT_ID");

        builder.HasMany(d => d.PhFormFields).WithOne(p => p.Section)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_FIELD_SECTION");

        builder.HasMany(d => d.PhFormValues).WithOne(p => p.Section)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_VALUE_SECTION");

        builder.HasMany(d => d.RptPhUserQueryOutputFields).WithOne(p => p.Section)
            .HasForeignKey(d => d.SectionId)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_OUTPUT_FIELD_SECTION");

        builder.HasIndex(e => e.ParentId, "IX_PH_SECTION_PARENT");
        builder.HasIndex(e => e.DisplayOrder, "IX_PH_SECTION_DISPLAY_ORDER");
        builder.HasIndex(e => e.IsTopLevel, "IX_PH_SECTION_IS_TOP_LEVEL");
    }
}
