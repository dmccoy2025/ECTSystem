using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AlodArcnetRaw"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the ALOD_ARCNET_Raw table,
/// which stores raw ARCNET (Air Reserve Component Network) training data.
/// Contains SSN, EDIPI (DoD ID Number), and training completion/due dates.
/// All properties are nullable strings for raw data import staging.
/// Used for importing and processing Air Reserve Component training records.
/// </remarks>
public class AlodArcnetRawConfiguration : IEntityTypeConfiguration<AlodArcnetRaw>
{
    /// <summary>
    /// Configures the entity of type <see cref="AlodArcnetRaw"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AlodArcnetRaw> builder)
    {
        // Table mapping
        builder.ToTable("ALOD_ARCNET_Raw", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration
        builder.Property(e => e.Ssn)
            .HasColumnName("SSN");

        builder.Property(e => e.Edipi)
            .HasColumnName("EDIPI");

        builder.Property(e => e.CompletionDate)
            .HasColumnName("CompletionDate");

        builder.Property(e => e.DueDate)
            .HasColumnName("DueDate");

        // Indexes for common queries
        builder.HasIndex(e => e.Ssn, "IX_alod_arcnet_raw_ssn");

        builder.HasIndex(e => e.Edipi, "IX_alod_arcnet_raw_edipi");
        
        builder.HasIndex(e => e.CompletionDate, "IX_alod_arcnet_raw_completion_date");
        
        builder.HasIndex(e => e.DueDate, "IX_alod_arcnet_raw_due_date");
    }
}
