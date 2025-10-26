using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupSuddenIncapacitationRisk"/>.
/// </summary>
public class CoreLkupSuddenIncapacitationRiskConfiguration : IEntityTypeConfiguration<CoreLkupSuddenIncapacitationRisk>
{
    /// <summary>
    /// Configures the entity type for CoreLkupSuddenIncapacitationRisk.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupSuddenIncapacitationRisk> builder)
    {
        builder.ToTable("Core_Lkup_SuddenIncapacitationRisk", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_SuddenIncapacitationRisk");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.RiskLevel).HasMaxLength(100);

        builder.HasIndex(e => e.RiskLevel, "IX_Core_Lkup_SuddenIncapacitationRisk_RiskLevel");
    }
}
