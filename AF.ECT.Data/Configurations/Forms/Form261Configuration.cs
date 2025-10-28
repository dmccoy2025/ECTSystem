using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form261"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents AF Form 261 - Report of Investigation (LOD) for Line of Duty determinations.
/// </remarks>
public class Form261Configuration : IEntityTypeConfiguration<Form261>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form261> builder)
    {
        // Table mapping
        builder.ToTable("Form261", "dbo");

        // Primary key
        builder.HasKey(e => e.LodId)
            .HasName("PK_Form261");

        // Properties
        builder.Property(e => e.LodId).HasColumnName("LodID");
        builder.Property(e => e.ReportDate).HasColumnName("ReportDate");
        builder.Property(e => e.InvestigationOf).HasColumnName("InvestigationOf");
        builder.Property(e => e.Status).HasColumnName("Status");
        builder.Property(e => e.InactiveDutyTraining).HasColumnName("InactiveDutyTraining").HasMaxLength(100);
        builder.Property(e => e.DurationStartDate).HasColumnName("DurationStartDate");
        builder.Property(e => e.DurationFinishDate).HasColumnName("DurationFinishDate");
        builder.Property(e => e.IoUserId).HasColumnName("IOUserID");
        builder.Property(e => e.OtherPersonnels).HasColumnName("OtherPersonnels");
        builder.Property(e => e.FinalApprovalFindings).HasColumnName("FinalApprovalFindings");
        builder.Property(e => e.FindingsDate).HasColumnName("FindingsDate");
        builder.Property(e => e.Place).HasColumnName("Place");
        builder.Property(e => e.HowSustained).HasColumnName("HowSustained");
        builder.Property(e => e.MedicalDiagnosis).HasColumnName("MedicalDiagnosis");
        builder.Property(e => e.PresentForDuty).HasColumnName("PresentForDuty");
        builder.Property(e => e.AbsentWithAuthority).HasColumnName("AbsentWithAuthority");
        builder.Property(e => e.IntentionalMisconduct).HasColumnName("IntentionalMisconduct");
        builder.Property(e => e.MentallySound).HasColumnName("MentallySound");
        builder.Property(e => e.Remarks).HasColumnName("Remarks");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.SigDateIo).HasColumnName("SigDateIO");
        builder.Property(e => e.SigInfoIo).HasColumnName("SigInfoIO");
        builder.Property(e => e.SigDateAppointing).HasColumnName("SigDateAppointing");
        builder.Property(e => e.SigInfoAppointing).HasColumnName("SigInfoAppointing");

        // Indexes for query performance
        builder.HasIndex(e => e.Status, "IX_Form261_Status");
        builder.HasIndex(e => e.ReportDate, "IX_Form261_ReportDate");
        builder.HasIndex(e => e.IoUserId, "IX_Form261_IOUserID");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form261_ModifiedBy");
        builder.HasIndex(e => e.ModifiedDate, "IX_Form261_ModifiedDate");
        builder.HasIndex(e => e.DurationStartDate, "IX_Form261_DurationStartDate");
        builder.HasIndex(e => e.DurationFinishDate, "IX_Form261_DurationFinishDate");
        builder.HasIndex(e => e.FindingsDate, "IX_Form261_FindingsDate");
        builder.HasIndex(e => new { e.Status, e.ReportDate }, "IX_Form261_Status_ReportDate");
    }
}
