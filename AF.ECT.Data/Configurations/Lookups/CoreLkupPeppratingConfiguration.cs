using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupPepprating"/>.
/// </summary>
public class CoreLkupPeppratingConfiguration : IEntityTypeConfiguration<CoreLkupPepprating>
{
    /// <summary>
    /// Configures the entity type for CoreLkupPepprating.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupPepprating> builder)
    {
        builder.ToTable("Core_Lkup_PEPPRating", "dbo");

        builder.HasKey(e => e.RatingId)
            .HasName("PK_Core_Lkup_PEPPRating");

        builder.Property(e => e.RatingId).HasColumnName("RatingID");
        builder.Property(e => e.RatingName).HasMaxLength(255);

        builder.HasIndex(e => e.Active, "IX_Core_Lkup_PEPPRating_Active");
    }
}
