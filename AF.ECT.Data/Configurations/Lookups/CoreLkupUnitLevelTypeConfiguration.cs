using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupUnitLevelType"/>.
/// </summary>
public class CoreLkupUnitLevelTypeConfiguration : IEntityTypeConfiguration<CoreLkupUnitLevelType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupUnitLevelType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupUnitLevelType> builder)
    {
        builder.ToTable("Core_Lkup_UnitLevelType", "dbo");

        builder.HasKey(e => e.LevelId)
            .HasName("PK_Core_Lkup_UnitLevelType");

        builder.Property(e => e.LevelId).HasColumnName("LevelID").HasMaxLength(50);
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.ModifiedBy).HasMaxLength(100);

        builder.HasIndex(e => e.Active, "IX_Core_Lkup_UnitLevelType_Active");
    }
}
