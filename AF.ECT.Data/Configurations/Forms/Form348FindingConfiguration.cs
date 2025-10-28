using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348Finding"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents findings for personnel involved in LOD (Line of Duty) investigations.
/// Contains the determination of duty status and associated explanations.
/// </remarks>
public class Form348FindingConfiguration : IEntityTypeConfiguration<Form348Finding>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348Finding> builder)
    {
        // Table mapping
        builder.ToTable("Form348Finding", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Form348Finding");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Lodid).HasColumnName("LODID");
        builder.Property(e => e.Ptype).HasColumnName("PTYPE");
        builder.Property(e => e.Ssn).HasColumnName("SSN").HasMaxLength(20);
        builder.Property(e => e.Name).HasColumnName("Name").HasMaxLength(200);
        builder.Property(e => e.Grade).HasColumnName("Grade").HasMaxLength(50);
        builder.Property(e => e.Compo).HasColumnName("Compo").HasMaxLength(50);
        builder.Property(e => e.Rank).HasColumnName("Rank").HasMaxLength(50);
        builder.Property(e => e.Pascode).HasColumnName("PASCODE").HasMaxLength(50);
        builder.Property(e => e.Finding).HasColumnName("Finding");
        builder.Property(e => e.DecisionYn).HasColumnName("DecisionYN").HasMaxLength(1);
        builder.Property(e => e.Explanation).HasColumnName("Explanation");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.FindingsText).HasColumnName("FindingsText");
        builder.Property(e => e.ReferDes).HasColumnName("ReferDES");
        builder.Property(e => e.Lodinitiation).HasColumnName("LODINITIATION");
        builder.Property(e => e.WrittenDiagnosis).HasColumnName("WrittenDiagnosis");
        builder.Property(e => e.MemberRequest).HasColumnName("MemberRequest");
        builder.Property(e => e.CorrectlyIdentified).HasColumnName("CorrectlyIdentified").HasMaxLength(1);
        builder.Property(e => e.VerifiedAndAttached).HasColumnName("VerifiedAndAttached").HasMaxLength(1);
        builder.Property(e => e.Idtstatus).HasColumnName("IDTSTATUS").HasMaxLength(50);
        builder.Property(e => e.Ipcarsattached).HasColumnName("IPCARSATTACHED");
        builder.Property(e => e.EightYearRule).HasColumnName("EightYearRule");
        builder.Property(e => e.PriorToDutytatus).HasColumnName("PriorToDutytatus");
        builder.Property(e => e.StatusWorsened).HasColumnName("StatusWorsened").HasMaxLength(1);

        // Indexes for query performance
        builder.HasIndex(e => e.Lodid, "IX_Form348Finding_LODID");
        builder.HasIndex(e => e.Ssn, "IX_Form348Finding_SSN");
        builder.HasIndex(e => new { e.Lodid, e.Ptype }, "IX_Form348Finding_LODID_PTYPE");
        builder.HasIndex(e => e.CreatedBy, "IX_Form348Finding_CreatedBy");
        builder.HasIndex(e => e.CreatedDate, "IX_Form348Finding_CreatedDate");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form348Finding_ModifiedBy");
        builder.HasIndex(e => e.ModifiedDate, "IX_Form348Finding_ModifiedDate");
        builder.HasIndex(e => e.Finding, "IX_Form348Finding_Finding");
        builder.HasIndex(e => e.Grade, "IX_Form348Finding_Grade");
    }
}
