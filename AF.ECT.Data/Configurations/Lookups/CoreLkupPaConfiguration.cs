using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupPa"/>.
/// </summary>
public class CoreLkupPaConfiguration : IEntityTypeConfiguration<CoreLkupPa>
{
    /// <summary>
    /// Configures the entity type for CoreLkupPa.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupPa> builder)
    {
        builder.ToTable("Core_Lkup_PA", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_PA");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Pas).HasMaxLength(100);

        builder.HasIndex(e => e.Priority, "IX_Core_Lkup_PA_Priority");
    }
}
