using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpLodmapping"/> entity.
/// Configures a staging table for mapping legacy ALOD LOD IDs to RCPHA (Reserve Component Physical Health Assessment) LOD IDs.
/// </summary>
/// <remarks>
/// ImpLodmapping is a temporary staging table used during data migration processes to track the mapping
/// between legacy ALOD (Army Lodging) LOD IDs and new RCPHA LOD IDs. This entity has no primary key
/// (keyless entity) as it represents transient mapping data used for cross-referencing during migration.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - Mixed nullable properties (integers and strings) to accommodate mapping data
/// - ALOD LOD ID to RCPHA LOD ID mapping
/// - Original RCPHA LOD ID tracking for reassignments
/// - Case ID association for case linking
/// - Status tracking (ALOD status, RCPHA validation status)
/// - Validation status meaning and procedure name for debugging
/// - Creation date for audit tracking
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration and validation
/// </remarks>
public class ImpLodmappingConfiguration : IEntityTypeConfiguration<ImpLodmapping>
{
    /// <summary>
    /// Configures the ImpLodmapping entity as a keyless staging table with LOD ID mapping fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpLodmapping.</param>
    public void Configure(EntityTypeBuilder<ImpLodmapping> builder)
    {
        builder.ToTable("ImpLODMapping", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // LOD ID mapping properties
        builder.Property(e => e.AlodLodId)
            .HasColumnName("ALOD_LODID");

        builder.Property(e => e.RcphaLodid)
            .HasColumnName("RCPHA_LODID");

        builder.Property(e => e.RcPhaOrgLodid)
            .HasColumnName("RC_PHA_ORG_LODID");

        // Case association
        builder.Property(e => e.CaseId)
            .HasMaxLength(20)
            .IsUnicode(false)
            .HasColumnName("CASE_ID");

        // Status tracking
        builder.Property(e => e.AlodStatus)
            .HasColumnName("ALOD_STATUS");

        builder.Property(e => e.RcphaValidStatus)
            .HasColumnName("RCPHA_VALID_STATUS");

        builder.Property(e => e.RcphaValidStatusMeaning)
            .HasMaxLength(200)
            .IsUnicode(false)
            .HasColumnName("RCPHA_VALID_STATUS_MEANING");

        builder.Property(e => e.RcphaProcName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("RCPHA_PROC_NAME");

        // Audit tracking
        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasColumnName("CREATED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.AlodLodId, "IX_imp_lod_mapping_alod_lod_id");
        
        builder.HasIndex(e => e.RcphaLodid, "IX_imp_lod_mapping_rcpha_lodid");
        
        builder.HasIndex(e => e.CaseId, "IX_imp_lod_mapping_case_id");
        
        builder.HasIndex(e => e.RcphaValidStatus, "IX_imp_lod_mapping_rcpha_valid_status");
        
        builder.HasIndex(e => e.CreatedDate, "IX_imp_lod_mapping_created_date");
    }
}
