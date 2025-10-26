using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupMedicalFacility"/>.
/// </summary>
public class CoreLkupMedicalFacilityConfiguration : IEntityTypeConfiguration<CoreLkupMedicalFacility>
{
    /// <summary>
    /// Configures the entity type for CoreLkupMedicalFacility.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupMedicalFacility> builder)
    {
        builder.ToTable("Core_Lkup_MedicalFacility", "dbo");

        builder.HasKey(e => e.FacilityId)
            .HasName("PK_Core_Lkup_MedicalFacility");

        builder.Property(e => e.FacilityId).HasColumnName("FacilityID");
        builder.Property(e => e.FacilityDescription).HasMaxLength(255);
        builder.Property(e => e.FacilityType).HasMaxLength(100);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_MedicalFacility_SortOrder");
        builder.HasIndex(e => e.FacilityType, "IX_Core_Lkup_MedicalFacility_FacilityType");
    }
}
