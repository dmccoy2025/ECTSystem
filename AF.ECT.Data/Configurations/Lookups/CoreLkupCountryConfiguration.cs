using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupCountry"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_country table,
/// which defines countries for address information, geographic filtering, and international case handling.
/// </remarks>
public class CoreLkupCountryConfiguration : IEntityTypeConfiguration<CoreLkupCountry>
{
    /// <summary>
    /// Configures the CoreLkupCountry entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupCountry> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_country", "dbo");

        // Primary key
        builder.HasKey(e => e.CountryId)
            .HasName("PK_core_lkup_country");

        // Property configurations
        builder.Property(e => e.CountryId)
            .HasColumnName("country_id");

        builder.Property(e => e.Country)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("country");

        builder.Property(e => e.Abbr)
            .HasMaxLength(10)
            .HasColumnName("abbr");

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("created_by");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("modified_date")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.ModifiedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("modified_by");

        // Indexes
        builder.HasIndex(e => e.Country)
            .IsUnique()
            .HasDatabaseName("UQ_core_lkup_country_country");

        builder.HasIndex(e => e.Abbr, "IX_core_lkup_country_abbr");
    }
}
