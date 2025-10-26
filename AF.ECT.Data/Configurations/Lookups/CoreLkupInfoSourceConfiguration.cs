using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupInfoSource"/>.
/// </summary>
public class CoreLkupInfoSourceConfiguration : IEntityTypeConfiguration<CoreLkupInfoSource>
{
    /// <summary>
    /// Configures the entity type for CoreLkupInfoSource.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupInfoSource> builder)
    {
        builder.ToTable("Core_Lkup_InfoSource", "dbo");

        builder.HasKey(e => e.SourceId)
            .HasName("PK_Core_Lkup_InfoSource");

        builder.Property(e => e.SourceId).HasColumnName("SourceID");
        builder.Property(e => e.SourceDescription).HasMaxLength(255);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_InfoSource_SortOrder");
    }
}
