using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupPhyCancelReason"/>.
/// </summary>
public class CoreLkupPhyCancelReasonConfiguration : IEntityTypeConfiguration<CoreLkupPhyCancelReason>
{
    /// <summary>
    /// Configures the entity type for CoreLkupPhyCancelReason.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupPhyCancelReason> builder)
    {
        builder.ToTable("Core_Lkup_PhyCancelReason", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_PhyCancelReason");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.Type).HasMaxLength(50);

        builder.HasIndex(e => e.Type, "IX_Core_Lkup_PhyCancelReason_Type");
    }
}
