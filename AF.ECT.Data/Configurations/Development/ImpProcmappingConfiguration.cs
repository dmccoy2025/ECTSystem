using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpProcmapping"/> entity.
/// Configures a staging table for mapping legacy ALOD work status codes to new RCPHA process
/// valid status identifiers.
/// </summary>
/// <remarks>
/// ImpProcmapping is a temporary staging table used during data migration processes to establish
/// the mapping between legacy ALOD (Army Lodging) work status codes and new RCPHA (Reserve Component
/// Physical Health Assessment) process valid status IDs. This entity facilitates the translation
/// of workflow states during migration from the old system to the new system. This entity has
/// no primary key (keyless entity) as it represents transient mapping data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for staging
/// - Required process valid status ID (non-nullable integer)
/// - Required process ID (non-nullable integer)
/// - Nullable ALOD work status for legacy compatibility
/// - Used for workflow state mapping during migration
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful workflow migration and validation
/// </remarks>
public class ImpProcmappingConfiguration : IEntityTypeConfiguration<ImpProcmapping>
{
    /// <summary>
    /// Configures the ImpProcmapping entity as a keyless staging table with process status
    /// mapping fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpProcmapping.</param>
    public void Configure(EntityTypeBuilder<ImpProcmapping> builder)
    {
        builder.ToTable("ImpPROCMapping", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Process mapping properties
        builder.Property(e => e.PvsId)
            .HasColumnName("PVS_ID");

        builder.Property(e => e.ProcId)
            .HasColumnName("PROC_ID");

        builder.Property(e => e.AlodWorkstatus)
            .HasColumnName("ALOD_WORKSTATUS");
        
        // Indexes for common queries
        builder.HasIndex(e => e.PvsId, "IX_imp_proc_mapping_pvs_id");
        
        builder.HasIndex(e => e.ProcId, "IX_imp_proc_mapping_proc_id");
        
        builder.HasIndex(e => e.AlodWorkstatus, "IX_imp_proc_mapping_alod_workstatus");
    }
}
