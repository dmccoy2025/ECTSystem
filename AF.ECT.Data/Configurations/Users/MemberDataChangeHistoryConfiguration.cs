using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Entity Framework configuration for the <see cref="MemberDataChangeHistory"/> entity.
/// Configures an audit table for tracking all changes to member data over time.
/// </summary>
/// <remarks>
/// MemberDataChangeHistory is an audit table that captures every change made to member records,
/// preserving the complete state of the member data along with metadata about the change. This entity
/// maintains comprehensive member information including personal data, assignment details, contact
/// information, AFSCs, deployment availability status, and service dates, plus additional change
/// tracking fields (ChangeType, Date, AttachPas). It supports full audit trails, compliance
/// requirements, and historical data analysis. This entity has no primary key (keyless entity)
/// as it stores multiple change records for the same member over time.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for audit storage
/// - Required SSAN (non-nullable) for member identification
/// - Comprehensive member data (53 properties - all fields from MemberData plus change metadata)
/// - Personal information (name components, suffix, DOB, sex)
/// - Assignment tracking (PAS codes, duty position, AFSC, office symbol, record status)
/// - Contact information (phones, addresses)
/// - Grade and rank (current grade as nullable integer for historical flexibility)
/// - Duty status with effective/expiration dates (DateTime)
/// - Deployment availability status (Admin, Physical, Legal, Time with expiration dates)
/// - AFSC tracking (Duty, Primary, Secondary, Tertiary)
/// - Service dates (DOS, ETS, TAFCSD, TAFMSD, retirement/separation projection)
/// - Service component
/// - Change metadata (ChangeType required, Date required as DateTime, AttachPas nullable)
/// - No foreign key relationships (audit isolation)
/// - Long-term retention for compliance, audit, and historical analysis
/// </remarks>
public class MemberDataChangeHistoryConfiguration : IEntityTypeConfiguration<MemberDataChangeHistory>
{
    /// <summary>
    /// Configures the MemberDataChangeHistory entity as a keyless audit table with comprehensive
    /// member data fields plus change tracking metadata.
    /// </summary>
    /// <param name="builder">The entity type builder for MemberDataChangeHistory.</param>
    public void Configure(EntityTypeBuilder<MemberDataChangeHistory> builder)
    {
        builder.ToTable("MemberDataChangeHistory", "dbo");

        // Keyless entity for audit storage
        builder.HasNoKey();

        // Member identification (required)
        builder.Property(e => e.Ssan).IsRequired().HasMaxLength(9).IsUnicode(false).HasColumnName("SSAN");

        // Personal information
        builder.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(false).HasColumnName("FIRST_NAME");
        builder.Property(e => e.MiddleNames).HasMaxLength(50).IsUnicode(false).HasColumnName("MIDDLE_NAMES");
        builder.Property(e => e.LastName).HasMaxLength(50).IsUnicode(false).HasColumnName("LAST_NAME");
        builder.Property(e => e.Suffix).HasMaxLength(10).IsUnicode(false).HasColumnName("SUFFIX");
        builder.Property(e => e.SexSvcMbr).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("SEX_SVC_MBR");
        builder.Property(e => e.Dob).HasColumnType("datetime").HasColumnName("DOB");

        // PAS codes and assignment
        builder.Property(e => e.Pas).HasMaxLength(10).IsUnicode(false).HasColumnName("PAS");
        builder.Property(e => e.PasGaining).HasMaxLength(10).IsUnicode(false).HasColumnName("PAS_GAINING");
        builder.Property(e => e.AsgReptNltDate).HasColumnType("datetime").HasColumnName("ASG_REPT_NLT_DATE");
        builder.Property(e => e.RcdId).HasMaxLength(20).IsUnicode(false).HasColumnName("RCD_ID");
        builder.Property(e => e.RecStatCurr).HasMaxLength(10).IsUnicode(false).HasColumnName("REC_STAT_CURR");
        builder.Property(e => e.PasNumber).HasMaxLength(20).IsUnicode(false).HasColumnName("PAS_NUMBER");

        // Contact information
        builder.Property(e => e.CommDutyPhone).HasMaxLength(20).IsUnicode(false).HasColumnName("COMM_DUTY_PHONE");
        builder.Property(e => e.HomePhone).HasMaxLength(20).IsUnicode(false).HasColumnName("HOME_PHONE");
        builder.Property(e => e.OfficeSymbol).HasMaxLength(20).IsUnicode(false).HasColumnName("OFFICE_SYMBOL");

        // Local address
        builder.Property(e => e.LocalAddrStreet).HasMaxLength(200).IsUnicode(false).HasColumnName("LOCAL_ADDR_STREET");
        builder.Property(e => e.LocalAddrCity).HasMaxLength(50).IsUnicode(false).HasColumnName("LOCAL_ADDR_CITY");
        builder.Property(e => e.LocalAddrState).HasMaxLength(2).IsUnicode(false).IsFixedLength().HasColumnName("LOCAL_ADDR_STATE");

        // Mailing address
        builder.Property(e => e.AdrsMailDomCity).HasMaxLength(50).IsUnicode(false).HasColumnName("ADRS_MAIL_DOM_CITY");
        builder.Property(e => e.AdrsMailDomState).HasMaxLength(2).IsUnicode(false).IsFixedLength().HasColumnName("ADRS_MAIL_DOM_STATE");
        builder.Property(e => e.AdrsMailZip).HasMaxLength(10).IsUnicode(false).HasColumnName("ADRS_MAIL_ZIP");
        builder.Property(e => e.Zip).HasMaxLength(10).IsUnicode(false).HasColumnName("ZIP");

        // Grade and duty position (nullable for historical flexibility)
        builder.Property(e => e.GrCurr).HasColumnName("GR_CURR");
        builder.Property(e => e.DyPosnNr).HasMaxLength(20).IsUnicode(false).HasColumnName("DY_POSN_NR");

        // AFSCs
        builder.Property(e => e.Dafsc).HasMaxLength(10).IsUnicode(false).HasColumnName("DAFSC");
        builder.Property(e => e.Pafsc).HasMaxLength(10).IsUnicode(false).HasColumnName("PAFSC");
        builder.Property(e => e.Afsc2).HasMaxLength(10).IsUnicode(false).HasColumnName("AFSC2");
        builder.Property(e => e.Afsc3).HasMaxLength(10).IsUnicode(false).HasColumnName("AFSC3");

        // Duty status
        builder.Property(e => e.DutyStatus).HasMaxLength(20).IsUnicode(false).HasColumnName("DUTY_STATUS");
        builder.Property(e => e.DutyStatusEffDate).HasColumnType("datetime").HasColumnName("DUTY_STATUS_EFF_DATE");
        builder.Property(e => e.DutyStatusExpDate).HasColumnType("datetime").HasColumnName("DUTY_STATUS_EXP_DATE");

        // Deployment availability status
        builder.Property(e => e.DeplAvailStatusAdmin).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_ADMIN");
        builder.Property(e => e.DeplAvailStatusAExD).HasColumnType("datetime").HasColumnName("DEPL_AVAIL_STATUS_A_EX_D");
        builder.Property(e => e.DeplAvailStatusPhys).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_PHYS");
        builder.Property(e => e.DeplAvailStatusPExD).HasColumnType("datetime").HasColumnName("DEPL_AVAIL_STATUS_P_EX_D");
        builder.Property(e => e.DeplAvailStatusLegal).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_LEGAL");
        builder.Property(e => e.DeplAvailStatusLExD).HasColumnType("datetime").HasColumnName("DEPL_AVAIL_STATUS_L_EX_D");
        builder.Property(e => e.DeplAvailStatusTime).HasMaxLength(10).IsUnicode(false).HasColumnName("DEPL_AVAIL_STATUS_TIME");
        builder.Property(e => e.DeplAvailStatusTExD).HasColumnType("datetime").HasColumnName("DEPL_AVAIL_STATUS_T_EX_D");

        // Service dates
        builder.Property(e => e.RetSepEffDateProj).HasColumnType("datetime").HasColumnName("RET_SEP_EFF_DATE_PROJ");
        builder.Property(e => e.Dos).HasColumnType("datetime").HasColumnName("DOS");
        builder.Property(e => e.Ets).HasColumnType("datetime").HasColumnName("ETS");
        builder.Property(e => e.Tafcsd).HasColumnType("datetime").HasColumnName("TAFCSD");
        builder.Property(e => e.Tafmsd).HasColumnType("datetime").HasColumnName("TAFMSD");

        // Service component
        builder.Property(e => e.SvcComp).HasMaxLength(10).IsUnicode(false).HasColumnName("SVC_COMP");

        // Change tracking metadata (required fields)
        builder.Property(e => e.ChangeType)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ChangeType");

        builder.Property(e => e.Date)
            .HasColumnType("datetime")
            .HasColumnName("Date");

        builder.Property(e => e.AttachPas)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("AttachPas");
    }
}
