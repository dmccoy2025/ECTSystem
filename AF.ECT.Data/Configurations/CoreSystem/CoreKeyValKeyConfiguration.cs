using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework configuration for the CoreKeyValKey entity.
/// </summary>
public class CoreKeyValKeyConfiguration : IEntityTypeConfiguration<CoreKeyValKey>
{
    /// <summary>
    /// Configures the CoreKeyValKey entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreKeyValKey> builder)
    {
        builder.ToTable("Core_KeyValKey", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_KeyValKey");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .HasColumnName("Description");
        builder.Property(e => e.KeyTypeId).HasColumnName("KeyTypeID");

        builder.HasIndex(e => e.KeyTypeId, "IX_Core_KeyValKey_KeyTypeID");
        builder.HasIndex(e => e.Description, "IX_Core_KeyValKey_Description");
    }
}
