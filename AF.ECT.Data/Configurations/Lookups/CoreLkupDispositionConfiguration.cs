using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupDisposition"/>.
/// </summary>
public class CoreLkupDispositionConfiguration : IEntityTypeConfiguration<CoreLkupDisposition>
{
    /// <summary>
    /// Configures the entity type for CoreLkupDisposition.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupDisposition> builder)
    {
        builder.ToTable("Core_Lkup_Disposition", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_Disposition");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Name).HasMaxLength(255);

        builder.HasIndex(e => e.Name, "IX_Core_Lkup_Disposition_Name");
    }
}
