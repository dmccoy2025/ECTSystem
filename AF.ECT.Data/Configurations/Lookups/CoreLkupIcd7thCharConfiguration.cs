using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupIcd7thChar"/>.
/// </summary>
public class CoreLkupIcd7thCharConfiguration : IEntityTypeConfiguration<CoreLkupIcd7thChar>
{
    /// <summary>
    /// Configures the entity type for CoreLkupIcd7thChar.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupIcd7thChar> builder)
    {
        builder.ToTable("Core_Lkup_ICD7thChar", "dbo");

        builder.HasNoKey();

        builder.Property(e => e.CharCode).HasMaxLength(10);
        builder.Property(e => e.CharExt).HasMaxLength(100);
        builder.Property(e => e.CharDef).HasMaxLength(500);
    }
}
