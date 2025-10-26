using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Entity Framework Core configuration for the PreviousWeeksEctcase entity.
/// Configures view for reporting on ECT cases from previous weeks with member information,
/// workflow details, and date ranges for weekly case tracking and metrics.
/// </summary>
public class PreviousWeeksEctcaseConfiguration : IEntityTypeConfiguration<PreviousWeeksEctcase>
{
    /// <summary>
    /// Configures the PreviousWeeksEctcase entity as a keyless view for read-only
    /// access to historical weekly case data with reporting period boundaries.
    /// </summary>
    /// <param name="builder">The entity type builder for PreviousWeeksEctcase.</param>
    public void Configure(EntityTypeBuilder<PreviousWeeksEctcase> builder)
    {
        builder.HasNoKey();

        builder.ToView("vw_PreviousWeeksECTCase");

        builder.Property(e => e.CaseId)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("CASE_ID");
        builder.Property(e => e.Description)
            .HasMaxLength(400)
            .IsUnicode(false)
            .HasColumnName("DESCRIPTION");
        builder.Property(e => e.EndDate).HasColumnName("END_DATE");
        builder.Property(e => e.MemberName)
            .HasMaxLength(81)
            .IsUnicode(false)
            .HasColumnName("MEMBER_NAME");
        builder.Property(e => e.MemberUnit)
            .HasMaxLength(6)
            .IsUnicode(false)
            .HasColumnName("MEMBER_UNIT");
        builder.Property(e => e.ReportEnd)
            .HasMaxLength(8)
            .IsUnicode(false)
            .HasColumnName("REPORT_END");
        builder.Property(e => e.ReportStart)
            .HasMaxLength(8)
            .IsUnicode(false)
            .HasColumnName("REPORT_START");
        builder.Property(e => e.StartDate).HasColumnName("START_DATE");
        builder.Property(e => e.WorkFlowId).HasColumnName("WORK_FLOW_ID");
    }
}
