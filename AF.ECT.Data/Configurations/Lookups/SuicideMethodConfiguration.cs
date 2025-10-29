using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity type configuration for the <see cref="SuicideMethod"/> entity.
/// Configures the schema, table name, primary key, properties, and indexes for suicide method lookup.
/// </summary>
public class SuicideMethodConfiguration : IEntityTypeConfiguration<SuicideMethod>
{
    /// <summary>
    /// Configures the entity of type <see cref="SuicideMethod"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<SuicideMethod> builder)
    {
        builder.ToTable("SuicideMethod", "dbo");

        // Primary Key
        builder.HasKey(e => e.Id)
            .HasName("PK_SuicideMethod");

        // Properties
        builder.Property(e => e.Id)
            .HasColumnName("ID");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Active);

        // Indexes
        builder.HasIndex(e => e.Active, "IX_SuicideMethod_Active");

        builder.HasIndex(e => e.Name, "IX_SuicideMethod_Name");
    }
}
