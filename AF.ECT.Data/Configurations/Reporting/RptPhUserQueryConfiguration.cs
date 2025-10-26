using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Entity Framework Core configuration for the RptPhUserQuery entity.
/// Configures user-defined Print Health report queries with report type, sorting,
/// and relationships to query clauses and output field definitions.
/// </summary>
public class RptPhUserQueryConfiguration : IEntityTypeConfiguration<RptPhUserQuery>
{
    /// <summary>
    /// Configures the RptPhUserQuery entity with table mapping, primary key, properties,
    /// and relationships for building dynamic Print Health reports with custom criteria.
    /// </summary>
    /// <param name="builder">The entity type builder for RptPhUserQuery.</param>
    public void Configure(EntityTypeBuilder<RptPhUserQuery> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__RPT_PH_U__3214EC27DA7EC3EF");

        builder.ToTable("RPT_PH_USER_QUERY", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.ReportType).HasColumnName("REPORT_TYPE");
        builder.Property(e => e.SortField)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("SORT_FIELD");
        builder.Property(e => e.UserQueryId).HasColumnName("USER_QUERY_ID");

        builder.HasOne(d => d.UserQuery).WithMany(p => p.RptPhUserQueries)
            .HasForeignKey(d => d.UserQueryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_USER_QUERY");

        builder.HasMany(d => d.RptPhUserQueryClauses).WithOne(p => p.Phquery)
            .HasForeignKey(d => d.PhqueryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_CLAUSE_PHQUERY");

        builder.HasMany(d => d.RptPhUserQueryOutputFields).WithOne(p => p.Phquery)
            .HasForeignKey(d => d.PhqueryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_OUTPUT_FIELD_PHQUERY");

        builder.HasIndex(e => e.UserQueryId, "IX_RPT_PH_USER_QUERY_USER_QUERY");
        builder.HasIndex(e => e.ReportType, "IX_RPT_PH_USER_QUERY_REPORT_TYPE");
    }
}
