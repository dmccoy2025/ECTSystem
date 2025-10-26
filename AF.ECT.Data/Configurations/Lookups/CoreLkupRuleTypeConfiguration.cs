using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupRuleType"/>.
/// </summary>
public class CoreLkupRuleTypeConfiguration : IEntityTypeConfiguration<CoreLkupRuleType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupRuleType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupRuleType> builder)
    {
        builder.ToTable("Core_Lkup_RuleType", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_RuleType");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Description).HasMaxLength(255);

        builder.HasIndex(e => e.Description, "IX_Core_Lkup_RuleType_Description");
    }
}
