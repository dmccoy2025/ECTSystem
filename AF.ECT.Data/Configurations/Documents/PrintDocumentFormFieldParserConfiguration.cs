using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Documents;

/// <summary>
/// Entity Framework Core configuration for the PrintDocumentFormFieldParser entity.
/// Configures form field parsing strategies for extracting and mapping data to document
/// templates with different parsing rules for various document formats.
/// </summary>
public class PrintDocumentFormFieldParserConfiguration : IEntityTypeConfiguration<PrintDocumentFormFieldParser>
{
    /// <summary>
    /// Configures the PrintDocumentFormFieldParser entity with table mapping, primary key,
    /// and unique name constraint for form field parsing strategy definitions.
    /// </summary>
    /// <param name="builder">The entity type builder for PrintDocumentFormFieldParser.</param>
    public void Configure(EntityTypeBuilder<PrintDocumentFormFieldParser> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__PRINT_DO__3214EC277AD1B0E7");

        builder.ToTable("PRINT_DOCUMENT_FORM_FIELD_PARSER", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("NAME");

        builder.HasIndex(e => e.Name, "UQ_PRINT_DOCUMENT_FORM_FIELD_PARSER_NAME").IsUnique();
    }
}
