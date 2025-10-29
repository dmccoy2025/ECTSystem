using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOracleLirPersonnelDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_LIR_Personnel_Data table,
/// which stores personnel involved in Line of Investigation Report (LIR) cases from AFRC Oracle migration.
/// Contains personnel identification (name, SSN, grade) and LOD investigation flag.
/// All properties are nullable strings for Oracle import staging.
/// </remarks>
public class AfrcOracleLirPersonnelDatumConfiguration : IEntityTypeConfiguration<AfrcOracleLirPersonnelDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOracleLirPersonnelDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOracleLirPersonnelDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_LIR_Personnel_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration
        builder.Property(e => e.LiId)
            .HasColumnName("li_id");

        builder.Property(e => e.LirId)
            .HasColumnName("lir_id");

        builder.Property(e => e.Name)
            .HasColumnName("name");

        builder.Property(e => e.Ssn)
            .HasColumnName("ssn");

        builder.Property(e => e.Grade)
            .HasColumnName("grade");

        builder.Property(e => e.LodInvestigationYn)
            .HasColumnName("lod_investigation_yn");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date");

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("modified_by");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("modified_date");

        // Indexes for common queries
        builder.HasIndex(e => e.LirId, "IX_afrc_oracle_lir_personnel_lir_id");

        builder.HasIndex(e => e.Ssn, "IX_afrc_oracle_lir_personnel_ssn");
    }
}
