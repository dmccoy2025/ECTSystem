using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="ApplicationWarmupProcessLog"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Application_Warmup_Process_Log table,
/// which stores execution logs for application warmup processes.
/// Each log entry records the process ID, execution timestamp, and optional message
/// (success confirmation, error details, or diagnostic information).
/// Used for monitoring warmup process execution and troubleshooting startup issues.
/// </remarks>
public class ApplicationWarmupProcessLogConfiguration : IEntityTypeConfiguration<ApplicationWarmupProcessLog>
{
    /// <summary>
    /// Configures the entity of type <see cref="ApplicationWarmupProcessLog"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<ApplicationWarmupProcessLog> builder)
    {
        // Table mapping
        builder.ToTable("Application_Warmup_Process_Log", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Application_Warmup_Process_Log");

        // Properties configuration
        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.ProcessId)
            .IsRequired()
            .HasColumnName("ProcessId");

        builder.Property(e => e.ExecutionDate)
            .IsRequired()
            .HasColumnName("ExecutionDate")
            .HasColumnType("datetime");

        builder.Property(e => e.Message)
            .HasMaxLength(2000)
            .HasColumnName("Message");

        // Relationships
        builder.HasOne(e => e.Process)
            .WithMany(e => e.ApplicationWarmupProcessLogs)
            .HasForeignKey(e => e.ProcessId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Application_Warmup_Process_Log_Process");

        // Indexes
        builder.HasIndex(e => e.ProcessId, "IX_application_warmup_process_log_process_id");

        builder.HasIndex(e => e.ExecutionDate, "IX_application_warmup_process_log_execution_date");
        
        builder.HasIndex(e => new { e.ProcessId, e.ExecutionDate }, "IX_application_warmup_process_log_process_date");
    }
}
