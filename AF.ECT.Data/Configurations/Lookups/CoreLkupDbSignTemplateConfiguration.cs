using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupDbSignTemplate"/>.
/// </summary>
public class CoreLkupDbSignTemplateConfiguration : IEntityTypeConfiguration<CoreLkupDbSignTemplate>
{
    /// <summary>
    /// Configures the entity type for CoreLkupDbSignTemplate.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupDbSignTemplate> builder)
    {
        builder.ToTable("Core_Lkup_dbSignTemplate", "dbo");

        builder.HasKey(e => e.TId)
            .HasName("PK_Core_Lkup_dbSignTemplate");

        builder.Property(e => e.TId).HasColumnName("tID");
        builder.Property(e => e.Title).HasMaxLength(255);
        builder.Property(e => e.TemplateTableName).HasMaxLength(255);
        builder.Property(e => e.PrimaryKey).HasMaxLength(255);
        builder.Property(e => e.SecondaryKey).HasMaxLength(255);

        builder.HasIndex(e => e.TemplateTableName, "IX_Core_Lkup_dbSignTemplate_TemplateTableName");
    }
}
