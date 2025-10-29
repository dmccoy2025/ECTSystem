using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupMajcom"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_majcom table,
/// which defines Major Commands (MAJCOMs) in the Air Force organizational structure. MAJCOMs are
/// major functional subdivisions that organize Air Force units by mission or geographic region.
/// </remarks>
public class CoreLkupMajcomConfiguration : IEntityTypeConfiguration<CoreLkupMajcom>
{
    /// <summary>
    /// Configures the CoreLkupMajcom entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupMajcom> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_majcom", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_lkup_majcom");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.MajcomName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("majcom_name");

        builder.Property(e => e.Active)
            .HasColumnName("active");

        // Indexes
        builder.HasIndex(e => e.MajcomName)
            .IsUnique()
            .HasDatabaseName("UQ_core_lkup_majcom_name");

        builder.HasIndex(e => e.Active, "IX_core_lkup_majcom_active");
    }
}
