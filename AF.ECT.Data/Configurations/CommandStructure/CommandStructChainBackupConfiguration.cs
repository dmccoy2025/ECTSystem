using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CommandStructure;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CommandStructChainBackup"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Command_Struct_Chain_Backup table,
/// which stores backup copies of command structure chain relationships for disaster recovery
/// or historical tracking purposes. Contains command structure IDs, parent relationships,
/// chain types, view types, and audit fields. Used to preserve organizational hierarchy
/// data before major changes or deletions.
/// </remarks>
public class CommandStructChainBackupConfiguration : IEntityTypeConfiguration<CommandStructChainBackup>
{
    /// <summary>
    /// Configures the entity of type <see cref="CommandStructChainBackup"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CommandStructChainBackup> builder)
    {
        // Table mapping
        builder.ToTable("Command_Struct_Chain_Backup", "dbo");

        // Primary key
        builder.HasKey(e => e.CscId)
            .HasName("PK_Command_Struct_Chain_Backup");

        // Properties configuration
        builder.Property(e => e.CscId)
            .HasColumnName("CSC_ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CsId)
            .HasColumnName("CS_ID");

        builder.Property(e => e.ChainType)
            .HasMaxLength(50)
            .HasColumnName("ChainType");

        builder.Property(e => e.CscIdParent)
            .HasColumnName("CSC_ID_Parent");

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(200)
            .HasColumnName("Created_By");

        builder.Property(e => e.CreatedDate)
            .HasColumnName("Created_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(200)
            .HasColumnName("Modified_By");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("Modified_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.ViewType)
            .HasColumnName("ViewType");

        // Indexes
        builder.HasIndex(e => e.CsId, "IX_command_struct_chain_backup_cs_id");

        builder.HasIndex(e => e.CscIdParent, "IX_command_struct_chain_backup_csc_id_parent");

        builder.HasIndex(e => e.ChainType, "IX_command_struct_chain_backup_chain_type");
    }
}
