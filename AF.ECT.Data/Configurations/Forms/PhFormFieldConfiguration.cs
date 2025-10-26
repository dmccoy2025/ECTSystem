using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the PhFormField entity.
/// Configures Print Health form field assignments linking fields and field types to
/// specific sections with display ordering and tooltip support for dynamic form rendering.
/// </summary>
public class PhFormFieldConfiguration : IEntityTypeConfiguration<PhFormField>
{
    /// <summary>
    /// Configures the PhFormField entity with composite key, relationships to fields,
    /// field types, and sections, and display order properties for form layout control.
    /// </summary>
    /// <param name="builder">The entity type builder for PhFormField.</param>
    public void Configure(EntityTypeBuilder<PhFormField> builder)
    {
        builder.HasKey(e => new { e.SectionId, e.FieldId, e.FieldTypeId })
            .HasName("PK__PH_FORM___0AD1DB6F0D685C72");

        builder.ToTable("PH_FORM_FIELD", "dbo");

        builder.Property(e => e.SectionId).HasColumnName("SECTION_ID");
        builder.Property(e => e.FieldId).HasColumnName("FIELD_ID");
        builder.Property(e => e.FieldTypeId).HasColumnName("FIELD_TYPE_ID");
        builder.Property(e => e.FieldDisplayOrder).HasColumnName("FIELD_DISPLAY_ORDER");
        builder.Property(e => e.FieldTypeDisplayOrder).HasColumnName("FIELD_TYPE_DISPLAY_ORDER");
        builder.Property(e => e.ToolTip)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("TOOL_TIP");

        builder.HasOne(d => d.Field).WithMany(p => p.PhFormFields)
            .HasForeignKey(d => d.FieldId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_FIELD_FIELD");

        builder.HasOne(d => d.FieldType).WithMany(p => p.PhFormFields)
            .HasForeignKey(d => d.FieldTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_FIELD_FIELD_TYPE");

        builder.HasOne(d => d.Section).WithMany(p => p.PhFormFields)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_FIELD_SECTION");

        builder.HasIndex(e => e.SectionId, "IX_PH_FORM_FIELD_SECTION");
        builder.HasIndex(e => e.FieldDisplayOrder, "IX_PH_FORM_FIELD_DISPLAY_ORDER");
    }
}
