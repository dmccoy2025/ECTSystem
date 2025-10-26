using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Documents;

/// <summary>
/// Entity Framework Core configuration for the PalDatum entity.
/// Configures production PAL (Physician Authorization Letter) document data with comprehensive
/// medical evaluation tracking, deployment restrictions, and disposition management.
/// </summary>
public class PalDatumConfiguration : IEntityTypeConfiguration<PalDatum>
{
    /// <summary>
    /// Configures the PalDatum entity with table mapping, primary key, properties,
    /// and indexes for efficient PAL document queries and medical evaluation tracking.
    /// </summary>
    /// <param name="builder">The entity type builder for PalDatum.</param>
    public void Configure(EntityTypeBuilder<PalDatum> builder)
    {
        builder.HasKey(e => e.Ssan).HasName("PK__PAL_DATA__CA1E8E3C7C6F7215");

        builder.ToTable("PAL_DATA", "dbo");

        builder.Property(e => e.Ssan)
            .HasMaxLength(11)
            .IsUnicode(false)
            .HasColumnName("SSAN");
        builder.Property(e => e.AdditionalComments)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("ADDITIONAL_COMMENTS");
        builder.Property(e => e.AfForm422Feb93Format).HasColumnName("AF_FORM_422_FEB93_FORMAT");
        builder.Property(e => e.AlcC).HasColumnName("ALC_C");
        builder.Property(e => e.Archive).HasColumnName("ARCHIVE");
        builder.Property(e => e.ArtAndDsn)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("ART_AND_DSN");
        builder.Property(e => e.AscCode)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("ASC_CODE");
        builder.Property(e => e.AsthmaGen12).HasColumnName("ASTHMA_GEN_12");
        builder.Property(e => e.Baseassign)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("BASEASSIGN");
        builder.Property(e => e.BirthDate).HasColumnName("BIRTH_DATE");
        builder.Property(e => e.Cad121).HasColumnName("CAD_12_1");
        builder.Property(e => e.Cancer).HasColumnName("CANCER");
        builder.Property(e => e.Completeby)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("COMPLETEBY");
        builder.Property(e => e.Conus).HasColumnName("CONUS");
        builder.Property(e => e.ConusIfCheckedFirst).HasColumnName("CONUS_IF_CHECKED_FIRST");
        builder.Property(e => e.Coronary).HasColumnName("CORONARY");
        builder.Property(e => e.DateOut).HasColumnName("DATE_OUT");
        builder.Property(e => e.DateRec).HasColumnName("DATE_REC");
        builder.Property(e => e.DateSentToFiche).HasColumnName("DATE_SENT_TO_FICHE");
        builder.Property(e => e.Dental).HasColumnName("DENTAL");
        builder.Property(e => e.Diagnosis)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("DIAGNOSIS");
        builder.Property(e => e.Discharged).HasColumnName("DISCHARGED");
        builder.Property(e => e.DischargedDesc)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("DISCHARGED_DESC");
        builder.Property(e => e.Disposition)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DISPOSITION");
        builder.Property(e => e.Dm).HasColumnName("DM");
        builder.Property(e => e.Dm122).HasColumnName("DM_12_2");
        builder.Property(e => e.DqDiagnosis)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("DQ_DIAGNOSIS");
        builder.Property(e => e.DqParaNumber)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("DQ_PARA_NUMBER");
        builder.Property(e => e.Eent).HasColumnName("EENT");
        builder.Property(e => e.Expiredate).HasColumnName("EXPIREDATE");
        builder.Property(e => e.Fiche)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("FICHE");
        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("FIRST_NAME");
        builder.Property(e => e.Gi).HasColumnName("GI");
        builder.Property(e => e.HeShe)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("HE_SHE");
        builder.Property(e => e.HeSheNotCapped)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("HE_SHE_NOT_CAPPED");
        builder.Property(e => e.HimHer)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("HIM_HER");
        builder.Property(e => e.HisHer)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("HIS_HER");
        builder.Property(e => e.Hiv).HasColumnName("HIV");
        builder.Property(e => e.Hiv123).HasColumnName("HIV_12_3");
        builder.Property(e => e.HomeStationNoMandays).HasColumnName("HOME_STATION_NO_MANDAYS");
        builder.Property(e => e.IndefiniteWaiver).HasColumnName("INDEFINITE_WAIVER");
        builder.Property(e => e.InputBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("INPUT_BY");
        builder.Property(e => e.IpebElectionIncluded).HasColumnName("IPEB_ELECTION_INCLUDED");
        builder.Property(e => e.IpebelectedNo).HasColumnName("IPEBELECTED_NO");
        builder.Property(e => e.IpebelectedYes).HasColumnName("IPEBELECTED_YES");
        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LAST_NAME");
        builder.Property(e => e.MedicalEvaluationFactSheet).HasColumnName("MEDICAL_EVALUATION_FACT_SHEET");
        builder.Property(e => e.Miscellaneous)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("MISCELLANEOUS");
        builder.Property(e => e.Musculoskeletal).HasColumnName("MUSCULOSKELETAL");
        builder.Property(e => e.NarrativeSummary).HasColumnName("NARRATIVE_SUMMARY");
        builder.Property(e => e.Neuro).HasColumnName("NEURO");
        builder.Property(e => e.NoHome).HasColumnName("NO_HOME");
        builder.Property(e => e.Other).HasColumnName("OTHER");
        builder.Property(e => e.Psych).HasColumnName("PSYCH");
        builder.Property(e => e.Psych124).HasColumnName("PSYCH_12_4");
        builder.Property(e => e.Pulmonary).HasColumnName("PULMONARY");
        builder.Property(e => e.Questionnaire).HasColumnName("QUESTIONNAIRE");
        builder.Property(e => e.Rank)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RANK");
        builder.Property(e => e.Rating)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RATING");
        builder.Property(e => e.RcdId).HasColumnName("RCD_ID");
        builder.Property(e => e.ReActivate).HasColumnName("RE_ACTIVATE");
        builder.Property(e => e.ReceivedFrom)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RECEIVED_FROM");
        builder.Property(e => e.Remarks)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("REMARKS");
        builder.Property(e => e.Renewal).HasColumnName("RENEWAL");
        builder.Property(e => e.RetToDutyBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RET_TO_DUTY_BY");
        builder.Property(e => e.Retired).HasColumnName("RETIRED");
        builder.Property(e => e.RetiredDesc)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("RETIRED_DESC");
        builder.Property(e => e.Rk).HasColumnName("RK");
        builder.Property(e => e.Rwoa).HasColumnName("RWOA");
        builder.Property(e => e.RwoaReason)
            .HasMaxLength(999)
            .IsUnicode(false)
            .HasColumnName("RWOA_REASON");
        builder.Property(e => e.SentTo)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("SENT_TO");
        builder.Property(e => e.Signature)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("SIGNATURE");
        builder.Property(e => e.Trwoa).HasColumnName("TRWOA");
        builder.Property(e => e.Type)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("TYPE");
        builder.Property(e => e.UnitCommanderSMemo).HasColumnName("UNIT_COMMANDER_S_MEMO");
        builder.Property(e => e.Wnlr).HasColumnName("WNLR");
        builder.Property(e => e._1).HasColumnName("_1");
        builder.Property(e => e._10).HasColumnName("_10");
        builder.Property(e => e._101).HasColumnName("_10_1");
        builder.Property(e => e._102).HasColumnName("_10_2");
        builder.Property(e => e._103).HasColumnName("_10_3");
        builder.Property(e => e._11).HasColumnName("_11");
        builder.Property(e => e._13).HasColumnName("_13");
        builder.Property(e => e._15).HasColumnName("_15");
        builder.Property(e => e._16).HasColumnName("_16");
        builder.Property(e => e._161).HasColumnName("_16_1");
        builder.Property(e => e._162).HasColumnName("_16_2");
        builder.Property(e => e._1621).HasColumnName("_16_2_1");
        builder.Property(e => e._163).HasColumnName("_16_3");
        builder.Property(e => e._164).HasColumnName("_16_4");
        builder.Property(e => e._165).HasColumnName("_16_5");
        builder.Property(e => e._165a).HasColumnName("_16_5A");
        builder.Property(e => e._166).HasColumnName("_16_6");
        builder.Property(e => e._167).HasColumnName("_16_7");
        builder.Property(e => e._167a).HasColumnName("_16_7A");
        builder.Property(e => e._167b).HasColumnName("_16_7B");
        builder.Property(e => e._18).HasColumnName("_18");
        builder.Property(e => e._20).HasColumnName("_20");
        builder.Property(e => e._21).HasColumnName("_21");
        builder.Property(e => e._211).HasColumnName("_21_1");
        builder.Property(e => e._22).HasColumnName("_22");
        builder.Property(e => e._3).HasColumnName("_3");
        builder.Property(e => e._31).HasColumnName("_3_1");
        builder.Property(e => e._4).HasColumnName("_4");
        builder.Property(e => e._41).HasColumnName("_4_1");
        builder.Property(e => e._411).HasColumnName("_4_1_1");
        builder.Property(e => e._42).HasColumnName("_4_2");
        builder.Property(e => e._43).HasColumnName("_4_3");
        builder.Property(e => e._44).HasColumnName("_4_4");
        builder.Property(e => e._45).HasColumnName("_4_5");
        builder.Property(e => e._5).HasColumnName("_5");
        builder.Property(e => e._6).HasColumnName("_6");
        builder.Property(e => e._61).HasColumnName("_6_1");
        builder.Property(e => e._7).HasColumnName("_7");
        builder.Property(e => e._8).HasColumnName("_8");
        builder.Property(e => e._9).HasColumnName("_9");
        builder.Property(e => e._91).HasColumnName("_9_1");

        builder.HasIndex(e => e.LastName, "IX_PAL_DATA_LAST_NAME");
        builder.HasIndex(e => e.Archive, "IX_PAL_DATA_ARCHIVE");
        builder.HasIndex(e => e.Expiredate, "IX_PAL_DATA_EXPIREDATE");
    }
}
