using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkUpFindingByReasonOf"/>.
/// </summary>
public class CoreLkUpFindingByReasonOfConfiguration : IEntityTypeConfiguration<CoreLkUpFindingByReasonOf>
{
    /// <summary>
    /// Configures the entity type for CoreLkUpFindingByReasonOf.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkUpFindingByReasonOf> builder)
    {
        builder.ToTable("Core_LkUp_FindingByReasonOf", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_LkUp_FindingByReasonOf");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Description).HasMaxLength(500);

        builder.HasIndex(e => e.Description, "IX_Core_LkUp_FindingByReasonOf_Description");
    }
}
