using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the PhFormValue entity.
/// Configures Print Health form field values storing user-entered data linked to
/// specific form instances (Form348Sc), sections, fields, and field types.
/// </summary>
public class PhFormValueConfiguration : IEntityTypeConfiguration<PhFormValue>
{
    /// <summary>
    /// Configures the PhFormValue entity with composite key, relationships to form
    /// references, fields, field types, and sections for complete form data storage.
    /// </summary>
    /// <param name="builder">The entity type builder for PhFormValue.</param>
    public void Configure(EntityTypeBuilder<PhFormValue> builder)
    {
        builder.HasKey(e => new { e.RefId, e.SectionId, e.FieldId, e.FieldTypeId })
            .HasName("PK__PH_FORM___63DC0DC33D07BBA7");

        builder.ToTable("PH_FORM_VALUE", "dbo");

        builder.Property(e => e.RefId).HasColumnName("REF_ID");
        builder.Property(e => e.SectionId).HasColumnName("SECTION_ID");
        builder.Property(e => e.FieldId).HasColumnName("FIELD_ID");
        builder.Property(e => e.FieldTypeId).HasColumnName("FIELD_TYPE_ID");
        builder.Property(e => e.Value)
            .HasMaxLength(4000)
            .IsUnicode(false)
            .HasColumnName("VALUE");

        builder.HasOne(d => d.Field).WithMany(p => p.PhFormValues)
            .HasForeignKey(d => d.FieldId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_VALUE_FIELD");

        builder.HasOne(d => d.FieldType).WithMany(p => p.PhFormValues)
            .HasForeignKey(d => d.FieldTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_VALUE_FIELD_TYPE");

        builder.HasOne(d => d.Ref).WithMany(p => p.PhFormValues)
            .HasForeignKey(d => d.RefId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_VALUE_REF");

        builder.HasOne(d => d.Section).WithMany(p => p.PhFormValues)
            .HasForeignKey(d => d.SectionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_VALUE_SECTION");

        builder.HasIndex(e => e.RefId, "IX_PH_FORM_VALUE_REF");
        builder.HasIndex(e => e.SectionId, "IX_PH_FORM_VALUE_SECTION");
        builder.HasIndex(e => e.FieldId, "IX_PH_FORM_VALUE_FIELD");
        builder.HasIndex(e => e.FieldTypeId, "IX_PH_FORM_VALUE_FIELD_TYPE");
    }
}
