using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpHistory"/> entity.
/// Configures a staging table for importing user workflow history data from legacy ALOD systems.
/// </summary>
/// <remarks>
/// ImpHistory is a temporary staging table used during data migration processes to load user
/// workflow history from legacy ALOD (Army Lodging) systems. This entity has no primary key
/// (keyless entity) as it represents transient import data used for historical workflow
/// tracking migration.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable properties to accommodate incomplete historical data
/// - User and workflow tracking (ALOD user ID, workflow status ID, LOD ID)
/// - Date range tracking (start date, end date)
/// - Username association for user mapping
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production history tables
/// </remarks>
public class ImpHistoryConfiguration : IEntityTypeConfiguration<ImpHistory>
{
    /// <summary>
    /// Configures the ImpHistory entity as a keyless staging table with workflow history import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpHistory.</param>
    public void Configure(EntityTypeBuilder<ImpHistory> builder)
    {
        builder.ToTable("ImpHistory", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // User identification properties
        builder.Property(e => e.AlodUserid)
            .HasColumnName("ALOD_USERID");

        builder.Property(e => e.Username)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("USERNAME");

        // Workflow tracking properties
        builder.Property(e => e.StartDate)
            .HasColumnType("datetime")
            .HasColumnName("START_DATE");

        builder.Property(e => e.EndDate)
            .HasColumnType("datetime")
            .HasColumnName("END_DATE");

        builder.Property(e => e.WsId)
            .HasColumnName("WS_ID");

        builder.Property(e => e.AlodLodid)
            .HasColumnName("ALOD_LODID");
        
        // Indexes for common queries
        builder.HasIndex(e => e.AlodUserid, "IX_imp_history_alod_userid");
        
        builder.HasIndex(e => e.Username, "IX_imp_history_username");
        
        builder.HasIndex(e => e.StartDate, "IX_imp_history_start_date");
        
        builder.HasIndex(e => e.WsId, "IX_imp_history_ws_id");
        
        builder.HasIndex(e => e.AlodLodid, "IX_imp_history_alod_lodid");
    }
}
