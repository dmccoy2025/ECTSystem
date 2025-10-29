using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOraclePersonProcessDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_Person_Process_Data table,
/// which stores personnel process workflow data from AFRC Oracle system migration.
/// Contains process tracking information including process name, status, LOD association,
/// personnel involvement, start/end dates, completion status, results, and remarks.
/// All properties are nullable strings for Oracle import staging. Tracks workflow
/// processes for personnel-related activities in legacy Oracle system.
/// </remarks>
public class AfrcOraclePersonProcessDatumConfiguration : IEntityTypeConfiguration<AfrcOraclePersonProcessDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOraclePersonProcessDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOraclePersonProcessDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_Person_Process_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration
        builder.Property(e => e.PpId)
            .HasColumnName("pp_id");

        builder.Property(e => e.PiId)
            .HasColumnName("pi_id");

        builder.Property(e => e.ProcessName)
            .HasColumnName("process_name");

        builder.Property(e => e.PvsId)
            .HasColumnName("pvs_id");

        builder.Property(e => e.StatusMeaning)
            .HasColumnName("status_meaning");

        builder.Property(e => e.LodId)
            .HasColumnName("lod_id");

        builder.Property(e => e.ProcessedBy)
            .HasColumnName("processed_by");

        builder.Property(e => e.StartDate)
            .HasColumnName("start_date");

        builder.Property(e => e.EndDate)
            .HasColumnName("end_date");

        builder.Property(e => e.CompletedYn)
            .HasColumnName("completed_yn");

        builder.Property(e => e.FinalResult)
            .HasColumnName("final_result");

        builder.Property(e => e.Remark)
            .HasColumnName("remark");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date");

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("modified_by");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("modified_date");

        builder.Property(e => e.CallingPpId)
            .HasColumnName("calling_pp_id");

        // Indexes for common queries
        builder.HasIndex(e => e.PpId, "IX_afrc_oracle_person_process_pp_id");

        builder.HasIndex(e => e.LodId, "IX_afrc_oracle_person_process_lod_id");

        builder.HasIndex(e => e.ProcessName, "IX_afrc_oracle_person_process_process_name");
        
        builder.HasIndex(e => e.CreatedDate, "IX_afrc_oracle_person_process_created_date");
        
        builder.HasIndex(e => e.ModifiedDate, "IX_afrc_oracle_person_process_modified_date");
        
        builder.HasIndex(e => e.CompletedYn, "IX_afrc_oracle_person_process_completed_yn");
        
        builder.HasIndex(e => e.StartDate, "IX_afrc_oracle_person_process_start_date");
    }
}
