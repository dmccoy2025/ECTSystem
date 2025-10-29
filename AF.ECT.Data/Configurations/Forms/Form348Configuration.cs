using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Form348"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema and relationships for the form348 table,
/// which represents DA Form 348 (Line of Duty) cases in the ECT system. The Form 348
/// is the official Army document used to determine whether a service member's injury,
/// disease, or death occurred in the line of duty and whether the member was at fault.
/// This entity manages both formal and informal investigations across all components
/// (Active, Reserve, Guard) and supports the complete lifecycle from initiation through
/// final adjudication and appeals.
/// </remarks>
public class Form348Configuration : IEntityTypeConfiguration<Form348>
{
    /// <summary>
    /// Configures the entity of type <see cref="Form348"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Form348> builder)
    {
        // Table mapping
        builder.ToTable("form348", "dbo");

        // Primary key
        builder.HasKey(e => e.LodId)
            .HasName("PK_form348");

        // Properties configuration
        builder.Property(e => e.LodId)
            .HasColumnName("lod_id");

        builder.Property(e => e.CaseId)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("case_id");

        builder.Property(e => e.Status)
            .HasColumnName("status");

        builder.Property(e => e.Workflow)
            .HasColumnName("workflow");

        builder.Property(e => e.MemberName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("member_name");

        builder.Property(e => e.MemberSsn)
            .IsRequired()
            .HasMaxLength(11)
            .HasColumnName("member_ssn");

        builder.Property(e => e.MemberGrade)
            .HasColumnName("member_grade");

        builder.Property(e => e.MemberUnit)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("member_unit");

        builder.Property(e => e.MemberUnitId)
            .HasColumnName("member_unit_id");

        builder.Property(e => e.MemberDob)
            .HasColumnType("date")
            .HasColumnName("member_dob");

        builder.Property(e => e.MemberCompo)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("member_compo");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasColumnName("created_date")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("modified_by");

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime")
            .HasColumnName("modified_date")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.DocGroupId)
            .HasColumnName("doc_group_id");

        builder.Property(e => e.FormalInv)
            .HasColumnName("formal_inv");

        builder.Property(e => e.MedTechComments)
            .HasColumnName("med_tech_comments");

        builder.Property(e => e.AppAuthUserId)
            .HasColumnName("app_auth_user_id");

        builder.Property(e => e.Deleted)
            .HasColumnName("deleted");

        builder.Property(e => e.RwoaReason)
            .HasColumnName("rwoa_reason");

        builder.Property(e => e.RwoaExplantion)
            .HasColumnName("rwoa_explantion");

        builder.Property(e => e.RwoaDate)
            .HasColumnType("date")
            .HasColumnName("rwoa_date");

        builder.Property(e => e.RwoaDirectReply)
            .HasColumnName("rwoa_direct_reply");

        builder.Property(e => e.FinalDecision)
            .HasMaxLength(500)
            .HasColumnName("final_decision");

        builder.Property(e => e.BoardForGeneralYn)
            .HasMaxLength(1)
            .HasColumnName("board_for_general_yn");

        builder.Property(e => e.FinalFindings)
            .HasColumnName("final_findings");

        builder.Property(e => e.IoCompletionDate)
            .HasColumnType("date")
            .HasColumnName("io_completion_date");

        builder.Property(e => e.IoInstructions)
            .HasColumnName("io_instructions");

        builder.Property(e => e.IoPocInfo)
            .HasMaxLength(500)
            .HasColumnName("io_poc_info");

        builder.Property(e => e.IoSsn)
            .HasMaxLength(11)
            .HasColumnName("io_ssn");

        builder.Property(e => e.IoUid)
            .HasColumnName("io_uid");

        builder.Property(e => e.Sarc)
            .HasColumnName("sarc");

        builder.Property(e => e.Restricted)
            .HasColumnName("restricted");

        builder.Property(e => e.AaPtype)
            .HasColumnName("aa_ptype");

        builder.Property(e => e.ReturnComment)
            .HasColumnName("return_comment");

        builder.Property(e => e.MemberNotified)
            .HasColumnName("member_notified");

        builder.Property(e => e.CompletedByUnit)
            .HasColumnName("completed_by_unit");

        builder.Property(e => e.ParentId)
            .HasColumnName("parent_id");

        builder.Property(e => e.IsAttachPas)
            .HasColumnName("is_attach_pas");

        builder.Property(e => e.MemberAttachedUnitId)
            .HasColumnName("member_attached_unit_id");

        builder.Property(e => e.ReturnToGroup)
            .HasColumnName("return_to_group");

        builder.Property(e => e.ReturnByGroup)
            .HasColumnName("return_by_group");

        builder.Property(e => e.BoardTechComments)
            .HasColumnName("board_tech_comments");

        builder.Property(e => e.IsFormalCancelRecommended)
            .HasColumnName("is_formal_cancel_recommended");

        builder.Property(e => e.AppointingCancelReasonId)
            .HasColumnName("appointing_cancel_reason_id");

        builder.Property(e => e.AppointingCancelExplanation)
            .HasColumnName("appointing_cancel_explanation");

        builder.Property(e => e.ApprovingCancelReasonId)
            .HasColumnName("approving_cancel_reason_id");

        builder.Property(e => e.ApprovingCancelExplanation)
            .HasColumnName("approving_cancel_explanation");

        builder.Property(e => e.HasCredibleService)
            .HasColumnName("has_credible_service");

        builder.Property(e => e.WasMemberOnOrders)
            .HasColumnName("was_member_on_orders");

        builder.Property(e => e.ApprovingAuthorityUserId)
            .HasColumnName("approving_authority_user_id");

        builder.Property(e => e.Tmn)
            .HasColumnName("tmn");

        builder.Property(e => e.Tms)
            .HasColumnName("tms");

        builder.Property(e => e.AppointingUnit)
            .HasMaxLength(100)
            .HasColumnName("appointing_unit");

        builder.Property(e => e.WingccNilodByreasonof)
            .HasColumnName("wingcc_nilod_byreasonof");

        builder.Property(e => e.IsPostProcessingComplete)
            .HasColumnName("is_post_processing_complete");

        builder.Property(e => e.LodPm)
            .HasMaxLength(100)
            .HasColumnName("lod_pm");

        // Signature fields
        builder.Property(e => e.SigDateUnitCommander)
            .HasColumnType("date")
            .HasColumnName("sig_date_unit_commander");

        builder.Property(e => e.SigNameUnitCommander)
            .HasMaxLength(100)
            .HasColumnName("sig_name_unit_commander");

        builder.Property(e => e.SigTitleUnitCommander)
            .HasMaxLength(100)
            .HasColumnName("sig_title_unit_commander");

        builder.Property(e => e.SigDateMedOfficer)
            .HasColumnType("date")
            .HasColumnName("sig_date_med_officer");

        builder.Property(e => e.SigNameMedOfficer)
            .HasMaxLength(100)
            .HasColumnName("sig_name_med_officer");

        builder.Property(e => e.SigTitleMedOfficer)
            .HasMaxLength(100)
            .HasColumnName("sig_title_med_officer");

        builder.Property(e => e.SigDateLegal)
            .HasColumnType("date")
            .HasColumnName("sig_date_legal");

        builder.Property(e => e.SigNameLegal)
            .HasMaxLength(100)
            .HasColumnName("sig_name_legal");

        builder.Property(e => e.SigTitleLegal)
            .HasMaxLength(100)
            .HasColumnName("sig_title_legal");

        builder.Property(e => e.SigDateAppointing)
            .HasColumnType("date")
            .HasColumnName("sig_date_appointing");

        builder.Property(e => e.SigNameAppointing)
            .HasMaxLength(100)
            .HasColumnName("sig_name_appointing");

        builder.Property(e => e.SigTitleAppointing)
            .HasMaxLength(100)
            .HasColumnName("sig_title_appointing");

        builder.Property(e => e.SigDateBoardLegal)
            .HasColumnType("date")
            .HasColumnName("sig_date_board_legal");

        builder.Property(e => e.SigNameBoardLegal)
            .HasMaxLength(100)
            .HasColumnName("sig_name_board_legal");

        builder.Property(e => e.SigTitleBoardLegal)
            .HasMaxLength(100)
            .HasColumnName("sig_title_board_legal");

        builder.Property(e => e.SigDateBoardMedical)
            .HasColumnType("date")
            .HasColumnName("sig_date_board_medical");

        builder.Property(e => e.SigNameBoardMedical)
            .HasMaxLength(100)
            .HasColumnName("sig_name_board_medical");

        builder.Property(e => e.SigTitleBoardMedical)
            .HasMaxLength(100)
            .HasColumnName("sig_title_board_medical");

        builder.Property(e => e.SigDateBoardAdmin)
            .HasColumnType("date")
            .HasColumnName("sig_date_board_admin");

        builder.Property(e => e.SigNameBoardAdmin)
            .HasMaxLength(100)
            .HasColumnName("sig_name_board_admin");

        builder.Property(e => e.SigTitleBoardAdmin)
            .HasMaxLength(100)
            .HasColumnName("sig_title_board_admin");

        builder.Property(e => e.SigDateBoardA1)
            .HasColumnType("date")
            .HasColumnName("sig_date_board_a1");

        builder.Property(e => e.SigNameBoardA1)
            .HasMaxLength(100)
            .HasColumnName("sig_name_board_a1");

        builder.Property(e => e.SigTitleBoardA1)
            .HasMaxLength(100)
            .HasColumnName("sig_title_board_a1");

        builder.Property(e => e.SigDateApproval)
            .HasColumnType("date")
            .HasColumnName("sig_date_approval");

        builder.Property(e => e.SigNameApproval)
            .HasMaxLength(100)
            .HasColumnName("sig_name_approval");

        builder.Property(e => e.SigTitleApproval)
            .HasMaxLength(100)
            .HasColumnName("sig_title_approval");

        builder.Property(e => e.SigDateMpf)
            .HasColumnType("date")
            .HasColumnName("sig_date_mpf");

        builder.Property(e => e.SigNameMpf)
            .HasMaxLength(100)
            .HasColumnName("sig_name_mpf");

        builder.Property(e => e.SigTitleMpf)
            .HasMaxLength(100)
            .HasColumnName("sig_title_mpf");

        builder.Property(e => e.SigDateLodPm)
            .HasColumnType("date")
            .HasColumnName("sig_date_lod_pm");

        builder.Property(e => e.SigNameLodPm)
            .HasMaxLength(100)
            .HasColumnName("sig_name_lod_pm");

        builder.Property(e => e.SigTitleLodPm)
            .HasMaxLength(100)
            .HasColumnName("sig_title_lod_pm");

        // Formal workflow signatures
        builder.Property(e => e.SigDateFormalApproval)
            .HasColumnType("date")
            .HasColumnName("sig_date_formal_approval");

        builder.Property(e => e.SigNameFormalApproval)
            .HasMaxLength(100)
            .HasColumnName("sig_name_formal_approval");

        builder.Property(e => e.SigTitleFormalApproval)
            .HasMaxLength(100)
            .HasColumnName("sig_title_formal_approval");

        builder.Property(e => e.SigDateFormalLegal)
            .HasColumnType("date")
            .HasColumnName("sig_date_formal_legal");

        builder.Property(e => e.SigNameFormalLegal)
            .HasMaxLength(100)
            .HasColumnName("sig_name_formal_legal");

        builder.Property(e => e.SigTitleFormalLegal)
            .HasMaxLength(100)
            .HasColumnName("sig_title_formal_legal");

        builder.Property(e => e.SigDateFormalAppointing)
            .HasColumnType("date")
            .HasColumnName("sig_date_formal_appointing");

        builder.Property(e => e.SigNameFormalAppointing)
            .HasMaxLength(100)
            .HasColumnName("sig_name_formal_appointing");

        builder.Property(e => e.SigTitleFormalAppointing)
            .HasMaxLength(100)
            .HasColumnName("sig_title_formal_appointing");

        builder.Property(e => e.SigDateFormalBoardMedical)
            .HasColumnType("date")
            .HasColumnName("sig_date_formal_board_medical");

        builder.Property(e => e.SigNameFormalBoardMedical)
            .HasMaxLength(100)
            .HasColumnName("sig_name_formal_board_medical");

        builder.Property(e => e.SigTitleFormalBoardMedical)
            .HasMaxLength(100)
            .HasColumnName("sig_title_formal_board_medical");

        builder.Property(e => e.SigDateFormalBoardLegal)
            .HasColumnType("date")
            .HasColumnName("sig_date_formal_board_legal");

        builder.Property(e => e.SigNameFormalBoardLegal)
            .HasMaxLength(100)
            .HasColumnName("sig_name_formal_board_legal");

        builder.Property(e => e.SigTitleFormalBoardLegal)
            .HasMaxLength(100)
            .HasColumnName("sig_title_formal_board_legal");

        builder.Property(e => e.SigDateFormalBoardAdmin)
            .HasColumnType("date")
            .HasColumnName("sig_date_formal_board_admin");

        builder.Property(e => e.SigNameFormalBoardAdmin)
            .HasMaxLength(100)
            .HasColumnName("sig_name_formal_board_admin");

        builder.Property(e => e.SigTitleFormalBoardAdmin)
            .HasMaxLength(100)
            .HasColumnName("sig_title_formal_board_admin");

        builder.Property(e => e.SigDateFormalBoardA1)
            .HasColumnType("date")
            .HasColumnName("sig_date_formal_board_a1");

        builder.Property(e => e.SigNameFormalBoardA1)
            .HasMaxLength(100)
            .HasColumnName("sig_name_formal_board_a1");

        builder.Property(e => e.SigTitleFormalBoardA1)
            .HasMaxLength(100)
            .HasColumnName("sig_title_formal_board_a1");

        builder.Property(e => e.ToUnit)
            .HasMaxLength(100)
            .HasColumnName("to_unit");

        builder.Property(e => e.FromUnit)
            .HasMaxLength(100)
            .HasColumnName("from_unit");

        // Relationships
        builder.HasOne(d => d.AaPtypeNavigation)
            .WithMany(p => p.Form348s)
            .HasForeignKey(d => d.AaPtype)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_form348_core_lkup_personnel_type");

        builder.HasOne(d => d.AppAuthUser)
            .WithMany(p => p.Form348AppAuthUsers)
            .HasForeignKey(d => d.AppAuthUserId)
            .HasConstraintName("FK_form348_core_user_app_auth");

        builder.HasOne(d => d.AppointingCancelReason)
            .WithMany(p => p.Form348AppointingCancelReasons)
            .HasForeignKey(d => d.AppointingCancelReasonId)
            .HasConstraintName("FK_form348_core_lkup_cancel_reason_appointing");

        builder.HasOne(d => d.ApprovingCancelReason)
            .WithMany(p => p.Form348ApprovingCancelReasons)
            .HasForeignKey(d => d.ApprovingCancelReasonId)
            .HasConstraintName("FK_form348_core_lkup_cancel_reason_approving");

        builder.HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.Form348CreatedByNavigations)
            .HasForeignKey(d => d.CreatedBy)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_form348_core_user_created");

