using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the PhField entity.
/// Configures Print Health form field definitions that can be used across multiple
/// forms and sections in the dynamic Print Health form system.
/// </summary>
public class PhFieldConfiguration : IEntityTypeConfiguration<PhField>
{
    /// <summary>
    /// Configures the PhField entity with table mapping, primary key, properties,
    /// and relationships to form fields, values, and query output definitions.
    /// </summary>
    /// <param name="builder">The entity type builder for PhField.</param>
    public void Configure(EntityTypeBuilder<PhField> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__PH_FIELD__3214EC27FA7CBB8B");

        builder.ToTable("PH_FIELD", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("NAME");

        builder.HasMany(d => d.PhFormFields).WithOne(p => p.Field)
            .HasForeignKey(d => d.FieldId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_FIELD_FIELD");

        builder.HasMany(d => d.PhFormValues).WithOne(p => p.Field)
            .HasForeignKey(d => d.FieldId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_VALUE_FIELD");

        builder.HasMany(d => d.RptPhUserQueryOutputFields).WithOne(p => p.Field)
            .HasForeignKey(d => d.FieldId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_OUTPUT_FIELD_FIELD");

        builder.HasIndex(e => e.Name, "UQ_PH_FIELD_NAME").IsUnique();
    }
}
