using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpPersonnelFeed"/> entity.
/// Configures a staging table for importing comprehensive personnel feed data from external systems.
/// </summary>
/// <remarks>
/// ImpPersonnelFeed is a temporary staging table used during data import processes to load detailed
/// personnel information from external personnel feed systems (e.g., MilPDS, DEERS). This entity
/// captures extensive personnel data including identification, position, rank, AFSC qualifications
/// (Primary, Secondary, Tertiary, Duty, Control), clearances, service dates, contact information,
/// and training status. This entity has no primary key (keyless entity) as it represents transient
/// import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate incomplete feed data
/// - Personnel and position identification (PersId, PosNbr)
/// - Personal information (name components, suffix, DOB)
/// - Rank and clearance tracking
/// - Duty location
/// - Deployment availability and flyer ID codes
/// - Service dates (DOA, DOE, DOR, ETS)
/// - Comprehensive AFSC tracking with prefix/suffix (PAFSC, SAFSC, TAFSC, DAFSC, CAFSC)
/// - SEI (Special Experience Identifier)
/// - Contact information (home phone, mailing address)
/// - Training assignment (PAS ATCH, training AFSC)
/// - Civilian Article ID
/// - String-based audit fields for flexible import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production personnel tables
/// </remarks>
public class ImpPersonnelFeedConfiguration : IEntityTypeConfiguration<ImpPersonnelFeed>
{
    /// <summary>
    /// Configures the ImpPersonnelFeed entity as a keyless staging table with comprehensive
    /// personnel feed import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpPersonnelFeed.</param>
    public void Configure(EntityTypeBuilder<ImpPersonnelFeed> builder)
    {
        builder.ToTable("ImpPersonnelFeed", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Personnel and position identification
        builder.Property(e => e.PersId).HasMaxLength(50).IsUnicode(false).HasColumnName("PERS_ID");
        builder.Property(e => e.PosNbr).HasMaxLength(20).IsUnicode(false).HasColumnName("POS_NBR");

        // Personal information
        builder.Property(e => e.Name).HasMaxLength(100).IsUnicode(false).HasColumnName("NAME");
        builder.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(false).HasColumnName("FIRST_NAME");
        builder.Property(e => e.Mi).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("MI");
        builder.Property(e => e.LastName).HasMaxLength(50).IsUnicode(false).HasColumnName("LAST_NAME");
        builder.Property(e => e.NameSuffix).HasMaxLength(10).IsUnicode(false).HasColumnName("NAME_SUFFIX");
        builder.Property(e => e.Dob).HasMaxLength(10).IsUnicode(false).HasColumnName("DOB");

        // Rank, clearance, and duty
        builder.Property(e => e.Rank).HasMaxLength(20).IsUnicode(false).HasColumnName("RANK");
        builder.Property(e => e.ClearanceId).HasMaxLength(20).IsUnicode(false).HasColumnName("CLEARANCE_ID");
        builder.Property(e => e.DutyLoc).HasMaxLength(50).IsUnicode(false).HasColumnName("DUTY_LOC");

        // Deployment and flyer codes
        builder.Property(e => e.DavCode).HasMaxLength(10).IsUnicode(false).HasColumnName("DAV_CODE");
        builder.Property(e => e.FlyerId).HasMaxLength(20).IsUnicode(false).HasColumnName("FLYER_ID");

        // Service dates
        builder.Property(e => e.Doa).HasMaxLength(10).IsUnicode(false).HasColumnName("DOA");
        builder.Property(e => e.Doe).HasMaxLength(10).IsUnicode(false).HasColumnName("DOE");
        builder.Property(e => e.Dor).HasMaxLength(10).IsUnicode(false).HasColumnName("DOR");
        builder.Property(e => e.Ets).HasMaxLength(10).IsUnicode(false).HasColumnName("ETS");

        // Primary AFSC (PAFSC)
        builder.Property(e => e.PafscPfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("PAFSC_PFX");
        builder.Property(e => e.Pafsc).HasMaxLength(5).IsUnicode(false).HasColumnName("PAFSC");
        builder.Property(e => e.PafscSfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("PAFSC_SFX");

        // Special Experience Identifier (SEI)
        builder.Property(e => e.Sei).HasMaxLength(10).IsUnicode(false).HasColumnName("SEI");

        // Secondary AFSC (SAFSC)
        builder.Property(e => e.SafscPfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("SAFSC_PFX");
        builder.Property(e => e.Safsc).HasMaxLength(5).IsUnicode(false).HasColumnName("SAFSC");
        builder.Property(e => e.SafscSfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("SAFSC_SFX");

        // Tertiary AFSC (TAFSC)
        builder.Property(e => e.TafscPfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("TAFSC_PFX");
        builder.Property(e => e.Tafsc).HasMaxLength(5).IsUnicode(false).HasColumnName("TAFSC");
        builder.Property(e => e.TafscSfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("TAFSC_SFX");

        // Duty AFSC (DAFSC)
        builder.Property(e => e.DafscPfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("DAFSC_PFX");
        builder.Property(e => e.Dafsc).HasMaxLength(5).IsUnicode(false).HasColumnName("DAFSC");
        builder.Property(e => e.DafscSfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("DAFSC_SFX");

        // Control AFSC (CAFSC)
        builder.Property(e => e.CafscPfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("CAFSC_PFX");
        builder.Property(e => e.Cafsc).HasMaxLength(5).IsUnicode(false).HasColumnName("CAFSC");
        builder.Property(e => e.CafscSfx).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("CAFSC_SFX");

        // Contact information
        builder.Property(e => e.HomePhone).HasMaxLength(20).IsUnicode(false).HasColumnName("HOME_PHONE");
        builder.Property(e => e.MailAddress1).HasMaxLength(100).IsUnicode(false).HasColumnName("MAIL_ADDRESS1");
        builder.Property(e => e.MailAddress2).HasMaxLength(100).IsUnicode(false).HasColumnName("MAIL_ADDRESS2");
        builder.Property(e => e.MailCity).HasMaxLength(50).IsUnicode(false).HasColumnName("MAIL_CITY");
        builder.Property(e => e.MailState).HasMaxLength(2).IsUnicode(false).IsFixedLength().HasColumnName("MAIL_STATE");
        builder.Property(e => e.MailPostalCode).HasMaxLength(10).IsUnicode(false).HasColumnName("MAIL_POSTAL_CODE");

        // Training assignment
        builder.Property(e => e.PasAtchTrng).HasMaxLength(10).IsUnicode(false).HasColumnName("PAS_ATCH_TRNG");
        builder.Property(e => e.TrngAfsc).HasMaxLength(10).IsUnicode(false).HasColumnName("TRNG_AFSC");

        // Civilian Article ID
        builder.Property(e => e.CivArtId).HasMaxLength(20).IsUnicode(false).HasColumnName("CIV_ART_ID");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_DATE");
        builder.Property(e => e.ModifiedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.PersId, "IX_imp_personnel_feed_pers_id");
        
        builder.HasIndex(e => e.LastName, "IX_imp_personnel_feed_last_name");
        
        builder.HasIndex(e => e.Rank, "IX_imp_personnel_feed_rank");
        
        builder.HasIndex(e => e.Pafsc, "IX_imp_personnel_feed_pafsc");
        
        builder.HasIndex(e => e.DutyLoc, "IX_imp_personnel_feed_duty_loc");
        
        builder.HasIndex(e => e.CreatedDate, "IX_imp_personnel_feed_created_date");
    }
}
