using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupFastTrackType"/>.
/// </summary>
public class CoreLkupFastTrackTypeConfiguration : IEntityTypeConfiguration<CoreLkupFastTrackType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupFastTrackType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupFastTrackType> builder)
    {
        builder.ToTable("Core_Lkup_FastTrackType", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_FastTrackType");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Description).HasMaxLength(255);

        builder.HasIndex(e => e.Description, "IX_Core_Lkup_FastTrackType_Description");
    }
}
