using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Integration;

/// <summary>
/// Entity type configuration for the <see cref="SsisConfiguration"/> entity.
/// Configures the schema, table name, and properties for SSIS package configuration settings (keyless table).
/// </summary>
public class SsisConfigurationConfiguration : IEntityTypeConfiguration<SsisConfiguration>
{
    /// <summary>
    /// Configures the entity of type <see cref="SsisConfiguration"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<SsisConfiguration> builder)
    {
        builder.ToTable("SSIS Configurations", "dbo");

        // Keyless Entity (Configuration table)
        builder.HasNoKey();

        // Properties
        builder.Property(e => e.ConfigurationFilter)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.ConfiguredValue)
            .HasMaxLength(255);

        builder.Property(e => e.PackagePath)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.ConfiguredValueType)
            .IsRequired()
            .HasMaxLength(20);
    }
}
