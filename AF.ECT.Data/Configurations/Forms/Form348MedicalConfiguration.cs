using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Form348Medical"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Form_348_Medical table,
/// which stores comprehensive medical information for Form 348 LOD cases.
/// Contains member status, event nature/details, medical facility information, treatment dates,
/// death/MVA involvement flags, ICD-9/ICD-10 diagnosis codes, EPTS (Existed Prior to Service) determination,
/// physician approval/cancellation, member classification (from/component/category), influence factors,
/// psychological evaluations, test results, deployment location, service aggravation, mobility standards,
/// alcohol/drug testing, board finalization, workflow status, and various LOD-specific flags.
/// This is a comprehensive medical assessment entity supporting LOD determinations.
/// Has a relationship with CoreLkupIcd9 for diagnosis code lookup.
/// </remarks>
public class Form348MedicalConfiguration : IEntityTypeConfiguration<Form348Medical>
{
    /// <summary>
    /// Configures the entity of type <see cref="Form348Medical"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Form348Medical> builder)
    {
        // Table mapping
        builder.ToTable("Form_348_Medical", "dbo");

        // Primary key
        builder.HasKey(e => e.Lodid)
            .HasName("PK_Form_348_Medical");

        // Properties configuration
        builder.Property(e => e.Lodid)
            .HasColumnName("LODID")
            .ValueGeneratedNever();

        builder.Property(e => e.MemberStatus)
            .HasMaxLength(100)
            .HasColumnName("Member_Status");

        builder.Property(e => e.EventNatureType)
            .HasMaxLength(100)
            .HasColumnName("Event_Nature_Type");

        builder.Property(e => e.EventNatureDetails)
            .HasMaxLength(4000)
            .HasColumnName("Event_Nature_Details");

        builder.Property(e => e.MedicalFacility)
            .HasMaxLength(200)
            .HasColumnName("Medical_Facility");

        builder.Property(e => e.MedicalFacilityType)
            .HasMaxLength(100)
            .HasColumnName("Medical_Facility_Type");

        builder.Property(e => e.TreatmentDate)
            .HasColumnName("Treatment_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.DeathInvolvedYn)
            .HasMaxLength(10)
            .HasColumnName("Death_Involved_YN");

        builder.Property(e => e.MvaInvolvedYn)
            .HasMaxLength(10)
            .HasColumnName("MVA_Involved_YN");

        builder.Property(e => e.Icd9Id)
            .HasColumnName("ICD9_ID");

        builder.Property(e => e.EptsYn)
            .HasColumnName("EPTS_YN");

        builder.Property(e => e.PhysicianApprovalComments)
            .HasMaxLength(4000)
            .HasColumnName("Physician_Approval_Comments");

        builder.Property(e => e.ModifiedBy)
            .IsRequired()
            .HasColumnName("Modified_By");

        builder.Property(e => e.ModifiedDate)
            .IsRequired()
            .HasColumnName("Modified_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.PhysicianCancelReason)
            .HasColumnName("Physician_Cancel_Reason");

        builder.Property(e => e.PhysicianCancelExplanation)
            .HasMaxLength(4000)
            .HasColumnName("Physician_Cancel_Explanation");

        builder.Property(e => e.DiagnosisText)
            .HasMaxLength(4000)
            .HasColumnName("Diagnosis_Text");

        builder.Property(e => e.Icd7thChar)
            .HasMaxLength(10)
            .HasColumnName("ICD_7th_Char");

        builder.Property(e => e.MemberFrom)
            .HasColumnName("Member_From");

        builder.Property(e => e.MemberComponent)
            .HasColumnName("Member_Component");

        builder.Property(e => e.MemberCategory)
            .HasColumnName("Member_Category");

        builder.Property(e => e.Influence)
            .HasColumnName("Influence");

        builder.Property(e => e.MemberResponsible)
            .HasMaxLength(4000)
            .HasColumnName("Member_Responsible");

        builder.Property(e => e.PsychEval)
            .HasMaxLength(4000)
            .HasColumnName("Psych_Eval");

        builder.Property(e => e.PsychDate)
            .HasColumnName("Psych_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.RelevantCondition)
            .HasMaxLength(4000)
            .HasColumnName("Relevant_Condition");

        builder.Property(e => e.OtherTest)
            .HasMaxLength(4000)
            .HasColumnName("Other_Test");

        builder.Property(e => e.OtherTestDate)
            .HasColumnName("Other_Test_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.DeployedLocation)
            .HasMaxLength(200)
            .HasColumnName("Deployed_Location");

        builder.Property(e => e.ConditionEpts)
            .HasColumnName("Condition_EPTS");

        builder.Property(e => e.ServiceAggravated)
            .HasColumnName("Service_Aggravated");

        builder.Property(e => e.MobilityStandards)
            .HasMaxLength(4000)
            .HasColumnName("Mobility_Standards");

        builder.Property(e => e.MemberCondition)
            .HasMaxLength(4000)
            .HasColumnName("Member_Condition");

        builder.Property(e => e.AlcoholTestDone)
            .HasMaxLength(100)
            .HasColumnName("Alcohol_Test_Done");

        builder.Property(e => e.DrugTestDone)
            .HasMaxLength(100)
            .HasColumnName("Drug_Test_Done");

        builder.Property(e => e.BoardFinalization)
            .HasMaxLength(100)
            .HasColumnName("Board_Finalization");

        builder.Property(e => e.Workflow)
            .HasColumnName("Workflow");

        builder.Property(e => e.LOdinitiation)
            .HasColumnName("LODInitiation");

        builder.Property(e => e.WrittenDiagnosis)
            .HasColumnName("Written_Diagnosis");

        builder.Property(e => e.MemberRequest)
            .HasColumnName("Member_Request");

        builder.Property(e => e.PriorToDutytatus)
            .HasColumnName("Prior_To_DutyTatus");

        builder.Property(e => e.StatusWorsened)
            .HasMaxLength(4000)
            .HasColumnName("Status_Worsened");

        // Relationships
        builder.HasOne(e => e.Icd9)
            .WithMany()
            .HasForeignKey(e => e.Icd9Id)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_Form_348_Medical_ICD9");

        // Indexes
        builder.HasIndex(e => e.Icd9Id, "IX_form_348_medical_icd9_id");

        builder.HasIndex(e => e.TreatmentDate, "IX_form_348_medical_treatment_date");

        builder.HasIndex(e => e.Workflow, "IX_form_348_medical_workflow");

        builder.HasIndex(e => e.ModifiedDate, "IX_form_348_medical_modified_date");

        builder.HasIndex(e => e.ModifiedBy, "IX_form_348_medical_modified_by");

        builder.HasIndex(e => e.MemberStatus, "IX_form_348_medical_member_status");

        builder.HasIndex(e => e.EventNatureType, "IX_form_348_medical_event_nature_type");

        builder.HasIndex(e => e.MedicalFacility, "IX_form_348_medical_facility");

        builder.HasIndex(e => e.MedicalFacilityType, "IX_form_348_medical_facility_type");

        builder.HasIndex(e => e.DeathInvolvedYn, "IX_form_348_medical_death_involved");

        builder.HasIndex(e => e.MvaInvolvedYn, "IX_form_348_medical_mva_involved");

        builder.HasIndex(e => e.EptsYn, "IX_form_348_medical_epts");

        builder.HasIndex(e => e.MemberFrom, "IX_form_348_medical_member_from");

        builder.HasIndex(e => e.MemberComponent, "IX_form_348_medical_member_component");

        builder.HasIndex(e => e.MemberCategory, "IX_form_348_medical_member_category");

        builder.HasIndex(e => e.Influence, "IX_form_348_medical_influence");

        builder.HasIndex(e => e.PhysicianCancelReason, "IX_form_348_medical_physician_cancel_reason");

        builder.HasIndex(e => new { e.Workflow, e.ModifiedDate }, "IX_form_348_medical_workflow_modified");
    }
}
