using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the MilpdsrawDatum entity.
/// Configures staging table for raw MILPDS (Military Personnel Data System) data
/// import with all fields stored as strings for initial data validation and transformation.
/// </summary>
public class MilpdsrawDatumConfiguration : IEntityTypeConfiguration<MilpdsrawDatum>
{
    /// <summary>
    /// Configures the MilpdsrawDatum entity with table mapping, primary key, and
    /// string-based properties for raw personnel data staging.
    /// </summary>
    /// <param name="builder">The entity type builder for MilpdsrawDatum.</param>
    public void Configure(EntityTypeBuilder<MilpdsrawDatum> builder)
    {
        builder.HasKey(e => e.Ssan).HasName("PK__MILPDSRA__CA1E8E3C5B21B1F5");

        builder.ToTable("MILPDSRawData", "dbo");

        builder.Property(e => e.Ssan)
            .HasMaxLength(11)
            .IsUnicode(false)
            .HasColumnName("SSAN");
        builder.Property(e => e.AdrsMailDomCity)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ADRS_MAIL_DOM_CITY");
        builder.Property(e => e.AdrsMailZip)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ADRS_MAIL_ZIP");
        builder.Property(e => e.Afsc2)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("AFSC_2");
        builder.Property(e => e.Afsc3)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("AFSC_3");
        builder.Property(e => e.AsgReptNltDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ASG_REPT_NLT_DATE");
        builder.Property(e => e.AttachPas)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ATTACH_PAS");
        builder.Property(e => e.AvnSvcCode)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("AVN_SVC_CODE");
        builder.Property(e => e.AvnSvcCodeDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("AVN_SVC_CODE_DATE");
        builder.Property(e => e.CommDutyPhone)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("COMM_DUTY_PHONE");
        builder.Property(e => e.CurrAeroRating)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CURR_AERO_RATING");
        builder.Property(e => e.CurrStreetAddr1st)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CURR_STREET_ADDR_1ST");
        builder.Property(e => e.CurrStreetAddr2nd)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CURR_STREET_ADDR_2ND");
        builder.Property(e => e.Dafsc)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DAFSC");
        builder.Property(e => e.DeplAvailStatusAExD)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_A_EX_D");
        builder.Property(e => e.DeplAvailStatusAdmin)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_ADMIN");
        builder.Property(e => e.DeplAvailStatusLExD)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_L_EX_D");
        builder.Property(e => e.DeplAvailStatusLegal)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_LEGAL");
        builder.Property(e => e.DeplAvailStatusPExD)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_P_EX_D");
        builder.Property(e => e.DeplAvailStatusPhys)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_PHYS");
        builder.Property(e => e.DeplAvailStatusTExD)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_T_EX_D");
        builder.Property(e => e.DeplAvailStatusTime)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_TIME");
        builder.Property(e => e.Dob)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DOB");
        builder.Property(e => e.Dor)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DOR");
        builder.Property(e => e.Dos)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DOS");
        builder.Property(e => e.DrsMailDomState)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DRS_MAIL_DOM_STATE");
        builder.Property(e => e.DutyStatus)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DUTY_STATUS");
        builder.Property(e => e.DutyStatusEffDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DUTY_STATUS_EFF_DATE");
        builder.Property(e => e.DutyStatusExpDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DUTY_STATUS_EXP_DATE");
        builder.Property(e => e.DyEffDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DY_EFF_DATE");
        builder.Property(e => e.DyPosnNr)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DY_POSN_NR");
        builder.Property(e => e.Eto)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ETO");
        builder.Property(e => e.Ets)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ETS");
        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("FIRST_NAME");
        builder.Property(e => e.GrCode).HasColumnName("GR_CODE");
        builder.Property(e => e.GrCurr)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("GR_CURR");
        builder.Property(e => e.HomePhone)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("HOME_PHONE");
        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LAST_NAME");
        builder.Property(e => e.LocalAddrCity)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOCAL_ADDR_CITY");
        builder.Property(e => e.LocalAddrState)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOCAL_ADDR_STATE");
        builder.Property(e => e.LocalAddrStreet1st)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOCAL_ADDR_STREET_1ST");
        builder.Property(e => e.LocalAddrStreet2nd)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOCAL_ADDR_STREET_2ND");
        builder.Property(e => e.LocalAddrZip)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOCAL_ADDR_ZIP");
        builder.Property(e => e.MiddleNames)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MIDDLE_NAMES");
        builder.Property(e => e.OfficeSymbol)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("OFFICE_SYMBOL");
        builder.Property(e => e.Pafsc)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PAFSC");
        builder.Property(e => e.Pas)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PAS");
        builder.Property(e => e.PasGaining)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PAS_GAINING");
        builder.Property(e => e.PasNumber)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PAS_NUMBER");
        builder.Property(e => e.RcdId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RCD_ID");
        builder.Property(e => e.RecStatCurr)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("REC_STAT_CURR");
        builder.Property(e => e.RetSepEffDateProj)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RET_SEP_EFF_DATE_PROJ");
        builder.Property(e => e.SexSvcMbr)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("SEX_SVC_MBR");
        builder.Property(e => e.Suffix)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("SUFFIX");
        builder.Property(e => e.SvcComp)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("SVC_COMP");
        builder.Property(e => e.Tafcsd)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("TAFCSD");
        builder.Property(e => e.Tafmsd)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("TAFMSD");
        
        // Indexes for common queries
        builder.HasIndex(e => e.Ssan, "IX_milpdsraw_datum_ssan");
        
        builder.HasIndex(e => e.LastName, "IX_milpdsraw_datum_last_name");
        
        builder.HasIndex(e => e.Pas, "IX_milpdsraw_datum_pas");
        
        builder.HasIndex(e => e.GrCurr, "IX_milpdsraw_datum_gr_curr");
        
        builder.HasIndex(e => e.DutyStatus, "IX_milpdsraw_datum_duty_status");
    }
}
