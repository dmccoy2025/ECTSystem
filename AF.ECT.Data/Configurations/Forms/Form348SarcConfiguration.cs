using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348Sarc"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents SARC (Sexual Assault Response Coordinator) cases for Line of Duty investigations
/// involving sexual assault incidents.
/// </remarks>
public class Form348SarcConfiguration : IEntityTypeConfiguration<Form348Sarc>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348Sarc> builder)
    {
        // Table mapping
        builder.ToTable("Form348SARC", "dbo");

        // Primary key
        builder.HasKey(e => e.SarcId)
            .HasName("PK_Form348SARC");

        // Properties
        builder.Property(e => e.SarcId).HasColumnName("SarcID");
        builder.Property(e => e.CaseId).HasColumnName("CaseID").HasMaxLength(50).IsRequired();
        builder.Property(e => e.Status).HasColumnName("Status");
        builder.Property(e => e.Workflow).HasColumnName("Workflow");
        builder.Property(e => e.MemberName).HasColumnName("MemberName").HasMaxLength(200).IsRequired();
        builder.Property(e => e.MemberSsn).HasColumnName("MemberSSN").HasMaxLength(20).IsRequired();
        builder.Property(e => e.MemberGrade).HasColumnName("MemberGrade");
        builder.Property(e => e.MemberUnit).HasColumnName("MemberUnit").HasMaxLength(200).IsRequired();
        builder.Property(e => e.MemberUnitId).HasColumnName("MemberUnitID");
        builder.Property(e => e.MemberDob).HasColumnName("MemberDOB");
        builder.Property(e => e.MemberCompo).HasColumnName("MemberCompo").HasMaxLength(50).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.DocGroupId).HasColumnName("DocGroupID");
        builder.Property(e => e.IncidentDate).HasColumnName("IncidentDate");
        builder.Property(e => e.DutyStatus).HasColumnName("DutyStatus");
        builder.Property(e => e.DurationStartDate).HasColumnName("DurationStartDate");
        builder.Property(e => e.DurationEndDate).HasColumnName("DurationEndDate");
        builder.Property(e => e.IcdE9688).HasColumnName("ICD_E9688");
        builder.Property(e => e.IcdE9699).HasColumnName("ICD_E9699");
        builder.Property(e => e.IcdOther).HasColumnName("ICD_Other");
        builder.Property(e => e.InDutyStatus).HasColumnName("InDutyStatus");
        builder.Property(e => e.SigDateRslWingSarc).HasColumnName("SigDate_RSL_WingSARC");
        builder.Property(e => e.SigNameRslWingSarc).HasColumnName("SigName_RSL_WingSARC").HasMaxLength(200);
        builder.Property(e => e.SigTitleRslWingSarc).HasColumnName("SigTitle_RSL_WingSARC").HasMaxLength(200);
        builder.Property(e => e.SigDateSarcA1).HasColumnName("SigDate_SARC_A1");
        builder.Property(e => e.SigNameSarcA1).HasColumnName("SigName_SARC_A1").HasMaxLength(200);
        builder.Property(e => e.SigTitleSarcA1).HasColumnName("SigTitle_SARC_A1").HasMaxLength(200);
        builder.Property(e => e.SigDateBoardMedical).HasColumnName("SigDate_BoardMedical");
        builder.Property(e => e.SigNameBoardMedical).HasColumnName("SigName_BoardMedical").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardMedical).HasColumnName("SigTitle_BoardMedical").HasMaxLength(200);
        builder.Property(e => e.SigDateBoardJa).HasColumnName("SigDate_BoardJA");
        builder.Property(e => e.SigNameBoardJa).HasColumnName("SigName_BoardJA").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardJa).HasColumnName("SigTitle_BoardJA").HasMaxLength(200);
        builder.Property(e => e.SigDateBoardAdmin).HasColumnName("SigDate_BoardAdmin");
        builder.Property(e => e.SigNameBoardAdmin).HasColumnName("SigName_BoardAdmin").HasMaxLength(200);
        builder.Property(e => e.SigTitleBoardAdmin).HasColumnName("SigTitle_BoardAdmin").HasMaxLength(200);
        builder.Property(e => e.SigDateApproving).HasColumnName("SigDate_Approving");
        builder.Property(e => e.SigNameApproving).HasColumnName("SigName_Approving").HasMaxLength(200);
        builder.Property(e => e.SigTitleApproving).HasColumnName("SigTitle_Approving").HasMaxLength(200);
        builder.Property(e => e.RwoaReason).HasColumnName("RWOAReason");
        builder.Property(e => e.RwoaExplanation).HasColumnName("RWOAExplanation");
        builder.Property(e => e.RwoaDate).HasColumnName("RWOADate");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.CancelReason).HasColumnName("CancelReason");
        builder.Property(e => e.CancelExplanation).HasColumnName("CancelExplanation");
        builder.Property(e => e.CancelDate).HasColumnName("CancelDate");
        builder.Property(e => e.DefSexAssaultDbCaseNum).HasColumnName("DefSexAssaultDBCaseNum").HasMaxLength(50);
        builder.Property(e => e.ReturnToGroup).HasColumnName("ReturnToGroup");
        builder.Property(e => e.ReturnByGroup).HasColumnName("ReturnByGroup");
        builder.Property(e => e.ConsultationFromUsergroupId).HasColumnName("ConsultationFromUsergroupID");
        builder.Property(e => e.ReturnComment).HasColumnName("ReturnComment");
        builder.Property(e => e.IsPostProcessingComplete).HasColumnName("IsPostProcessingComplete");

        // Indexes for query performance
        builder.HasIndex(e => e.CaseId, "IX_Form348SARC_CaseID").IsUnique();
        builder.HasIndex(e => new { e.Status, e.Workflow }, "IX_Form348SARC_Status_Workflow");
        builder.HasIndex(e => e.MemberSsn, "IX_Form348SARC_MemberSSN");
        builder.HasIndex(e => e.IncidentDate, "IX_Form348SARC_IncidentDate");

        builder.HasIndex(e => e.CreatedBy, "IX_Form348SARC_CreatedBy");

        builder.HasIndex(e => e.ModifiedBy, "IX_Form348SARC_ModifiedBy");

        builder.HasIndex(e => e.CreatedDate, "IX_Form348SARC_CreatedDate");

        builder.HasIndex(e => e.ModifiedDate, "IX_Form348SARC_ModifiedDate");

        builder.HasIndex(e => e.DutyStatus, "IX_Form348SARC_DutyStatus");

        builder.HasIndex(e => e.DurationStartDate, "IX_Form348SARC_DurationStartDate");

        builder.HasIndex(e => e.DurationEndDate, "IX_Form348SARC_DurationEndDate");

        builder.HasIndex(e => e.CancelDate, "IX_Form348SARC_CancelDate");

        builder.HasIndex(e => e.CancelReason, "IX_Form348SARC_CancelReason");

        builder.HasIndex(e => e.MemberGrade, "IX_Form348SARC_MemberGrade");

        builder.HasIndex(e => e.MemberUnitId, "IX_Form348SARC_MemberUnitId");

        builder.HasIndex(e => e.ReturnToGroup, "IX_Form348SARC_ReturnToGroup");

        builder.HasIndex(e => e.ReturnByGroup, "IX_Form348SARC_ReturnByGroup");

        builder.HasIndex(e => e.DocGroupId, "IX_Form348SARC_DocGroupId");
    }
}
