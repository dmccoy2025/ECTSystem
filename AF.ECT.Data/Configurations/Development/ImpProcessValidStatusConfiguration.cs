using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpProcessValidStatus"/> entity.
/// Configures a staging table for importing workflow process valid status definitions from legacy workflow systems.
/// </summary>
/// <remarks>
/// ImpProcessValidStatus is a temporary staging table used during data migration processes to load
/// valid status definitions for workflow processes from legacy workflow management systems. This entity
/// defines the allowable statuses for each process, their sequencing, meaning, requirements, and
/// whether they close the process. It establishes the state machine for workflow progression.
/// This entity has no primary key (keyless entity) as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate legacy data variations
/// - Process valid status identification (PVS_ID)
/// - Process association (PROC_ID, process name)
/// - Status definition (PROC_STATUS code, status meaning)
/// - Status sequencing (STATUS_SEQ for ordering)
/// - Status requirements (RequiredYN flag)
/// - Process closure tracking (ClosesProcessYN flag)
/// - Responsibility level assignment
/// - String-based audit fields for flexible import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production workflow status tables
/// </remarks>
public class ImpProcessValidStatusConfiguration : IEntityTypeConfiguration<ImpProcessValidStatus>
{
    /// <summary>
    /// Configures the ImpProcessValidStatus entity as a keyless staging table with workflow
    /// process valid status definition import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpProcessValidStatus.</param>
    public void Configure(EntityTypeBuilder<ImpProcessValidStatus> builder)
    {
        builder.ToTable("ImpProcessValidStatus", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Process valid status identification
        builder.Property(e => e.PvsId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PVS_ID");

        // Process association
        builder.Property(e => e.ProcId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PROC_ID");

        builder.Property(e => e.ProcessName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("PROCESS_NAME");

        // Status definition
        builder.Property(e => e.ProcStatus)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PROC_STATUS");

        builder.Property(e => e.StatusMeaning)
            .HasMaxLength(200)
            .IsUnicode(false)
            .HasColumnName("STATUS_MEANING");

        // Status sequencing
        builder.Property(e => e.StatusSeq)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("STATUS_SEQ");

        // Status requirements and closure
        builder.Property(e => e.RequiredYn)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("REQUIRED_YN");

        builder.Property(e => e.ClosesProcessYn)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("CLOSES_PROCESS_YN");

        // Responsibility level
        builder.Property(e => e.ResponsibilityLevel)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RESPONSIBILITY_LEVEL");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_BY");

        builder.Property(e => e.CreatedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_DATE");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_BY");

        builder.Property(e => e.ModifiedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MODIFIED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.PvsId, "IX_imp_process_valid_status_pvs_id");
        
        builder.HasIndex(e => e.ProcId, "IX_imp_process_valid_status_proc_id");
        
        builder.HasIndex(e => e.ProcStatus, "IX_imp_process_valid_status_proc_status");
        
        builder.HasIndex(e => e.StatusSeq, "IX_imp_process_valid_status_status_seq");
        
        builder.HasIndex(e => e.RequiredYn, "IX_imp_process_valid_status_required_yn");
        
        builder.HasIndex(e => e.ClosesProcessYn, "IX_imp_process_valid_status_closes_process_yn");
    }
}
