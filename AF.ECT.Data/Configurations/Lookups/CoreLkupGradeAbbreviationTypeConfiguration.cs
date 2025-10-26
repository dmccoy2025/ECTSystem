using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupGradeAbbreviationType"/>.
/// </summary>
public class CoreLkupGradeAbbreviationTypeConfiguration : IEntityTypeConfiguration<CoreLkupGradeAbbreviationType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupGradeAbbreviationType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupGradeAbbreviationType> builder)
    {
        builder.ToTable("Core_Lkup_GradeAbbreviationType", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_GradeAbbreviationType");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Name).HasMaxLength(100);

        builder.HasIndex(e => e.Name, "IX_Core_Lkup_GradeAbbreviationType_Name");
    }
}
