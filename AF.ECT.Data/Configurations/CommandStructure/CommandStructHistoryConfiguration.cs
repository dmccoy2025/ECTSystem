using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CommandStructure;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CommandStructHistory"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Command_Struct_History table,
/// which stores historical snapshots of command structure changes over time.
/// Contains complete organizational unit data including PAS code, addresses, command codes,
/// UIC, unit identifiers, parent relationships, change types, and package log references.
/// Used for auditing organizational changes, tracking unit activations/deactivations,
/// and maintaining historical command structure for reporting and compliance.
/// </remarks>
public class CommandStructHistoryConfiguration : IEntityTypeConfiguration<CommandStructHistory>
{
    /// <summary>
    /// Configures the entity of type <see cref="CommandStructHistory"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CommandStructHistory> builder)
    {
        // Table mapping
        builder.ToTable("Command_Struct_History", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Command_Struct_History");

        // Properties configuration
        builder.Property(e => e.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.PkgLogId)
            .HasColumnName("Pkg_Log_ID");

        builder.Property(e => e.Pascode)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("PASCODE");

        builder.Property(e => e.CsId)
            .IsRequired()
            .HasColumnName("CS_ID");

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasColumnName("Created_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasColumnName("Created_By");

        builder.Property(e => e.Address1)
            .HasMaxLength(100)
            .HasColumnName("Address_1");

        builder.Property(e => e.Address2)
            .HasMaxLength(100)
            .HasColumnName("Address_2");

        builder.Property(e => e.BaseCode)
            .HasMaxLength(50)
            .HasColumnName("Base_Code");

        builder.Property(e => e.City)
            .HasMaxLength(100)
            .HasColumnName("City");

        builder.Property(e => e.CommandCode)
            .HasMaxLength(50)
            .HasColumnName("Command_Code");

        builder.Property(e => e.CommandStructUtc)
            .HasMaxLength(50)
            .HasColumnName("Command_Struct_UTC");

        builder.Property(e => e.Component)
            .HasMaxLength(50)
            .HasColumnName("Component");

        builder.Property(e => e.Country)
            .HasMaxLength(50)
            .HasColumnName("Country");

        builder.Property(e => e.CsIdParent)
            .HasColumnName("CS_ID_Parent");

        builder.Property(e => e.CsLevel)
            .HasMaxLength(50)
            .HasColumnName("CS_Level");

        builder.Property(e => e.CsOperType)
            .HasMaxLength(50)
            .HasColumnName("CS_Oper_Type");

        builder.Property(e => e.GeoLoc)
            .HasMaxLength(50)
            .HasColumnName("GeoLoc");

        builder.Property(e => e.LongName)
            .HasMaxLength(200)
            .HasColumnName("Long_Name");

        builder.Property(e => e.PostalCode)
            .HasMaxLength(20)
            .HasColumnName("Postal_Code");

        builder.Property(e => e.State)
            .HasMaxLength(50)
            .HasColumnName("State");

        builder.Property(e => e.Uic)
            .HasMaxLength(50)
            .HasColumnName("UIC");

        builder.Property(e => e.UnitDet)
            .HasMaxLength(50)
            .HasColumnName("Unit_Det");

        builder.Property(e => e.UnitKind)
            .HasMaxLength(50)
            .HasColumnName("Unit_Kind");

        builder.Property(e => e.UnitNbr)
            .HasMaxLength(50)
            .HasColumnName("Unit_Nbr");

        builder.Property(e => e.UnitType)
            .HasMaxLength(50)
            .HasColumnName("Unit_Type");

        builder.Property(e => e.ChangeType)
            .HasMaxLength(50)
            .HasColumnName("ChangeType");

        builder.Property(e => e.ParentPasCode)
            .HasMaxLength(50)
            .HasColumnName("Parent_PAS_Code");

        // Indexes
        builder.HasIndex(e => e.CsId, "IX_command_struct_history_cs_id");

        builder.HasIndex(e => e.Pascode, "IX_command_struct_history_pascode");

        builder.HasIndex(e => e.CreatedDate, "IX_command_struct_history_created_date");

        builder.HasIndex(e => e.ChangeType, "IX_command_struct_history_change_type");

        builder.HasIndex(e => e.PkgLogId, "IX_command_struct_history_pkg_log_id");

        builder.HasIndex(e => e.Component, "IX_command_struct_history_component");

        builder.HasIndex(e => new { e.CreatedDate, e.ChangeType }, "IX_command_struct_history_date_change_type");

        builder.HasIndex(e => e.CsIdParent, "IX_command_struct_history_cs_id_parent");

        builder.HasIndex(e => e.CreatedBy, "IX_command_struct_history_created_by");
    }
}
