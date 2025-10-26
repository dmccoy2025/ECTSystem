using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework configuration for the CoreKeyValValue entity.
/// </summary>
public class CoreKeyValValueConfiguration : IEntityTypeConfiguration<CoreKeyValValue>
{
    /// <summary>
    /// Configures the CoreKeyValValue entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreKeyValValue> builder)
    {
        builder.ToTable("Core_KeyValValue", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_KeyValValue");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.KeyId).HasColumnName("KeyID");
        builder.Property(e => e.ValueId)
            .HasMaxLength(100)
            .HasColumnName("ValueID");
        builder.Property(e => e.Value)
            .HasMaxLength(500)
            .HasColumnName("Value");
        builder.Property(e => e.ValueDescription)
            .HasMaxLength(1000)
            .HasColumnName("ValueDescription");

        builder.HasIndex(e => e.KeyId, "IX_Core_KeyValValue_KeyID");
        builder.HasIndex(e => e.ValueId, "IX_Core_KeyValValue_ValueID");
    }
}
