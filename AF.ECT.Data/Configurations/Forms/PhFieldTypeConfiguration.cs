using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the PhFieldType entity.
/// Configures Print Health field type definitions specifying data types, presentation
/// properties, and validation rules for dynamic form field rendering.
/// </summary>
public class PhFieldTypeConfiguration : IEntityTypeConfiguration<PhFieldType>
{
    /// <summary>
    /// Configures the PhFieldType entity with table mapping, primary key, properties,
    /// relationships, and indexes for efficient field type lookups and form rendering.
    /// </summary>
    /// <param name="builder">The entity type builder for PhFieldType.</param>
    public void Configure(EntityTypeBuilder<PhFieldType> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__PH_FIELD__3214EC27F0D9BEB2");

        builder.ToTable("PH_FIELD_TYPE", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Color)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("COLOR");
        builder.Property(e => e.DataTypeId).HasColumnName("DATA_TYPE_ID");
        builder.Property(e => e.Datasource)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("DATASOURCE");
        builder.Property(e => e.Length).HasColumnName("LENGTH");
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("NAME");
        builder.Property(e => e.Placeholder)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("PLACEHOLDER");

        builder.HasOne(d => d.DataType).WithMany(p => p.PhFieldTypes)
            .HasForeignKey(d => d.DataTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FIELD_TYPE_DATA_TYPE");

        builder.HasMany(d => d.PhFormFields).WithOne(p => p.FieldType)
            .HasForeignKey(d => d.FieldTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_FIELD_FIELD_TYPE");

        builder.HasMany(d => d.PhFormValues).WithOne(p => p.FieldType)
            .HasForeignKey(d => d.FieldTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PH_FORM_VALUE_FIELD_TYPE");

        builder.HasMany(d => d.RptPhUserQueryOutputFields).WithOne(p => p.FieldType)
            .HasForeignKey(d => d.FieldTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_OUTPUT_FIELD_FIELD_TYPE");

        builder.HasIndex(e => e.DataTypeId, "IX_PH_FIELD_TYPE_DATA_TYPE");
        builder.HasIndex(e => e.Name, "UQ_PH_FIELD_TYPE_NAME").IsUnique();
    }
}
