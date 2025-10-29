using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpPersonnelFeedOld"/> entity.
/// Configures an archival staging table for historical personnel feed data from legacy import processes.
/// </summary>
/// <remarks>
/// ImpPersonnelFeedOld is an archival staging table that preserves historical personnel feed data
/// from previous data migration cycles. This entity maintains the same comprehensive personnel
/// structure as ImpPersonnelFeed but represents older import runs that may be needed for data
/// reconciliation, rollback, or historical comparison. This entity has no primary key (keyless entity)
/// as it represents archival staging data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for archival staging
/// - All nullable string properties to accommodate historical data variations
/// - Same structure as ImpPersonnelFeed but for historical imports
/// - Personnel and position identification (PersId, PosNbr)
/// - Personal information (name components, suffix, DOB)
/// - Rank, clearance, duty location
/// - Deployment availability and flyer codes
/// - Service dates (DOA, DOE, DOR, ETS)
/// - Comprehensive AFSC tracking (PAFSC, SAFSC, TAFSC, DAFSC, CAFSC with prefix/suffix)
/// - SEI (Special Experience Identifier)
/// - Contact information (home phone, mailing address)
/// - Training assignment tracking
/// - Civilian Article ID
/// - String-based audit fields
/// - Column46 for legacy compatibility (unknown column from old schema)
/// - No foreign key relationships (archival isolation)
/// - Retained for data reconciliation and historical analysis
/// </remarks>
public class ImpPersonnelFeedOldConfiguration : IEntityTypeConfiguration<ImpPersonnelFeedOld>
{
    /// <summary>
    /// Configures the ImpPersonnelFeedOld entity as a keyless archival staging table with
    /// historical personnel feed fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpPersonnelFeedOld.</param>
    public void Configure(EntityTypeBuilder<ImpPersonnelFeedOld> builder)
    {
        builder.ToTable("ImpPersonnelFeed_old", "dbo");

        // Keyless entity for archival staging
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

        // Audit properties (string-based for archival staging)
        builder.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_DATE");
        builder.Property(e => e.ModifiedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_DATE");

        // Legacy compatibility column (unknown purpose from old schema)
        builder.Property(e => e.Column46).HasMaxLength(50).IsUnicode(false).HasColumnName("Column46");
        
        // Indexes for common queries
        builder.HasIndex(e => e.PersId, "IX_imp_personnel_feed_old_pers_id");
        
        builder.HasIndex(e => e.LastName, "IX_imp_personnel_feed_old_last_name");
        
        builder.HasIndex(e => e.Rank, "IX_imp_personnel_feed_old_rank");
        
        builder.HasIndex(e => e.Pafsc, "IX_imp_personnel_feed_old_pafsc");
        
        builder.HasIndex(e => e.CreatedDate, "IX_imp_personnel_feed_old_created_date");
    }
}
