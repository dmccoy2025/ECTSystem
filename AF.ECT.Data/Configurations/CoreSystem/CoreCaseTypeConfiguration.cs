using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreCaseType"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_case_type table,
/// which defines the types of cases that can be managed in the system (e.g., LOD, IDES, Medical Review).
/// </remarks>
public class CoreCaseTypeConfiguration : IEntityTypeConfiguration<CoreCaseType>
{
    /// <summary>
    /// Configures the CoreCaseType entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreCaseType> builder)
    {
        // Table mapping
        builder.ToTable("core_case_type", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_case_type");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UQ_core_case_type_name");
    }
}
