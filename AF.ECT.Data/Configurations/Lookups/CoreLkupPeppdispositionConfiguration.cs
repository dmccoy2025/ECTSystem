using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupPeppdisposition"/>.
/// </summary>
public class CoreLkupPeppdispositionConfiguration : IEntityTypeConfiguration<CoreLkupPeppdisposition>
{
    /// <summary>
    /// Configures the entity type for CoreLkupPeppdisposition.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupPeppdisposition> builder)
    {
        builder.ToTable("Core_Lkup_PEPPDisposition", "dbo");

        builder.HasKey(e => e.DispositionId)
            .HasName("PK_Core_Lkup_PEPPDisposition");

        builder.Property(e => e.DispositionId).HasColumnName("DispositionID");
        builder.Property(e => e.DispositionName).HasMaxLength(255);

        builder.HasIndex(e => e.Active, "IX_Core_Lkup_PEPPDisposition_Active");
    }
}
