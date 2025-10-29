using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity type configuration for the <see cref="TestCommandStruct"/> entity.
/// Configures the schema, table name, primary key, properties, relationships, and indexes for test command structure data.
/// </summary>
public class TestCommandStructConfiguration : IEntityTypeConfiguration<TestCommandStruct>
{
    /// <summary>
    /// Configures the entity of type <see cref="TestCommandStruct"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TestCommandStruct> builder)
    {
        builder.ToTable("Test_CommandStruct", "dbo");

        // Primary Key
        builder.HasKey(e => e.CsId)
            .HasName("PK_Test_CommandStruct");

        // Properties
        builder.Property(e => e.CsId)
            .HasColumnName("CS_ID");

        builder.Property(e => e.Address1)
            .HasMaxLength(150);

        builder.Property(e => e.Address2)
            .HasMaxLength(150);

        builder.Property(e => e.BaseCode)
            .HasMaxLength(20);

        builder.Property(e => e.City)
            .HasMaxLength(100);

        builder.Property(e => e.CommandCode)
            .HasMaxLength(20);

        builder.Property(e => e.CommandStructUtc)
            .HasMaxLength(100)
            .HasColumnName("CommandStructUTC");

        builder.Property(e => e.Component)
            .HasMaxLength(20);

        builder.Property(e => e.Country)
            .HasMaxLength(100);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50);

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime");

        builder.Property(e => e.CsIdParent)
            .HasColumnName("CS_ID_PARENT");

        builder.Property(e => e.CsLevel)
            .HasMaxLength(20)
            .HasColumnName("CS_LEVEL");

        builder.Property(e => e.CsOperType)
            .HasMaxLength(20)
            .HasColumnName("CS_OPER_TYPE");

        builder.Property(e => e.EMail)
            .HasMaxLength(100)
            .HasColumnName("E_MAIL");

        builder.Property(e => e.GainingCommandCsId)
            .HasColumnName("GainingCommand_CS_ID");

        builder.Property(e => e.GeoLoc)
            .HasMaxLength(100);

        builder.Property(e => e.LongName)
            .HasMaxLength(250);

        builder.Property(e => e.MedicalService)
            .HasMaxLength(20);

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50);

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime");

        builder.Property(e => e.MrdssDocDate)
            .HasColumnType("datetime")
            .HasColumnName("MRDSS_DOC_DATE");

        builder.Property(e => e.MrdssDocId)
            .HasMaxLength(50)
            .HasColumnName("MRDSS_DOC_ID");

        builder.Property(e => e.MrdssDocReview)
            .HasMaxLength(10)
            .HasColumnName("MRDSS_DOC_REVIEW");

        builder.Property(e => e.MrdssKind)
            .HasMaxLength(50)
            .HasColumnName("MRDSS_KIND");

        builder.Property(e => e.PasCode)
            .HasMaxLength(20)
            .HasColumnName("PAS_CODE");

        builder.Property(e => e.PhysExamYn)
            .HasMaxLength(1)
            .HasColumnName("PhysExamYN");

        builder.Property(e => e.PostalCode)
            .HasMaxLength(20);

        builder.Property(e => e.SchedulingYn)
            .HasMaxLength(1)
            .HasColumnName("SchedulingYN");

        builder.Property(e => e.State)
            .HasMaxLength(50);

        builder.Property(e => e.TimeZone)
            .HasMaxLength(50);

        builder.Property(e => e.Uic)
            .HasMaxLength(10)
            .HasColumnName("UIC");

        builder.Property(e => e.UnitDet)
            .HasMaxLength(20)
            .HasColumnName("UNIT_DET");

        builder.Property(e => e.UnitKind)
            .HasMaxLength(20)
            .HasColumnName("UNIT_KIND");

        builder.Property(e => e.UnitNbr)
            .HasMaxLength(20)
            .HasColumnName("UNIT_NBR");

        builder.Property(e => e.UnitType)
            .HasMaxLength(20)
            .HasColumnName("UNIT_TYPE");

        // Relationships
        builder.HasOne(d => d.CsLevelNavigation)
            .WithMany(p => p.TestCommandStructs)
            .HasForeignKey(d => d.CsLevel)
            .HasConstraintName("FK_Test_CommandStruct_CoreLkupUnitLevelType");

        builder.HasOne(d => d.CsOperTypeNavigation)
            .WithMany(p => p.TestCommandStructs)
            .HasForeignKey(d => d.CsOperType)
            .HasConstraintName("FK_Test_CommandStruct_CoreLkupOperationType");

        builder.HasOne(d => d.TimeZoneNavigation)
            .WithMany(p => p.TestCommandStructs)
            .HasForeignKey(d => d.TimeZone)
            .HasConstraintName("FK_Test_CommandStruct_CoreLkupTimeZone");

        // Indexes
        builder.HasIndex(e => e.Uic, "IX_Test_CommandStruct_UIC");

        builder.HasIndex(e => e.CsIdParent, "IX_Test_CommandStruct_CS_ID_PARENT");

        builder.HasIndex(e => e.CsLevel, "IX_Test_CommandStruct_CS_LEVEL");

        builder.HasIndex(e => e.BaseCode, "IX_Test_CommandStruct_BaseCode");
    }
}
