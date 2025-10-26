using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupRwoareason"/>.
/// </summary>
public class CoreLkupRwoareasonConfiguration : IEntityTypeConfiguration<CoreLkupRwoareason>
{
    /// <summary>
    /// Configures the entity type for CoreLkupRwoareason.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupRwoareason> builder)
    {
        builder.ToTable("Core_Lkup_RWOAReason", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_RWOAReason");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.Type).HasMaxLength(50);

        builder.HasIndex(e => e.Type, "IX_Core_Lkup_RWOAReason_Type");
    }
}
