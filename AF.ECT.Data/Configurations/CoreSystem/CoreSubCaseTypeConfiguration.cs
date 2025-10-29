using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreSubCaseType"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_sub_case_type table,
/// which defines subcategories or specializations of case types for more granular classification.
/// </remarks>
public class CoreSubCaseTypeConfiguration : IEntityTypeConfiguration<CoreSubCaseType>
{
    /// <summary>
    /// Configures the CoreSubCaseType entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreSubCaseType> builder)
    {
        // Table mapping
        builder.ToTable("core_sub_case_type", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_sub_case_type");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        // Indexes
        builder.HasIndex(e => e.Name, "IX_core_sub_case_type_name");
    }
}
