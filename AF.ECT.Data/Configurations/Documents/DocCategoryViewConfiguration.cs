using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Documents;

/// <summary>
/// Configures the <see cref="DocCategoryView"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents the mapping between document views and document categories with view ordering
/// and redaction flags.
/// </remarks>
public class DocCategoryViewConfiguration : IEntityTypeConfiguration<DocCategoryView>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<DocCategoryView> builder)
    {
        // Table mapping
        builder.ToTable("DocCategoryView", "dbo");

        // Primary key
        builder.HasKey(e => e.DocCatViewId)
            .HasName("PK_DocCategoryView");

        // Properties
        builder.Property(e => e.DocCatViewId).HasColumnName("DocCatViewID");
        builder.Property(e => e.DocViewId).HasColumnName("DocViewID");
        builder.Property(e => e.DocCatId).HasColumnName("DocCatID");
        builder.Property(e => e.ViewOrder).HasColumnName("ViewOrder");
        builder.Property(e => e.IsRedacted).HasColumnName("IsRedacted");

        // Indexes for query performance
        builder.HasIndex(e => e.DocViewId, "IX_DocCategoryView_DocViewID");
        builder.HasIndex(e => e.DocCatId, "IX_DocCategoryView_DocCatID");
        builder.HasIndex(e => new { e.DocViewId, e.ViewOrder }, "IX_DocCategoryView_DocViewID_ViewOrder");
    }
}
