using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the Personnel entity.
/// Configures legacy personnel data table with contact information, addresses,
/// and alternative deployment codes for data migration and integration purposes.
/// </summary>
public class PersonnelConfiguration : IEntityTypeConfiguration<Personnel>
{
    /// <summary>
    /// Configures the Personnel entity as a keyless table for legacy data access
    /// with comprehensive personnel and contact information properties.
    /// </summary>
    /// <param name="builder">The entity type builder for Personnel.</param>
    public void Configure(EntityTypeBuilder<Personnel> builder)
    {
        builder.HasNoKey();

        builder.ToTable("PERSONNEL", "dbo");

        builder.Property(e => e.AltDavCode)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ALT_DAV_CODE");
        builder.Property(e => e.AltDavCodeExpire)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ALT_DAV_CODE_EXPIRE");
        builder.Property(e => e.AltFlyerId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ALT_FLYER_ID");
        builder.Property(e => e.AltFlyerIdExpire)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ALT_FLYER_ID_EXPIRE");
        builder.Property(e => e.AltPasAtchTrng)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ALT_PAS_ATCH_TRNG");
        builder.Property(e => e.AltPasAtchTrngExpire)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ALT_PAS_ATCH_TRNG_EXPIRE");
        builder.Property(e => e.ContactInfo)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("CONTACT_INFO");
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_DATE");
        builder.Property(e => e.DutySection)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("DUTY_SECTION");
        builder.Property(e => e.EMailHome)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("E_MAIL_HOME");
        builder.Property(e => e.EMailUnit)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("E_MAIL_UNIT");
        builder.Property(e => e.EMailWork)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("E_MAIL_WORK");
        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_DATE");
        builder.Property(e => e.PersId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PERS_ID");
        builder.Property(e => e.ResAddress1)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("RES_ADDRESS_1");
        builder.Property(e => e.ResAddress2)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("RES_ADDRESS_2");
        builder.Property(e => e.ResCity)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RES_CITY");
        builder.Property(e => e.ResCountry)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RES_COUNTRY");
        builder.Property(e => e.ResPostalCode)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RES_POSTAL_CODE");
        builder.Property(e => e.ResState)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RES_STATE");
        builder.Property(e => e.Sex)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("SEX");
        builder.Property(e => e.SpclOpsYn)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("SPCL_OPS_YN");
        builder.Property(e => e.Ssn)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("SSN");
        builder.Property(e => e.Username)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("USERNAME");
        
        // Indexes for common queries
        builder.HasIndex(e => e.PersId, "IX_personnel_pers_id");
        
        builder.HasIndex(e => e.Ssn, "IX_personnel_ssn");
        
        builder.HasIndex(e => e.Username, "IX_personnel_username");
        
        builder.HasIndex(e => e.DutySection, "IX_personnel_duty_section");
        
        builder.HasIndex(e => e.CreatedDate, "IX_personnel_created_date");
    }
}
