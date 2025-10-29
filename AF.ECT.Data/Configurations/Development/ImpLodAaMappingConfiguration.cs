using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpLodAaMapping"/> entity.
/// Configures a staging table for importing LOD (Line of Duty) Approving Authority (AA) name mappings
/// from legacy systems.
/// </summary>
/// <remarks>
/// ImpLodAaMapping is a temporary staging table used during data migration processes to map
/// legacy approving authority names to standardized names in the new system. This entity has
/// no primary key (keyless entity) as it represents transient import data used for name
/// normalization and standardization.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate raw import data
/// - Import name to standardized name mapping
/// - Rank and branch tracking for authority verification
/// - Used for approving authority name standardization during migration
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful import and validation
/// </remarks>
public class ImpLodAaMappingConfiguration : IEntityTypeConfiguration<ImpLodAaMapping>
{
    /// <summary>
    /// Configures the ImpLodAaMapping entity as a keyless staging table with approving authority
    /// mapping fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpLodAaMapping.</param>
    public void Configure(EntityTypeBuilder<ImpLodAaMapping> builder)
    {
        builder.ToTable("ImpLodAaMapping", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Approving authority mapping properties
        builder.Property(e => e.ImportName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("IMPORT_NAME");

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("NAME");

        builder.Property(e => e.Rank)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("RANK");

        builder.Property(e => e.Branch)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("BRANCH");
        
        // Indexes for common queries
        builder.HasIndex(e => e.ImportName, "IX_imp_lod_aa_mapping_import_name");
        
        builder.HasIndex(e => e.Name, "IX_imp_lod_aa_mapping_name");
        
        builder.HasIndex(e => e.Rank, "IX_imp_lod_aa_mapping_rank");
    }
}
