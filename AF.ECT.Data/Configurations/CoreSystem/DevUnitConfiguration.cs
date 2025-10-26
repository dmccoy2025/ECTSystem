using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Configures the <see cref="DevUnit"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents development unit test data for command structure testing.
/// </remarks>
public class DevUnitConfiguration : IEntityTypeConfiguration<DevUnit>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<DevUnit> builder)
    {
        // Table mapping
        builder.ToTable("DevUnit", "dbo");

        // Primary key
        builder.HasKey(e => e.CsId)
            .HasName("PK_DevUnit");

        // Properties
        builder.Property(e => e.CsId).HasColumnName("CSID");
        builder.Property(e => e.UnitType).HasColumnName("UnitType").HasMaxLength(50).IsRequired();
        builder.Property(e => e.OrderNum).HasColumnName("OrderNum");

        // Indexes for query performance
        builder.HasIndex(e => e.UnitType, "IX_DevUnit_UnitType");
        builder.HasIndex(e => e.OrderNum, "IX_DevUnit_OrderNum");
    }
}
