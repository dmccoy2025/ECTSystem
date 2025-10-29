using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework configuration for the <see cref="Lodmapping"/> entity.
/// Configures a production mapping table for LOD ID translation between legacy ALOD and new RCPHA systems.
/// </summary>
/// <remarks>
/// Lodmapping is a production reference table that maintains the permanent mapping between legacy
/// ALOD (Army Lodging) LOD IDs and new RCPHA (Reserve Component Physical Health Assessment) LOD IDs.
/// Unlike the temporary ImpLODMapping staging table, this entity represents the finalized, validated
/// mapping used for ongoing cross-referencing between old and new LOD case identifiers. This entity
/// has no primary key (keyless entity) as it serves as a cross-reference mapping table.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for cross-reference mapping
/// - Nullable integer properties for flexible ID mapping
/// - Legacy ALOD LOD ID to new RCPHA LOD ID mapping
/// - Original RCPHA LOD ID tracking (RcPhaOrgLodid) for reassignments
/// - Case ID association for case linking
/// - Status tracking (ALOD status, RCPHA validation status)
/// - Procedure name for workflow process identification
/// - Creation date (DateTime) for audit tracking
/// - Used for bidirectional LOD case lookups between systems
/// - Supports legacy system integration and case history
/// - No foreign key relationships (reference table isolation)
/// - Permanent production data (unlike temporary ImpLODMapping)
/// </remarks>
public class LodmappingConfiguration : IEntityTypeConfiguration<Lodmapping>
{
    /// <summary>
    /// Configures the Lodmapping entity as a keyless production mapping table with LOD ID
    /// cross-reference fields.
    /// </summary>
    /// <param name="builder">The entity type builder for Lodmapping.</param>
    public void Configure(EntityTypeBuilder<Lodmapping> builder)
    {
        builder.ToTable("LODMapping", "dbo");

        // Keyless entity for cross-reference mapping
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

        builder.Property(e => e.RcPhaValidStatus)
            .HasColumnName("RC_PHA_VALID_STATUS");

        // Procedure name
        builder.Property(e => e.RcphaProcName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("RCPHA_PROC_NAME");

        // Audit tracking
        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasColumnName("CREATED_DATE");
        
        // Indexes for LOD ID lookups
        builder.HasIndex(e => e.AlodLodId, "IX_LODMapping_ALOD_LODID");
        
        builder.HasIndex(e => e.RcphaLodid, "IX_LODMapping_RCPHA_LODID");
        
        builder.HasIndex(e => e.CaseId, "IX_LODMapping_CASE_ID");
        
        builder.HasIndex(e => e.CreatedDate, "IX_LODMapping_CREATED_DATE");
    }
}
