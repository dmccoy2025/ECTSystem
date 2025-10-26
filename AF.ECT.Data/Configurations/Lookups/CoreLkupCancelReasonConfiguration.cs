using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupCancelReason"/>.
/// </summary>
public class CoreLkupCancelReasonConfiguration : IEntityTypeConfiguration<CoreLkupCancelReason>
{
    /// <summary>
    /// Configures the entity type for CoreLkupCancelReason.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupCancelReason> builder)
    {
        builder.ToTable("Core_Lkup_CancelReason", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_CancelReason");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Description).HasMaxLength(500);

        builder.HasIndex(e => e.DisplayOrder, "IX_Core_Lkup_CancelReason_DisplayOrder");
    }
}
