using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AlodPasRaw"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the ALOD_PAS_Raw table,
/// which stores raw PAS (Personnel Accounting Symbol) code data from external systems.
/// Contains 75+ properties with cryptic field codes (Aav3, Aku121, Anu45, etc.) representing
/// various PAS data elements including organization IDs, unit identifiers, dates, locations,
/// UIC, medical facility numbers, AFSC codes, and hierarchical relationships.
/// All properties are nullable strings for raw data import staging. Used for importing
/// and reconciling PAS data from Personnel Data System (PDS) or other source systems
/// before processing into normalized command structure tables.
/// </remarks>
public class AlodPasRawConfiguration : IEntityTypeConfiguration<AlodPasRaw>
{
    /// <summary>
    /// Configures the entity of type <see cref="AlodPasRaw"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AlodPasRaw> builder)
    {
        // Table mapping
        builder.ToTable("ALOD_PAS_Raw", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration - raw PAS data fields
        builder.Property(e => e.Aav3).HasColumnName("AAV3");
        builder.Property(e => e.Aku121).HasColumnName("AKU121");
        builder.Property(e => e.Anu45).HasColumnName("ANU45");
        builder.Property(e => e.Anw).HasColumnName("ANW");
        builder.Property(e => e.Afp5).HasColumnName("AFP5");
        builder.Property(e => e.Ans).HasColumnName("ANS");
        builder.Property(e => e.Ant20).HasColumnName("ANT20");
        builder.Property(e => e.Beb6).HasColumnName("BEB6");
        builder.Property(e => e.Crx).HasColumnName("CRX");
        builder.Property(e => e.OrganizationId).HasColumnName("OrganizationId");
        builder.Property(e => e.Afp1).HasColumnName("AFP1");
        builder.Property(e => e.Amx5).HasColumnName("AMX5");
        builder.Property(e => e.Ana).HasColumnName("ANA");
        builder.Property(e => e.Amy).HasColumnName("AMY");
        builder.Property(e => e.Anc).HasColumnName("ANC");
        builder.Property(e => e.Arb).HasColumnName("ARB");
        builder.Property(e => e.Ant).HasColumnName("ANT");
        builder.Property(e => e.Ant2).HasColumnName("ANT2");
        builder.Property(e => e.Anu30).HasColumnName("ANU30");
        builder.Property(e => e.Cap).HasColumnName("CAP");
        builder.Property(e => e.Acr1).HasColumnName("ACR1");
        builder.Property(e => e.Cap2).HasColumnName("CAP2");
        builder.Property(e => e.Alb).HasColumnName("ALB");
        builder.Property(e => e.Anu).HasColumnName("ANU");
        builder.Property(e => e.Atj).HasColumnName("ATJ");
        builder.Property(e => e.Aep).HasColumnName("AEP");
        builder.Property(e => e.Xwx115).HasColumnName("XWX115");
        builder.Property(e => e.Anf).HasColumnName("ANF");
        builder.Property(e => e.Uic).HasColumnName("UIC");
        builder.Property(e => e.Crz).HasColumnName("CRZ");
        builder.Property(e => e.ProjectedChangeEffectiveDate).HasColumnName("ProjectedChangeEffectiveDate");
        builder.Property(e => e.Aen53).HasColumnName("AEN53");
        builder.Property(e => e.Aen62).HasColumnName("AEN62");
        builder.Property(e => e.Aen43).HasColumnName("AEN43");
        builder.Property(e => e.Aen29).HasColumnName("AEN29");
        builder.Property(e => e.Aen15).HasColumnName("AEN15");
        builder.Property(e => e.Aku60).HasColumnName("AKU60");
        builder.Property(e => e.Aku19).HasColumnName("AKU19");
        builder.Property(e => e.Aku51).HasColumnName("AKU51");
        builder.Property(e => e.MedFacNr).HasColumnName("MedFacNr");
        builder.Property(e => e.Aaa195).HasColumnName("AAA195");
        builder.Property(e => e.Aaa222).HasColumnName("AAA222");
        builder.Property(e => e.Aaa199).HasColumnName("AAA199");
        builder.Property(e => e.Ofprml).HasColumnName("OFPRML");
        builder.Property(e => e.LocationId).HasColumnName("LocationId");
        builder.Property(e => e.DateFrom).HasColumnName("DateFrom");
        builder.Property(e => e.DateTo).HasColumnName("DateTo");
        builder.Property(e => e.Efa).HasColumnName("EFA");
        builder.Property(e => e.Ady7).HasColumnName("ADY7");
        builder.Property(e => e.EndDateActive).HasColumnName("EndDateActive");
        builder.Property(e => e.LookupCode).HasColumnName("LookupCode");
        builder.Property(e => e.RiTimestamp).HasColumnName("RI_TIMESTAMP");
        builder.Property(e => e.Asq7).HasColumnName("ASQ7");
        builder.Property(e => e.Aba71).HasColumnName("ABA71");
        builder.Property(e => e.Aba72).HasColumnName("ABA72");
        builder.Property(e => e.Unit).HasColumnName("Unit");
        builder.Property(e => e.Amf504).HasColumnName("AMF504");
        builder.Property(e => e.MpfPasAdminMeaning).HasColumnName("MPF_PAS_ADMIN_MEANING");
        builder.Property(e => e.Aev24).HasColumnName("AEV24");
        builder.Property(e => e.Aew).HasColumnName("AEW");
        builder.Property(e => e.Ajg).HasColumnName("AJG");
        builder.Property(e => e.Ajh8).HasColumnName("AJH8");
        builder.Property(e => e.Ajj).HasColumnName("AJJ");
        builder.Property(e => e.Amf503).HasColumnName("AMF503");
        builder.Property(e => e.Aeq1).HasColumnName("AEQ1");
        builder.Property(e => e.Ael).HasColumnName("AEL");
        builder.Property(e => e.Aen1).HasColumnName("AEN1");
        builder.Property(e => e.Amf303).HasColumnName("AMF303");
        builder.Property(e => e.Abc2).HasColumnName("ABC2");
        builder.Property(e => e.Afc175).HasColumnName("AFC175");
        builder.Property(e => e.ParentCsId).HasColumnName("ParentCsId");
        builder.Property(e => e.UnitDerived).HasColumnName("UnitDerived");

        // Indexes for common queries
        builder.HasIndex(e => e.OrganizationId, "IX_alod_pas_raw_organization_id");

        builder.HasIndex(e => e.Uic, "IX_alod_pas_raw_uic");

        builder.HasIndex(e => e.ParentCsId, "IX_alod_pas_raw_parent_cs_id");
        
        builder.HasIndex(e => e.LocationId, "IX_alod_pas_raw_location_id");
        
        builder.HasIndex(e => e.DateFrom, "IX_alod_pas_raw_date_from");
        
        builder.HasIndex(e => e.DateTo, "IX_alod_pas_raw_date_to");
        
        builder.HasIndex(e => e.Unit, "IX_alod_pas_raw_unit");
    }
}
