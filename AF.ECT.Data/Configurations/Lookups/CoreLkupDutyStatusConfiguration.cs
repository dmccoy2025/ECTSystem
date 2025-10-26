using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupDutyStatus"/>.
/// </summary>
public class CoreLkupDutyStatusConfiguration : IEntityTypeConfiguration<CoreLkupDutyStatus>
{
    /// <summary>
    /// Configures the entity type for CoreLkupDutyStatus.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupDutyStatus> builder)
    {
        builder.ToTable("Core_Lkup_DutyStatus", "dbo");

        builder.HasKey(e => e.DutyId)
            .HasName("PK_Core_Lkup_DutyStatus");

        builder.Property(e => e.DutyId).HasColumnName("DutyID");
        builder.Property(e => e.DutyType).HasMaxLength(100);
        builder.Property(e => e.DutyDescription).HasMaxLength(255);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_DutyStatus_SortOrder");
    }
}
