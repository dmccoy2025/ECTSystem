using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Entity type configuration for the <see cref="RptQuerySource"/> entity.
/// Configures the schema, table name, primary key, properties, and relationships for report query source catalog.
/// </summary>
public class RptQuerySourceConfiguration : IEntityTypeConfiguration<RptQuerySource>
{
    /// <summary>
    /// Configures the entity of type <see cref="RptQuerySource"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<RptQuerySource> builder)
    {
        builder.ToTable("rptQuerySource", "dbo");

        // Primary Key
        builder.HasKey(e => e.Id)
            .HasName("PK_rptQuerySource");

        // Properties
        builder.Property(e => e.Id)
            .HasColumnName("ID");

        builder.Property(e => e.DisplayName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.FieldName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.TableName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.DataType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.LookupSource)
            .HasMaxLength(100);

        builder.Property(e => e.LookupValue)
            .HasMaxLength(100);

        builder.Property(e => e.LookupText)
            .HasMaxLength(100);

        builder.Property(e => e.LookupSort)
            .HasMaxLength(100);

        builder.Property(e => e.LookupWhere)
            .HasMaxLength(500);

        builder.Property(e => e.LookupWhereValue)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(e => e.TableName, "IX_rptQuerySource_TableName");

        builder.HasIndex(e => e.FieldName, "IX_rptQuerySource_FieldName");

        builder.HasIndex(e => e.DataType, "IX_rptQuerySource_DataType");
    }
}
