using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupPersonnelType"/>.
/// </summary>
public class CoreLkupPersonnelTypeConfiguration : IEntityTypeConfiguration<CoreLkupPersonnelType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupPersonnelType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupPersonnelType> builder)
    {
        builder.ToTable("Core_Lkup_PersonnelType", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_PersonnelType");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.RoleName).HasMaxLength(100);

        builder.HasIndex(e => e.Formal, "IX_Core_Lkup_PersonnelType_Formal");
    }
}
