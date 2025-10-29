using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpPersonnelOld"/> entity.
/// Configures an archival staging table for historical unit personnel data from legacy import processes.
/// </summary>
/// <remarks>
/// ImpPersonnelOld is an archival staging table that preserves historical unit personnel data from
/// previous data migration cycles. This entity maintains the same structure as ImpPersonnel but
/// represents older import runs that may be needed for data reconciliation, rollback, comparison,
/// or audit trail purposes. This entity has no primary key (keyless entity) as it represents
/// archival staging data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for archival staging
/// - All nullable string properties to accommodate historical data variations
/// - Same structure as ImpPersonnel but for historical imports
/// - Personnel identification (PersId, SSN, username)
/// - Duty section and sex tracking
/// - Email addresses (home, work, unit)
/// - Contact information
/// - Residential address (street, city, state, postal code, country)
/// - Alternative status codes with expiration dates (DAV code, Flyer ID, PAS ATCH training)
/// - Special operations flag
/// - String-based audit fields
/// - No foreign key relationships (archival isolation)
/// - Retained for data reconciliation, historical analysis, and rollback scenarios
/// </remarks>
public class ImpPersonnelOldConfiguration : IEntityTypeConfiguration<ImpPersonnelOld>
{
    /// <summary>
    /// Configures the ImpPersonnelOld entity as a keyless archival staging table with
    /// historical unit personnel fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpPersonnelOld.</param>
    public void Configure(EntityTypeBuilder<ImpPersonnelOld> builder)
    {
        builder.ToTable("ImpPersonnel_old", "dbo");

        // Keyless entity for archival staging
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

        // Audit properties (string-based for archival staging)
        builder.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_DATE");
        builder.Property(e => e.ModifiedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.PersId, "IX_imp_personnel_old_pers_id");
        
        builder.HasIndex(e => e.Ssn, "IX_imp_personnel_old_ssn");
        
        builder.HasIndex(e => e.Username, "IX_imp_personnel_old_username");
        
        builder.HasIndex(e => e.DutySection, "IX_imp_personnel_old_duty_section");
        
        builder.HasIndex(e => e.CreatedDate, "IX_imp_personnel_old_created_date");
    }
}
