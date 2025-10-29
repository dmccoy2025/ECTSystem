using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpProcessInstance"/> entity.
/// Configures a staging table for importing workflow process instance data from legacy workflow systems.
/// </summary>
/// <remarks>
/// ImpProcessInstance is a temporary staging table used during data migration processes to load
/// workflow process instance execution records from legacy workflow management systems. This entity
/// tracks specific executions of workflow processes, including when they started, ended, who was
/// assigned, completion status, due dates, and deployment associations. Each instance represents
/// a single execution of a workflow process definition. This entity has no primary key (keyless entity)
/// as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate legacy data variations
/// - Process instance identification (PI_ID)
/// - Process definition reference (PROC_ID, process name)
/// - Case association (LOD_ID for Line of Duty cases)
/// - Personnel assignment (PERS_ID)
/// - Execution timeline (StartDate, EndDate, DueDate, ExpirationDate, ReqCompleteDate as strings)
/// - Completion tracking (CompletedYn flag)
/// - Instance type classification (InstType)
/// - Script closure tracking (ScriptClosed)
/// - Deployment association (DeploymentID)
/// - String-based audit fields for flexible import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production workflow instance tables
/// </remarks>
public class ImpProcessInstanceConfiguration : IEntityTypeConfiguration<ImpProcessInstance>
{
    /// <summary>
    /// Configures the ImpProcessInstance entity as a keyless staging table with workflow
    /// process instance import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpProcessInstance.</param>
    public void Configure(EntityTypeBuilder<ImpProcessInstance> builder)
    {
        builder.ToTable("ImpProcessInstance", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Process instance identification
        builder.Property(e => e.PiId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PI_ID");

        // Process definition reference
        builder.Property(e => e.ProcId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PROC_ID");

        builder.Property(e => e.ProcessName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("PROCESS_NAME");

        // Case and personnel associations
        builder.Property(e => e.LodId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOD_ID");

        builder.Property(e => e.PersId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PERS_ID");

        // Execution timeline (string-based for flexible import)
        builder.Property(e => e.StartDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("START_DATE");

        builder.Property(e => e.EndDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("END_DATE");

        builder.Property(e => e.DueDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DUE_DATE");

        builder.Property(e => e.ExpirationDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("EXPIRATION_DATE");

        builder.Property(e => e.ReqCompleteDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("REQ_COMPLETE_DATE");

        // Completion and instance type tracking
        builder.Property(e => e.CompletedYn)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("COMPLETED_YN");

        builder.Property(e => e.InstType)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("INST_TYPE");

        builder.Property(e => e.ScriptClosed)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("SCRIPT_CLOSED");

        // Deployment association
        builder.Property(e => e.DeploymentId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DEPLOYMENT_ID");

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
        builder.HasIndex(e => e.PiId, "IX_imp_process_instance_pi_id");
        
        builder.HasIndex(e => e.ProcId, "IX_imp_process_instance_proc_id");
        
        builder.HasIndex(e => e.LodId, "IX_imp_process_instance_lod_id");
        
        builder.HasIndex(e => e.PersId, "IX_imp_process_instance_pers_id");
        
        builder.HasIndex(e => e.CompletedYn, "IX_imp_process_instance_completed_yn");
        
        builder.HasIndex(e => e.StartDate, "IX_imp_process_instance_start_date");
    }
}
