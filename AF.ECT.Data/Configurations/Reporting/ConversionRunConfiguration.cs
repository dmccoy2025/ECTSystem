using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Configures the <see cref="ConversionRun"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents a data conversion run tracking record with status, timestamps, and progress metrics.
/// </remarks>
public class ConversionRunConfiguration : IEntityTypeConfiguration<ConversionRun>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<ConversionRun> builder)
    {
        // Table mapping
        builder.ToTable("ConversionRun", "dbo");

        // Primary key
        builder.HasKey(e => e.RunId)
            .HasName("PK_ConversionRun");

        // Properties
        builder.Property(e => e.RunId).HasColumnName("RunID");
        builder.Property(e => e.StartDatestamp).HasColumnName("StartDatestamp");
        builder.Property(e => e.EndDatestamp).HasColumnName("EndDatestamp");
        builder.Property(e => e.Status).HasColumnName("Status").HasMaxLength(50);
        builder.Property(e => e.RecordsProcessed).HasColumnName("RecordsProcessed");
        builder.Property(e => e.ErrosFound).HasColumnName("ErrosFound");
        builder.Property(e => e.LastUserId).HasColumnName("LastUserID").HasMaxLength(50);
        builder.Property(e => e.LastPatientId).HasColumnName("LastPatientID").HasMaxLength(50);
        builder.Property(e => e.LastLodCaseId).HasColumnName("LastLodCaseID").HasMaxLength(50);

        // Indexes for query performance
        builder.HasIndex(e => e.StartDatestamp, "IX_ConversionRun_StartDatestamp");
        builder.HasIndex(e => e.Status, "IX_ConversionRun_Status");
    }
}
