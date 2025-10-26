using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupSpecialistsRequiredForManagement"/>.
/// </summary>
public class CoreLkupSpecialistsRequiredForManagementConfiguration : IEntityTypeConfiguration<CoreLkupSpecialistsRequiredForManagement>
{
    /// <summary>
    /// Configures the entity type for CoreLkupSpecialistsRequiredForManagement.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupSpecialistsRequiredForManagement> builder)
    {
        builder.ToTable("Core_Lkup_SpecialistsRequiredForManagement", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_SpecialistsRequiredForManagement");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.AmountPerYear).HasMaxLength(100);

        builder.HasIndex(e => e.AmountPerYear, "IX_Core_Lkup_SpecialistsRequiredForManagement_AmountPerYear");
    }
}
