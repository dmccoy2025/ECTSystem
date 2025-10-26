using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupIograde"/>.
/// </summary>
public class CoreLkupIogradeConfiguration : IEntityTypeConfiguration<CoreLkupIograde>
{
    /// <summary>
    /// Configures the entity type for CoreLkupIograde.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupIograde> builder)
    {
        builder.ToTable("Core_Lkup_IOGrade", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_IOGrade");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Grade).HasMaxLength(50);

        builder.HasIndex(e => e.Grade, "IX_Core_Lkup_IOGrade_Grade");
    }
}
