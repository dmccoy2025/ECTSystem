using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupAction"/>.
/// </summary>
public class CoreLkupActionConfiguration : IEntityTypeConfiguration<CoreLkupAction>
{
    /// <summary>
    /// Configures the entity type for CoreLkupAction.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupAction> builder)
    {
        builder.ToTable("Core_Lkup_Action");

        builder.HasKey(e => e.ActionId);

        builder.Property(e => e.ActionId).HasColumnName("ActionID");
        builder.Property(e => e.ActionName).HasMaxLength(255);

        builder.HasIndex(e => e.ActionName, "IX_Core_Lkup_Action_ActionName");
    }
}
