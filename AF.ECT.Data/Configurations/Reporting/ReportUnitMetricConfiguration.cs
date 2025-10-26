using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Entity Framework Core configuration for the ReportUnitMetric entity.
/// Configures unit-level metrics for reporting including medical staff counts,
/// unit personnel, wing leadership, and case processing statistics.
/// </summary>
public class ReportUnitMetricConfiguration : IEntityTypeConfiguration<ReportUnitMetric>
{
    /// <summary>
    /// Configures the ReportUnitMetric entity with table mapping, primary key,
    /// and properties for comprehensive unit performance and staffing metrics.
    /// </summary>
    /// <param name="builder">The entity type builder for ReportUnitMetric.</param>
    public void Configure(EntityTypeBuilder<ReportUnitMetric> builder)
    {
        builder.HasKey(e => e.CsId).HasName("PK__REPORT_U__F2FEFB5E4F96E8CE");

        builder.ToTable("REPORT_UNIT_METRICS", "dbo");

        builder.Property(e => e.CsId).HasColumnName("CS_ID");
        builder.Property(e => e.BoardCount).HasColumnName("BOARD_COUNT");
        builder.Property(e => e.InvCount).HasColumnName("INV_COUNT");
        builder.Property(e => e.MedOfficerCount).HasColumnName("MED_OFFICER_COUNT");
        builder.Property(e => e.MedTechCount).HasColumnName("MED_TECH_COUNT");
        builder.Property(e => e.UnitCount).HasColumnName("UNIT_COUNT");
        builder.Property(e => e.WindCccount).HasColumnName("WIND_CCCOUNT");
        builder.Property(e => e.WingJacount).HasColumnName("WING_JACOUNT");

        builder.HasIndex(e => e.CsId, "IX_REPORT_UNIT_METRICS_CS_ID");
    }
}
