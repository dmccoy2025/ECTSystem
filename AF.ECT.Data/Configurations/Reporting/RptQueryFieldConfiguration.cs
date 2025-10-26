using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Entity Framework Core configuration for the RptQueryField entity.
/// Configures report query field metadata defining available fields for dynamic reporting
/// with table names, query types, and sort ordering for report builder functionality.
/// </summary>
public class RptQueryFieldConfiguration : IEntityTypeConfiguration<RptQueryField>
{
    /// <summary>
    /// Configures the RptQueryField entity with table mapping, primary key, and properties
    /// for report field catalog and query construction support.
    /// </summary>
    /// <param name="builder">The entity type builder for RptQueryField.</param>
    public void Configure(EntityTypeBuilder<RptQueryField> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__RPT_QUER__3214EC27C04B7573");

        builder.ToTable("RPT_QUERY_FIELD", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.FieldName)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("FIELD_NAME");
        builder.Property(e => e.QueryType)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("QUERY_TYPE");
        builder.Property(e => e.SortOrder).HasColumnName("SORT_ORDER");
        builder.Property(e => e.TableName)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("TABLE_NAME");

        builder.HasIndex(e => e.QueryType, "IX_RPT_QUERY_FIELD_QUERY_TYPE");
        builder.HasIndex(e => e.TableName, "IX_RPT_QUERY_FIELD_TABLE_NAME");
        builder.HasIndex(e => e.SortOrder, "IX_RPT_QUERY_FIELD_SORT_ORDER");
    }
}
