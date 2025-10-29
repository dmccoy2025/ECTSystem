using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpPersonProcess"/> entity.
/// Configures a staging table for importing person-specific process execution data from legacy workflow systems.
/// </summary>
/// <remarks>
/// ImpPersonProcess is a temporary staging table used during data migration processes to load
/// person-process relationship data from legacy workflow management systems. This entity tracks
/// individual person assignments to workflow processes, their status progression, completion dates,
/// results, and remarks. It represents the execution history of processes assigned to specific
/// personnel. This entity has no primary key (keyless entity) as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - Mixed nullable properties (strings and DateTimes) to accommodate legacy data variations
/// - Person-process execution tracking (PP_ID as person-process identifier)
/// - Process instance association (PI_ID)
/// - Process identification (process name)
/// - Status tracking (PVS_ID for process valid status, status meaning)
/// - LOD case association (LOD_ID)
/// - Personnel assignment (ProcessedBy)
/// - Execution timeline (StartDate, EndDate as DateTime for temporal tracking)
/// - Completion flag and final result
/// - Remarks for additional context
/// - Calling process tracking (CallingPP_ID for hierarchical workflows)
/// - String-based audit fields for flexible import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production workflow tables
/// </remarks>
public class ImpPersonProcessConfiguration : IEntityTypeConfiguration<ImpPersonProcess>
{
    /// <summary>
    /// Configures the ImpPersonProcess entity as a keyless staging table with person-process
    /// execution import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpPersonProcess.</param>
    public void Configure(EntityTypeBuilder<ImpPersonProcess> builder)
    {
        builder.ToTable("ImpPersonProcess", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Person-process identification
        builder.Property(e => e.PpId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PP_ID");

        builder.Property(e => e.PiId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PI_ID");

        // Process identification
        builder.Property(e => e.ProcessName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("PROCESS_NAME");

        // Status tracking
        builder.Property(e => e.PvsId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PVS_ID");

        builder.Property(e => e.StatusMeaning)
            .HasMaxLength(200)
            .IsUnicode(false)
            .HasColumnName("STATUS_MEANING");

        // LOD association
        builder.Property(e => e.LodId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOD_ID");

        // Personnel assignment
        builder.Property(e => e.ProcessedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("PROCESSED_BY");

        // Execution timeline (DateTime for temporal operations)
        builder.Property(e => e.StartDate)
            .HasColumnType("datetime")
            .HasColumnName("START_DATE");

        builder.Property(e => e.EndDate)
            .HasColumnType("datetime")
            .HasColumnName("END_DATE");

        // Completion tracking
        builder.Property(e => e.CompletedYn)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("COMPLETED_YN");

        builder.Property(e => e.FinalResult)
            .HasMaxLength(200)
            .IsUnicode(false)
            .HasColumnName("FINAL_RESULT");

        // Remarks
        builder.Property(e => e.Remark)
            .HasColumnType("ntext")
            .HasColumnName("REMARK");

        // Hierarchical workflow tracking
        builder.Property(e => e.CallingPpId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CALLING_PP_ID");

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
        builder.HasIndex(e => e.PpId, "IX_imp_person_process_pp_id");
        
        builder.HasIndex(e => e.PiId, "IX_imp_person_process_pi_id");
        
        builder.HasIndex(e => e.LodId, "IX_imp_person_process_lod_id");
        
        builder.HasIndex(e => e.ProcessedBy, "IX_imp_person_process_processed_by");
        
        builder.HasIndex(e => e.CompletedYn, "IX_imp_person_process_completed_yn");
        
        builder.HasIndex(e => e.StartDate, "IX_imp_person_process_start_date");
    }
}
