using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework configuration for the <see cref="GradeAbbreviation"/> entity.
/// Configures military grade abbreviations for different abbreviation type contexts
/// (e.g., formal, informal, display formats).
/// </summary>
/// <remarks>
/// GradeAbbreviation defines how military grades are abbreviated in various contexts throughout
/// the application. The same grade can have multiple abbreviations depending on the abbreviation
/// type (e.g., "E-5" might be abbreviated as "SSgt" in formal contexts, "SSGT" in all-caps contexts).
/// 
/// Key characteristics:
/// - Many-to-many junction table (CoreLkupGradeAbbreviationType ↔ CoreLkupGrade)
/// - Composite primary key (AbbreviationTypeId, GradeCode)
/// - Required abbreviation string value
/// - Supports context-specific grade abbreviations
/// - No audit trail (lookup/reference data)
/// </remarks>
public class GradeAbbreviationConfiguration : IEntityTypeConfiguration<GradeAbbreviation>
{
    /// <summary>
    /// Configures the GradeAbbreviation entity with table mapping, composite primary key,
    /// required fields, and foreign key relationships.
    /// </summary>
    /// <param name="builder">The entity type builder for GradeAbbreviation.</param>
    public void Configure(EntityTypeBuilder<GradeAbbreviation> builder)
    {
        builder.ToTable("GradeAbbreviation", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.AbbreviationTypeId, e.GradeCode })
            .HasName("PK_GradeAbbreviation");

        builder.Property(e => e.AbbreviationTypeId)
            .HasColumnName("AbbreviationTypeID");

        builder.Property(e => e.GradeCode)
            .HasColumnName("GradeCode");

        // Required abbreviation value
        builder.Property(e => e.Abbreviation)
            .IsRequired()
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("Abbreviation");

        // Foreign key relationships
        builder.HasOne(d => d.AbbreviationType)
            .WithMany(p => p.GradeAbbreviations)
            .HasForeignKey(d => d.AbbreviationTypeId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_GradeAbbreviation_Core_Lkup_GradeAbbreviationType");

        builder.HasOne(d => d.GradeCodeNavigation)
            .WithMany(p => p.GradeAbbreviations)
            .HasForeignKey(d => d.GradeCode)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_GradeAbbreviation_Core_Lkup_Grade");

        // Indexes for foreign keys
        builder.HasIndex(e => e.AbbreviationTypeId, "IX_GradeAbbreviation_AbbreviationTypeID");
        builder.HasIndex(e => e.GradeCode, "IX_GradeAbbreviation_GradeCode");
    }
}
