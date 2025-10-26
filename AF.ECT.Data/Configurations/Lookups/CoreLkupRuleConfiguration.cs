using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupRule"/>.
/// </summary>
public class CoreLkupRuleConfiguration : IEntityTypeConfiguration<CoreLkupRule>
{
    /// <summary>
    /// Configures the entity type for CoreLkupRule.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupRule> builder)
    {
        builder.ToTable("Core_Lkup_Rule", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_Rule");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.WorkFlow).HasColumnName("WorkFlow");
        builder.Property(e => e.Name).HasMaxLength(255);
        builder.Property(e => e.Prompt).HasMaxLength(500);

        builder.HasIndex(e => e.Active, "IX_Core_Lkup_Rule_Active");
        builder.HasIndex(e => e.RuleType, "IX_Core_Lkup_Rule_RuleType");
    }
}
