using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupFtrecommendation"/>.
/// </summary>
public class CoreLkupFtrecommendationConfiguration : IEntityTypeConfiguration<CoreLkupFtrecommendation>
{
    /// <summary>
    /// Configures the entity type for CoreLkupFtrecommendation.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupFtrecommendation> builder)
    {
        builder.ToTable("Core_Lkup_FTRecommendation", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_FTRecommendation");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Abbrev).HasMaxLength(50);
        builder.Property(e => e.Description).HasMaxLength(255);

        builder.HasIndex(e => e.Abbrev, "IX_Core_Lkup_FTRecommendation_Abbrev");
    }
}
