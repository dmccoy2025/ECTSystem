using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework configuration for the <see cref="HyperLinkType"/> entity.
/// Configures hyperlink type classifications for categorizing and filtering application hyperlinks.
/// </summary>
/// <remarks>
/// HyperLinkType defines the types/categories of hyperlinks available in the application
/// (e.g., "External Resource", "Internal Navigation", "Documentation", "Help"). This classification
/// enables filtering, grouping, and specialized handling of different link types in the UI.
/// 
/// Key characteristics:
/// - Identity primary key
/// - Required unique name
/// - One-to-many relationship with HyperLink
/// - No audit trail (lookup/reference data)
/// - Parent entity for hyperlink categorization
/// </remarks>
public class HyperLinkTypeConfiguration : IEntityTypeConfiguration<HyperLinkType>
{
    /// <summary>
    /// Configures the HyperLinkType entity with table mapping, primary key, required fields,
    /// and one-to-many relationship with HyperLink.
    /// </summary>
    /// <param name="builder">The entity type builder for HyperLinkType.</param>
    public void Configure(EntityTypeBuilder<HyperLinkType> builder)
    {
        builder.ToTable("HyperLinkType", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_HyperLinkType");

        // Primary key
        builder.Property(e => e.Id)
            .HasColumnName("ID");

        // Required properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("Name");

        // One-to-many relationship configured on HyperLink side
        // No additional indexes needed (primary key index sufficient for parent lookups)
    }
}
