using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348Audit"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents audit and review data for Form 348 LOD cases including medical, legal, 
/// and administrative assessments.
/// </remarks>
public class Form348AuditConfiguration : IEntityTypeConfiguration<Form348Audit>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348Audit> builder)
    {
        // Table mapping
        builder.ToTable("Form348Audit", "dbo");

        // Primary key
        builder.HasKey(e => e.AuditId)
            .HasName("PK_Form348Audit");

        // Properties
        builder.Property(e => e.AuditId).HasColumnName("AuditID");
        builder.Property(e => e.LodId).HasColumnName("LodID");
        builder.Property(e => e.CaseId).HasColumnName("CaseID").HasMaxLength(50).IsRequired();
        builder.Property(e => e.Workflow).HasColumnName("Workflow");
        builder.Property(e => e.MedicallyAppropriate).HasColumnName("MedicallyAppropriate");
        
        // Surgeon General (SG) fields
        builder.Property(e => e.SgDx).HasColumnName("SG_DX");
        builder.Property(e => e.SgIsupport).HasColumnName("SG_ISupport");
        builder.Property(e => e.SgEpts).HasColumnName("SG_EPTS");
        builder.Property(e => e.SgAggravation).HasColumnName("SG_Aggravation");
        builder.Property(e => e.SgPrinciples).HasColumnName("SG_Principles");
        builder.Property(e => e.SgOther).HasColumnName("SG_Other");
        builder.Property(e => e.SgProofApplied).HasColumnName("SG_ProofApplied");
        builder.Property(e => e.SgCorrectStandard).HasColumnName("SG_CorrectStandard");
        builder.Property(e => e.SgProofMet).HasColumnName("SG_ProofMet");
        builder.Property(e => e.SgEvidence).HasColumnName("SG_Evidence");
        builder.Property(e => e.SgMisconduct).HasColumnName("SG_Misconduct");
        builder.Property(e => e.SgFormalInvestigation).HasColumnName("SG_FormalInvestigation");
        builder.Property(e => e.SgComment).HasColumnName("SG_Comment");
        
        // Judge Advocate (JA) fields
        builder.Property(e => e.LegallySufficient).HasColumnName("LegallySufficient");
        builder.Property(e => e.JaStandardOfProof).HasColumnName("JA_StandardOfProof");
        builder.Property(e => e.JaDeathAndMva).HasColumnName("JA_DeathAndMVA");
        builder.Property(e => e.JaFormalPolicy).HasColumnName("JA_FormalPolicy");
        builder.Property(e => e.JaAfi).HasColumnName("JA_AFI");
        builder.Property(e => e.JaOther).HasColumnName("JA_Other");
        builder.Property(e => e.JaProofApplied).HasColumnName("JA_ProofApplied");
        builder.Property(e => e.JaCorrectStandard).HasColumnName("JA_CorrectStandard");
        builder.Property(e => e.JaProofMet).HasColumnName("JA_ProofMet");
        builder.Property(e => e.JaEvidence).HasColumnName("JA_Evidence");
        builder.Property(e => e.JaMisconduct).HasColumnName("JA_Misconduct");
        builder.Property(e => e.JaFormalInvestigation).HasColumnName("JA_FormalInvestigation");
        builder.Property(e => e.JaComment).HasColumnName("JA_Comment");
        
        // A1 (Admin) fields
        builder.Property(e => e.StatusValidated).HasColumnName("StatusValidated");
        builder.Property(e => e.StatusOfMember).HasColumnName("StatusOfMember");
        builder.Property(e => e.Orders).HasColumnName("Orders");
        builder.Property(e => e.A1Epts).HasColumnName("A1_EPTS");
        builder.Property(e => e.Idt).HasColumnName("IDT");
        builder.Property(e => e.Pcars).HasColumnName("PCARS");
        builder.Property(e => e.EightYearRule).HasColumnName("EightYearRule");
        builder.Property(e => e.A1Other).HasColumnName("A1_Other");
        builder.Property(e => e.Lodinitiation).HasColumnName("LODInitiation");
        builder.Property(e => e.WrittenDiagnosis).HasColumnName("WrittenDiagnosis");
        builder.Property(e => e.MemberRequest).HasColumnName("MemberRequest");
        builder.Property(e => e.IncurredOrAggravated).HasColumnName("IncurredOrAggravated");
        builder.Property(e => e.IllnessOrDisease).HasColumnName("IllnessOrDisease");
        builder.Property(e => e.Activites).HasColumnName("Activites");
        builder.Property(e => e.A1Comment).HasColumnName("A1_Comment");
        builder.Property(e => e.Determination).HasColumnName("Determination");
        builder.Property(e => e.A1DeterminationNotCorrect).HasColumnName("A1_DeterminationNotCorrect");

        // Indexes for query performance
        builder.HasIndex(e => e.LodId, "IX_Form348Audit_LodID");
        builder.HasIndex(e => e.CaseId, "IX_Form348Audit_CaseID");
        builder.HasIndex(e => e.Workflow, "IX_Form348Audit_Workflow");
        builder.HasIndex(e => e.MedicallyAppropriate, "IX_Form348Audit_MedicallyAppropriate");
        builder.HasIndex(e => e.LegallySufficient, "IX_Form348Audit_LegallySufficient");
        builder.HasIndex(e => e.StatusValidated, "IX_Form348Audit_StatusValidated");
    }
}
