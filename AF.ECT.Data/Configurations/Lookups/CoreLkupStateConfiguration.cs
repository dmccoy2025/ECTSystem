using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupState"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_state table,
/// which defines U.S. states, territories, and international regions. Used for address information,
/// geographic filtering, and location-based reporting.
/// </remarks>
public class CoreLkupStateConfiguration : IEntityTypeConfiguration<CoreLkupState>
{
    /// <summary>
    /// Configures the CoreLkupState entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupState> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_state", "dbo");

        // Primary key (composite)
        builder.HasKey(e => e.State)
            .HasName("PK_core_lkup_state");

        // Property configurations
        builder.Property(e => e.State)
            .HasMaxLength(2)
            .HasColumnName("state");

        builder.Property(e => e.StateName)
            .HasMaxLength(100)
            .HasColumnName("state_name");

        builder.Property(e => e.Country)
            .HasMaxLength(100)
            .HasColumnName("country");

        // Indexes
        builder.HasIndex(e => e.StateName, "IX_core_lkup_state_state_name");

        builder.HasIndex(e => e.Country, "IX_core_lkup_state_country");
    }
}
