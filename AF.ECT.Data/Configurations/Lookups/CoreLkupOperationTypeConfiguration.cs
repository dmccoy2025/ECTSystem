using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupOperationType"/>.
/// </summary>
public class CoreLkupOperationTypeConfiguration : IEntityTypeConfiguration<CoreLkupOperationType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupOperationType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupOperationType> builder)
    {
        builder.ToTable("Core_Lkup_OperationType", "dbo");

        builder.HasKey(e => e.OperationTypeId)
            .HasName("PK_Core_Lkup_OperationType");

        builder.Property(e => e.OperationTypeId).HasColumnName("OperationTypeID").HasMaxLength(50);
        builder.Property(e => e.Description).HasMaxLength(255);

        builder.HasIndex(e => e.Active, "IX_Core_Lkup_OperationType_Active");
    }
}
