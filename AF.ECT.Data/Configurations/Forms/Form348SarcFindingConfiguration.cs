using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348SarcFinding"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents findings for personnel involved in SARC (Sexual Assault Response Coordinator) cases.
/// </remarks>
public class Form348SarcFindingConfiguration : IEntityTypeConfiguration<Form348SarcFinding>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348SarcFinding> builder)
    {
        // Table mapping
        builder.ToTable("Form348SARCFinding", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Form348SARCFinding");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.SarcId).HasColumnName("SarcID");
        builder.Property(e => e.Ptype).HasColumnName("PTYPE");
        builder.Property(e => e.Ssn).HasColumnName("SSN").HasMaxLength(20);
        builder.Property(e => e.Name).HasColumnName("Name").HasMaxLength(200);
        builder.Property(e => e.Grade).HasColumnName("Grade").HasMaxLength(50);
        builder.Property(e => e.Compo).HasColumnName("Compo").HasMaxLength(50);
        builder.Property(e => e.Finding).HasColumnName("Finding");
        builder.Property(e => e.Remarks).HasColumnName("Remarks");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.Pascode).HasColumnName("PASCODE").HasMaxLength(50);
        builder.Property(e => e.Rank).HasColumnName("Rank").HasMaxLength(50);
        builder.Property(e => e.FindingsText).HasColumnName("FindingsText");
        builder.Property(e => e.Concur).HasColumnName("Concur").HasMaxLength(10);

        // Indexes for query performance
        builder.HasIndex(e => e.SarcId, "IX_Form348SARCFinding_SarcID");
        builder.HasIndex(e => e.Ssn, "IX_Form348SARCFinding_SSN");
        builder.HasIndex(e => new { e.SarcId, e.Ptype }, "IX_Form348SARCFinding_SarcID_PTYPE");
        builder.HasIndex(e => e.CreatedBy, "IX_Form348SARCFinding_CreatedBy");
        builder.HasIndex(e => e.CreatedDate, "IX_Form348SARCFinding_CreatedDate");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form348SARCFinding_ModifiedBy");
        builder.HasIndex(e => e.ModifiedDate, "IX_Form348SARCFinding_ModifiedDate");
        builder.HasIndex(e => e.Finding, "IX_Form348SARCFinding_Finding");
        builder.HasIndex(e => e.Grade, "IX_Form348SARCFinding_Grade");
    }
}
