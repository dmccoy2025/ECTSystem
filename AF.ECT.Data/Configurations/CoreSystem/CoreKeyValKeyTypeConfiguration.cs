using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework configuration for the CoreKeyValKeyType entity.
/// </summary>
public class CoreKeyValKeyTypeConfiguration : IEntityTypeConfiguration<CoreKeyValKeyType>
{
    /// <summary>
    /// Configures the CoreKeyValKeyType entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreKeyValKeyType> builder)
    {
        builder.ToTable("Core_KeyValKeyType", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_KeyValKeyType");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.TypeName)
            .HasMaxLength(255)
            .HasColumnName("TypeName");

        builder.HasIndex(e => e.TypeName, "IX_Core_KeyValKeyType_TypeName");
    }
}
