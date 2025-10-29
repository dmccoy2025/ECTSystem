using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpProcess"/> entity.
/// Configures a staging table for importing workflow process definitions from legacy workflow systems.
/// </summary>
/// <remarks>
/// ImpProcess is a temporary staging table used during data migration processes to load complete
/// workflow process definitions from legacy workflow management systems. This entity captures
/// comprehensive process metadata including identification, classification, hierarchy, versioning,
/// configuration flags, form associations, validation rules, and execution parameters. This entity
/// has no primary key (keyless entity) as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate legacy data variations
/// - Process identification (PROC_ID, process name, CS_ID for command structure)
/// - Process classification (type, classification, program code)
/// - Hierarchical structure (ParentPROC_ID for process hierarchy)
/// - Versioning (version number, version date)
/// - Configuration flags (Active, MultRespAllowed, RespRequired, ProcessHelp, WorkflowMultiple, NewWindow)
/// - Paper form association (paper form number, months valid)
/// - Execution configuration (routine type/name, default result)
/// - Inbox parameters (inbox param/value for workflow routing)
/// - Description and help text
/// - String-based audit fields for flexible import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production workflow process tables
/// </remarks>
public class ImpProcessConfiguration : IEntityTypeConfiguration<ImpProcess>
{
    /// <summary>
    /// Configures the ImpProcess entity as a keyless staging table with workflow process
    /// definition import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpProcess.</param>
    public void Configure(EntityTypeBuilder<ImpProcess> builder)
    {
        builder.ToTable("ImpProcess", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Process identification
        builder.Property(e => e.ProcId).HasMaxLength(50).IsUnicode(false).HasColumnName("PROC_ID");
        builder.Property(e => e.CsId).HasMaxLength(50).IsUnicode(false).HasColumnName("CS_ID");
        builder.Property(e => e.ProcessName).HasMaxLength(100).IsUnicode(false).HasColumnName("PROCESS_NAME");
        builder.Property(e => e.ProgramCode).HasMaxLength(20).IsUnicode(false).HasColumnName("PROGRAM_CODE");

        // Process classification
        builder.Property(e => e.ProcType).HasMaxLength(50).IsUnicode(false).HasColumnName("PROC_TYPE");
        builder.Property(e => e.ProcClassification).HasMaxLength(50).IsUnicode(false).HasColumnName("PROC_CLASSIFICATION");

        // Hierarchical structure
        builder.Property(e => e.ParentProcId).HasMaxLength(50).IsUnicode(false).HasColumnName("PARENT_PROC_ID");

        // Versioning
        builder.Property(e => e.Version).HasMaxLength(20).IsUnicode(false).HasColumnName("VERSION");
        builder.Property(e => e.VersionDate).HasMaxLength(50).IsUnicode(false).HasColumnName("VERSION_DATE");

        // Configuration flags
        builder.Property(e => e.ActiveYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("ACTIVE_YN");
        builder.Property(e => e.MultRespAllowedYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("MULT_RESP_ALLOWED_YN");
        builder.Property(e => e.RespRequiredYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("RESP_REQUIRED_YN");
        builder.Property(e => e.ProcessHelpYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("PROCESS_HELP_YN");
        builder.Property(e => e.WorkflowMultipleYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("WORKFLOW_MULTIPLE_YN");
        builder.Property(e => e.NewWindowYn).HasMaxLength(1).IsUnicode(false).IsFixedLength().HasColumnName("NEW_WINDOW_YN");

        // Paper form association
        builder.Property(e => e.PaperForm).HasMaxLength(50).IsUnicode(false).HasColumnName("PAPER_FORM");
        builder.Property(e => e.MonthsValid).HasMaxLength(10).IsUnicode(false).HasColumnName("MONTHS_VALID");

        // Execution configuration
        builder.Property(e => e.RoutineType).HasMaxLength(50).IsUnicode(false).HasColumnName("ROUTINE_TYPE");
        builder.Property(e => e.RoutineName).HasMaxLength(100).IsUnicode(false).HasColumnName("ROUTINE_NAME");
        builder.Property(e => e.DefaultResult).HasMaxLength(100).IsUnicode(false).HasColumnName("DEFAULT_RESULT");

        // Inbox routing parameters
        builder.Property(e => e.InboxParam).HasMaxLength(100).IsUnicode(false).HasColumnName("INBOX_PARAM");
        builder.Property(e => e.InboxValue).HasMaxLength(100).IsUnicode(false).HasColumnName("INBOX_VALUE");

        // Description
        builder.Property(e => e.Description).HasColumnType("ntext").HasColumnName("DESCRIPTION");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("CREATED_DATE");
        builder.Property(e => e.ModifiedBy).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_BY");
        builder.Property(e => e.ModifiedDate).HasMaxLength(50).IsUnicode(false).HasColumnName("MODIFIED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.ProcId, "IX_imp_process_proc_id");
        
        builder.HasIndex(e => e.ProcessName, "IX_imp_process_process_name");
        
        builder.HasIndex(e => e.ActiveYn, "IX_imp_process_active_yn");
        
        builder.HasIndex(e => e.ProcType, "IX_imp_process_proc_type");
        
        builder.HasIndex(e => e.ParentProcId, "IX_imp_process_parent_proc_id");
    }
}
