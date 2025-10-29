using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOracleLirDispDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_LIR_Disp_Data table,
/// which stores Line of Investigation Report (LIR) disposition data from AFRC Oracle system migration.
/// Contains investigation details, findings, approval chain data (Wing JA/AA, HQ SG/JA/AA/SR),
/// and personnel information for LIR cases. All properties are nullable strings for Oracle import staging.
/// LIR is used for certain types of investigations distinct from standard LOD (Line of Duty) cases.
/// </remarks>
public class AfrcOracleLirDispDatumConfiguration : IEntityTypeConfiguration<AfrcOracleLirDispDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOracleLirDispDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOracleLirDispDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_LIR_Disp_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration - all string fields for Oracle import
        builder.Property(e => e.LirId).HasColumnName("lir_id");
        builder.Property(e => e.LodId).HasColumnName("lod_id");
        builder.Property(e => e.ReportDate).HasColumnName("report_date");
        builder.Property(e => e.InvestigationOf).HasColumnName("investigation_of");
        builder.Property(e => e.Status).HasColumnName("status");
        builder.Property(e => e.StatusInactiveType).HasColumnName("status_inactive_type");
        builder.Property(e => e.StatusDurationStart).HasColumnName("status_duration_start");
        builder.Property(e => e.StatusDurationFinish).HasColumnName("status_duration_finish");
        builder.Property(e => e.AddressedTo).HasColumnName("addressed_to");
        builder.Property(e => e.CircumstancesTime).HasColumnName("circumstances_time");
        builder.Property(e => e.CircumstancesPlace).HasColumnName("circumstances_place");
        builder.Property(e => e.SustainedDesc).HasColumnName("sustained_desc");
        builder.Property(e => e.Diagnosis).HasColumnName("diagnosis");
        builder.Property(e => e.PresentForDutyYn).HasColumnName("present_for_duty_yn");
        builder.Property(e => e.AbsentWithAuthYn).HasColumnName("absent_with_auth_yn");
        builder.Property(e => e.IntentionalMisconductYn).HasColumnName("intentional_misconduct_yn");
        builder.Property(e => e.MentallySoundYn).HasColumnName("mentally_sound_yn");
        builder.Property(e => e.Remarks).HasColumnName("remarks");
        builder.Property(e => e.Findings).HasColumnName("findings");
        builder.Property(e => e.InvOfficerPersId).HasColumnName("inv_officer_pers_id");
        builder.Property(e => e.AaDate).HasColumnName("aa_date");
        builder.Property(e => e.RaPersId).HasColumnName("ra_pers_id");
        builder.Property(e => e.RaDate).HasColumnName("ra_date");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.CreatedDate).HasColumnName("created_date");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.ModifiedDate).HasColumnName("modified_date");
        builder.Property(e => e.WingJaPersId).HasColumnName("wing_ja_pers_id");
        builder.Property(e => e.WingJaApprovedYn).HasColumnName("wing_ja_approved_yn");
        builder.Property(e => e.WingJaReasons).HasColumnName("wing_ja_reasons");
        builder.Property(e => e.WingAaPersId).HasColumnName("wing_aa_pers_id");
        builder.Property(e => e.WingAaApprovedYn).HasColumnName("wing_aa_approved_yn");
        builder.Property(e => e.WingAaFindings).HasColumnName("wing_aa_findings");
        builder.Property(e => e.WingAaReasons).HasColumnName("wing_aa_reasons");
        builder.Property(e => e.HqsgPersId).HasColumnName("hqsg_pers_id");
        builder.Property(e => e.HqsgFindings).HasColumnName("hqsg_findings");
        builder.Property(e => e.HqsgReasons).HasColumnName("hqsg_reasons");
        builder.Property(e => e.HqjaPersId).HasColumnName("hqja_pers_id");
        builder.Property(e => e.HqjaFindings).HasColumnName("hqja_findings");
        builder.Property(e => e.HqjaReasons).HasColumnName("hqja_reasons");
        builder.Property(e => e.RaReasons).HasColumnName("ra_reasons");
        builder.Property(e => e.HqaaPersId).HasColumnName("hqaa_pers_id");
        builder.Property(e => e.FinalApprovalFindings).HasColumnName("final_approval_findings");
        builder.Property(e => e.HqaaReasons).HasColumnName("hqaa_reasons");
        builder.Property(e => e.WingJaName).HasColumnName("wing_ja_name");
        builder.Property(e => e.WingJaUnit).HasColumnName("wing_ja_unit");
        builder.Property(e => e.WingJaGrade).HasColumnName("wing_ja_grade");
        builder.Property(e => e.WingAaName).HasColumnName("wing_aa_name");
        builder.Property(e => e.WingAaUnit).HasColumnName("wing_aa_unit");
        builder.Property(e => e.WingAaGrade).HasColumnName("wing_aa_grade");
        builder.Property(e => e.RaName).HasColumnName("ra_name");
        builder.Property(e => e.RaUnit).HasColumnName("ra_unit");
        builder.Property(e => e.RaGrade).HasColumnName("ra_grade");
        builder.Property(e => e.HqsgName).HasColumnName("hqsg_name");
        builder.Property(e => e.HqsgUnit).HasColumnName("hqsg_unit");
        builder.Property(e => e.HqsgGrade).HasColumnName("hqsg_grade");
        builder.Property(e => e.HqjaName).HasColumnName("hqja_name");
        builder.Property(e => e.HqjaUnit).HasColumnName("hqja_unit");
        builder.Property(e => e.HqjaGrade).HasColumnName("hqja_grade");
        builder.Property(e => e.HqaaName).HasColumnName("hqaa_name");
        builder.Property(e => e.HqaaUnit).HasColumnName("hqaa_unit");
        builder.Property(e => e.HqaaGrade).HasColumnName("hqaa_grade");
        builder.Property(e => e.HqsrPersId).HasColumnName("hqsr_pers_id");
        builder.Property(e => e.HqsrName).HasColumnName("hqsr_name");
        builder.Property(e => e.HqsrGrade).HasColumnName("hqsr_grade");
        builder.Property(e => e.HqsrUnit).HasColumnName("hqsr_unit");
        builder.Property(e => e.HqsrFindings).HasColumnName("hqsr_findings");
        builder.Property(e => e.HqsrReasons).HasColumnName("hqsr_reasons");

        // Indexes for common queries
        builder.HasIndex(e => e.LirId, "IX_afrc_oracle_lir_disp_lir_id");

        builder.HasIndex(e => e.LodId, "IX_afrc_oracle_lir_disp_lod_id");

        builder.HasIndex(e => e.Status, "IX_afrc_oracle_lir_disp_status");
        
        builder.HasIndex(e => e.CreatedDate, "IX_afrc_oracle_lir_disp_created_date");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_afrc_oracle_lir_disp_modified_date");
        
        builder.HasIndex(e => e.InvOfficerPersId, "IX_afrc_oracle_lir_disp_inv_officer_pers_id");
        
        builder.HasIndex(e => e.RaPersId, "IX_afrc_oracle_lir_disp_ra_pers_id");
    }
}
