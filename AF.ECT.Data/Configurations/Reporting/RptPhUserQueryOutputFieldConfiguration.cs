using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Entity Framework Core configuration for the RptPhUserQueryOutputField entity.
/// Configures output field selections for Print Health user-defined reports specifying
/// which form fields to include in report results with section and field type context.
/// </summary>
public class RptPhUserQueryOutputFieldConfiguration : IEntityTypeConfiguration<RptPhUserQueryOutputField>
{
    /// <summary>
    /// Configures the RptPhUserQueryOutputField entity with table mapping, primary key,
    /// and relationships to queries, sections, fields, and field types for report columns.
    /// </summary>
    /// <param name="builder">The entity type builder for RptPhUserQueryOutputField.</param>
    public void Configure(EntityTypeBuilder<RptPhUserQueryOutputField> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__RPT_PH_U__3214EC27C9E8F2A1");

        builder.ToTable("RPT_PH_USER_QUERY_OUTPUT_FIELD", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.FieldId).HasColumnName("FIELD_ID");
        builder.Property(e => e.FieldTypeId).HasColumnName("FIELD_TYPE_ID");
        builder.Property(e => e.PhqueryId).HasColumnName("PHQUERY_ID");
        builder.Property(e => e.SectionId).HasColumnName("SECTION_ID");

        builder.HasOne(d => d.Field).WithMany(p => p.RptPhUserQueryOutputFields)
            .HasForeignKey(d => d.FieldId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_OUTPUT_FIELD_FIELD");

        builder.HasOne(d => d.FieldType).WithMany(p => p.RptPhUserQueryOutputFields)
            .HasForeignKey(d => d.FieldTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_OUTPUT_FIELD_FIELD_TYPE");

        builder.HasOne(d => d.Phquery).WithMany(p => p.RptPhUserQueryOutputFields)
            .HasForeignKey(d => d.PhqueryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_OUTPUT_FIELD_PHQUERY");

        builder.HasOne(d => d.Section).WithMany(p => p.RptPhUserQueryOutputFields)
            .HasForeignKey(d => d.SectionId)
            .HasConstraintName("FK_RPT_PH_USER_QUERY_OUTPUT_FIELD_SECTION");

        builder.HasIndex(e => e.PhqueryId, "IX_RPT_PH_USER_QUERY_OUTPUT_FIELD_PHQUERY");
    }
}
