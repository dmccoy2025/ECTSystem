using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Documents;

/// <summary>
/// Entity Framework Core configuration for the PrintDocument entity.
/// Configures printable document templates with file information, stored procedure
/// references for data retrieval, and form field parser assignments for dynamic document generation.
/// </summary>
public class PrintDocumentConfiguration : IEntityTypeConfiguration<PrintDocument>
{
    /// <summary>
    /// Configures the PrintDocument entity with table mapping, primary key, properties,
    /// and relationship to form field parsers for template-based document printing.
    /// </summary>
    /// <param name="builder">The entity type builder for PrintDocument.</param>
    public void Configure(EntityTypeBuilder<PrintDocument> builder)
    {
        builder.HasKey(e => e.Docid).HasName("PK__PRINT_DO__0F53CFBBB20DA1A4");

        builder.ToTable("PRINT_DOCUMENT", "dbo");

        builder.Property(e => e.Docid).HasColumnName("DOCID");
        builder.Property(e => e.Compo).HasColumnName("COMPO");
        builder.Property(e => e.DocName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("DOC_NAME");
        builder.Property(e => e.Filename)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("FILENAME");
        builder.Property(e => e.Filetype)
            .HasMaxLength(30)
            .IsUnicode(false)
            .HasColumnName("FILETYPE");
        builder.Property(e => e.FormFieldParserId).HasColumnName("FORM_FIELD_PARSER_ID");
        builder.Property(e => e.SpGetdata)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("SP_GETDATA");

        builder.HasOne(d => d.FormFieldParser).WithMany()
            .HasForeignKey(d => d.FormFieldParserId)
            .HasConstraintName("FK_PRINT_DOCUMENT_FORM_FIELD_PARSER");

        builder.HasIndex(e => e.DocName, "IX_PRINT_DOCUMENT_DOC_NAME");
        builder.HasIndex(e => e.Filetype, "IX_PRINT_DOCUMENT_FILETYPE");
    }
}
