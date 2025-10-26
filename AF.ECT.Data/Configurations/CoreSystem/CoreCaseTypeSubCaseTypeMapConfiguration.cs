using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Configures the <see cref="CoreCaseTypeSubCaseTypeMap"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents the many-to-many mapping between case types and sub-case types.
/// </remarks>
public class CoreCaseTypeSubCaseTypeMapConfiguration : IEntityTypeConfiguration<CoreCaseTypeSubCaseTypeMap>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreCaseTypeSubCaseTypeMap> builder)
    {
        // Table mapping
        builder.ToTable("Core_CaseType_SubCaseType_Map", "dbo");

        // Primary key (composite)
        builder.HasKey(e => new { e.CaseTypeId, e.SubCaseTypeId })
            .HasName("PK_Core_CaseType_SubCaseType_Map");

        // Properties
        builder.Property(e => e.CaseTypeId).HasColumnName("CaseTypeID");
        builder.Property(e => e.SubCaseTypeId).HasColumnName("SubCaseTypeID");

        // Indexes for query performance
        builder.HasIndex(e => e.CaseTypeId, "IX_Core_CaseType_SubCaseType_Map_CaseTypeID");
        builder.HasIndex(e => e.SubCaseTypeId, "IX_Core_CaseType_SubCaseType_Map_SubCaseTypeID");
    }
}
