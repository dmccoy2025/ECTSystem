using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupYearsSatisfactoryService"/>.
/// </summary>
public class CoreLkupYearsSatisfactoryServiceConfiguration : IEntityTypeConfiguration<CoreLkupYearsSatisfactoryService>
{
    /// <summary>
    /// Configures the entity type for CoreLkupYearsSatisfactoryService.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupYearsSatisfactoryService> builder)
    {
        builder.ToTable("Core_Lkup_YearsSatisfactoryService", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_YearsSatisfactoryService");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.RangeCategory).HasMaxLength(100);

        builder.HasIndex(e => e.RangeCategory, "IX_Core_Lkup_YearsSatisfactoryService_RangeCategory");
    }
}
