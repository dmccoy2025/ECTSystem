using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Permissions;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CorePage"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_page table,
/// which catalogs pages and features in the application for access control purposes.
/// </remarks>
public class CorePageConfiguration : IEntityTypeConfiguration<CorePage>
{
    /// <summary>
    /// Configures the CorePage entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CorePage> builder)
    {
        // Table mapping
        builder.ToTable("core_page", "dbo");

        // Primary key
        builder.HasKey(e => e.PageId)
            .HasName("PK_core_page");

        // Property configurations
        builder.Property(e => e.PageId)
            .HasColumnName("page_id");

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        // Indexes
        builder.HasIndex(e => e.Title, "IX_core_page_title");
    }
}
