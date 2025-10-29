using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpPersonnel"/> entity.
/// Configures a staging table for importing unit personnel data from legacy systems.
/// </summary>
/// <remarks>
/// ImpPersonnel is a temporary staging table used during data migration processes to load
/// unit-assigned personnel information from legacy systems. This entity captures core personnel
/// data including identification, contact information, residential address, and special status
/// indicators (deployment availability codes, flyer ID, special operations). This entity has
/// no primary key (keyless entity) as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate incomplete legacy data
/// - Personnel identification (PersId, SSN, username)
/// - Duty section and sex tracking
/// - Email addresses (home, work, unit)
/// - Contact information
/// - Residential address (street, city, state, postal code, country)
/// - Alternative status codes with expiration dates (DAV code, Flyer ID, PAS ATCH training)
/// - Special operations flag
/// - String-based audit fields (CreatedBy/ModifiedBy as strings, dates as strings)
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production personnel tables
/// </remarks>
public class ImpPersonnelConfiguration : IEntityTypeConfiguration<ImpPersonnel>
{
    /// <summary>
    /// Configures the ImpPersonnel entity as a keyless staging table with unit personnel import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpPersonnel.</param>
    public void Configure(EntityTypeBuilder<ImpPersonnel> builder)
    {
        builder.ToTable("ImpPersonnel", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Personnel identification properties
        builder.Property(e => e.PersId).HasMaxLength(50).IsUnicode(false).HasColumnName("PERS_ID");
        builder.Property(e => e.Ssn).HasMaxLength(11).IsUnicode(false).HasColumnName("SSN");
        builder.Property(e => e.Username).HasMaxLength(50).IsUnicode(false).HasColumnName("USERNAME");

        // Assignment properties
        builder.Property(e => e.DutySection).HasMaxLength(100).IsUnicode(false).HasColumnName("DUTY_SECTION");
        builder.Property(e => e.Sex).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("SEX");

        // Contact properties
        builder.Property(e => e.EMailHome).HasMaxLength(100).IsUnicode(false).HasColumnName("E_MAIL_HOME");
        builder.Property(e => e.EMailWork).HasMaxLength(100).IsUnicode(false).HasColumnName("E_MAIL_WORK");
        builder.Property(e => e.EMailUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("E_MAIL_UNIT");
        builder.Property(e => e.ContactInfo).HasMaxLength(200).IsUnicode(false).HasColumnName("CONTACT_INFO");

        // Residential address properties
        builder.Property(e => e.ResAddress1).HasMaxLength(100).IsUnicode(false).HasColumnName("RES_ADDRESS1");
        builder.Property(e => e.ResAddress2).HasMaxLength(100).IsUnicode(false).HasColumnName("RES_ADDRESS2");
        builder.Property(e => e.ResCity).HasMaxLength(50).IsUnicode(false).HasColumnName("RES_CITY");
        builder.Property(e => e.ResPostalCode).HasMaxLength(10).IsUnicode(false).HasColumnName("RES_POSTAL_CODE");
        builder.Property(e => e.ResCountry).HasMaxLength(50).IsUnicode(false).HasColumnName("RES_COUNTRY");
        builder.Property(e => e.ResState).HasMaxLength(2).IsUnicode(false).IsFixedLength().HasColumnName("RES_STATE");

        // Alternative status codes with expiration dates
        builder.Property(e => e.AltDavCode).HasMaxLength(10).IsUnicode(false).HasColumnName("ALT_DAV_CODE");
        builder.Property(e => e.AltDavCodeExpire).HasMaxLength(50).IsUnicode(false).HasColumnName("ALT_DAV_CODE_EXPIRE");
        builder.Property(e => e.AltFlyerId).HasMaxLength(20).IsUnicode(false).HasColumnName("ALT_FLYER_ID");
        builder.Property(e => e.AltFlyerIdExpire).HasMaxLength(50).IsUnicode(false).HasColumnName("ALT_FLYER_ID_EXPIRE");
        builder.Property(e => e.AltPasAtchTrng).HasMaxLength(10).IsUnicode(false).HasColumnName("ALT_PAS_ATCH_TRNG");
        builder.Property(e => e.AltPasAtchTrngExpire).HasMaxLength(50).IsUnicode(false).HasColumnName("ALT_PAS_ATCH_TRNG_EXPIRE");

        // Special operations flag
        builder.Property(e => e.SpclOpsYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("SPCL_OPS_YN");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_DATE");
        builder.Property(e => e.ModifiedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.PersId, "IX_imp_personnel_pers_id");
        
        builder.HasIndex(e => e.Ssn, "IX_imp_personnel_ssn");
        
        builder.HasIndex(e => e.Username, "IX_imp_personnel_username");
        
        builder.HasIndex(e => e.DutySection, "IX_imp_personnel_duty_section");
        
        builder.HasIndex(e => e.CreatedDate, "IX_imp_personnel_created_date");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_imp_personnel_modified_date");
    }
}
