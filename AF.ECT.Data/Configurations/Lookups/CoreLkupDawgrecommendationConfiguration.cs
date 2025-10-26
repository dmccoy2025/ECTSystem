using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupDawgrecommendation"/>.
/// </summary>
public class CoreLkupDawgrecommendationConfiguration : IEntityTypeConfiguration<CoreLkupDawgrecommendation>
{
    /// <summary>
    /// Configures the entity type for CoreLkupDawgrecommendation.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupDawgrecommendation> builder)
    {
        builder.ToTable("Core_Lkup_DAWGRecommendation", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_DAWGRecommendation");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Recommendation).HasMaxLength(500);

        builder.HasIndex(e => e.Recommendation, "IX_Core_Lkup_DAWGRecommendation_Recommendation");
    }
}
