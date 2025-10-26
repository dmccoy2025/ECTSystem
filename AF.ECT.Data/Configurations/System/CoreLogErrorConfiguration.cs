using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.System;

/// <summary>
/// Entity Framework configuration for the CoreLogError entity.
/// </summary>
public class CoreLogErrorConfiguration : IEntityTypeConfiguration<CoreLogError>
{
    /// <summary>
    /// Configures the CoreLogError entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLogError> builder)
    {
        builder.ToTable("Core_LogError");

        builder.HasKey(e => e.LogId);

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

        builder.HasIndex(e => e.ErrorTime, "IX_Core_LogError_ErrorTime");
        builder.HasIndex(e => e.UserName, "IX_Core_LogError_UserName");
    }
}
