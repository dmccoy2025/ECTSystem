using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupPwcategory"/>.
/// </summary>
public class CoreLkupPwcategoryConfiguration : IEntityTypeConfiguration<CoreLkupPwcategory>
{
    /// <summary>
    /// Configures the entity type for CoreLkupPwcategory.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupPwcategory> builder)
    {
        builder.ToTable("Core_Lkup_PWCategory", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_PWCategory");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Name).HasMaxLength(255);
        builder.Property(e => e.ParaInfo).HasMaxLength(500);

        builder.HasIndex(e => e.Name, "IX_Core_Lkup_PWCategory_Name");
    }
}
