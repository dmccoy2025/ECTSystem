using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity type configuration for the <see cref="TmpCommandStruct"/> entity.
/// Configures the schema, table name, and properties for temporary command structure staging data (keyless table).
/// </summary>
public class TmpCommandStructConfiguration : IEntityTypeConfiguration<TmpCommandStruct>
{
    /// <summary>
    /// Configures the entity of type <see cref="TmpCommandStruct"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TmpCommandStruct> builder)
    {
        builder.ToTable("tmp_CommandStruct", "dbo");

        // Keyless Entity (Temporary staging table)
        builder.HasNoKey();

        // Properties
        builder.Property(e => e.CsId)
            .HasColumnName("CS_ID");

        builder.Property(e => e.Uic)
            .HasMaxLength(10)
            .HasColumnName("UIC");

        builder.Property(e => e.UnitNbr)
            .HasMaxLength(20)
            .HasColumnName("UNIT_NBR");

        builder.Property(e => e.UnitKind)
            .HasMaxLength(20)
            .HasColumnName("UNIT_KIND");

        builder.Property(e => e.UnitType)
            .HasMaxLength(20)
            .HasColumnName("UNIT_TYPE");

        builder.Property(e => e.UnitDet)
            .HasMaxLength(20)
            .HasColumnName("UNIT_DET");

        builder.Property(e => e.LongName)
            .HasMaxLength(250);

        builder.Property(e => e.CsIdParent)
            .HasColumnName("CS_ID_PARENT");

        builder.Property(e => e.CsOperType)
            .HasMaxLength(20)
            .HasColumnName("CS_OPER_TYPE");

        builder.Property(e => e.CsLevel)
            .HasMaxLength(20)
            .HasColumnName("CS_LEVEL");

        builder.Property(e => e.PasCode)
            .HasMaxLength(20)
            .HasColumnName("PAS_CODE");

        builder.Property(e => e.Address1)
            .HasMaxLength(150);

        builder.Property(e => e.Address2)
            .HasMaxLength(150);

        builder.Property(e => e.City)
            .HasMaxLength(100);

        builder.Property(e => e.State)
            .HasMaxLength(50);

        builder.Property(e => e.PostalCode)
            .HasMaxLength(20);

        builder.Property(e => e.Country)
            .HasMaxLength(100);

        builder.Property(e => e.EMail)
            .HasMaxLength(100)
            .HasColumnName("E_MAIL");

        builder.Property(e => e.Component)
            .HasMaxLength(20);

        builder.Property(e => e.CommandCode)
            .HasMaxLength(20);

        builder.Property(e => e.GainingCommandCsId)
            .HasColumnName("GainingCommand_CS_ID");

        builder.Property(e => e.MrdssDocId)
            .HasMaxLength(50)
            .HasColumnName("MRDSS_DOC_ID");

        builder.Property(e => e.MrdssDocDate)
            .HasMaxLength(50)
            .HasColumnName("MRDSS_DOC_DATE");

        builder.Property(e => e.MrdssDocReview)
            .HasMaxLength(10)
            .HasColumnName("MRDSS_DOC_REVIEW");

        builder.Property(e => e.MrdssKind)
            .HasMaxLength(50)
            .HasColumnName("MRDSS_KIND");

        builder.Property(e => e.MedicalService)
            .HasMaxLength(20);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50);

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50);

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime");

        builder.Property(e => e.CommandStructUtc)
            .HasMaxLength(100)
            .HasColumnName("CommandStructUTC");

        builder.Property(e => e.GeoLoc)
            .HasMaxLength(100);

        builder.Property(e => e.BaseCode)
            .HasMaxLength(20);

        builder.Property(e => e.PhysExamYn)
            .HasMaxLength(1)
            .HasColumnName("PhysExamYN");

        builder.Property(e => e.SchedulingYn)
            .HasMaxLength(1)
            .HasColumnName("SchedulingYN");

        builder.Property(e => e.TimeZone)
            .HasMaxLength(50);
        
        // Indexes for common queries
        builder.HasIndex(e => e.CsId, "IX_tmp_command_struct_cs_id");
        
        builder.HasIndex(e => e.Uic, "IX_tmp_command_struct_uic");
        
        builder.HasIndex(e => e.PasCode, "IX_tmp_command_struct_pas_code");
        
        builder.HasIndex(e => e.CsIdParent, "IX_tmp_command_struct_cs_id_parent");
        
        builder.HasIndex(e => e.CsLevel, "IX_tmp_command_struct_cs_level");
    }
}
