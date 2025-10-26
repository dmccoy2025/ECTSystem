using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Documents;

/// <summary>
/// Configures the <see cref="DocumentView"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents a document view definition for organizing and displaying documents.
/// </remarks>
public class DocumentViewConfiguration : IEntityTypeConfiguration<DocumentView>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<DocumentView> builder)
    {
        // Table mapping
        builder.ToTable("DocumentView", "dbo");

        // Primary key
        builder.HasKey(e => e.ViewId)
            .HasName("PK_DocumentView");

        // Properties
        builder.Property(e => e.ViewId).HasColumnName("ViewID");
        builder.Property(e => e.Description).HasColumnName("Description").HasMaxLength(500).IsRequired();

        // Indexes for query performance
        builder.HasIndex(e => e.Description, "IX_DocumentView_Description");
    }
}
