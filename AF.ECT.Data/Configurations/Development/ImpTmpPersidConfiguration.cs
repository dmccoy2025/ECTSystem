using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpTmpPersid"/> entity.
/// Configures a temporary staging table for storing personnel IDs during import processing.
/// </summary>
/// <remarks>
/// ImpTmpPersid is a temporary working table used during data import processes to store personnel
/// IDs for batch operations, data validation, or temporary tracking. This lightweight table serves
/// as a scratch space for storing personnel identifiers that need to be processed, validated, or
/// cross-referenced during complex import operations. This entity has no primary key (keyless entity)
/// as it represents temporary working data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for temporary storage
/// - Single nullable string property (PERS_ID)
/// - Used for batch operations and temporary tracking during imports
/// - Facilitates set-based operations on personnel IDs
/// - No foreign key relationships (temporary isolation)
/// - Typically cleared after each import operation or batch completes
/// - Supports deduplication, validation, and cross-referencing operations
/// </remarks>
public class ImpTmpPersidConfiguration : IEntityTypeConfiguration<ImpTmpPersid>
{
    /// <summary>
    /// Configures the ImpTmpPersid entity as a keyless temporary table with personnel ID field.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpTmpPersid.</param>
    public void Configure(EntityTypeBuilder<ImpTmpPersid> builder)
    {
        builder.ToTable("ImpTmpPersid", "dbo");

        // Keyless entity for temporary storage
        builder.HasNoKey();

        // Personnel ID property
        builder.Property(e => e.PersId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PERS_ID");
        
        // Index for common queries
        builder.HasIndex(e => e.PersId, "IX_imp_tmp_persid_pers_id");
    }
}
