using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpManpower"/> entity.
/// Configures a staging table for importing manpower position to PAS code mappings from external systems.
/// </summary>
/// <remarks>
/// ImpManpower is a temporary staging table used during data import processes to map position
/// numbers to their corresponding PAS (Personnel Accounting Symbol) codes. This entity has
/// no primary key (keyless entity) as it represents transient import data used for manpower
/// position mapping and validation.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate raw import data
/// - Position number to PAS code mapping
/// - Used for manpower allocation and position tracking
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful import and validation
/// </remarks>
public class ImpManpowerConfiguration : IEntityTypeConfiguration<ImpManpower>
{
    /// <summary>
    /// Configures the ImpManpower entity as a keyless staging table with manpower position mapping fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpManpower.</param>
    public void Configure(EntityTypeBuilder<ImpManpower> builder)
    {
        builder.ToTable("ImpManpower", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Manpower mapping properties
        builder.Property(e => e.PosNbr)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("POS_NBR");

        builder.Property(e => e.PasCode)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("PAS_CODE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.PosNbr, "IX_imp_manpower_pos_nbr");
        
        builder.HasIndex(e => e.PasCode, "IX_imp_manpower_pas_code");
    }
}
