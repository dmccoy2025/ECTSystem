using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpCommandStructChain"/> entity.
/// Configures a staging table for importing command structure chain hierarchy data from external systems.
/// </summary>
/// <remarks>
/// ImpCommandStructChain is a temporary staging table used during data import processes to load
/// command structure chain relationships from external sources. This represents the hierarchical
/// parent-child relationships between command structures before validation and migration to production
/// tables. This entity has no primary key (keyless entity) as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All string properties (nullable) to accommodate raw import data
/// - Chain hierarchy tracking (parent-child relationships)
/// - Chain type classification
/// - String-based audit fields (CreatedBy/ModifiedBy as strings, dates as strings)
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful import and validation
/// </remarks>
public class ImpCommandStructChainConfiguration : IEntityTypeConfiguration<ImpCommandStructChain>
{
    /// <summary>
    /// Configures the ImpCommandStructChain entity as a keyless staging table with command
    /// structure chain hierarchy import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpCommandStructChain.</param>
    public void Configure(EntityTypeBuilder<ImpCommandStructChain> builder)
    {
        builder.ToTable("ImpCommandStructChain", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Chain identifiers
        builder.Property(e => e.CscId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CSC_ID");

        builder.Property(e => e.CsId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CS_ID");

        builder.Property(e => e.ChainType)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CHAIN_TYPE");

        builder.Property(e => e.CscIdParent)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CSC_ID_PARENT");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_BY");

        builder.Property(e => e.CreatedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_DATE");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_BY");

        builder.Property(e => e.ModifiedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.CscId, "IX_imp_command_struct_chain_csc_id");
        
        builder.HasIndex(e => e.CsId, "IX_imp_command_struct_chain_cs_id");
        
        builder.HasIndex(e => e.ChainType, "IX_imp_command_struct_chain_chain_type");
        
        builder.HasIndex(e => e.CscIdParent, "IX_imp_command_struct_chain_csc_id_parent");
    }
}
