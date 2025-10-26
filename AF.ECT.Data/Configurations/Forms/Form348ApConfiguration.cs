using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348Ap"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents Form 348 Appeal cases with approval workflow, board signatures, and case status tracking.
/// </remarks>
public class Form348ApConfiguration : IEntityTypeConfiguration<Form348Ap>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348Ap> builder)
    {
        // Table mapping
        builder.ToTable("Form348AP", "dbo");

        // Primary key
        builder.HasKey(e => e.AppealId)
            .HasName("PK_Form348AP");

        // Properties
        builder.Property(e => e.AppealId).HasColumnName("AppealID");
        builder.Property(e => e.InitialLodId).HasColumnName("InitialLodID");
        builder.Property(e => e.CaseId).HasColumnName("CaseID").HasMaxLength(50).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.Workflow).HasColumnName("Workflow");
        builder.Property(e => e.Status).HasColumnName("Status");
        builder.Property(e => e.CancelReason).HasColumnName("CancelReason");
        builder.Property(e => e.CancelExplanation).HasColumnName("CancelExplanation");
        builder.Property(e => e.CancelDate).HasColumnName("CancelDate");
        builder.Property(e => e.RwoaReason).HasColumnName("RwoaReason");
        builder.Property(e => e.RwoaDate).HasColumnName("RwoaDate");
        builder.Property(e => e.RwoaExplanation).HasColumnName("RwoaExplanation");
        builder.Property(e => e.ReturnToGroup).HasColumnName("ReturnToGroup");
        builder.Property(e => e.ReturnByGroup).HasColumnName("ReturnByGroup");
        builder.Property(e => e.MemberSsn).HasColumnName("MemberSSN").HasMaxLength(20);
        builder.Property(e => e.MemberName).HasColumnName("MemberName").HasMaxLength(200);
        builder.Property(e => e.DocGroupId).HasColumnName("DocGroupID");
        builder.Property(e => e.MemberNotified).HasColumnName("MemberNotified");
        builder.Property(e => e.IsPostProcessingComplete).HasColumnName("IsPostProcessingComplete");
        builder.Property(e => e.MemberUnit).HasColumnName("MemberUnit").HasMaxLength(255).IsRequired();
        builder.Property(e => e.MemberUnitId).HasColumnName("MemberUnitID");
        builder.Property(e => e.MemberCompo).HasColumnName("MemberCompo").HasMaxLength(50).IsRequired();
        builder.Property(e => e.MemberGrade).HasColumnName("MemberGrade");
        builder.Property(e => e.IsNonDbsignCase).HasColumnName("IsNonDbsignCase");

        // Signature fields - PM
        builder.Property(e => e.SigDatePm).HasColumnName("SigDatePM");
        builder.Property(e => e.SigNamePm).HasColumnName("SigNamePM").HasMaxLength(200);
        builder.Property(e => e.SigTitlePm).HasColumnName("SigTitlePM").HasMaxLength(200);
        builder.Property(e => e.LodPmApproved).HasColumnName("LodPMApproved");
        builder.Property(e => e.LodPmApprovalComment).HasColumnName("LodPMApprovalComment");

        // Signature fields - Board Tech
        builder.Property(e => e.SigDateBoardTech).HasColumnName("SigDateBoardTech");
        builder.Property(e => e.SigNameBoardTech).HasColumnName("SigNameBoardTech").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardTech).HasColumnName("SigTitleBoardTech").HasMaxLength(200);
        builder.Property(e => e.BoardTechApproved).HasColumnName("BoardTechApproved");
        builder.Property(e => e.BoardTechApprovalComment).HasColumnName("BoardTechApprovalComment");

        // Signature fields - Board Medical
        builder.Property(e => e.SigDateBoardMedical).HasColumnName("SigDateBoardMedical");
        builder.Property(e => e.SigNameBoardMedical).HasColumnName("SigNameBoardMedical").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardMedical).HasColumnName("SigTitleBoardMedical").HasMaxLength(200);
        builder.Property(e => e.BoardMedicalApproved).HasColumnName("BoardMedicalApproved");
        builder.Property(e => e.BoardMedicalApprovalComment).HasColumnName("BoardMedicalApprovalComment");

        // Signature fields - Board Legal
        builder.Property(e => e.SigDateBoardLegal).HasColumnName("SigDateBoardLegal");
        builder.Property(e => e.SigNameBoardLegal).HasColumnName("SigNameBoardLegal").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardLegal).HasColumnName("SigTitleBoardLegal").HasMaxLength(200);
        builder.Property(e => e.BoardLegalApproved).HasColumnName("BoardLegalApproved");
        builder.Property(e => e.BoardLegalApprovalComment).HasColumnName("BoardLegalApprovalComment");

        // Signature fields - Board Admin
        builder.Property(e => e.SigDateBoardAdmin).HasColumnName("SigDateBoardAdmin");
        builder.Property(e => e.SigNameBoardAdmin).HasColumnName("SigNameBoardAdmin").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardAdmin).HasColumnName("SigTitleBoardAdmin").HasMaxLength(200);
        builder.Property(e => e.BoardAdminApproved).HasColumnName("BoardAdminApproved");
        builder.Property(e => e.BoardAdminApprovalComment).HasColumnName("BoardAdminApprovalComment");

        // Signature fields - Approving Authority
        builder.Property(e => e.SigDateApprovingAuth).HasColumnName("SigDateApprovingAuth");
        builder.Property(e => e.SigNameApprovingAuth).HasColumnName("SigNameApprovingAuth").HasMaxLength(200);
        builder.Property(e => e.SigTitleApprovingAuth).HasColumnName("SigTitleApprovingAuth").HasMaxLength(200);
        builder.Property(e => e.ApprovalAuthApproved).HasColumnName("ApprovalAuthApproved");
        builder.Property(e => e.ApprovalAuthApprovalComment).HasColumnName("ApprovalAuthApprovalComment");

        // Signature fields - Appellate Authority
        builder.Property(e => e.SigDateAppellateAuth).HasColumnName("SigDateAppellateAuth");
        builder.Property(e => e.SigNameAppellateAuth).HasColumnName("SigNameAppellateAuth").HasMaxLength(200);
        builder.Property(e => e.SigTitleAppellateAuth).HasColumnName("SigTitleAppellateAuth").HasMaxLength(200);
        builder.Property(e => e.AppellateAuthApproved).HasColumnName("AppellateAuthApproved");
        builder.Property(e => e.AppellateAuthApprovalComment).HasColumnName("AppellateAuthApprovalComment");

        builder.Property(e => e.ReturnComment).HasColumnName("ReturnComment");

        // Indexes for query performance
        builder.HasIndex(e => e.CaseId, "IX_Form348AP_CaseID").IsUnique();
        builder.HasIndex(e => e.InitialLodId, "IX_Form348AP_InitialLodID");
        builder.HasIndex(e => new { e.Status, e.Workflow }, "IX_Form348AP_Status_Workflow");
        builder.HasIndex(e => e.MemberSsn, "IX_Form348AP_MemberSSN");
        builder.HasIndex(e => e.MemberUnitId, "IX_Form348AP_MemberUnitID");
    }
}
