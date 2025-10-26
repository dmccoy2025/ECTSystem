using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupAvailabilityCode"/>.
/// </summary>
public class CoreLkupAvailabilityCodeConfiguration : IEntityTypeConfiguration<CoreLkupAvailabilityCode>
{
    /// <summary>
    /// Configures the entity type for CoreLkupAvailabilityCode.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupAvailabilityCode> builder)
    {
        builder.ToTable("Core_Lkup_AvailabilityCode", "dbo");

        builder.HasKey(e => e.AvailabilityCode)
            .HasName("PK_Core_Lkup_AvailabilityCode");

        builder.Property(e => e.Description).HasMaxLength(255);

        builder.HasIndex(e => e.Description, "IX_Core_Lkup_AvailabilityCode_Description");
    }
}
