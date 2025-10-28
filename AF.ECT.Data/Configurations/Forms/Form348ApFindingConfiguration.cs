using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348ApFinding"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents findings for personnel involved in Form 348 Appeal cases.
/// </remarks>
public class Form348ApFindingConfiguration : IEntityTypeConfiguration<Form348ApFinding>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348ApFinding> builder)
    {
        // Table mapping
        builder.ToTable("Form348APFinding", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Form348APFinding");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.AppealId).HasColumnName("AppealID");
        builder.Property(e => e.Ptype).HasColumnName("PTYPE");
        builder.Property(e => e.LastName).HasColumnName("LastName").HasMaxLength(100);
        builder.Property(e => e.FirstName).HasColumnName("FirstName").HasMaxLength(100);
        builder.Property(e => e.MiddleName).HasColumnName("MiddleName").HasMaxLength(100);
        builder.Property(e => e.Grade).HasColumnName("Grade").HasMaxLength(50);
        builder.Property(e => e.Rank).HasColumnName("Rank").HasMaxLength(50);
        builder.Property(e => e.Compo).HasColumnName("Compo").HasMaxLength(50);
        builder.Property(e => e.Pascode).HasColumnName("PASCODE").HasMaxLength(50);
        builder.Property(e => e.Finding).HasColumnName("Finding");
        builder.Property(e => e.Explanation).HasColumnName("Explanation");
        builder.Property(e => e.IsLegacyFinding).HasColumnName("IsLegacyFinding");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.Concur).HasColumnName("Concur").HasMaxLength(10);

        // Indexes for query performance
        builder.HasIndex(e => e.AppealId, "IX_Form348APFinding_AppealID");
        builder.HasIndex(e => new { e.AppealId, e.Ptype }, "IX_Form348APFinding_AppealID_PTYPE");
        builder.HasIndex(e => e.CreatedBy, "IX_Form348APFinding_CreatedBy");
        builder.HasIndex(e => e.CreatedDate, "IX_Form348APFinding_CreatedDate");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form348APFinding_ModifiedBy");
        builder.HasIndex(e => e.ModifiedDate, "IX_Form348APFinding_ModifiedDate");
        builder.HasIndex(e => e.Finding, "IX_Form348APFinding_Finding");
        builder.HasIndex(e => e.Grade, "IX_Form348APFinding_Grade");
    }
}
