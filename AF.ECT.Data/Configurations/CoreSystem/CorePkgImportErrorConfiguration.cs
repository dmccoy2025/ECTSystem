using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework configuration for the CorePkgImportError entity.
/// </summary>
public class CorePkgImportErrorConfiguration : IEntityTypeConfiguration<CorePkgImportError>
{
    /// <summary>
    /// Configures the CorePkgImportError entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CorePkgImportError> builder)
    {
        builder.ToTable("Core_PkgImportError", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_PkgImportError");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.StoredProcName)
            .HasMaxLength(255)
            .HasColumnName("StoredProcName");
        builder.Property(e => e.KeyValue)
            .HasMaxLength(255)
            .HasColumnName("KeyValue");
        builder.Property(e => e.PkgLogId).HasColumnName("PkgLogID");
        builder.Property(e => e.Time).HasColumnName("Time");
        builder.Property(e => e.ErrorMessage).HasColumnName("ErrorMessage");

        builder.HasIndex(e => e.PkgLogId, "IX_Core_PkgImportError_PkgLogID");
        builder.HasIndex(e => e.Time, "IX_Core_PkgImportError_Time");
    }
}
