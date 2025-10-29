using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="ApplicationWarmupProcess"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Application_Warmup_Process table,
/// which stores definitions of application warmup processes that execute on startup
/// to initialize caches, warm up connections, or perform other startup optimizations.
/// Each process has a name, active status, and associated execution logs.
/// Used to track which warmup processes are configured and enabled.
/// </remarks>
public class ApplicationWarmupProcessConfiguration : IEntityTypeConfiguration<ApplicationWarmupProcess>
{
    /// <summary>
    /// Configures the entity of type <see cref="ApplicationWarmupProcess"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<ApplicationWarmupProcess> builder)
    {
        // Table mapping
        builder.ToTable("Application_Warmup_Process", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Application_Warmup_Process");

        // Properties configuration
        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        builder.Property(e => e.Active)
            .IsRequired()
            .HasColumnName("Active");

        // Relationships
        builder.HasMany(e => e.ApplicationWarmupProcessLogs)
            .WithOne(e => e.Process)
            .HasForeignKey(e => e.ProcessId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Application_Warmup_Process_Log_Process");

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("IX_application_warmup_process_name");

        builder.HasIndex(e => e.Active, "IX_application_warmup_process_active");
        
        builder.HasIndex(e => new { e.Active, e.Name }, "IX_application_warmup_process_active_name");
    }
}
