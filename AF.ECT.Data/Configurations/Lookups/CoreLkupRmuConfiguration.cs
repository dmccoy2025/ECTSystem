using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupRmu"/>.
/// </summary>
public class CoreLkupRmuConfiguration : IEntityTypeConfiguration<CoreLkupRmu>
{
    /// <summary>
    /// Configures the entity type for CoreLkupRmu.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupRmu> builder)
    {
        builder.ToTable("Core_Lkup_RMU", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_RMU");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Rmu).HasMaxLength(255);
        builder.Property(e => e.CsId).HasColumnName("CS_ID");

        builder.HasIndex(e => e.Collocated, "IX_Core_Lkup_RMU_Collocated");
    }
}
