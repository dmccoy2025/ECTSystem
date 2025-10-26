using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Logging;

/// <summary>
/// Entity Framework Core configuration for the PkgLog entity.
/// Configures SSIS package execution logging tracking data import statistics including
/// rows inserted, updated, deleted, and source file information for auditing purposes.
/// </summary>
public class PkgLogConfiguration : IEntityTypeConfiguration<PkgLog>
{
    /// <summary>
    /// Configures the PkgLog entity with table mapping, primary key, and properties
    /// for comprehensive SSIS package execution tracking and performance metrics.
    /// </summary>
    /// <param name="builder">The entity type builder for PkgLog.</param>
    public void Configure(EntityTypeBuilder<PkgLog> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK__PKG_LOG__3214EC27E0D18A88");

        builder.ToTable("PKG_LOG", "dbo");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Endtime).HasColumnName("ENDTIME");
        builder.Property(e => e.NDeletedMembers).HasColumnName("N_DELETED_MEMBERS");
        builder.Property(e => e.NModifiedRecords).HasColumnName("N_MODIFIED_RECORDS");
        builder.Property(e => e.NRowInserted).HasColumnName("N_ROW_INSERTED");
        builder.Property(e => e.NRowRawInserted).HasColumnName("N_ROW_RAW_INSERTED");
        builder.Property(e => e.NRowUpdated).HasColumnName("N_ROW_UPDATED");
        builder.Property(e => e.PkgName)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("PKG_NAME");
        builder.Property(e => e.SourceFileName)
            .HasMaxLength(255)
            .IsUnicode(false)
            .HasColumnName("SOURCE_FILE_NAME");
        builder.Property(e => e.Starttime).HasColumnName("STARTTIME");

        builder.HasIndex(e => e.PkgName, "IX_PKG_LOG_PKG_NAME");
        builder.HasIndex(e => e.Starttime, "IX_PKG_LOG_STARTTIME");
    }
}