        builder.HasOne(d => d.MemberGradeNavigation)
            .WithMany(p => p.Form348s)
            .HasForeignKey(d => d.MemberGrade)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_form348_core_lkup_grade");

        builder.HasOne(d => d.ModifiedByNavigation)
            .WithMany(p => p.Form348ModifiedByNavigations)
            .HasForeignKey(d => d.ModifiedBy)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_form348_core_user_modified");

        builder.HasOne(d => d.RwoaReasonNavigation)
            .WithMany(p => p.Form348s)
            .HasForeignKey(d => d.RwoaReason)
            .HasConstraintName("FK_form348_core_lkup_rwoa_reason");

        builder.HasOne(d => d.StatusNavigation)
            .WithMany(p => p.Form348s)
            .HasForeignKey(d => d.Status)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_form348_core_work_status");

        builder.HasOne(d => d.WingccNilodByreasonofNavigation)
            .WithMany(p => p.Form348s)
            .HasForeignKey(d => d.WingccNilodByreasonof)
            .HasConstraintName("FK_form348_core_lkup_finding_by_reason_of");

        builder.HasOne(d => d.WorkflowNavigation)
            .WithMany(p => p.Form348s)
            .HasForeignKey(d => d.Workflow)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_form348_core_workflow");

        // Indexes
        builder.HasIndex(e => e.CaseId)
            .HasDatabaseName("IX_form348_case_id")
            .IsUnique();

