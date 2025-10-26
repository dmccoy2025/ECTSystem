using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework configuration for the <see cref="HyperLink"/> entity.
/// Configures application hyperlinks for navigation, external resources, and documentation references.
/// </summary>
/// <remarks>
/// HyperLink manages hyperlinks throughout the application, including external URLs, internal navigation
/// links, and documentation references. Each hyperlink has a unique name, display text for presentation,
/// a value (URL or path), and a type classification for categorization and filtering.
/// 
/// Key characteristics:
/// - Identity primary key
/// - Required name, display text, and value
/// - Type classification via foreign key to HyperLinkType
/// - Supports external URLs, internal navigation, and documentation links
/// - No audit trail (configuration data managed by administrators)
/// </remarks>
public class HyperLinkConfiguration : IEntityTypeConfiguration<HyperLink>
{
    /// <summary>
    /// Configures the HyperLink entity with table mapping, primary key, required fields,
    /// and foreign key relationship to HyperLinkType.
    /// </summary>
    /// <param name="builder">The entity type builder for HyperLink.</param>
    public void Configure(EntityTypeBuilder<HyperLink> builder)
    {
        builder.ToTable("HyperLink", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_HyperLink");

        // Primary key
        builder.Property(e => e.Id)
            .HasColumnName("ID");

        // Required properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("Name");

        builder.Property(e => e.TypeId)
            .HasColumnName("TypeID");

        builder.Property(e => e.DisplayText)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("DisplayText");

        builder.Property(e => e.Value)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("Value");

        // Foreign key relationship
        builder.HasOne(d => d.Type)
            .WithMany(p => p.HyperLinks)
            .HasForeignKey(d => d.TypeId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_HyperLink_HyperLinkType");

        // Indexes
        builder.HasIndex(e => e.TypeId, "IX_HyperLink_TypeID");
        builder.HasIndex(e => e.Name, "IX_HyperLink_Name");
    }
}
