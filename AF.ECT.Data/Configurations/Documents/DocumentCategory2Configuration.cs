using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Documents;

/// <summary>
/// Configures the <see cref="DocumentCategory2"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents document categories for organizing and classifying documents.
/// </remarks>
public class DocumentCategory2Configuration : IEntityTypeConfiguration<DocumentCategory2>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<DocumentCategory2> builder)
    {
        // Table mapping
        builder.ToTable("DocumentCategory2", "dbo");

        // Primary key
        builder.HasKey(e => e.DocCatId)
            .HasName("PK_DocumentCategory2");

        // Properties
        builder.Property(e => e.DocCatId).HasColumnName("DocCatID");
        builder.Property(e => e.CategoryDescription).HasColumnName("CategoryDescription").HasMaxLength(500).IsRequired();

        // Indexes for query performance
        builder.HasIndex(e => e.CategoryDescription, "IX_DocumentCategory2_CategoryDescription");
    }
}
