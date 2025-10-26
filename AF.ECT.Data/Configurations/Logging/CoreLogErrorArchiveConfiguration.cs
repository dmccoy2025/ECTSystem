using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Logging;

/// <summary>
/// Entity Framework configuration for the CoreLogErrorArchive entity.
/// </summary>
public class CoreLogErrorArchiveConfiguration : IEntityTypeConfiguration<CoreLogErrorArchive>
{
    /// <summary>
    /// Configures the CoreLogErrorArchive entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLogErrorArchive> builder)
    {
        builder.ToTable("Core_LogErrorArchive", "dbo");

        builder.HasKey(e => e.LogId)
            .HasName("PK_Core_LogErrorArchive");

        builder.Property(e => e.LogId).HasColumnName("LogID");
        builder.Property(e => e.ErrorTime).HasColumnName("ErrorTime");
        builder.Property(e => e.UserName)
            .HasMaxLength(255)
            .HasColumnName("UserName");
        builder.Property(e => e.Page)
            .HasMaxLength(500)
            .HasColumnName("Page");
        builder.Property(e => e.AppVersion)
            .HasMaxLength(50)
            .HasColumnName("AppVersion");
        builder.Property(e => e.Browser)
            .HasMaxLength(255)
            .HasColumnName("Browser");
        builder.Property(e => e.Message).HasColumnName("Message");
        builder.Property(e => e.StackTrace).HasColumnName("StackTrace");
        builder.Property(e => e.Caller)
            .HasMaxLength(255)
            .HasColumnName("Caller");
        builder.Property(e => e.Address)
            .HasMaxLength(50)
            .HasColumnName("Address");

        builder.HasIndex(e => e.ErrorTime, "IX_Core_LogErrorArchive_ErrorTime");
        builder.HasIndex(e => e.UserName, "IX_Core_LogErrorArchive_UserName");
    }
}
