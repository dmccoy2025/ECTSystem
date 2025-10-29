using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupGrade"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_grade table,
/// which defines military pay grades and ranks (E-1 through E-9, O-1 through O-10, W-1 through W-5, etc.).
/// Used throughout the system for member identification and organizational hierarchy.
/// </remarks>
public class CoreLkupGradeConfiguration : IEntityTypeConfiguration<CoreLkupGrade>
{
    /// <summary>
    /// Configures the CoreLkupGrade entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupGrade> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_grade", "dbo");

        // Primary key
        builder.HasKey(e => e.Code)
            .HasName("PK_core_lkup_grade");

        // Property configurations
        builder.Property(e => e.Code)
            .HasColumnName("code");

        builder.Property(e => e.Rank)
            .HasMaxLength(50)
            .HasColumnName("rank");

        builder.Property(e => e.Grade)
            .HasMaxLength(10)
            .HasColumnName("grade");

        builder.Property(e => e.Title)
            .HasMaxLength(100)
            .HasColumnName("title");

        builder.Property(e => e.Displayorder)
            .HasColumnName("displayorder");

        // Indexes
        builder.HasIndex(e => e.Grade, "IX_core_lkup_grade_grade");

        builder.HasIndex(e => e.Displayorder, "IX_core_lkup_grade_displayorder");
    }
}
