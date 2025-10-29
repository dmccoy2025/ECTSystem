using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOraclePersonnelDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_Personnel_Data table,
/// which stores standard personnel data from AFRC Oracle system migration.
/// Contains personnel identification (SSN, username), contact information (email, residential address),
/// duty section, special operations flags, and alternative codes (DAV, Flyer ID, PAS attachment training).
/// All properties are nullable strings for Oracle import staging. This is the primary personnel data
/// table for unit-assigned AFRC personnel.
/// </remarks>
public class AfrcOraclePersonnelDatumConfiguration : IEntityTypeConfiguration<AfrcOraclePersonnelDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOraclePersonnelDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOraclePersonnelDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_Personnel_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration
        builder.Property(e => e.PersId).HasColumnName("pers_id");
        builder.Property(e => e.Ssn).HasColumnName("ssn");
        builder.Property(e => e.Username).HasColumnName("username");
        builder.Property(e => e.DutySection).HasColumnName("duty_section");
        builder.Property(e => e.Sex).HasColumnName("sex");
        builder.Property(e => e.EMailHome).HasColumnName("e_mail_home");
        builder.Property(e => e.EMailWork).HasColumnName("e_mail_work");
        builder.Property(e => e.EMailUnit).HasColumnName("e_mail_unit");
        builder.Property(e => e.ContactInfo).HasColumnName("contact_info");
        builder.Property(e => e.ResAddress1).HasColumnName("res_address1");
        builder.Property(e => e.ResAddress2).HasColumnName("res_address2");
        builder.Property(e => e.ResCity).HasColumnName("res_city");
        builder.Property(e => e.ResPostalCode).HasColumnName("res_postal_code");
        builder.Property(e => e.ResCountry).HasColumnName("res_country");
        builder.Property(e => e.ResState).HasColumnName("res_state");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.CreatedDate).HasColumnName("created_date");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.ModifiedDate).HasColumnName("modified_date");
        builder.Property(e => e.AltDavCode).HasColumnName("alt_dav_code");
        builder.Property(e => e.AltDavCodeExpire).HasColumnName("alt_dav_code_expire");
        builder.Property(e => e.AltFlyerId).HasColumnName("alt_flyer_id");
        builder.Property(e => e.AltFlyerIdExpire).HasColumnName("alt_flyer_id_expire");
        builder.Property(e => e.AltPasAtchTrng).HasColumnName("alt_pas_atch_trng");
        builder.Property(e => e.AltPasAtchTrngExpire).HasColumnName("alt_pas_atch_trng_expire");
        builder.Property(e => e.SpclOpsYn).HasColumnName("spcl_ops_yn");

        // Indexes for common queries
        builder.HasIndex(e => e.PersId, "IX_afrc_oracle_personnel_pers_id");

        builder.HasIndex(e => e.Ssn, "IX_afrc_oracle_personnel_ssn");

        builder.HasIndex(e => e.Username, "IX_afrc_oracle_personnel_username");
        
        builder.HasIndex(e => e.CreatedDate, "IX_afrc_oracle_personnel_created_date");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_afrc_oracle_personnel_modified_date");
        
        builder.HasIndex(e => e.DutySection, "IX_afrc_oracle_personnel_duty_section");
    }
}
