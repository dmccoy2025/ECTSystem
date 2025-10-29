using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOracleManpowerAllDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_Manpower_All_Data table,
/// which stores comprehensive manpower and position data from AFRC Oracle system migration.
/// Contains PAS (Personnel Accounting Symbol) codes, AFSC (Air Force Specialty Code) information
/// for authorized/wartime/peacetime configurations, unit designations, position numbers,
/// command codes, operational locations, and duty titles. All properties are nullable strings
/// for Oracle import staging. This data supports personnel assignments and organizational structure.
/// </remarks>
public class AfrcOracleManpowerAllDatumConfiguration : IEntityTypeConfiguration<AfrcOracleManpowerAllDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOracleManpowerAllDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOracleManpowerAllDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_Manpower_All_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration - comprehensive manpower data
        builder.Property(e => e.MpId).HasColumnName("mp_id");
        builder.Property(e => e.MpFilePart).HasColumnName("mp_file_part");
        builder.Property(e => e.PasCode).HasColumnName("pas_code");
        builder.Property(e => e.AuthRecYn).HasColumnName("auth_rec_yn");
        builder.Property(e => e.WarRecYn).HasColumnName("war_rec_yn");
        builder.Property(e => e.PeaceRecYn).HasColumnName("peace_rec_yn");
        builder.Property(e => e.AuthAfscPfx).HasColumnName("auth_afsc_pfx");
        builder.Property(e => e.AuthAfsc).HasColumnName("auth_afsc");
        builder.Property(e => e.AuthAfscSfx).HasColumnName("auth_afsc_sfx");
        builder.Property(e => e.AuthGrade).HasColumnName("auth_grade");
        builder.Property(e => e.WarAfscPfx).HasColumnName("war_afsc_pfx");
        builder.Property(e => e.WarAfsc).HasColumnName("war_afsc");
        builder.Property(e => e.WarAfscSfx).HasColumnName("war_afsc_sfx");
        builder.Property(e => e.WarGrade).HasColumnName("war_grade");
        builder.Property(e => e.PeaceAfscPfx).HasColumnName("peace_afsc_pfx");
        builder.Property(e => e.PeaceAfsc).HasColumnName("peace_afsc");
        builder.Property(e => e.PeaceAfscSfx).HasColumnName("peace_afsc_sfx");
        builder.Property(e => e.PeaceGrade).HasColumnName("peace_grade");
        builder.Property(e => e.ArtYn).HasColumnName("art_yn");
        builder.Property(e => e.PosNbr).HasColumnName("pos_nbr");
        builder.Property(e => e.GainingCommandCode).HasColumnName("gaining_command_code");
        builder.Property(e => e.ReqType).HasColumnName("req_type");
        builder.Property(e => e.FundYn).HasColumnName("fund_yn");
        builder.Property(e => e.InstLoc).HasColumnName("inst_loc");
        builder.Property(e => e.AuthUnitNbr).HasColumnName("auth_unit_nbr");
        builder.Property(e => e.AuthUnitKind).HasColumnName("auth_unit_kind");
        builder.Property(e => e.AuthUnitType).HasColumnName("auth_unit_type");
        builder.Property(e => e.WarUnitNbr).HasColumnName("war_unit_nbr");
        builder.Property(e => e.WarUnitKind).HasColumnName("war_unit_kind");
        builder.Property(e => e.WarUnitType).HasColumnName("war_unit_type");
        builder.Property(e => e.PeaceUnitNbr).HasColumnName("peace_unit_nbr");
        builder.Property(e => e.PeaceUnitKind).HasColumnName("peace_unit_kind");
        builder.Property(e => e.PeaceUnitType).HasColumnName("peace_unit_type");
        builder.Property(e => e.OperLoc).HasColumnName("oper_loc");
        builder.Property(e => e.Det).HasColumnName("det");
        builder.Property(e => e.Pec).HasColumnName("pec");
        builder.Property(e => e.Utc).HasColumnName("utc");
        builder.Property(e => e.GradeType).HasColumnName("grade_type");
        builder.Property(e => e.CommandCode).HasColumnName("command_code");
        builder.Property(e => e.PosNbrXref).HasColumnName("pos_nbr_xref");
        builder.Property(e => e.Rpi).HasColumnName("rpi");
        builder.Property(e => e.Osc).HasColumnName("osc");
        builder.Property(e => e.Fac).HasColumnName("fac");
        builder.Property(e => e.Occ).HasColumnName("occ");
        builder.Property(e => e.DutyCode).HasColumnName("duty_code");
        builder.Property(e => e.AuthAmt).HasColumnName("auth_amt");
        builder.Property(e => e.OscDesc).HasColumnName("osc_desc");
        builder.Property(e => e.FacTitle).HasColumnName("fac_title");
        builder.Property(e => e.Ilk).HasColumnName("ilk");
        builder.Property(e => e.CmdRmk).HasColumnName("cmd_rmk");
        builder.Property(e => e.DutyTitle).HasColumnName("duty_title");
        builder.Property(e => e.PosNbrActual).HasColumnName("pos_nbr_actual");

        // Indexes for common queries
        builder.HasIndex(e => e.PasCode, "IX_afrc_oracle_manpower_pas_code");

        builder.HasIndex(e => e.PosNbr, "IX_afrc_oracle_manpower_pos_nbr");

        builder.HasIndex(e => e.CommandCode, "IX_afrc_oracle_manpower_command_code");
        
        builder.HasIndex(e => e.AuthAfsc, "IX_afrc_oracle_manpower_auth_afsc");
        
        builder.HasIndex(e => e.AuthGrade, "IX_afrc_oracle_manpower_auth_grade");
        
        builder.HasIndex(e => e.DutyTitle, "IX_afrc_oracle_manpower_duty_title");
    }
}
