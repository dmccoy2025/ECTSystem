using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOracleNonunitPersonnelDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_NonUnit_Personnel_Data table,
/// which stores non-unit assigned personnel data from AFRC Oracle system migration.
/// Contains personnel information for individuals not assigned to a specific unit including
/// name components, contact information (email, addresses), duty section, and demographics.
/// All properties are nullable strings for Oracle import staging. Used for personnel who
/// may be in transit, TDY, or other non-standard assignments.
/// </remarks>
public class AfrcOracleNonunitPersonnelDatumConfiguration : IEntityTypeConfiguration<AfrcOracleNonunitPersonnelDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOracleNonunitPersonnelDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOracleNonunitPersonnelDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_NonUnit_Personnel_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration
        builder.Property(e => e.PersId).HasColumnName("pers_id");
        builder.Property(e => e.Username).HasColumnName("username");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.FirstName).HasColumnName("first_name");
        builder.Property(e => e.Mi).HasColumnName("mi");
        builder.Property(e => e.LastName).HasColumnName("last_name");
        builder.Property(e => e.NameSuffix).HasColumnName("name_suffix");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.CreatedDate).HasColumnName("created_date");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.ModifiedDate).HasColumnName("modified_date");
        builder.Property(e => e.CsId).HasColumnName("cs_id");
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

        // Indexes for common queries
        builder.HasIndex(e => e.PersId, "IX_afrc_oracle_nonunit_personnel_pers_id");

        builder.HasIndex(e => e.Username, "IX_afrc_oracle_nonunit_personnel_username");

        builder.HasIndex(e => e.LastName, "IX_afrc_oracle_nonunit_personnel_last_name");
        
        builder.HasIndex(e => e.CreatedDate, "IX_afrc_oracle_nonunit_personnel_created_date");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_afrc_oracle_nonunit_personnel_modified_date");
        
        builder.HasIndex(e => e.CsId, "IX_afrc_oracle_nonunit_personnel_cs_id");
    }
}
