using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpCommandStruct"/> entity.
/// Configures a staging table for importing command structure data from external systems.
/// </summary>
/// <remarks>
/// ImpCommandStruct is a temporary staging table used during data import processes to load
/// command structure information from external sources before validation and migration to the
/// production CommandStruct table. This entity has no primary key (keyless entity) as it
/// represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable properties to accommodate incomplete import data
/// - Comprehensive command structure fields (UIC, PAS code, hierarchy, contact info)
/// - MRDSS (Medical Readiness Decision Support System) integration fields
/// - Geographic location and timezone tracking
/// - Medical service and scheduling flags
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful import and validation
/// </remarks>
public class ImpCommandStructConfiguration : IEntityTypeConfiguration<ImpCommandStruct>
{
    /// <summary>
    /// Configures the ImpCommandStruct entity as a keyless staging table with comprehensive
    /// command structure import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpCommandStruct.</param>
    public void Configure(EntityTypeBuilder<ImpCommandStruct> builder)
    {
        builder.ToTable("ImpCommandStruct", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Command structure identifiers
        builder.Property(e => e.CsId)
            .HasColumnName("CS_ID");

        builder.Property(e => e.Uic)
            .HasMaxLength(6)
            .IsUnicode(false)
            .HasColumnName("UIC");

        builder.Property(e => e.UnitNbr)
            .HasMaxLength(4)
            .IsUnicode(false)
            .HasColumnName("UNIT_NBR");

        builder.Property(e => e.UnitKind)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("UNIT_KIND");

        builder.Property(e => e.UnitType)
            .HasMaxLength(2)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("UNIT_TYPE");

        builder.Property(e => e.UnitDet)
            .HasMaxLength(2)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("UNIT_DET");

        builder.Property(e => e.LongName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("LONG_NAME");

        builder.Property(e => e.CsIdParent)
            .HasColumnName("CS_ID_PARENT");

        builder.Property(e => e.CsOperType)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("CS_OPER_TYPE");

        builder.Property(e => e.CsLevel)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("CS_LEVEL");

        builder.Property(e => e.PasCode)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("PAS_CODE");

        // Address properties
        builder.Property(e => e.Address1)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("ADDRESS1");

        builder.Property(e => e.Address2)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("ADDRESS2");

        builder.Property(e => e.City)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CITY");

        builder.Property(e => e.State)
            .HasMaxLength(2)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("STATE");

        builder.Property(e => e.PostalCode)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("POSTAL_CODE");

        builder.Property(e => e.Country)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("COUNTRY");

        // Contact properties
        builder.Property(e => e.EMail)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("E_MAIL");

        // Component and command properties
        builder.Property(e => e.Component)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("COMPONENT");

        builder.Property(e => e.CommandCode)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("COMMAND_CODE");

        builder.Property(e => e.GainingCommandCsId)
            .HasColumnName("GAINING_COMMAND_CS_ID");

        // MRDSS (Medical Readiness Decision Support System) properties
        builder.Property(e => e.MrdssDocId)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("MRDSS_DOC_ID");

        builder.Property(e => e.MrdssDocDate)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("MRDSS_DOC_DATE");

        builder.Property(e => e.MrdssDocReview)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("MRDSS_DOC_REVIEW");

        builder.Property(e => e.MrdssKind)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("MRDSS_KIND");

        // Medical and operational properties
        builder.Property(e => e.MedicalService)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("MEDICAL_SERVICE");

        builder.Property(e => e.PhysExamYn)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("PHYS_EXAM_YN");

        builder.Property(e => e.SchedulingYn)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("SCHEDULING_YN");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_BY");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasColumnName("CREATED_DATE");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_BY");

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime")
            .HasColumnName("MODIFIED_DATE");

        // Geographic and timezone properties
        builder.Property(e => e.CommandStructUtc)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("COMMAND_STRUCT_UTC");

        builder.Property(e => e.GeoLoc)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("GEO_LOC");

        builder.Property(e => e.BaseCode)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("BASE_CODE");

        builder.Property(e => e.TimeZone)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("TIME_ZONE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.CsId, "IX_imp_command_struct_cs_id");
        
        builder.HasIndex(e => e.Uic, "IX_imp_command_struct_uic");
        
        builder.HasIndex(e => e.PasCode, "IX_imp_command_struct_pas_code");
        
        builder.HasIndex(e => e.CsIdParent, "IX_imp_command_struct_cs_id_parent");
        
        builder.HasIndex(e => e.CreatedDate, "IX_imp_command_struct_created_date");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_imp_command_struct_modified_date");
    }
}
