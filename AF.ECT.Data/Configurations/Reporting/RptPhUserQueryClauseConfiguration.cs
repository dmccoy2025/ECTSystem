using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Entity Framework Core configuration for the RptPhUserQueryClause entity.
/// Configures query clauses (WHERE conditions) for Print Health user-defined reports
/// with support for AND/OR logic and parameter grouping.
/// </summary>
public class RptPhUserQueryClauseConfiguration : IEntityTypeConfiguration<RptPhUserQueryClause>
{
    /// <summary>
    /// Configures the RptPhUserQueryClause entity with table mapping, primary key,
    /// properties, and relationships to parent queries and query parameters.
    /// </summary>
    /// <param name="builder">The entity type builder for RptPhUserQueryClause.</param>
    public void Configure(EntityTypeBuilder<RptPhUserQueryClause> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__RPT_PH_U__3214EC27B6C8A9D3");

        builder.ToTable("RPT_PH_USER_QUERY_CLAUSE", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.PhqueryId).HasColumnName("PHQUERY_ID");
        builder.Property(e => e.WhereType)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("WHERE_TYPE");

        builder.HasOne(d => d.Phquery).WithMany(p => p.RptPhUserQueryClauses)
            .HasForeignKey(d => d.PhqueryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_CLAUSE_PHQUERY");

        builder.HasMany(d => d.RptPhUserQueryParams).WithOne(p => p.Phclause)
            .HasForeignKey(d => d.PhclauseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_PARAM_PHCLAUSE");

        builder.HasIndex(e => e.PhqueryId, "IX_RPT_PH_USER_QUERY_CLAUSE_PHQUERY");
    }
}
