using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupIncidentType"/>.
/// </summary>
public class CoreLkupIncidentTypeConfiguration : IEntityTypeConfiguration<CoreLkupIncidentType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupIncidentType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupIncidentType> builder)
    {
        builder.ToTable("Core_Lkup_IncidentType", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_IncidentType");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.IncidentType).HasMaxLength(100);
        builder.Property(e => e.IncidentDescr).HasMaxLength(500);

        builder.HasIndex(e => e.IncidentType, "IX_Core_Lkup_IncidentType_IncidentType");
    }
}
