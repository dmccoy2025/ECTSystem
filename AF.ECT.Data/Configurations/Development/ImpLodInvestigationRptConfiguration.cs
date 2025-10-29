using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpLodInvestigationRpt"/> entity.
/// Configures a staging table for importing LOD (Line of Duty) Investigation Report data
/// from legacy ALOD systems.
/// </summary>
/// <remarks>
/// ImpLodInvestigationRpt is a temporary staging table used during data migration processes to load
/// formal investigation reports associated with LOD cases from legacy systems. This entity captures
/// the investigating officer's report, findings, and multi-level approval workflow. This entity has
/// no primary key (keyless entity) as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate incomplete legacy data
/// - Investigation report details (date, officer, status, circumstances)
/// - Investigation findings (present for duty, authorized absence, misconduct, mental soundness)
/// - Diagnosis and sustained injuries
/// - Multi-level approval workflow (Wing JA/AA, RA, HQ SG/JA/AA/SR)
/// - Personnel information for all approval authorities
/// - String-based dates and audit fields for flexible import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production tables
/// </remarks>
public class ImpLodInvestigationRptConfiguration : IEntityTypeConfiguration<ImpLodInvestigationRpt>
{
    /// <summary>
    /// Configures the ImpLodInvestigationRpt entity as a keyless staging table with comprehensive
    /// investigation report import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpLodInvestigationRpt.</param>
    public void Configure(EntityTypeBuilder<ImpLodInvestigationRpt> builder)
    {
        builder.ToTable("ImpLodInvestigationRpt", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Report identifiers
        builder.Property(e => e.LirId).HasMaxLength(50).IsUnicode(false).HasColumnName("LIR_ID");
        builder.Property(e => e.LodId).HasMaxLength(50).IsUnicode(false).HasColumnName("LOD_ID");

        // Report details
        builder.Property(e => e.ReportDate).HasMaxLength(50).IsUnicode(false).HasColumnName("REPORT_DATE");
        builder.Property(e => e.InvestigationOf).HasMaxLength(200).IsUnicode(false).HasColumnName("INVESTIGATION_OF");
        builder.Property(e => e.AddressedTo).HasMaxLength(200).IsUnicode(false).HasColumnName("ADDRESSED_TO");

        // Status tracking
        builder.Property(e => e.Status).HasMaxLength(50).IsUnicode(false).HasColumnName("STATUS");
        builder.Property(e => e.StatusInactiveType).HasMaxLength(50).IsUnicode(false).HasColumnName("STATUS_INACTIVE_TYPE");
        builder.Property(e => e.StatusDurationStart).HasMaxLength(50).IsUnicode(false).HasColumnName("STATUS_DURATION_START");
        builder.Property(e => e.StatusDurationFinish).HasMaxLength(50).IsUnicode(false).HasColumnName("STATUS_DURATION_FINISH");

        // Circumstances
        builder.Property(e => e.CircumstancesTime).HasMaxLength(100).IsUnicode(false).HasColumnName("CIRCUMSTANCES_TIME");
        builder.Property(e => e.CircumstancesPlace).HasMaxLength(200).IsUnicode(false).HasColumnName("CIRCUMSTANCES_PLACE");

        // Injuries and diagnosis
        builder.Property(e => e.SustainedDesc).HasColumnType("ntext").HasColumnName("SUSTAINED_DESC");
        builder.Property(e => e.Diagnosis).HasColumnType("ntext").HasColumnName("DIAGNOSIS");

        // Investigation findings (Yes/No)
        builder.Property(e => e.PresentForDutyYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("PRESENT_FOR_DUTY_YN");
        builder.Property(e => e.AbsentWithAuthYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("ABSENT_WITH_AUTH_YN");
        builder.Property(e => e.IntentionalMisconductYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("INTENTIONAL_MISCONDUCT_YN");
        builder.Property(e => e.MentallySoundYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("MENTALLY_SOUND_YN");

        // Remarks and findings
        builder.Property(e => e.Remarks).HasColumnType("ntext").HasColumnName("REMARKS");
        builder.Property(e => e.Findings).HasMaxLength(50).IsUnicode(false).HasColumnName("FINDINGS");

        // Investigation officer
        builder.Property(e => e.InvOfficerPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("INV_OFFICER_PERS_ID");

        // Approving authority and reviewing authority
        builder.Property(e => e.AaDate).HasMaxLength(50).IsUnicode(false).HasColumnName("AA_DATE");
        builder.Property(e => e.RaPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("RA_PERS_ID");
        builder.Property(e => e.RaDate).HasMaxLength(50).IsUnicode(false).HasColumnName("RA_DATE");
        builder.Property(e => e.RaReasons).HasColumnType("ntext").HasColumnName("RA_REASONS");

        // Wing JA review
        builder.Property(e => e.WingJaPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("WING_JA_PERS_ID");
        builder.Property(e => e.WingJaApprovedYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("WING_JA_APPROVED_YN");
        builder.Property(e => e.WingJaReasons).HasColumnType("ntext").HasColumnName("WING_JA_REASONS");

        // Wing AA review
        builder.Property(e => e.WingAaPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("WING_AA_PERS_ID");
        builder.Property(e => e.WingAaApprovedYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("WING_AA_APPROVED_YN");
        builder.Property(e => e.WingAaFindings).HasMaxLength(50).IsUnicode(false).HasColumnName("WING_AA_FINDINGS");
        builder.Property(e => e.WingAaReasons).HasColumnType("ntext").HasColumnName("WING_AA_REASONS");

        // HQ SG review
        builder.Property(e => e.HqsgPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("HQSG_PERS_ID");
        builder.Property(e => e.HqsgFindings).HasMaxLength(50).IsUnicode(false).HasColumnName("HQSG_FINDINGS");
        builder.Property(e => e.HqsgReasons).HasColumnType("ntext").HasColumnName("HQSG_REASONS");

        // HQ JA review
        builder.Property(e => e.HqjaPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("HQJA_PERS_ID");
        builder.Property(e => e.HqjaFindings).HasMaxLength(50).IsUnicode(false).HasColumnName("HQJA_FINDINGS");
        builder.Property(e => e.HqjaReasons).HasColumnType("ntext").HasColumnName("HQJA_REASONS");

        // HQ AA final approval
        builder.Property(e => e.HqaaPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("HQAA_PERS_ID");
        builder.Property(e => e.FinalApprovalFindings).HasMaxLength(50).IsUnicode(false).HasColumnName("FINAL_APPROVAL_FINDINGS");
        builder.Property(e => e.HqaaReasons).HasColumnType("ntext").HasColumnName("HQAA_REASONS");

        // HQ SR review
        builder.Property(e => e.HqsrPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("HQSR_PERS_ID");
        builder.Property(e => e.HqsrFindings).HasMaxLength(50).IsUnicode(false).HasColumnName("HQSR_FINDINGS");
        builder.Property(e => e.HqsrReasons).HasColumnType("ntext").HasColumnName("HQSR_REASONS");

        // Personnel names and details (Wing JA)
        builder.Property(e => e.WingJaName).HasMaxLength(100).IsUnicode(false).HasColumnName("WING_JA_NAME");
        builder.Property(e => e.WingJaUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("WING_JA_UNIT");
        builder.Property(e => e.WingJaGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("WING_JA_GRADE");

        // Wing AA information
        builder.Property(e => e.WingAaName).HasMaxLength(100).IsUnicode(false).HasColumnName("WING_AA_NAME");
        builder.Property(e => e.WingAaUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("WING_AA_UNIT");
        builder.Property(e => e.WingAaGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("WING_AA_GRADE");

        // RA information
        builder.Property(e => e.RaName).HasMaxLength(100).IsUnicode(false).HasColumnName("RA_NAME");
        builder.Property(e => e.RaUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("RA_UNIT");
        builder.Property(e => e.RaGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("RA_GRADE");

        // HQ SG information
        builder.Property(e => e.HqsgName).HasMaxLength(100).IsUnicode(false).HasColumnName("HQSG_NAME");
        builder.Property(e => e.HqsgUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("HQSG_UNIT");
        builder.Property(e => e.HqsgGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("HQSG_GRADE");

        // HQ JA information
        builder.Property(e => e.HqjaName).HasMaxLength(100).IsUnicode(false).HasColumnName("HQJA_NAME");
        builder.Property(e => e.HqjaUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("HQJA_UNIT");
        builder.Property(e => e.HqjaGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("HQJA_GRADE");

        // HQ AA information
        builder.Property(e => e.HqaaName).HasMaxLength(100).IsUnicode(false).HasColumnName("HQAA_NAME");
        builder.Property(e => e.HqaaUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("HQAA_UNIT");
        builder.Property(e => e.HqaaGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("HQAA_GRADE");

        // HQ SR information
        builder.Property(e => e.HqsrName).HasMaxLength(100).IsUnicode(false).HasColumnName("HQSR_NAME");
        builder.Property(e => e.HqsrUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("HQSR_UNIT");
        builder.Property(e => e.HqsrGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("HQSR_GRADE");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_DATE");
        builder.Property(e => e.ModifiedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.LirId, "IX_imp_lod_investigation_rpt_lir_id");
        
        builder.HasIndex(e => e.LodId, "IX_imp_lod_investigation_rpt_lod_id");
        
        builder.HasIndex(e => e.Status, "IX_imp_lod_investigation_rpt_status");
        
        builder.HasIndex(e => e.Findings, "IX_imp_lod_investigation_rpt_findings");
        
        builder.HasIndex(e => e.InvOfficerPersId, "IX_imp_lod_investigation_rpt_inv_officer_pers_id");
    }
}
