using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework configuration for the <see cref="Form348ScReassessment"/> entity.
/// Configures the self-referential many-to-many relationship between Form 348 Special Category (SC)
/// cases and their reassessments, tracking which SC cases are reassessments of original cases.
/// </summary>
/// <remarks>
/// Form348ScReassessment is a junction table that links original Form 348 SC cases to their
/// reassessment cases. This enables tracking of case review and reassessment workflows where
/// a previous SC case is reevaluated and a new reassessment case is created.
/// 
/// Key characteristics:
/// - Self-referential many-to-many relationship (Form348Sc ↔ Form348Sc)
/// - Composite primary key (OriginalRefId, ReassessmentRefId)
/// - Distinguishes between original cases and their reassessments
/// - Supports reassessment workflow tracking
/// - No additional properties beyond the relationship keys
/// </remarks>
public class Form348ScReassessmentConfiguration : IEntityTypeConfiguration<Form348ScReassessment>
{
    /// <summary>
    /// Configures the Form348ScReassessment entity with table mapping, composite primary key,
    /// and self-referential foreign key relationships to Form348Sc.
    /// </summary>
    /// <param name="builder">The entity type builder for Form348ScReassessment.</param>
    public void Configure(EntityTypeBuilder<Form348ScReassessment> builder)
    {
        builder.ToTable("Form348_SC_Reassessment", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.OriginalRefId, e.ReassessmentRefId })
            .HasName("PK_Form348_SC_Reassessment");

        builder.Property(e => e.OriginalRefId)
            .HasColumnName("OriginalRefID");

        builder.Property(e => e.ReassessmentRefId)
            .HasColumnName("ReassessmentRefID");

        // Self-referential foreign key relationships
        builder.HasOne(d => d.OriginalRef)
            .WithMany()
            .HasForeignKey(d => d.OriginalRefId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Form348_SC_Reassessment_Form348_SC_Original");

        builder.HasOne(d => d.ReassessmentRef)
            .WithMany()
            .HasForeignKey(d => d.ReassessmentRefId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Form348_SC_Reassessment_Form348_SC_Reassessment");

        // Indexes for foreign keys
        builder.HasIndex(e => e.OriginalRefId, "IX_Form348_SC_Reassessment_OriginalRefID");
        builder.HasIndex(e => e.ReassessmentRefId, "IX_Form348_SC_Reassessment_ReassessmentRefID");
    }
}
