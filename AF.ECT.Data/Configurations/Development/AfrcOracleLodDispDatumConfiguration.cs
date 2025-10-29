using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOracleLodDispDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_LOD_Disp_Data table,
/// which stores Line of Duty (LOD) disposition data from AFRC Oracle system migration.
/// Contains comprehensive LOD case data including medical evaluation, event details, investigation findings,
/// approval chain (Physician, Commander, Wing JA, Appointing Authority, HQ SG/JA/Board/AA/SR),
/// member information, and RWOA (Returned Without Action) tracking. All properties are nullable strings
/// for Oracle import staging. This is the primary LOD case data from legacy Oracle system.
/// </remarks>
public class AfrcOracleLodDispDatumConfiguration : IEntityTypeConfiguration<AfrcOracleLodDispDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOracleLodDispDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOracleLodDispDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_LOD_Disp_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration - comprehensive LOD disposition data
        builder.Property(e => e.LodId).HasColumnName("lod_id");
        builder.Property(e => e.PiId).HasColumnName("pi_id");
        builder.Property(e => e.MedTechPersId).HasColumnName("med_tech_pers_id");
        builder.Property(e => e.ActiveDutyYn).HasColumnName("active_duty_yn");
        builder.Property(e => e.ActiveDutyUnit).HasColumnName("active_duty_unit");
        builder.Property(e => e.MemberStatus).HasColumnName("member_status");
        builder.Property(e => e.EventNatureType).HasColumnName("event_nature_type");
        builder.Property(e => e.EventNatureDetails).HasColumnName("event_nature_details");
        builder.Property(e => e.MedicalFacility).HasColumnName("medical_facility");
        builder.Property(e => e.MedicalFacilityType).HasColumnName("medical_facility_type");
        builder.Property(e => e.AggravatedMilitaryYn).HasColumnName("aggravated_military_yn");
        builder.Property(e => e.EptsYn).HasColumnName("epts_yn");
        builder.Property(e => e.TreatmentDate).HasColumnName("treatment_date");
        builder.Property(e => e.MedOpinionAlcoholYn).HasColumnName("med_opinion_alcohol_yn");
        builder.Property(e => e.MedOpinionDrugYn).HasColumnName("med_opinion_drug_yn");
        builder.Property(e => e.MedOpinionDrugKind).HasColumnName("med_opinion_drug_kind");
        builder.Property(e => e.MedOpinionPhysicalYn).HasColumnName("med_opinion_physical_yn");
        builder.Property(e => e.MedOpinionMentalYn).HasColumnName("med_opinion_mental_yn");
        builder.Property(e => e.MedOpinionCondition).HasColumnName("med_opinion_condition");
        builder.Property(e => e.TestAlcoholYn).HasColumnName("test_alcohol_yn");
        builder.Property(e => e.TestAlcoholResults).HasColumnName("test_alcohol_results");
        builder.Property(e => e.TestPsychiatricalYn).HasColumnName("test_psychiatrical_yn");
        builder.Property(e => e.TestsOthers).HasColumnName("tests_others");
        builder.Property(e => e.EventDetails).HasColumnName("event_details");
        builder.Property(e => e.PhysicianPersId).HasColumnName("physician_pers_id");
        builder.Property(e => e.PhysicianDecision).HasColumnName("physician_decision");
        builder.Property(e => e.PhysicianApprovalReason).HasColumnName("physician_approval_reason");
        builder.Property(e => e.CmdrDutyDetermination).HasColumnName("cmdr_duty_determination");
        builder.Property(e => e.CmdrDutyFrom).HasColumnName("cmdr_duty_from");
        builder.Property(e => e.CmdrDutyTo).HasColumnName("cmdr_duty_to");
        builder.Property(e => e.CmdrDutyOthers).HasColumnName("cmdr_duty_others");
        builder.Property(e => e.CmdrCircDetails).HasColumnName("cmdr_circ_details");
        builder.Property(e => e.CmdrProximateCause).HasColumnName("cmdr_proximate_cause");
        builder.Property(e => e.CmdrProximateSpecify).HasColumnName("cmdr_proximate_specify");
        builder.Property(e => e.CmdrPersId).HasColumnName("cmdr_pers_id");
        builder.Property(e => e.CmdrRecFindings).HasColumnName("cmdr_rec_findings");
        builder.Property(e => e.CmdrExplanation).HasColumnName("cmdr_explanation");
        builder.Property(e => e.WingJaPersId).HasColumnName("wing_ja_pers_id");
        builder.Property(e => e.WingJaDecision).HasColumnName("wing_ja_decision");
        builder.Property(e => e.WingJaExplanation).HasColumnName("wing_ja_explanation");
        builder.Property(e => e.AppointAuthPersId).HasColumnName("appoint_auth_pers_id");
        builder.Property(e => e.AppointAuthDecision).HasColumnName("appoint_auth_decision");
        builder.Property(e => e.AppointAuthExplanation).HasColumnName("appoint_auth_explanation");
        builder.Property(e => e.AppointAuthPoc).HasColumnName("appoint_auth_poc");
        builder.Property(e => e.AssignInvOfficerPersId).HasColumnName("assign_inv_officer_pers_id");
        builder.Property(e => e.AssignInvInstructions).HasColumnName("assign_inv_instructions");
        builder.Property(e => e.AssignInvCompletion).HasColumnName("assign_inv_completion");
        builder.Property(e => e.MpfPersId).HasColumnName("mpf_pers_id");
        builder.Property(e => e.HqsgPersId).HasColumnName("hqsg_pers_id");
        builder.Property(e => e.HqsgDecision).HasColumnName("hqsg_decision");
        builder.Property(e => e.HqsgExplanation).HasColumnName("hqsg_explanation");
        builder.Property(e => e.HqjaPersId).HasColumnName("hqja_pers_id");
        builder.Property(e => e.HqjaDecision).HasColumnName("hqja_decision");
        builder.Property(e => e.HqjaExplanation).HasColumnName("hqja_explanation");
        builder.Property(e => e.HqboardPersId).HasColumnName("hqboard_pers_id");
        builder.Property(e => e.HqboardDecision).HasColumnName("hqboard_decision");
        builder.Property(e => e.HqboardExplanation).HasColumnName("hqboard_explanation");
        builder.Property(e => e.HqaaPersId).HasColumnName("hqaa_pers_id");
        builder.Property(e => e.HqaaDecision).HasColumnName("hqaa_decision");
        builder.Property(e => e.HqaaExplanation).HasColumnName("hqaa_explanation");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.CreatedDate).HasColumnName("created_date");
        builder.Property(e => e.ModifiedBy).HasColumnName("modified_by");
        builder.Property(e => e.ModifiedDate).HasColumnName("modified_date");
        builder.Property(e => e.Icd9Id).HasColumnName("icd9_id");
        builder.Property(e => e.FormalInvYn).HasColumnName("formal_inv_yn");
        builder.Property(e => e.MajcomCsId).HasColumnName("majcom_cs_id");
        builder.Property(e => e.HqaaSignature).HasColumnName("hqaa_signature");
        builder.Property(e => e.HqaaRank).HasColumnName("hqaa_rank");
        builder.Property(e => e.HqaaName).HasColumnName("hqaa_name");
        builder.Property(e => e.RwoaReason).HasColumnName("rwoa_reason");
        builder.Property(e => e.RwoaExplanation).HasColumnName("rwoa_explanation");
        builder.Property(e => e.RwoaMedtechComments).HasColumnName("rwoa_medtech_comments");
        builder.Property(e => e.RwoaDate).HasColumnName("rwoa_date");
        builder.Property(e => e.MedtechName).HasColumnName("medtech_name");
        builder.Property(e => e.MedtechGrade).HasColumnName("medtech_grade");
        builder.Property(e => e.MedtechUnit).HasColumnName("medtech_unit");
        builder.Property(e => e.MemberName).HasColumnName("member_name");
        builder.Property(e => e.MemberSsn).HasColumnName("member_ssn");
        builder.Property(e => e.MemberGrade).HasColumnName("member_grade");
        builder.Property(e => e.MemberUnit).HasColumnName("member_unit");
        builder.Property(e => e.PhysicianName).HasColumnName("physician_name");
        builder.Property(e => e.PhysicianGrade).HasColumnName("physician_grade");
        builder.Property(e => e.CmdrName).HasColumnName("cmdr_name");
        builder.Property(e => e.CmdrGrade).HasColumnName("cmdr_grade");
        builder.Property(e => e.CmdrUnit).HasColumnName("cmdr_unit");
        builder.Property(e => e.JaName).HasColumnName("ja_name");
        builder.Property(e => e.JaGrade).HasColumnName("ja_grade");
        builder.Property(e => e.AppointAuthName).HasColumnName("appoint_auth_name");
        builder.Property(e => e.AppointAuthGrade).HasColumnName("appoint_auth_grade");
        builder.Property(e => e.AppointAuthUnit).HasColumnName("appoint_auth_unit");
        builder.Property(e => e.HqboardName).HasColumnName("hqboard_name");
        builder.Property(e => e.HqboardGrade).HasColumnName("hqboard_grade");
        builder.Property(e => e.HqjaName).HasColumnName("hqja_name");
        builder.Property(e => e.HqjaGrade).HasColumnName("hqja_grade");
        builder.Property(e => e.HqsgName).HasColumnName("hqsg_name");
        builder.Property(e => e.HqsgGrade).HasColumnName("hqsg_grade");
        builder.Property(e => e.InvOfficerName).HasColumnName("inv_officer_name");
        builder.Property(e => e.InvOfficerGrade).HasColumnName("inv_officer_grade");
        builder.Property(e => e.InvOfficerSsn).HasColumnName("inv_officer_ssn");
        builder.Property(e => e.InvOfficerUnit).HasColumnName("inv_officer_unit");
        builder.Property(e => e.InvOfficerBranch).HasColumnName("inv_officer_branch");
        builder.Property(e => e.MemberDob).HasColumnName("member_dob");
        builder.Property(e => e.MemberCsId).HasColumnName("member_cs_id");
        builder.Property(e => e.MpfUnit).HasColumnName("mpf_unit");
        builder.Property(e => e.MpfGrade).HasColumnName("mpf_grade");
        builder.Property(e => e.MpfName).HasColumnName("mpf_name");
        builder.Property(e => e.CmdrActivatedYn).HasColumnName("cmdr_activated_yn");
        builder.Property(e => e.FinalDecision).HasColumnName("final_decision");
        builder.Property(e => e.HqsrPersId).HasColumnName("hqsr_pers_id");
        builder.Property(e => e.HqsrDecision).HasColumnName("hqsr_decision");
        builder.Property(e => e.HqsrExplanation).HasColumnName("hqsr_explanation");
        builder.Property(e => e.HqsrName).HasColumnName("hqsr_name");
        builder.Property(e => e.HqsrGrade).HasColumnName("hqsr_grade");
        builder.Property(e => e.AdId).HasColumnName("ad_id");
        builder.Property(e => e.AdComment).HasColumnName("ad_comment");
        builder.Property(e => e.PhysicianCancelReason).HasColumnName("physician_cancel_reason");
        builder.Property(e => e.PhysicianCancelExplanation).HasColumnName("physician_cancel_explanation");
        builder.Property(e => e.AppointAuthRouting).HasColumnName("appoint_auth_routing");
        builder.Property(e => e.DeathInvolvedYn).HasColumnName("death_involved_yn");
        builder.Property(e => e.MvaInvolvedYn).HasColumnName("mva_involved_yn");
        builder.Property(e => e.LodIdOriginal).HasColumnName("lod_id_original");
        builder.Property(e => e.BoardForGeneralYn).HasColumnName("board_for_general_yn");

        // Indexes for common queries
        builder.HasIndex(e => e.LodId, "IX_afrc_oracle_lod_disp_lod_id");

        builder.HasIndex(e => e.MemberSsn, "IX_afrc_oracle_lod_disp_member_ssn");

        builder.HasIndex(e => e.CreatedDate, "IX_afrc_oracle_lod_disp_created_date");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_afrc_oracle_lod_disp_modified_date");
        
        builder.HasIndex(e => e.MemberStatus, "IX_afrc_oracle_lod_disp_member_status");
        
        builder.HasIndex(e => e.FinalDecision, "IX_afrc_oracle_lod_disp_final_decision");
        
        builder.HasIndex(e => e.PiId, "IX_afrc_oracle_lod_disp_pi_id");
        
        builder.HasIndex(e => e.MemberCsId, "IX_afrc_oracle_lod_disp_member_cs_id");
    }
}
