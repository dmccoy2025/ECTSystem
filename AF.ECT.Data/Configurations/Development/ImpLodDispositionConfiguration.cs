using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpLodDisposition"/> entity.
/// Configures a staging table for importing comprehensive LOD (Line of Duty) disposition data
/// from legacy ALOD systems.
/// </summary>
/// <remarks>
/// ImpLodDisposition is a temporary staging table used during data migration processes to load
/// complete LOD disposition information including medical evaluations, commander determinations,
/// investigative findings, and approval authority decisions from legacy systems. This is one of
/// the most complex staging tables with 120+ properties covering the entire LOD workflow.
/// This entity has no primary key (keyless entity) as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable properties (mostly strings) to accommodate incomplete legacy data
/// - Comprehensive LOD workflow tracking (medical, commander, JA, approving authority)
/// - Event details and nature of incident tracking
/// - Medical opinions and test results
/// - Commander duty determination and proximate cause analysis
/// - Multi-level approval workflow (Wing JA, Appointing Authority, HQ SG/JA/Board/AA)
/// - RWOA (Return Without Authority) tracking
/// - Personnel information for all workflow participants
/// - String-based dates and audit fields for flexible import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production Form348 tables
/// </remarks>
public class ImpLodDispositionConfiguration : IEntityTypeConfiguration<ImpLodDisposition>
{
    /// <summary>
    /// Configures the ImpLodDisposition entity as a keyless staging table with comprehensive
    /// LOD disposition import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpLodDisposition.</param>
    public void Configure(EntityTypeBuilder<ImpLodDisposition> builder)
    {
        builder.ToTable("ImpLodDisposition", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // LOD and personnel identifiers
        builder.Property(e => e.LodId).HasColumnName("LOD_ID");
        builder.Property(e => e.PiId).HasColumnName("PI_ID");
        builder.Property(e => e.MedTechPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("MED_TECH_PERS_ID");

        // Active duty status
        builder.Property(e => e.ActiveDutyYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("ACTIVE_DUTY_YN");
        builder.Property(e => e.ActiveDutyUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("ACTIVE_DUTY_UNIT");
        builder.Property(e => e.MemberStatus).HasMaxLength(50).IsUnicode(false).HasColumnName("MEMBER_STATUS");

        // Event nature and details
        builder.Property(e => e.EventNatureType).HasMaxLength(100).IsUnicode(false).HasColumnName("EVENT_NATURE_TYPE");
        builder.Property(e => e.EventNatureDetails).HasColumnType("ntext").HasColumnName("EVENT_NATURE_DETAILS");
        builder.Property(e => e.EventDetails).HasColumnType("ntext").HasColumnName("EVENT_DETAILS");

        // Medical facility and treatment
        builder.Property(e => e.MedicalFacility).HasMaxLength(100).IsUnicode(false).HasColumnName("MEDICAL_FACILITY");
        builder.Property(e => e.MedicalFacilityType).HasMaxLength(50).IsUnicode(false).HasColumnName("MEDICAL_FACILITY_TYPE");
        builder.Property(e => e.TreatmentDate).HasMaxLength(50).IsUnicode(false).HasColumnName("TREATMENT_DATE");

        // Medical conditions and EPTS
        builder.Property(e => e.AggravatedMilitaryYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("AGGRAVATED_MILITARY_YN");
        builder.Property(e => e.EptsYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("EPTS_YN");

        // Medical opinions
        builder.Property(e => e.MedOpinionAlcoholYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("MED_OPINION_ALCOHOL_YN");
        builder.Property(e => e.MedOpinionDrugYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("MED_OPINION_DRUG_YN");
        builder.Property(e => e.MedOpinionDrugKind).HasMaxLength(100).IsUnicode(false).HasColumnName("MED_OPINION_DRUG_KIND");
        builder.Property(e => e.MedOpinionPhysicalYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("MED_OPINION_PHYSICAL_YN");
        builder.Property(e => e.MedOpinionMentalYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("MED_OPINION_MENTAL_YN");
        builder.Property(e => e.MedOpinionCondition).HasColumnType("ntext").HasColumnName("MED_OPINION_CONDITION");

        // Test results
        builder.Property(e => e.TestAlcoholYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("TEST_ALCOHOL_YN");
        builder.Property(e => e.TestAlcoholResults).HasMaxLength(100).IsUnicode(false).HasColumnName("TEST_ALCOHOL_RESULTS");
        builder.Property(e => e.TestPsychiatricalYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("TEST_PSYCHIATRICAL_YN");
        builder.Property(e => e.TestsOthers).HasColumnType("ntext").HasColumnName("TESTS_OTHERS");

        // Physician decision
        builder.Property(e => e.PhysicianPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("PHYSICIAN_PERS_ID");
        builder.Property(e => e.PhysicianDecision).HasMaxLength(50).IsUnicode(false).HasColumnName("PHYSICIAN_DECISION");
        builder.Property(e => e.PhysicianApprovalReason).HasColumnType("ntext").HasColumnName("PHYSICIAN_APPROVAL_REASON");
        builder.Property(e => e.PhysicianCancelReason).HasMaxLength(100).IsUnicode(false).HasColumnName("PHYSICIAN_CANCEL_REASON");
        builder.Property(e => e.PhysicianCancelExplanation).HasColumnType("ntext").HasColumnName("PHYSICIAN_CANCEL_EXPLANATION");

        // Commander determination
        builder.Property(e => e.CmdrDutyDetermination).HasMaxLength(50).IsUnicode(false).HasColumnName("CMDR_DUTY_DETERMINATION");
        builder.Property(e => e.CmdrDutyFrom).HasMaxLength(100).IsUnicode(false).HasColumnName("CMDR_DUTY_FROM");
        builder.Property(e => e.CmdrDutyTo).HasMaxLength(100).IsUnicode(false).HasColumnName("CMDR_DUTY_TO");
        builder.Property(e => e.CmdrDutyOthers).HasColumnType("ntext").HasColumnName("CMDR_DUTY_OTHERS");
        builder.Property(e => e.CmdrCircDetails).HasColumnType("ntext").HasColumnName("CMDR_CIRC_DETAILS");
        builder.Property(e => e.CmdrProximateCause).HasMaxLength(100).IsUnicode(false).HasColumnName("CMDR_PROXIMATE_CAUSE");
        builder.Property(e => e.CmdrProximateSpecify).HasColumnType("ntext").HasColumnName("CMDR_PROXIMATE_SPECIFY");
        builder.Property(e => e.CmdrPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("CMDR_PERS_ID");
        builder.Property(e => e.CmdrRecFindings).HasMaxLength(50).IsUnicode(false).HasColumnName("CMDR_REC_FINDINGS");
        builder.Property(e => e.CmdrExplanation).HasColumnType("ntext").HasColumnName("CMDR_EXPLANATION");
        builder.Property(e => e.CmdrActivatedYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("CMDR_ACTIVATED_YN");

        // Wing JA review
        builder.Property(e => e.WingJaPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("WING_JA_PERS_ID");
        builder.Property(e => e.WingJaDecision).HasMaxLength(50).IsUnicode(false).HasColumnName("WING_JA_DECISION");
        builder.Property(e => e.WingJaExplanation).HasColumnType("ntext").HasColumnName("WING_JA_EXPLANATION");

        // Appointing authority
        builder.Property(e => e.AppointAuthPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("APPOINT_AUTH_PERS_ID");
        builder.Property(e => e.AppointAuthDecision).HasMaxLength(50).IsUnicode(false).HasColumnName("APPOINT_AUTH_DECISION");
        builder.Property(e => e.AppointAuthExplanation).HasColumnType("ntext").HasColumnName("APPOINT_AUTH_EXPLANATION");
        builder.Property(e => e.AppointAuthPoc).HasMaxLength(100).IsUnicode(false).HasColumnName("APPOINT_AUTH_POC");
        builder.Property(e => e.AppointAuthRouting).HasMaxLength(100).IsUnicode(false).HasColumnName("APPOINT_AUTH_ROUTING");

        // Investigation officer assignment
        builder.Property(e => e.AssignInvOfficerPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("ASSIGN_INV_OFFICER_PERS_ID");
        builder.Property(e => e.AssignInvInstructions).HasColumnType("ntext").HasColumnName("ASSIGN_INV_INSTRUCTIONS");
        builder.Property(e => e.AssignInvCompletion).HasMaxLength(50).IsUnicode(false).HasColumnName("ASSIGN_INV_COMPLETION");

        // MPF coordination
        builder.Property(e => e.MpfPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("MPF_PERS_ID");

        // HQ SG review
        builder.Property(e => e.HqsgPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("HQSG_PERS_ID");
        builder.Property(e => e.HqsgDecision).HasMaxLength(50).IsUnicode(false).HasColumnName("HQSG_DECISION");
        builder.Property(e => e.HqsgExplanation).HasColumnType("ntext").HasColumnName("HQSG_EXPLANATION");

        // HQ JA review
        builder.Property(e => e.HqjaPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("HQJA_PERS_ID");
        builder.Property(e => e.HqjaDecision).HasMaxLength(50).IsUnicode(false).HasColumnName("HQJA_DECISION");
        builder.Property(e => e.HqjaExplanation).HasColumnType("ntext").HasColumnName("HQJA_EXPLANATION");

        // HQ Board review
        builder.Property(e => e.HqboardPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("HQBOARD_PERS_ID");
        builder.Property(e => e.HqboardDecision).HasMaxLength(50).IsUnicode(false).HasColumnName("HQBOARD_DECISION");
        builder.Property(e => e.HqboardExplanation).HasColumnType("ntext").HasColumnName("HQBOARD_EXPLANATION");

        // HQ AA final approval
        builder.Property(e => e.HqaaPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("HQAA_PERS_ID");
        builder.Property(e => e.HqaaDecision).HasMaxLength(50).IsUnicode(false).HasColumnName("HQAA_DECISION");
        builder.Property(e => e.HqaaExplanation).HasColumnType("ntext").HasColumnName("HQAA_EXPLANATION");
        builder.Property(e => e.HqaaSignature).HasMaxLength(100).IsUnicode(false).HasColumnName("HQAA_SIGNATURE");
        builder.Property(e => e.HqaaRank).HasMaxLength(20).IsUnicode(false).HasColumnName("HQAA_RANK");
        builder.Property(e => e.HqaaName).HasMaxLength(100).IsUnicode(false).HasColumnName("HQAA_NAME");

        // HQ SR review
        builder.Property(e => e.HqsrPersId).HasMaxLength(50).IsUnicode(false).HasColumnName("HQSR_PERS_ID");
        builder.Property(e => e.HqsrDecision).HasMaxLength(50).IsUnicode(false).HasColumnName("HQSR_DECISION");
        builder.Property(e => e.HqsrExplanation).HasColumnType("ntext").HasColumnName("HQSR_EXPLANATION");

        // RWOA tracking
        builder.Property(e => e.RwoaReason).HasMaxLength(100).IsUnicode(false).HasColumnName("RWOA_REASON");
        builder.Property(e => e.RwoaExplanation).HasColumnType("ntext").HasColumnName("RWOA_EXPLANATION");
        builder.Property(e => e.RwoaMedtechComments).HasColumnType("ntext").HasColumnName("RWOA_MEDTECH_COMMENTS");
        builder.Property(e => e.RwoaDate).HasMaxLength(50).IsUnicode(false).HasColumnName("RWOA_DATE");

        // Personnel names and details (Med Tech)
        builder.Property(e => e.MedtechName).HasMaxLength(100).IsUnicode(false).HasColumnName("MEDTECH_NAME");
        builder.Property(e => e.MedtechGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("MEDTECH_GRADE");
        builder.Property(e => e.MedtechUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("MEDTECH_UNIT");

        // Member information
        builder.Property(e => e.MemberName).HasMaxLength(100).IsUnicode(false).HasColumnName("MEMBER_NAME");
        builder.Property(e => e.MemberSsn).HasMaxLength(11).IsUnicode(false).HasColumnName("MEMBER_SSN");
        builder.Property(e => e.MemberGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("MEMBER_GRADE");
        builder.Property(e => e.MemberUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("MEMBER_UNIT");
        builder.Property(e => e.MemberDob).HasMaxLength(50).IsUnicode(false).HasColumnName("MEMBER_DOB");
        builder.Property(e => e.MemberCsId).HasMaxLength(50).IsUnicode(false).HasColumnName("MEMBER_CS_ID");

        // Physician information
        builder.Property(e => e.PhysicianName).HasMaxLength(100).IsUnicode(false).HasColumnName("PHYSICIAN_NAME");
        builder.Property(e => e.PhysicianGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("PHYSICIAN_GRADE");

        // Commander information
        builder.Property(e => e.CmdrName).HasMaxLength(100).IsUnicode(false).HasColumnName("CMDR_NAME");
        builder.Property(e => e.CmdrGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("CMDR_GRADE");
        builder.Property(e => e.CmdrUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("CMDR_UNIT");

        // JA information
        builder.Property(e => e.JaName).HasMaxLength(100).IsUnicode(false).HasColumnName("JA_NAME");
        builder.Property(e => e.JaGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("JA_GRADE");

        // Appointing authority information
        builder.Property(e => e.AppointAuthName).HasMaxLength(100).IsUnicode(false).HasColumnName("APPOINT_AUTH_NAME");
        builder.Property(e => e.AppointAuthGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("APPOINT_AUTH_GRADE");
        builder.Property(e => e.AppointAuthUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("APPOINT_AUTH_UNIT");

        // HQ personnel information
        builder.Property(e => e.HqboardName).HasMaxLength(100).IsUnicode(false).HasColumnName("HQBOARD_NAME");
        builder.Property(e => e.HqboardGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("HQBOARD_GRADE");
        builder.Property(e => e.HqjaName).HasMaxLength(100).IsUnicode(false).HasColumnName("HQJA_NAME");
        builder.Property(e => e.HqjaGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("HQJA_GRADE");
        builder.Property(e => e.HqsgName).HasMaxLength(100).IsUnicode(false).HasColumnName("HQSG_NAME");
        builder.Property(e => e.HqsgGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("HQSG_GRADE");
        builder.Property(e => e.HqsrName).HasMaxLength(100).IsUnicode(false).HasColumnName("HQSR_NAME");
        builder.Property(e => e.HqsrGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("HQSR_GRADE");

        // Investigation officer information
        builder.Property(e => e.InvOfficerName).HasMaxLength(100).IsUnicode(false).HasColumnName("INV_OFFICER_NAME");
        builder.Property(e => e.InvOfficerGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("INV_OFFICER_GRADE");
        builder.Property(e => e.InvOfficerSsn).HasMaxLength(11).IsUnicode(false).HasColumnName("INV_OFFICER_SSN");
        builder.Property(e => e.InvOfficerUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("INV_OFFICER_UNIT");
        builder.Property(e => e.InvOfficerBranch).HasMaxLength(50).IsUnicode(false).HasColumnName("INV_OFFICER_BRANCH");

        // MPF information
        builder.Property(e => e.MpfUnit).HasMaxLength(100).IsUnicode(false).HasColumnName("MPF_UNIT");
        builder.Property(e => e.MpfGrade).HasMaxLength(20).IsUnicode(false).HasColumnName("MPF_GRADE");
        builder.Property(e => e.MpfName).HasMaxLength(100).IsUnicode(false).HasColumnName("MPF_NAME");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate).HasColumnType("datetime").HasColumnName("CREATED_DATE");
        builder.Property(e => e.ModifiedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_DATE");

        // Additional tracking properties
        builder.Property(e => e.Icd9Id).HasMaxLength(50).IsUnicode(false).HasColumnName("ICD9_ID");
        builder.Property(e => e.FormalInvYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("FORMAL_INV_YN");
        builder.Property(e => e.MajcomCsId).HasMaxLength(50).IsUnicode(false).HasColumnName("MAJCOM_CS_ID");
        builder.Property(e => e.FinalDecision).HasMaxLength(50).IsUnicode(false).HasColumnName("FINAL_DECISION");
        builder.Property(e => e.AdId).HasMaxLength(50).IsUnicode(false).HasColumnName("AD_ID");
        builder.Property(e => e.AdComment).HasColumnType("ntext").HasColumnName("AD_COMMENT");
        builder.Property(e => e.DeathInvolvedYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("DEATH_INVOLVED_YN");
        builder.Property(e => e.MvaInvolvedYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("MVA_INVOLVED_YN");
        builder.Property(e => e.LodIdOriginal).HasColumnName("LOD_ID_ORIGINAL");
        builder.Property(e => e.BoardForGeneralYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("BOARD_FOR_GENERAL_YN");
        
        // Indexes for common queries
        builder.HasIndex(e => e.LodId, "IX_imp_lod_disposition_lod_id");
        
        builder.HasIndex(e => e.PiId, "IX_imp_lod_disposition_pi_id");
        
        builder.HasIndex(e => e.MemberSsn, "IX_imp_lod_disposition_member_ssn");
        
        builder.HasIndex(e => e.MemberStatus, "IX_imp_lod_disposition_member_status");
        
        builder.HasIndex(e => e.FinalDecision, "IX_imp_lod_disposition_final_decision");
        
        builder.HasIndex(e => e.CreatedDate, "IX_imp_lod_disposition_created_date");
        
        builder.HasIndex(e => e.MemberCsId, "IX_imp_lod_disposition_member_cs_id");
    }
}
