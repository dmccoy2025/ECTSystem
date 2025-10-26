using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupMedGroup"/>.
/// </summary>
public class CoreLkupMedGroupConfiguration : IEntityTypeConfiguration<CoreLkupMedGroup>
{
    /// <summary>
    /// Configures the entity type for CoreLkupMedGroup.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupMedGroup> builder)
    {
        builder.ToTable("Core_Lkup_MedGroup", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_MedGroup");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Mtf).HasMaxLength(255);

        builder.HasIndex(e => e.Mtf, "IX_Core_Lkup_MedGroup_MTF");
    }
}
