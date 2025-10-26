using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupMissedWorkDay"/>.
/// </summary>
public class CoreLkupMissedWorkDayConfiguration : IEntityTypeConfiguration<CoreLkupMissedWorkDay>
{
    /// <summary>
    /// Configures the entity type for CoreLkupMissedWorkDay.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupMissedWorkDay> builder)
    {
        builder.ToTable("Core_Lkup_MissedWorkDay", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_MissedWorkDay");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.DayIntervals).HasMaxLength(100);

        builder.HasIndex(e => e.DayIntervals, "IX_Core_Lkup_MissedWorkDay_DayIntervals");
    }
}
