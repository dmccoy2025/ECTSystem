using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348Unit"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents unit commander's assessment and supporting documentation for LOD investigations.
/// </remarks>
public class Form348UnitConfiguration : IEntityTypeConfiguration<Form348Unit>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348Unit> builder)
    {
        // Table mapping
        builder.ToTable("Form348Unit", "dbo");

        // Primary key
        builder.HasKey(e => e.Lodid)
            .HasName("PK_Form348Unit");

        // Properties
        builder.Property(e => e.Lodid).HasColumnName("LODID");
        builder.Property(e => e.CmdrCircDetails).HasColumnName("CmdrCircDetails");
        builder.Property(e => e.CmdrDutyDetermination).HasColumnName("CmdrDutyDetermination").HasMaxLength(50);
        builder.Property(e => e.CmdrDutyFrom).HasColumnName("CmdrDutyFrom");
        builder.Property(e => e.CmdrDutyOthers).HasColumnName("CmdrDutyOthers");
        builder.Property(e => e.CmdrDutyTo).HasColumnName("CmdrDutyTo");
        builder.Property(e => e.CmdrActivatedYn).HasColumnName("CmdrActivatedYN").HasMaxLength(1);
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.SourceInformation).HasColumnName("SourceInformation");
        builder.Property(e => e.Witnesses).HasColumnName("Witnesses");
        builder.Property(e => e.SourceInformationSpecify).HasColumnName("SourceInformationSpecify");
        builder.Property(e => e.MemberOccurrence).HasColumnName("MemberOccurrence");
        builder.Property(e => e.AbsentFrom).HasColumnName("AbsentFrom");
        builder.Property(e => e.AbsentTo).HasColumnName("AbsentTo");
        builder.Property(e => e.MemberOnOrders).HasColumnName("MemberOnOrders").HasMaxLength(1);
        builder.Property(e => e.MemberCredible).HasColumnName("MemberCredible").HasMaxLength(1);
        builder.Property(e => e.ProximateCause).HasColumnName("ProximateCause");
        builder.Property(e => e.ProximateCauseSpecify).HasColumnName("ProximateCauseSpecify");
        builder.Property(e => e.Workflow).HasColumnName("Workflow");
        builder.Property(e => e.TravelOccurrence).HasColumnName("TravelOccurrence");
        builder.Property(e => e.VerifiedStatus).HasColumnName("VerifiedStatus");
        builder.Property(e => e.ProofOfStatus).HasColumnName("ProofOfStatus");
        builder.Property(e => e.LOdinitiation).HasColumnName("LODInitiation");
        builder.Property(e => e.WrittenDiagnosis).HasColumnName("WrittenDiagnosis");
        builder.Property(e => e.MemberRequest).HasColumnName("MemberRequest");
        builder.Property(e => e.PriorToDutytatus).HasColumnName("PriorToDutyStatus");
        builder.Property(e => e.StatusWorsened).HasColumnName("StatusWorsened").HasMaxLength(1);
        builder.Property(e => e.IncurredOrAggravated).HasColumnName("IncurredOrAggravated");
        builder.Property(e => e.StatusWhenInjuryed).HasColumnName("StatusWhenInjuryed").HasMaxLength(1);
        builder.Property(e => e.StatusWhenInjuryedExplanation).HasColumnName("StatusWhenInjuryedExplanation");
        builder.Property(e => e.OrdersAttached).HasColumnName("OrdersAttached").HasMaxLength(1);
        builder.Property(e => e.Idtstatus).HasColumnName("IDTStatus").HasMaxLength(50);
        builder.Property(e => e.Utapsattached).HasColumnName("UTAPSAttached").HasMaxLength(1);
        builder.Property(e => e.Pcarsattached).HasColumnName("PCARSAttached");
        builder.Property(e => e.Pcarshistory).HasColumnName("PCARSHistory");
        builder.Property(e => e.EightYearRule).HasColumnName("EightYearRule");

        // Indexes for query performance
        builder.HasIndex(e => e.ModifiedDate, "IX_Form348Unit_ModifiedDate");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form348Unit_ModifiedBy");
        builder.HasIndex(e => e.Workflow, "IX_Form348Unit_Workflow");
        builder.HasIndex(e => e.CmdrDutyDetermination, "IX_Form348Unit_CmdrDutyDetermination");
        builder.HasIndex(e => e.CmdrDutyFrom, "IX_Form348Unit_CmdrDutyFrom");
        builder.HasIndex(e => e.CmdrDutyTo, "IX_Form348Unit_CmdrDutyTo");
        builder.HasIndex(e => e.Idtstatus, "IX_Form348Unit_IDTStatus");
    }
}
