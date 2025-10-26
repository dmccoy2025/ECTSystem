using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Entity Framework Core configuration for the RptPhUserQueryParam entity.
/// Configures query parameters (filter values) for Print Health report clauses
/// with operator types, value ranges, and execution ordering for complex filtering.
/// </summary>
public class RptPhUserQueryParamConfiguration : IEntityTypeConfiguration<RptPhUserQueryParam>
{
    /// <summary>
    /// Configures the RptPhUserQueryParam entity with table mapping, primary key,
    /// properties, and relationship to parent clauses for building dynamic WHERE conditions.
    /// </summary>
    /// <param name="builder">The entity type builder for RptPhUserQueryParam.</param>
    public void Configure(EntityTypeBuilder<RptPhUserQueryParam> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__RPT_PH_U__3214EC272E5F8764");

        builder.ToTable("RPT_PH_USER_QUERY_PARAM", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.EndDisplay)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("END_DISPLAY");
        builder.Property(e => e.EndValue)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("END_VALUE");
        builder.Property(e => e.ExecuteOrder).HasColumnName("EXECUTE_ORDER");
        builder.Property(e => e.OperatorType)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("OPERATOR_TYPE");
        builder.Property(e => e.PhclauseId).HasColumnName("PHCLAUSE_ID");
        builder.Property(e => e.StartDisplay)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("START_DISPLAY");
        builder.Property(e => e.StartValue)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("START_VALUE");
        builder.Property(e => e.WhereType)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("WHERE_TYPE");

        builder.HasOne(d => d.Phclause).WithMany(p => p.RptPhUserQueryParams)
            .HasForeignKey(d => d.PhclauseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_PARAM_PHCLAUSE");

        builder.HasIndex(e => e.PhclauseId, "IX_RPT_PH_USER_QUERY_PARAM_PHCLAUSE");
        builder.HasIndex(e => e.ExecuteOrder, "IX_RPT_PH_USER_QUERY_PARAM_EXECUTE_ORDER");
    }
}
