using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupPepptype"/>.
/// </summary>
public class CoreLkupPepptypeConfiguration : IEntityTypeConfiguration<CoreLkupPepptype>
{
    /// <summary>
    /// Configures the entity type for CoreLkupPepptype.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupPepptype> builder)
    {
        builder.ToTable("Core_Lkup_PEPPType", "dbo");

        builder.HasKey(e => e.TypeId)
            .HasName("PK_Core_Lkup_PEPPType");

        builder.Property(e => e.TypeId).HasColumnName("TypeID");
        builder.Property(e => e.TypeName).HasMaxLength(255);

        builder.HasIndex(e => e.Active, "IX_Core_Lkup_PEPPType_Active");
    }
}
