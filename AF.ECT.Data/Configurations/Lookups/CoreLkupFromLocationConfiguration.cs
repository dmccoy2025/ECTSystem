using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupFromLocation"/>.
/// </summary>
public class CoreLkupFromLocationConfiguration : IEntityTypeConfiguration<CoreLkupFromLocation>
{
    /// <summary>
    /// Configures the entity type for CoreLkupFromLocation.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupFromLocation> builder)
    {
        builder.ToTable("Core_Lkup_FromLocation", "dbo");

        builder.HasKey(e => e.LocationId)
            .HasName("PK_Core_Lkup_FromLocation");

        builder.Property(e => e.LocationId).HasColumnName("LocationID");
        builder.Property(e => e.LocationDescription).HasMaxLength(255);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_FromLocation_SortOrder");
    }
}
