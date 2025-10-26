using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Configures the <see cref="DataElementDetail"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents metadata for data elements including schema, table, column information,
/// and data lineage tracking.
/// </remarks>
public class DataElementDetailConfiguration : IEntityTypeConfiguration<DataElementDetail>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<DataElementDetail> builder)
    {
        // Table mapping
        builder.ToTable("DataElementDetail", "dbo");

        // Primary key
        builder.HasKey(e => e.DataElementId)
            .HasName("PK_DataElementDetail");

        // Properties
        builder.Property(e => e.DataElementId).HasColumnName("DataElementID");
        builder.Property(e => e.DataElementName).HasColumnName("DataElementName").HasMaxLength(255).IsRequired();
        builder.Property(e => e.DataElementType).HasColumnName("DataElementType").HasMaxLength(100).IsRequired();
        builder.Property(e => e.DataType).HasColumnName("DataType").HasMaxLength(100).IsRequired();
        builder.Property(e => e.DataPurpose).HasColumnName("DataPurpose").IsRequired();
        builder.Property(e => e.EntityPurpose).HasColumnName("EntityPurpose").IsRequired();
        builder.Property(e => e.DatabaseServer).HasColumnName("DatabaseServer").HasMaxLength(255).IsRequired();
        builder.Property(e => e.DatabaseName).HasColumnName("DatabaseName").HasMaxLength(255).IsRequired();
        builder.Property(e => e.SchemaName).HasColumnName("SchemaName").HasMaxLength(255).IsRequired();
        builder.Property(e => e.TableName).HasColumnName("TableName").HasMaxLength(255).IsRequired();
        builder.Property(e => e.ColumnName).HasColumnName("ColumnName").HasMaxLength(255).IsRequired();
        builder.Property(e => e.OriginalDataSource).HasColumnName("OriginalDataSource").IsRequired();
        builder.Property(e => e.Notes).HasColumnName("Notes").IsRequired();
        builder.Property(e => e.ForeignKeyTo).HasColumnName("ForeignKeyTo").HasMaxLength(500);
        builder.Property(e => e.RowCount).HasColumnName("RowCount");
        builder.Property(e => e.DataStartTime).HasColumnName("DataStartTime");
        builder.Property(e => e.DataEndTime).HasColumnName("DataEndTime");
        builder.Property(e => e.CreateTime).HasColumnName("CreateTime");
        builder.Property(e => e.LastUpdateTime).HasColumnName("LastUpdateTime");
        builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
        builder.Property(e => e.IsNullable).HasColumnName("IsNullable");

        // Indexes for query performance
        builder.HasIndex(e => new { e.TableName, e.ColumnName }, "IX_DataElementDetail_Table_Column");
        builder.HasIndex(e => e.DataElementName, "IX_DataElementDetail_DataElementName");
        builder.HasIndex(e => e.LastUpdateTime, "IX_DataElementDetail_LastUpdateTime");
    }
}
