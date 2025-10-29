using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity type configuration for the <see cref="TmpCommandStructChain"/> entity.
/// Configures the schema, table name, and properties for temporary command structure chain staging data (keyless table).
/// </summary>
public class TmpCommandStructChainConfiguration : IEntityTypeConfiguration<TmpCommandStructChain>
{
    /// <summary>
    /// Configures the entity of type <see cref="TmpCommandStructChain"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TmpCommandStructChain> builder)
    {
        builder.ToTable("tmp_CommandStructChain", "dbo");

        // Keyless Entity (Temporary staging table)
        builder.HasNoKey();

        // Properties
        builder.Property(e => e.CscId)
            .HasMaxLength(50)
            .HasColumnName("CSC_ID");

        builder.Property(e => e.CsId)
            .HasMaxLength(50)
            .HasColumnName("CS_ID");

        builder.Property(e => e.ChainType)
            .HasMaxLength(20)
            .HasColumnName("CHAIN_TYPE");

        builder.Property(e => e.CscIdParent)
            .HasMaxLength(50)
            .HasColumnName("CSC_ID_PARENT");

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50);

        builder.Property(e => e.CreatedDate)
            .HasMaxLength(50);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50);

        builder.Property(e => e.ModifiedDate)
            .HasMaxLength(50);
        
        // Indexes for common queries
        builder.HasIndex(e => e.CscId, "IX_tmp_command_struct_chain_csc_id");
        
        builder.HasIndex(e => e.CsId, "IX_tmp_command_struct_chain_cs_id");
        
        builder.HasIndex(e => e.CscIdParent, "IX_tmp_command_struct_chain_csc_id_parent");
        
        builder.HasIndex(e => e.ChainType, "IX_tmp_command_struct_chain_chain_type");
    }
}
