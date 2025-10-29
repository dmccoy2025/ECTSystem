using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOraclePersonnelFeedDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_Personnel_Feed_Data table,
/// which stores comprehensive personnel feed data from AFRC Oracle system migration.
/// Contains detailed personnel information including name components, position numbers, rank,
/// clearance, multiple AFSC (Air Force Specialty Code) types (PAFSC, SAFSC, TAFSC, DAFSC, CAFSC),
/// dates (DOB, DOA, DOE, DOR, ETS), duty location, contact information, and training codes.
/// All properties are nullable strings for Oracle import staging. This represents the full
/// personnel data feed used for synchronization with master personnel systems.
/// </remarks>
public class AfrcOraclePersonnelFeedDatumConfiguration : IEntityTypeConfiguration<AfrcOraclePersonnelFeedDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOraclePersonnelFeedDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOraclePersonnelFeedDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_Personnel_Feed_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration - comprehensive personnel feed data
        builder.Property(e => e.PersId).HasColumnName("pers_id");
        builder.Property(e => e.PosNbr).HasColumnName("pos_nbr");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.FirstName).HasColumnName("first_name");
        builder.Property(e => e.Mi).HasColumnName("mi");
        builder.Property(e => e.LastName).HasColumnName("last_name");
        builder.Property(e => e.NameSuffix).HasColumnName("name_suffix");
        builder.Property(e => e.Rank).HasColumnName("rank");
        builder.Property(e => e.ClearanceId).HasColumnName("clearance_id");
        builder.Property(e => e.DutyLoc).HasColumnName("duty_loc");
        builder.Property(e => e.DavCode).HasColumnName("dav_code");
        builder.Property(e => e.FlyerId).HasColumnName("flyer_id");
        builder.Property(e => e.Dob).HasColumnName("dob");
        builder.Property(e => e.Doa).HasColumnName("doa");
        builder.Property(e => e.Doe).HasColumnName("doe");
        builder.Property(e => e.Dor).HasColumnName("dor");
        builder.Property(e => e.Ets).HasColumnName("ets");
        builder.Property(e => e.PafscPfx).HasColumnName("pafsc_pfx");
        builder.Property(e => e.Pafsc).HasColumnName("pafsc");
        builder.Property(e => e.PafscSfx).HasColumnName("pafsc_sfx");
        builder.Property(e => e.Sei).HasColumnName("sei");
        builder.Property(e => e.SafscPfx).HasColumnName("safsc_pfx");
        builder.Property(e => e.Safsc).HasColumnName("safsc");
        builder.Property(e => e.SafscSfx).HasColumnName("safsc_sfx");
        builder.Property(e => e.TafscPfx).HasColumnName("tafsc_pfx");
        builder.Property(e => e.Tafsc).HasColumnName("tafsc");
        builder.Property(e => e.TafscSfx).HasColumnName("tafsc_sfx");
        builder.Property(e => e.DafscPfx).HasColumnName("dafsc_pfx");
        builder.Property(e => e.Dafsc).HasColumnName("dafsc");
        builder.Property(e => e.DafscSfx).HasColumnName("dafsc_sfx");
        builder.Property(e => e.CafscPfx).HasColumnName("cafsc_pfx");
        builder.Property(e => e.Cafsc).HasColumnName("cafsc");
        builder.Property(e => e.CafscSfx).HasColumnName("cafsc_sfx");
        builder.Property(e => e.HomePhone).HasColumnName("home_phone");
        builder.Property(e => e.MailAddress1).HasColumnName("mail_address1");
        builder.Property(e => e.MailAddress2).HasColumnName("mail_address2");
        builder.Property(e => e.MailCity).HasColumnName("mail_city");
        builder.Property(e => e.MailState).HasColumnName("mail_state");
        builder.Property(e => e.MailPostalCode).HasColumnName("mail_postal_code");
        builder.Property(e => e.PasAtchTrng).HasColumnName("pas_atch_trng");
        builder.Property(e => e.TrngAfsc).HasColumnName("trng_afsc");
        builder.Property(e => e.CivArtId).HasColumnName("civ_art_id");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.CreatedDate).HasColumnName("created_date");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.ModifiedDate).HasColumnName("modified_date");

        // Indexes for common queries
        builder.HasIndex(e => e.PersId, "IX_afrc_oracle_personnel_feed_pers_id");

        builder.HasIndex(e => e.PosNbr, "IX_afrc_oracle_personnel_feed_pos_nbr");

        builder.HasIndex(e => e.LastName, "IX_afrc_oracle_personnel_feed_last_name");

        builder.HasIndex(e => e.Rank, "IX_afrc_oracle_personnel_feed_rank");
        
        builder.HasIndex(e => e.CreatedDate, "IX_afrc_oracle_personnel_feed_created_date");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_afrc_oracle_personnel_feed_modified_date");
        
        builder.HasIndex(e => e.DutyLoc, "IX_afrc_oracle_personnel_feed_duty_loc");
        
        builder.HasIndex(e => e.Pafsc, "IX_afrc_oracle_personnel_feed_pafsc");
    }
}
