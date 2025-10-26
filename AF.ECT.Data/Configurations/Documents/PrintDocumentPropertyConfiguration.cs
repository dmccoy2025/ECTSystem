using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Documents;

/// <summary>
/// Entity Framework Core configuration for the PrintDocumentProperty entity.
/// Configures key-value properties for print documents enabling flexible metadata
/// and configuration settings specific to each document template.
/// </summary>
public class PrintDocumentPropertyConfiguration : IEntityTypeConfiguration<PrintDocumentProperty>
{
    /// <summary>
    /// Configures the PrintDocumentProperty entity with composite key, properties,
    /// and indexes for efficient document property lookups and management.
    /// </summary>
    /// <param name="builder">The entity type builder for PrintDocumentProperty.</param>
    public void Configure(EntityTypeBuilder<PrintDocumentProperty> builder)
    {
        builder.HasKey(e => e.PropertyId).HasName("PK__PRINT_DO__CFB9A9A4A4956C87");

        builder.ToTable("PRINT_DOCUMENT_PROPERTY", "dbo");

        builder.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");
        builder.Property(e => e.DocId).HasColumnName("DOC_ID");
        builder.Property(e => e.PropertyName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("PROPERTY_NAME");
        builder.Property(e => e.PropertyValue)
            .HasMaxLength(500)
            .IsUnicode(false)
            .HasColumnName("PROPERTY_VALUE");

        builder.HasIndex(e => e.DocId, "IX_PRINT_DOCUMENT_PROPERTY_DOC_ID");
        builder.HasIndex(e => new { e.DocId, e.PropertyName }, "UQ_PRINT_DOCUMENT_PROPERTY_DOC_NAME")
            .IsUnique();
    }
}
