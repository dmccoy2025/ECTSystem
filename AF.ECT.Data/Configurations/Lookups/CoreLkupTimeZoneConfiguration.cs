using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupTimeZone"/>.
/// </summary>
public class CoreLkupTimeZoneConfiguration : IEntityTypeConfiguration<CoreLkupTimeZone>
{
    /// <summary>
    /// Configures the entity type for CoreLkupTimeZone.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupTimeZone> builder)
    {
        builder.ToTable("Core_Lkup_TimeZone", "dbo");

        builder.HasKey(e => e.ZoneId)
            .HasName("PK_Core_Lkup_TimeZone");

        builder.Property(e => e.ZoneId).HasColumnName("ZoneID").HasMaxLength(50);
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.ModifiedBy).HasMaxLength(100);

        builder.HasIndex(e => e.Active, "IX_Core_Lkup_TimeZone_Active");
    }
}
