using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupFollowUpInterval"/>.
/// </summary>
public class CoreLkupFollowUpIntervalConfiguration : IEntityTypeConfiguration<CoreLkupFollowUpInterval>
{
    /// <summary>
    /// Configures the entity type for CoreLkupFollowUpInterval.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupFollowUpInterval> builder)
    {
        builder.ToTable("Core_Lkup_FollowUpInterval", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_FollowUpInterval");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Interval).HasMaxLength(100);

        builder.HasIndex(e => e.Interval, "IX_Core_Lkup_FollowUpInterval_Interval");
    }
}
