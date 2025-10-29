using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Form348Rr"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Form_348_RR table,
/// which stores Reinvestigation Request (RR) data for Form 348 LOD cases.
/// Contains comprehensive workflow including initial/reinvestigation LOD IDs, case ID,
/// workflow and status tracking, RWOA (Returned Without Action) information,
/// signature blocks for multiple reviewing authorities (MPF, Wing JA, Wing CC, Board Admin/Medical/Legal/Tech,
/// Approval Authority, LOD PM, Board A1), approval flags and comments, member information (SSN, name, unit, grade),
/// return tracking, cancellation details, document group, and non-DB signature case flag.
/// Has relationships with Form348RrFinding (one-to-many) and CoreLkupGrade for member grade lookup.
/// Used for requesting reinvestigation of previously adjudicated LOD cases.
/// </remarks>
public class Form348RrConfiguration : IEntityTypeConfiguration<Form348Rr>
{
    /// <summary>
    /// Configures the entity of type <see cref="Form348Rr"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Form348Rr> builder)
    {
        // Table mapping
        builder.ToTable("Form_348_RR", "dbo");

        // Primary key
        builder.HasKey(e => e.RequestId)
            .HasName("PK_Form_348_RR");

        // Properties configuration
        builder.Property(e => e.RequestId)
            .HasColumnName("RequestID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.ReinvestigationLodId)
            .HasColumnName("ReinvestigationLODID");

        builder.Property(e => e.InitialLodId)
            .IsRequired()
            .HasColumnName("InitialLODID");

        builder.Property(e => e.CaseId)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("CaseID");

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasColumnName("Created_By");

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasColumnName("Created_Date")
            .HasColumnType("date");

        builder.Property(e => e.Workflow)
            .HasColumnName("Workflow");

        builder.Property(e => e.Status)
            .HasColumnName("Status");

        builder.Property(e => e.RwoaReason)
            .HasColumnName("RWOA_Reason");

        builder.Property(e => e.RwoaDate)
            .HasColumnName("RWOA_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.RwoaExplanation)
            .HasMaxLength(4000)
            .HasColumnName("RWOA_Explanation");

        // Signature blocks
        builder.Property(e => e.SigDateMpf)
            .HasColumnName("Sig_Date_MPF")
            .HasColumnType("datetime");

        builder.Property(e => e.SigNameMpf)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_MPF");

        builder.Property(e => e.SigTitleMpf)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_MPF");

        builder.Property(e => e.SigDateWingJa)
            .HasColumnName("Sig_Date_Wing_JA")
            .HasColumnType("datetime");

        builder.Property(e => e.SigNameWingJa)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_Wing_JA");

        builder.Property(e => e.SigTitleWingJa)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_Wing_JA");

        builder.Property(e => e.SigDateWingCc)
            .HasColumnName("Sig_Date_Wing_CC")
            .HasColumnType("datetime");

        builder.Property(e => e.SigNameWingCc)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_Wing_CC");

        builder.Property(e => e.SigTitleWingCc)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_Wing_CC");

        builder.Property(e => e.SigDateBoardAdmin)
            .HasColumnName("Sig_Date_Board_Admin")
            .HasColumnType("datetime");

        builder.Property(e => e.SigNameBoardAdmin)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_Board_Admin");

        builder.Property(e => e.SigTitleBoardAdmin)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_Board_Admin");

        builder.Property(e => e.SigDateBoardMedical)
            .HasColumnName("Sig_Date_Board_Medical")
            .HasColumnType("datetime");

        builder.Property(e => e.SigNameBoardMedical)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_Board_Medical");

        builder.Property(e => e.SigTitleBoardMedical)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_Board_Medical");

        builder.Property(e => e.SigDateBoardLegal)
            .HasColumnName("Sig_Date_Board_Legal")
            .HasColumnType("datetime");

        builder.Property(e => e.SigNameBoardLegal)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_Board_Legal");

        builder.Property(e => e.SigTitleBoardLegal)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_Board_Legal");

        builder.Property(e => e.SigDateApproval)
            .HasColumnName("Sig_Date_Approval")
            .HasColumnType("datetime");

        builder.Property(e => e.SigNameApproval)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_Approval");

        builder.Property(e => e.SigTitleApproval)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_Approval");

        builder.Property(e => e.SigDateBoardTechFinal)
            .HasColumnName("Sig_Date_Board_Tech_Final")
            .HasColumnType("datetime");

        builder.Property(e => e.SigNameBoardTechFinal)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_Board_Tech_Final");

        builder.Property(e => e.SigTitleBoardTechFinal)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_Board_Tech_Final");

        builder.Property(e => e.SigNameLodPm)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_LOD_PM");

        builder.Property(e => e.SigDateLodPm)
            .HasColumnName("Sig_Date_LOD_PM")
            .HasColumnType("datetime");

        builder.Property(e => e.SigTitleLodPm)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_LOD_PM");

        builder.Property(e => e.SigNameBoardA1)
            .HasMaxLength(200)
            .HasColumnName("Sig_Name_Board_A1");

        builder.Property(e => e.SigDateBoardA1)
            .HasColumnName("Sig_Date_Board_A1")
            .HasColumnType("datetime");

        builder.Property(e => e.SigTitleBoardA1)
            .HasMaxLength(200)
            .HasColumnName("Sig_Title_Board_A1");

        // Approval flags and comments
        builder.Property(e => e.WingCcApproved)
            .HasColumnName("Wing_CC_Approved");

        builder.Property(e => e.BoardMedicalApproved)
            .HasColumnName("Board_Medical_Approved");

        builder.Property(e => e.BoardLegalApproved)
            .HasColumnName("Board_Legal_Approved");

        builder.Property(e => e.AaFinalApproved)
            .HasColumnName("AA_Final_Approved");

        builder.Property(e => e.WingJaApproved)
            .HasColumnName("Wing_JA_Approved");

        builder.Property(e => e.BoardTechApproval1)
            .HasColumnName("Board_Tech_Approval_1");

        builder.Property(e => e.BoardTechApproval2)
            .HasColumnName("Board_Tech_Approval_2");

        builder.Property(e => e.BoardA1Approved)
            .HasColumnName("Board_A1_Approved");

        builder.Property(e => e.ReturnComment)
            .HasMaxLength(4000)
            .HasColumnName("Return_Comment");

        builder.Property(e => e.WingJaApprovalComment)
            .HasMaxLength(4000)
            .HasColumnName("Wing_JA_Approval_Comment");

        builder.Property(e => e.WingCcApprovalComment)
            .HasMaxLength(4000)
            .HasColumnName("Wing_CC_Approval_Comment");

        builder.Property(e => e.BoardTechApproval1Comment)
            .HasMaxLength(4000)
            .HasColumnName("Board_Tech_Approval_1_Comment");

        builder.Property(e => e.BoardMedicalApprovalComment)
            .HasMaxLength(4000)
            .HasColumnName("Board_Medical_Approval_Comment");

        builder.Property(e => e.BoardLegalApprovalComment)
            .HasMaxLength(4000)
            .HasColumnName("Board_Legal_Approval_Comment");

        builder.Property(e => e.AaFinalApprovalComment)
            .HasMaxLength(4000)
            .HasColumnName("AA_Final_Approval_Comment");

        builder.Property(e => e.BoardA1ApprovalComment)
            .HasMaxLength(4000)
            .HasColumnName("Board_A1_Approval_Comment");

        // Member information
        builder.Property(e => e.MemberSsn)
            .HasMaxLength(20)
            .HasColumnName("Member_SSN");

        builder.Property(e => e.MemberName)
            .HasMaxLength(200)
            .HasColumnName("Member_Name");

        builder.Property(e => e.MemberUnit)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Member_Unit");

        builder.Property(e => e.MemberUnitId)
            .IsRequired()
            .HasColumnName("Member_Unit_ID");

        builder.Property(e => e.MemberCompo)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("Member_Compo");

        builder.Property(e => e.MemberGrade)
            .IsRequired()
            .HasColumnName("Member_Grade");

        // Additional fields
        builder.Property(e => e.ModifiedBy)
            .HasColumnName("Modified_By");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("Modified_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.DocGroupId)
            .HasColumnName("Doc_Group_ID");

        builder.Property(e => e.CancelReason)
            .HasColumnName("Cancel_Reason");

        builder.Property(e => e.CancelExplanation)
            .HasMaxLength(4000)
            .HasColumnName("Cancel_Explanation");

        builder.Property(e => e.CancelDate)
            .HasColumnName("Cancel_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.ReturnToGroup)
            .HasColumnName("Return_To_Group");

        builder.Property(e => e.ReturnByGroup)
            .HasColumnName("Return_By_Group");

        builder.Property(e => e.IsNonDbsignCase)
            .IsRequired()
            .HasColumnName("IsNonDBSignCase")
            .HasDefaultValue(false);

        // Relationships
        builder.HasMany(e => e.Form348RrFindings)
            .WithOne(e => e.Request)
            .HasForeignKey(e => e.RequestId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Form_348_RR_Finding");

        builder.HasOne(e => e.MemberGradeNavigation)
            .WithMany()
            .HasForeignKey(e => e.MemberGrade)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Form_348_RR_Grade");

        // Indexes
        builder.HasIndex(e => e.InitialLodId, "IX_form_348_rr_initial_lod_id");

        builder.HasIndex(e => e.ReinvestigationLodId, "IX_form_348_rr_reinvestigation_lod_id");

        builder.HasIndex(e => e.CaseId, "IX_form_348_rr_case_id");

        builder.HasIndex(e => e.MemberSsn, "IX_form_348_rr_member_ssn");

        builder.HasIndex(e => e.Workflow, "IX_form_348_rr_workflow");

        builder.HasIndex(e => e.Status, "IX_form_348_rr_status");

        builder.HasIndex(e => e.CreatedDate, "IX_form_348_rr_created_date");

        builder.HasIndex(e => e.MemberGrade, "IX_form_348_rr_member_grade");
        
        builder.HasIndex(e => e.ModifiedBy, "IX_form_348_rr_modified_by");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_form_348_rr_modified_date");
        
        builder.HasIndex(e => e.CreatedBy, "IX_form_348_rr_created_by");
        
        builder.HasIndex(e => e.ReturnToGroup, "IX_form_348_rr_return_to_group");
        
        builder.HasIndex(e => e.ReturnByGroup, "IX_form_348_rr_return_by_group");
        
        builder.HasIndex(e => e.DocGroupId, "IX_form_348_rr_doc_group_id");
        
        builder.HasIndex(e => e.CancelDate, "IX_form_348_rr_cancel_date");
        
        builder.HasIndex(e => e.CancelReason, "IX_form_348_rr_cancel_reason");
        
        builder.HasIndex(e => new { e.Workflow, e.Status }, "IX_form_348_rr_workflow_status");
    }
}
