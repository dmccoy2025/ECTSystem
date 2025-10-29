using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpLkupAccessStatus"/> entity.
/// Configures a staging table for importing access status lookup mappings from external systems.
/// </summary>
/// <remarks>
/// ImpLkupAccessStatus is a temporary staging table used during data import processes to map
/// account status strings (e.g., "OPEN", "LOCKED", "EXPIRED") to their corresponding status IDs
/// in the CoreLkupAccessStatus lookup table. This entity has no primary key (keyless entity)
/// as it represents transient import data used for access status normalization.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable properties to accommodate raw import data
/// - Account status string to status ID mapping
/// - Used for access status standardization during import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful import and validation
/// </remarks>
public class ImpLkupAccessStatusConfiguration : IEntityTypeConfiguration<ImpLkupAccessStatus>
{
    /// <summary>
    /// Configures the ImpLkupAccessStatus entity as a keyless staging table with access status
    /// lookup mapping fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpLkupAccessStatus.</param>
    public void Configure(EntityTypeBuilder<ImpLkupAccessStatus> builder)
    {
        builder.ToTable("ImpLkupAccessStatus", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Access status mapping properties
        builder.Property(e => e.Accountstatus)
            .HasMaxLength(32)
            .IsUnicode(false)
            .HasColumnName("ACCOUNTSTATUS");

        builder.Property(e => e.StatusId)
            .HasColumnName("STATUS_ID");
        
        // Indexes for common queries
        builder.HasIndex(e => e.Accountstatus, "IX_imp_lkup_access_status_accountstatus");
        
        builder.HasIndex(e => e.StatusId, "IX_imp_lkup_access_status_status_id");
    }
}
