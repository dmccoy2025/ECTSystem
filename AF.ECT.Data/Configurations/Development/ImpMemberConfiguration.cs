using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpMember"/> entity.
/// Configures a staging table for importing comprehensive military member personnel data from external systems.
/// </summary>
/// <remarks>
/// ImpMember is a temporary staging table used during data import processes to load complete
/// military member records from external personnel systems (e.g., DEERS, MilPDS). This is one of
/// the most comprehensive staging tables with 54 properties covering personal information, duty
/// assignments, deployment status, AFSC qualifications, service dates, and aviation ratings.
/// This entity has no primary key (keyless entity) as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate incomplete import data
/// - Personal identification (SSAN, name, sex)
/// - Duty assignment (PAS codes, duty position, AFSC, office symbol)
/// - Contact information (addresses, phone numbers)
/// - Grade and rank (current grade, date of rank)
/// - Duty status tracking (current status, effective/expiration dates)
/// - Deployment availability status (Admin, Physical, Legal, Time with explanations)
/// - AFSC tracking (Duty, Primary, Secondary, Tertiary)
/// - Service dates (DOB, DOS, ETS, ETO, TAFCSD, TAFMSD)
/// - Aviation information (aeronautical rating, aviation service code)
/// - Service component and record status
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production member tables
/// </remarks>
public class ImpMemberConfiguration : IEntityTypeConfiguration<ImpMember>
{
    /// <summary>
    /// Configures the ImpMember entity as a keyless staging table with comprehensive military
    /// member personnel import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpMember.</param>
    public void Configure(EntityTypeBuilder<ImpMember> builder)
    {
        builder.ToTable("ImpMember", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Personal identification properties
        builder.Property(e => e.Ssan).HasMaxLength(9).IsUnicode(false).HasColumnName("SSAN");
        builder.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(false).HasColumnName("FIRST_NAME");
        builder.Property(e => e.MiddleNames).HasMaxLength(50).IsUnicode(false).HasColumnName("MIDDLE_NAMES");
        builder.Property(e => e.LastName).HasMaxLength(50).IsUnicode(false).HasColumnName("LAST_NAME");
        builder.Property(e => e.Suffix).HasMaxLength(10).IsUnicode(false).HasColumnName("SUFFIX");
        builder.Property(e => e.SexSvcMbr).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("SEX_SVC_MBR");
        builder.Property(e => e.Dob).HasMaxLength(10).IsUnicode(false).HasColumnName("DOB");

        // PAS code and assignment properties
        builder.Property(e => e.Pas).HasMaxLength(10).IsUnicode(false).HasColumnName("PAS");
        builder.Property(e => e.PasGaining).HasMaxLength(10).IsUnicode(false).HasColumnName("PAS_GAINING");
        builder.Property(e => e.AsgReptNltDate).HasMaxLength(10).IsUnicode(false).HasColumnName("ASG_REPT_NLT_DATE");
        builder.Property(e => e.RcdId).HasMaxLength(20).IsUnicode(false).HasColumnName("RCD_ID");
        builder.Property(e => e.RecStatCurr).HasMaxLength(10).IsUnicode(false).HasColumnName("REC_STAT_CURR");

        // Contact information properties
        builder.Property(e => e.CommDutyPhone).HasMaxLength(20).IsUnicode(false).HasColumnName("COMM_DUTY_PHONE");
        builder.Property(e => e.HomePhone).HasMaxLength(20).IsUnicode(false).HasColumnName("HOME_PHONE");
        builder.Property(e => e.OfficeSymbol).HasMaxLength(20).IsUnicode(false).HasColumnName("OFFICE_SYMBOL");

        // Local address properties
        builder.Property(e => e.LocalAddrStreet1st).HasMaxLength(100).IsUnicode(false).HasColumnName("LOCAL_ADDR_STREET_1ST");
        builder.Property(e => e.LocalAddrStreet2nd).HasMaxLength(100).IsUnicode(false).HasColumnName("LOCAL_ADDR_STREET_2ND");
        builder.Property(e => e.LocalAddrCity).HasMaxLength(50).IsUnicode(false).HasColumnName("LOCAL_ADDR_CITY");
        builder.Property(e => e.LocalAddrState).HasMaxLength(2).IsUnicode(false).IsFixedLength().HasColumnName("LOCAL_ADDR_STATE");
        builder.Property(e => e.LocalAddrZip).HasMaxLength(10).IsUnicode(false).HasColumnName("LOCAL_ADDR_ZIP");

        // Current/mailing address properties
        builder.Property(e => e.CurrStreetAddr1st).HasMaxLength(100).IsUnicode(false).HasColumnName("CURR_STREET_ADDR_1ST");
        builder.Property(e => e.CurrStreetAddr2nd).HasMaxLength(100).IsUnicode(false).HasColumnName("CURR_STREET_ADDR_2ND");
        builder.Property(e => e.AdrsMailDomCity).HasMaxLength(50).IsUnicode(false).HasColumnName("ADRS_MAIL_DOM_CITY");
        builder.Property(e => e.AdrsMailDomState).HasMaxLength(2).IsUnicode(false).IsFixedLength().HasColumnName("ADRS_MAIL_DOM_STATE");
        builder.Property(e => e.AdrsMailZip).HasMaxLength(10).IsUnicode(false).HasColumnName("ADRS_MAIL_ZIP");

        // Grade and rank properties
        builder.Property(e => e.GrCurr).HasMaxLength(10).IsUnicode(false).HasColumnName("GR_CURR");
        builder.Property(e => e.Dor).HasMaxLength(10).IsUnicode(false).HasColumnName("DOR");

        // Duty position and AFSC properties
        builder.Property(e => e.DyPosnNr).HasMaxLength(20).IsUnicode(false).HasColumnName("DY_POSN_NR");
        builder.Property(e => e.Dafsc).HasMaxLength(10).IsUnicode(false).HasColumnName("DAFSC");
        builder.Property(e => e.Pafsc).HasMaxLength(10).IsUnicode(false).HasColumnName("PAFSC");
        builder.Property(e => e.Afsc2).HasMaxLength(10).IsUnicode(false).HasColumnName("AFSC2");
        builder.Property(e => e.Afsc3).HasMaxLength(10).IsUnicode(false).HasColumnName("AFSC3");
        builder.Property(e => e.DyEffDate).HasMaxLength(10).IsUnicode(false).HasColumnName("DY_EFF_DATE");

        // Duty status properties
        builder.Property(e => e.DutyStatus).HasMaxLength(20).IsUnicode(false).HasColumnName("DUTY_STATUS");
        builder.Property(e => e.DutyStatusEffDate).HasMaxLength(10).IsUnicode(false).HasColumnName("DUTY_STATUS_EFF_DATE");
        builder.Property(e => e.DutyStatusExpDate).HasMaxLength(10).IsUnicode(false).HasColumnName("DUTY_STATUS_EXP_DATE");

        // Deployment availability status properties (Admin, Physical, Legal, Time)
        builder.Property(e => e.DeplAvailStatusAdmin).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_ADMIN");
        builder.Property(e => e.DeplAvailStatusAExD).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_A_EX_D");
        builder.Property(e => e.DeplAvailStatusPhys).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_PHYS");
        builder.Property(e => e.DeplAvailStatusPExD).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_P_EX_D");
        builder.Property(e => e.DeplAvailStatusLegal).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_LEGAL");
        builder.Property(e => e.DeplAvailStatusLExD).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_L_EX_D");
        builder.Property(e => e.DeplAvailStatusTime).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_TIME");
        builder.Property(e => e.DeplAvailStatusTExD).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_T_EX_D");

        // Service dates properties
        builder.Property(e => e.RetSepEffDateProj).HasMaxLength(10).IsUnicode(false).HasColumnName("RET_SEP_EFF_DATE_PROJ");
        builder.Property(e => e.Dos).HasMaxLength(10).IsUnicode(false).HasColumnName("DOS");
        builder.Property(e => e.Eto).HasMaxLength(10).IsUnicode(false).HasColumnName("ETO");
        builder.Property(e => e.Ets).HasMaxLength(10).IsUnicode(false).HasColumnName("ETS");
        builder.Property(e => e.Tafcsd).HasMaxLength(10).IsUnicode(false).HasColumnName("TAFCSD");
        builder.Property(e => e.Tafmsd).HasMaxLength(10).IsUnicode(false).HasColumnName("TAFMSD");

        // Service component
        builder.Property(e => e.SvcComp).HasMaxLength(10).IsUnicode(false).HasColumnName("SVC_COMP");

        // Aviation properties
        builder.Property(e => e.CurrAeroRating).HasMaxLength(10).IsUnicode(false).HasColumnName("CURR_AERO_RATING");
        builder.Property(e => e.AvnSvcCode).HasMaxLength(10).IsUnicode(false).HasColumnName("AVN_SVC_CODE");
        builder.Property(e => e.AvnSvcCodeDate).HasMaxLength(10).IsUnicode(false).HasColumnName("AVN_SVC_CODE_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.Ssan, "IX_imp_member_ssan");
        
        builder.HasIndex(e => e.LastName, "IX_imp_member_last_name");
        
        builder.HasIndex(e => e.Pas, "IX_imp_member_pas");
        
        builder.HasIndex(e => e.GrCurr, "IX_imp_member_gr_curr");
        
        builder.HasIndex(e => e.DutyStatus, "IX_imp_member_duty_status");
    }
}
