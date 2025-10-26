using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupDataType"/>.
/// </summary>
public class CoreLkupDataTypeConfiguration : IEntityTypeConfiguration<CoreLkupDataType>
{
    /// <summary>
    /// Configures the entity type for CoreLkupDataType.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupDataType> builder)
    {
        builder.ToTable("Core_Lkup_DataType", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_DataType");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Name).HasMaxLength(100);

        builder.HasIndex(e => e.Name, "IX_Core_Lkup_DataType_Name");
    }
}