        builder.HasIndex(e => e.Status, "IX_form348_status");

        builder.HasIndex(e => e.Workflow, "IX_form348_workflow");

        builder.HasIndex(e => e.MemberSsn, "IX_form348_member_ssn");

        builder.HasIndex(e => e.MemberUnitId, "IX_form348_member_unit_id");

        builder.HasIndex(e => e.CreatedDate, "IX_form348_created_date");

        builder.HasIndex(e => e.Deleted, "IX_form348_deleted");

        builder.HasIndex(e => new { e.Status, e.Deleted }, "IX_form348_status_deleted");

        builder.HasIndex(e => e.FormalInv, "IX_form348_formal_inv");

        builder.HasIndex(e => e.IoCompletionDate, "IX_form348_io_completion_date");

        builder.HasIndex(e => e.ModifiedDate, "IX_form348_modified_date");

        builder.HasIndex(e => new { e.Workflow, e.Status, e.Deleted }, "IX_form348_workflow_status_deleted");

        builder.HasIndex(e => e.MemberAttachedUnitId, "IX_form348_member_attached_unit_id");

        builder.HasIndex(e => e.IoUid, "IX_form348_io_uid");

        builder.HasIndex(e => e.ApprovingAuthorityUserId, "IX_form348_approving_authority_user_id");

        builder.HasIndex(e => e.ReturnToGroup, "IX_form348_return_to_group");

        builder.HasIndex(e => e.ReturnByGroup, "IX_form348_return_by_group");

        builder.HasIndex(e => e.DocGroupId, "IX_form348_doc_group_id");

        builder.HasIndex(e => e.ParentId, "IX_form348_parent_id");
    }
}
