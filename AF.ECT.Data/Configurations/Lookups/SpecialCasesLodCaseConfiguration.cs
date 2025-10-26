using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity type configuration for the <see cref="SpecialCasesLodCase"/> entity.
/// Configures the schema, table name, and properties for special LOD case tracking (keyless view).
/// </summary>
public class SpecialCasesLodCaseConfiguration : IEntityTypeConfiguration<SpecialCasesLodCase>
{
    /// <summary>
    /// Configures the entity of type <see cref="SpecialCasesLodCase"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<SpecialCasesLodCase> builder)
    {
        builder.ToTable("SpecialCasesLODCase", "dbo");

        // Keyless Entity (View or staging table)
        builder.HasNoKey();

        // Properties
        builder.Property(e => e.Status)
            .HasMaxLength(50);

        builder.Property(e => e.Position)
            .HasMaxLength(50);

        builder.Property(e => e.Location)
            .HasMaxLength(100);
    }
}
