using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkUpFinding"/>.
/// </summary>
public class CoreLkUpFindingConfiguration : IEntityTypeConfiguration<CoreLkUpFinding>
{
    /// <summary>
    /// Configures the entity type for CoreLkUpFinding.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkUpFinding> builder)
    {
        builder.ToTable("Core_LkUp_Finding", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_LkUp_Finding");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.FindingType).HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(500);

        builder.HasIndex(e => e.FindingType, "IX_Core_LkUp_Finding_FindingType");
    }
}
