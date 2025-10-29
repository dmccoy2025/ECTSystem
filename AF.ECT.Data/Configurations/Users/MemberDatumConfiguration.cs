using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Entity Framework Core configuration for the MemberDatum entity.
/// Configures current member data with comprehensive personnel information including demographics,
/// assignments, duty status, deployment availability, AFSCs, and contact information.
/// </summary>
public class MemberDatumConfiguration : IEntityTypeConfiguration<MemberDatum>
{
    /// <summary>
    /// Configures the MemberDatum entity with table mapping, primary key, properties,
    /// relationships, and indexes for efficient member data queries.
    /// </summary>
    /// <param name="builder">The entity type builder for MemberDatum.</param>
    public void Configure(EntityTypeBuilder<MemberDatum> builder)
    {
        builder.ToTable("MEMBER_DATA", "dbo");

        builder.HasKey(e => e.Ssan)
            .HasName("PK_MEMBER_DATA");

        builder.Property(e => e.Ssan)
            .HasMaxLength(11)
            .IsUnicode(false)
            .HasColumnName("SSAN");
        builder.Property(e => e.AdrsMailDomCity)
            .HasMaxLength(30)
            .IsUnicode(false)
            .HasColumnName("ADRS_MAIL_DOM_CITY");
        builder.Property(e => e.AdrsMailDomState)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("ADRS_MAIL_DOM_STATE");
        builder.Property(e => e.AdrsMailZip)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("ADRS_MAIL_ZIP");
        builder.Property(e => e.Afsc2)
            .HasMaxLength(7)
            .IsUnicode(false)
            .HasColumnName("AFSC_2");
        builder.Property(e => e.Afsc3)
            .HasMaxLength(7)
            .IsUnicode(false)
            .HasColumnName("AFSC_3");
        builder.Property(e => e.AsgReptNltDate)
            .HasColumnType("date")
            .HasColumnName("ASG_REPT_NLT_DATE");
        builder.Property(e => e.AttachPas)
            .HasMaxLength(6)
            .IsUnicode(false)
            .HasColumnName("ATTACH_PAS");
        builder.Property(e => e.CommDutyPhone)
            .HasMaxLength(15)
            .IsUnicode(false)
            .HasColumnName("COMM_DUTY_PHONE");
        builder.Property(e => e.Dafsc)
            .HasMaxLength(7)
            .IsUnicode(false)
            .HasColumnName("DAFSC");
        builder.Property(e => e.Deleted).HasColumnName("Deleted");
        builder.Property(e => e.DeletedDate).HasColumnName("DeletedDate");
        builder.Property(e => e.DeplAvailStatusAExD)
            .HasColumnType("date")
            .HasColumnName("DEPL_AVAIL_STATUS_A_EX_D");
        builder.Property(e => e.DeplAvailStatusAdmin)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_ADMIN");
        builder.Property(e => e.DeplAvailStatusLExD)
            .HasColumnType("date")
            .HasColumnName("DEPL_AVAIL_STATUS_L_EX_D");
        builder.Property(e => e.DeplAvailStatusLegal)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_LEGAL");
        builder.Property(e => e.DeplAvailStatusPExD)
            .HasColumnType("date")
            .HasColumnName("DEPL_AVAIL_STATUS_P_EX_D");
        builder.Property(e => e.DeplAvailStatusPhys)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_PHYS");
        builder.Property(e => e.DeplAvailStatusTExD)
            .HasColumnType("date")
            .HasColumnName("DEPL_AVAIL_STATUS_T_EX_D");
        builder.Property(e => e.DeplAvailStatusTime)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("DEPL_AVAIL_STATUS_TIME");
        builder.Property(e => e.Dob)
            .HasColumnType("date")
            .HasColumnName("DOB");
        builder.Property(e => e.Dos)
            .HasColumnType("date")
            .HasColumnName("DOS");
        builder.Property(e => e.DutyStatus)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("DUTY_STATUS");
        builder.Property(e => e.DutyStatusEffDate)
            .HasColumnType("date")
            .HasColumnName("DUTY_STATUS_EFF_DATE");
        builder.Property(e => e.DutyStatusExpDate)
            .HasColumnType("date")
            .HasColumnName("DUTY_STATUS_EXP_DATE");
        builder.Property(e => e.DyPosnNr)
            .HasMaxLength(9)
            .IsUnicode(false)
            .HasColumnName("DY_POSN_NR");
        builder.Property(e => e.Ets)
            .HasColumnType("date")
            .HasColumnName("ETS");
        builder.Property(e => e.FirstName)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("FIRST_NAME");
        builder.Property(e => e.GrCurr).HasColumnName("GR_CURR");
        builder.Property(e => e.HomePhone)
            .HasMaxLength(15)
            .IsUnicode(false)
            .HasColumnName("HOME_PHONE");
        builder.Property(e => e.LastName)
            .HasMaxLength(30)
            .IsUnicode(false)
            .HasColumnName("LAST_NAME");
        builder.Property(e => e.LocalAddrCity)
            .HasMaxLength(30)
            .IsUnicode(false)
            .HasColumnName("LOCAL_ADDR_CITY");
        builder.Property(e => e.LocalAddrState)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("LOCAL_ADDR_STATE");
        builder.Property(e => e.LocalAddrStreet)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOCAL_ADDR_STREET");
        builder.Property(e => e.MiddleNames)
            .HasMaxLength(30)
            .IsUnicode(false)
            .HasColumnName("MIDDLE_NAMES");
        builder.Property(e => e.OfficeSymbol)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("OFFICE_SYMBOL");
        builder.Property(e => e.Pafsc)
            .HasMaxLength(7)
            .IsUnicode(false)
            .HasColumnName("PAFSC");
        builder.Property(e => e.Pas)
            .HasMaxLength(6)
            .IsUnicode(false)
            .HasColumnName("PAS");
        builder.Property(e => e.PasGaining)
            .HasMaxLength(6)
            .IsUnicode(false)
            .HasColumnName("PAS_GAINING");
        builder.Property(e => e.PasNumber)
            .HasMaxLength(4)
            .IsUnicode(false)
            .HasColumnName("PAS_NUMBER");
        builder.Property(e => e.RcdId)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("RCD_ID");
        builder.Property(e => e.RecStatCurr)
            .HasMaxLength(2)
            .IsUnicode(false)
            .HasColumnName("REC_STAT_CURR");
        builder.Property(e => e.RetSepEffDateProj)
            .HasColumnType("date")
            .HasColumnName("RET_SEP_EFF_DATE_PROJ");
        builder.Property(e => e.SexSvcMbr)
            .HasMaxLength(1)
            .IsUnicode(false)
            .HasColumnName("SEX_SVC_MBR");
        builder.Property(e => e.Suffix)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("SUFFIX");
        builder.Property(e => e.SvcComp)
            .HasMaxLength(1)
            .IsUnicode(false)
            .HasColumnName("SVC_COMP");
        builder.Property(e => e.Tafcsd)
            .HasColumnType("date")
            .HasColumnName("TAFCSD");
        builder.Property(e => e.Tafmsd)
            .HasColumnType("date")
            .HasColumnName("TAFMSD");
        builder.Property(e => e.Zip)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("ZIP");

        // Relationships
        builder.HasOne(d => d.GrCurrNavigation).WithMany(p => p.MemberData)
            .HasForeignKey(d => d.GrCurr)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_MEMBER_DATA_LKUP_GRADE");

        // Indexes
        builder.HasIndex(e => e.GrCurr, "IX_MEMBER_DATA_GR_CURR");
        builder.HasIndex(e => e.Pas, "IX_MEMBER_DATA_PAS");
        builder.HasIndex(e => new { e.LastName, e.FirstName }, "IX_MEMBER_DATA_NAME");

        builder.HasIndex(e => e.RcdId, "IX_MEMBER_DATA_RCD_ID");

        builder.HasIndex(e => e.DutyStatus, "IX_MEMBER_DATA_DUTY_STATUS");

        builder.HasIndex(e => new { e.SvcComp, e.RecStatCurr }, "IX_MEMBER_DATA_COMP_REC_STAT");

        builder.HasIndex(e => e.Dos, "IX_MEMBER_DATA_DOS");

        builder.HasIndex(e => e.Ets, "IX_MEMBER_DATA_ETS");

        builder.HasIndex(e => e.DeplAvailStatusAdmin, "IX_MEMBER_DATA_DEPL_ADMIN");

        builder.HasIndex(e => e.DeplAvailStatusLegal, "IX_MEMBER_DATA_DEPL_LEGAL");

        builder.HasIndex(e => e.DeplAvailStatusPhys, "IX_MEMBER_DATA_DEPL_PHYS");

        builder.HasIndex(e => e.DeplAvailStatusTime, "IX_MEMBER_DATA_DEPL_TIME");

        builder.HasIndex(e => e.Deleted, "IX_MEMBER_DATA_DELETED");

        builder.HasIndex(e => e.DeletedDate, "IX_MEMBER_DATA_DELETED_DATE");
    }
}
