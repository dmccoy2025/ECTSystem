using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupComponent"/>.
/// </summary>
public class CoreLkupComponentConfiguration : IEntityTypeConfiguration<CoreLkupComponent>
{
    /// <summary>
    /// Configures the entity type for CoreLkupComponent.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupComponent> builder)
    {
        builder.ToTable("Core_Lkup_Component", "dbo");

        builder.HasKey(e => e.ComponentId)
            .HasName("PK_Core_Lkup_Component");

        builder.Property(e => e.ComponentId).HasColumnName("ComponentID");
        builder.Property(e => e.ComponentDescription).HasMaxLength(255);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_Component_SortOrder");
    }
}
