using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpGradeLookUp"/> entity.
/// Configures a staging table for importing military grade lookup mappings from external systems.
/// </summary>
/// <remarks>
/// ImpGradeLookUp is a temporary staging table used during data import processes to map grade
/// strings (e.g., "E-5", "O-3") to their corresponding grade codes in the system. This entity
/// has no primary key (keyless entity) as it represents transient import data used for
/// grade standardization and normalization.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable properties to accommodate raw import data
/// - Grade string to grade code mapping
/// - Used for grade standardization during import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful import and validation
/// </remarks>
public class ImpGradeLookUpConfiguration : IEntityTypeConfiguration<ImpGradeLookUp>
{
    /// <summary>
    /// Configures the ImpGradeLookUp entity as a keyless staging table with grade lookup mapping fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpGradeLookUp.</param>
    public void Configure(EntityTypeBuilder<ImpGradeLookUp> builder)
    {
        builder.ToTable("ImpGradeLookUp", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Grade mapping properties
        builder.Property(e => e.Grade)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("GRADE");

        builder.Property(e => e.Gradecode)
            .HasColumnName("GRADECODE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.Grade, "IX_imp_grade_lookup_grade");
        
        builder.HasIndex(e => e.Gradecode, "IX_imp_grade_lookup_gradecode");
    }
}
