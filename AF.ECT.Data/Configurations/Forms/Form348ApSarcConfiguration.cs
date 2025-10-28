using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348ApSarc"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents Form 348 SARC Appeal cases with board signatures, workflow status, and member information.
/// </remarks>
public class Form348ApSarcConfiguration : IEntityTypeConfiguration<Form348ApSarc>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348ApSarc> builder)
    {
        // Table mapping
        builder.ToTable("Form348APSARC", "dbo");

        // Primary key
        builder.HasKey(e => e.AppealSarcId)
            .HasName("PK_Form348APSARC");

        // Properties
        builder.Property(e => e.AppealSarcId).HasColumnName("AppealSarcID");
        builder.Property(e => e.InitialId).HasColumnName("InitialID");
        builder.Property(e => e.InitialWorkflow).HasColumnName("InitialWorkflow");
        builder.Property(e => e.CaseId).HasColumnName("CaseID").HasMaxLength(50).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.Status).HasColumnName("Status");
        builder.Property(e => e.Workflow).HasColumnName("Workflow");
        builder.Property(e => e.CancelReason).HasColumnName("CancelReason");
        builder.Property(e => e.CancelExplanation).HasColumnName("CancelExplanation");
        builder.Property(e => e.CancelDate).HasColumnName("CancelDate");
        builder.Property(e => e.RwoaReason).HasColumnName("RwoaReason");
        builder.Property(e => e.RwoaExplanation).HasColumnName("RwoaExplanation");
        builder.Property(e => e.RwoaDate).HasColumnName("RwoaDate");
        builder.Property(e => e.RwoaReply).HasColumnName("RwoaReply");
        builder.Property(e => e.ReturnToGroup).HasColumnName("ReturnToGroup");
        builder.Property(e => e.ReturnByGroup).HasColumnName("ReturnByGroup");
        builder.Property(e => e.MemberName).HasColumnName("MemberName").HasMaxLength(200).IsRequired();
        builder.Property(e => e.MemberSsn).HasColumnName("MemberSSN").HasMaxLength(20).IsRequired();
        builder.Property(e => e.ReturnComment).HasColumnName("ReturnComment");
        builder.Property(e => e.DocGroupId).HasColumnName("DocGroupID");
        builder.Property(e => e.MemberNotified).HasColumnName("MemberNotified");
        builder.Property(e => e.ConsultationFromUsergroupId).HasColumnName("ConsultationFromUsergroupID");
        builder.Property(e => e.IsPostProcessingComplete).HasColumnName("IsPostProcessingComplete");
        builder.Property(e => e.MemberUnit).HasColumnName("MemberUnit").HasMaxLength(255).IsRequired();
        builder.Property(e => e.MemberUnitId).HasColumnName("MemberUnitID");
        builder.Property(e => e.MemberCompo).HasColumnName("MemberCompo").HasMaxLength(50).IsRequired();
        builder.Property(e => e.MemberGrade).HasColumnName("MemberGrade");
        builder.Property(e => e.IsNonDbsignCase).HasColumnName("IsNonDbsignCase");

        // Signature fields - Wing SARC
        builder.Property(e => e.SigDateWingSarc).HasColumnName("SigDateWingSARC");
        builder.Property(e => e.SigNameWingSarc).HasColumnName("SigNameWingSARC").HasMaxLength(200);
        builder.Property(e => e.SigTitleWingSarc).HasColumnName("SigTitleWingSARC").HasMaxLength(200);

        // Signature fields - SARC Admin
        builder.Property(e => e.SigDateSarcAdmin).HasColumnName("SigDateSARCAdmin");
        builder.Property(e => e.SigNameSarcAdmin).HasColumnName("SigNameSARCAdmin").HasMaxLength(200);
        builder.Property(e => e.SigTitleSarcAdmin).HasColumnName("SigTitleSARCAdmin").HasMaxLength(200);

        // Signature fields - Appellate Authority
        builder.Property(e => e.SigDateAppellateAuth).HasColumnName("SigDateAppellateAuth");
        builder.Property(e => e.SigNameAppellateAuth).HasColumnName("SigNameAppellateAuth").HasMaxLength(200);
        builder.Property(e => e.SigTitleAppellateAuth).HasColumnName("SigTitleAppellateAuth").HasMaxLength(200);

        // Signature fields - Board Medical
        builder.Property(e => e.SigDateBoardMedical).HasColumnName("SigDateBoardMedical");
        builder.Property(e => e.SigNameBoardMedical).HasColumnName("SigNameBoardMedical").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardMedical).HasColumnName("SigTitleBoardMedical").HasMaxLength(200);

        // Signature fields - Board Legal
        builder.Property(e => e.SigDateBoardLegal).HasColumnName("SigDateBoardLegal");
        builder.Property(e => e.SigNameBoardLegal).HasColumnName("SigNameBoardLegal").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardLegal).HasColumnName("SigTitleBoardLegal").HasMaxLength(200);

        // Signature fields - Board Admin
        builder.Property(e => e.SigDateBoardAdmin).HasColumnName("SigDateBoardAdmin");
        builder.Property(e => e.SigNameBoardAdmin).HasColumnName("SigNameBoardAdmin").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardAdmin).HasColumnName("SigTitleBoardAdmin").HasMaxLength(200);

        // Indexes for query performance
        builder.HasIndex(e => e.CaseId, "IX_Form348APSARC_CaseID").IsUnique();
        builder.HasIndex(e => e.InitialId, "IX_Form348APSARC_InitialID");
        builder.HasIndex(e => new { e.Status, e.Workflow }, "IX_Form348APSARC_Status_Workflow");
        builder.HasIndex(e => e.MemberSsn, "IX_Form348APSARC_MemberSSN");
        builder.HasIndex(e => e.MemberUnitId, "IX_Form348APSARC_MemberUnitID");
        builder.HasIndex(e => e.CreatedBy, "IX_Form348APSARC_CreatedBy");
        builder.HasIndex(e => e.CreatedDate, "IX_Form348APSARC_CreatedDate");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form348APSARC_ModifiedBy");
        builder.HasIndex(e => e.ModifiedDate, "IX_Form348APSARC_ModifiedDate");
        builder.HasIndex(e => e.MemberGrade, "IX_Form348APSARC_MemberGrade");
        builder.HasIndex(e => e.ReturnToGroup, "IX_Form348APSARC_ReturnToGroup");
        builder.HasIndex(e => e.ReturnByGroup, "IX_Form348APSARC_ReturnByGroup");
        builder.HasIndex(e => e.DocGroupId, "IX_Form348APSARC_DocGroupID");
        builder.HasIndex(e => e.CancelDate, "IX_Form348APSARC_CancelDate");
        builder.HasIndex(e => e.CancelReason, "IX_Form348APSARC_CancelReason");
    }
}
