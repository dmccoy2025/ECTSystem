using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.System;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupAmrodisposition"/>.
/// </summary>
public class CoreLkupAmrodispositionConfiguration : IEntityTypeConfiguration<CoreLkupAmrodisposition>
{
    /// <summary>
    /// Configures the entity type for CoreLkupAmrodisposition.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupAmrodisposition> builder)
    {
        builder.ToTable("Core_Lkup_AMRODisposition");

        builder.HasKey(e => e.DispositionId);

        builder.Property(e => e.DispositionId).HasColumnName("DispositionID");
        builder.Property(e => e.Description).HasMaxLength(255);

        builder.HasIndex(e => e.Description, "IX_Core_Lkup_AMRODisposition_Description");
    }
}
