using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348PscdFinding"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents findings for PSCD (Physical Standards/Classification Determination) cases.
/// </remarks>
public class Form348PscdFindingConfiguration : IEntityTypeConfiguration<Form348PscdFinding>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348PscdFinding> builder)
    {
        // Table mapping
        builder.ToTable("Form348PSCDFinding", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Form348PSCDFinding");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.PscdId).HasColumnName("PSCDID");
        builder.Property(e => e.Name).HasColumnName("Name").HasMaxLength(200);
        builder.Property(e => e.Ptype).HasColumnName("PTYPE");
        builder.Property(e => e.Remarks).HasColumnName("Remarks");
        builder.Property(e => e.Finding).HasColumnName("Finding");
        builder.Property(e => e.FindingText).HasColumnName("FindingText");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.AdditionalRemarks).HasColumnName("AdditionalRemarks");
        builder.Property(e => e.ReferToDes).HasColumnName("ReferToDES");

        // Indexes for query performance
        builder.HasIndex(e => e.PscdId, "IX_Form348PSCDFinding_PSCDID");
        builder.HasIndex(e => new { e.PscdId, e.Ptype }, "IX_Form348PSCDFinding_PSCDID_PTYPE");
        builder.HasIndex(e => e.CreatedBy, "IX_Form348PSCDFinding_CreatedBy");
        builder.HasIndex(e => e.CreatedDate, "IX_Form348PSCDFinding_CreatedDate");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form348PSCDFinding_ModifiedBy");
        builder.HasIndex(e => e.ModifiedDate, "IX_Form348PSCDFinding_ModifiedDate");
        builder.HasIndex(e => e.Finding, "IX_Form348PSCDFinding_Finding");
    }
}
