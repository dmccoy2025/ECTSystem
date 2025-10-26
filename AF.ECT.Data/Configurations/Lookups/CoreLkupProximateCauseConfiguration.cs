using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupProximateCause"/>.
/// </summary>
public class CoreLkupProximateCauseConfiguration : IEntityTypeConfiguration<CoreLkupProximateCause>
{
    /// <summary>
    /// Configures the entity type for CoreLkupProximateCause.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupProximateCause> builder)
    {
        builder.ToTable("Core_Lkup_ProximateCause");

        builder.HasKey(e => e.CauseId);

        builder.Property(e => e.CauseId).HasColumnName("CauseID");
        builder.Property(e => e.CauseDescription).HasMaxLength(500);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_ProximateCause_SortOrder");
    }
}
