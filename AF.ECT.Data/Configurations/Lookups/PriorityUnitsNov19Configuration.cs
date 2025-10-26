using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the PriorityUnitsNov19 entity.
/// Configures historical priority unit assignments from November 2019 for reference
/// and legacy data support in unit priority calculations and reporting.
/// </summary>
public class PriorityUnitsNov19Configuration : IEntityTypeConfiguration<PriorityUnitsNov19>
{
    /// <summary>
    /// Configures the PriorityUnitsNov19 entity as a keyless table for historical
    /// unit priority data with PAS codes and priority rankings.
    /// </summary>
    /// <param name="builder">The entity type builder for PriorityUnitsNov19.</param>
    public void Configure(EntityTypeBuilder<PriorityUnitsNov19> builder)
    {
        builder.HasNoKey();

        builder.ToTable("PRIORITY_UNITS_NOV19", "dbo");

        builder.Property(e => e.Id)
            .HasColumnType("numeric(18, 0)")
            .HasColumnName("ID");
        builder.Property(e => e.Pas)
            .HasMaxLength(255)
            .HasColumnName("PAS");
        builder.Property(e => e.Priority)
            .HasColumnType("numeric(18, 0)")
            .HasColumnName("PRIORITY");
    }
}
