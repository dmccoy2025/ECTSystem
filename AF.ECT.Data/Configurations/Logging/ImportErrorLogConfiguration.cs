using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Logging;

/// <summary>
/// Entity Framework configuration for the <see cref="ImportErrorLog"/> entity.
/// Configures a logging table for tracking errors that occur during data import processes.
/// </summary>
/// <remarks>
/// ImportErrorLog is a keyless logging table that captures detailed error information during
/// data import and migration operations. This entity tracks stored procedure execution errors,
/// data update failures, and provides diagnostic information including error messages, RCPHA
/// LOD IDs, and timestamps for troubleshooting import issues.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for logging
/// - All nullable properties to accommodate various error scenarios
/// - Stored procedure name tracking for error source identification
/// - Data being updated (UpdatingData) for context
/// - Multiple message fields (Message, ErrorMessage) for detailed diagnostics
/// - RCPHA LOD ID association for case linking
/// - Timestamp tracking for error occurrence
/// - No foreign key relationships (logging isolation)
/// - Long-term retention for import troubleshooting and audit
/// </remarks>
public class ImportErrorLogConfiguration : IEntityTypeConfiguration<ImportErrorLog>
{
    /// <summary>
    /// Configures the ImportErrorLog entity as a keyless logging table with import error tracking fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImportErrorLog.</param>
    public void Configure(EntityTypeBuilder<ImportErrorLog> builder)
    {
        builder.ToTable("ImportErrorLog", "dbo");

        // Keyless entity for logging
        builder.HasNoKey();

        // Data context properties
        builder.Property(e => e.UpdatingData)
            .HasColumnType("ntext")
            .HasColumnName("UpdatingData");

        builder.Property(e => e.StoredProc)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("StoredProc");

        // Message properties
        builder.Property(e => e.Message)
            .HasColumnType("ntext")
            .HasColumnName("Message");

        builder.Property(e => e.ErrorMessage)
            .HasColumnType("ntext")
            .HasColumnName("ErrorMessage");

        // Case association
        builder.Property(e => e.Rcphalodid)
            .HasColumnName("RCPHALODID");

        // Timestamp
        builder.Property(e => e.Time)
            .HasColumnType("datetime")
            .HasColumnName("Time");
    }
}
