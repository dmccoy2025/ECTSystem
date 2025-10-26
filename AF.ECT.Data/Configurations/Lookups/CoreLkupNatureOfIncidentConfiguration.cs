using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupNatureOfIncident"/>.
/// </summary>
public class CoreLkupNatureOfIncidentConfiguration : IEntityTypeConfiguration<CoreLkupNatureOfIncident>
{
    /// <summary>
    /// Configures the entity type for CoreLkupNatureOfIncident.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupNatureOfIncident> builder)
    {
        builder.ToTable("Core_Lkup_NatureOfIncident", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_NatureOfIncident");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Value).HasMaxLength(50);
        builder.Property(e => e.Text).HasMaxLength(500);

        builder.HasIndex(e => e.Value, "IX_Core_Lkup_NatureOfIncident_Value");
    }
}
