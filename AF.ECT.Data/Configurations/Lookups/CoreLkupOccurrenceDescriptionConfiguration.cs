using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupOccurrenceDescription"/>.
/// </summary>
public class CoreLkupOccurrenceDescriptionConfiguration : IEntityTypeConfiguration<CoreLkupOccurrenceDescription>
{
    /// <summary>
    /// Configures the entity type for CoreLkupOccurrenceDescription.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupOccurrenceDescription> builder)
    {
        builder.ToTable("Core_Lkup_OccurrenceDescription", "dbo");

        builder.HasKey(e => e.OccurrenceId)
            .HasName("PK_Core_Lkup_OccurrenceDescription");

        builder.Property(e => e.OccurrenceId).HasColumnName("OccurrenceID");
        builder.Property(e => e.OccurrenceDescription).HasMaxLength(500);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_OccurrenceDescription_SortOrder");
    }
}
