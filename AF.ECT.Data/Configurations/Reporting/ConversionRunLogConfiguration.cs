using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Configures the <see cref="ConversionRunLog"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents log messages for data conversion runs.
/// </remarks>
public class ConversionRunLogConfiguration : IEntityTypeConfiguration<ConversionRunLog>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<ConversionRunLog> builder)
    {
        // Table mapping
        builder.ToTable("ConversionRunLog", "dbo");

        // Primary key (composite)
        builder.HasKey(e => new { e.RunId, e.LineNumber })
            .HasName("PK_ConversionRunLog");

        // Properties
        builder.Property(e => e.RunId).HasColumnName("RunID");
        builder.Property(e => e.LineNumber).HasColumnName("LineNumber");
        builder.Property(e => e.Message).HasColumnName("Message").IsRequired();

        // Indexes for query performance
        builder.HasIndex(e => e.RunId, "IX_ConversionRunLog_RunID");
    }
}
