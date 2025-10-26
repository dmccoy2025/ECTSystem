using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Logging;

/// <summary>
/// Entity Framework configuration for the CoreLogPageGenerationTime entity.
/// </summary>
public class CoreLogPageGenerationTimeConfiguration : IEntityTypeConfiguration<CoreLogPageGenerationTime>
{
    /// <summary>
    /// Configures the CoreLogPageGenerationTime entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLogPageGenerationTime> builder)
    {
        builder.ToTable("Core_LogPageGenerationTime", "dbo");

        builder.HasKey(e => e.GId)
            .HasName("PK_Core_LogPageGenerationTime");

        builder.Property(e => e.GId).HasColumnName("gID");
        builder.Property(e => e.ActionDate).HasColumnName("ActionDate");
        builder.Property(e => e.MeasuredTime)
            .HasMaxLength(50)
            .HasColumnName("MeasuredTime");
        builder.Property(e => e.CurrentPage)
            .HasMaxLength(500)
            .HasColumnName("CurrentPage");
        builder.Property(e => e.ReferringPage)
            .HasMaxLength(500)
            .HasColumnName("ReferringPage");
        builder.Property(e => e.Username)
            .HasMaxLength(255)
            .HasColumnName("Username");
        builder.Property(e => e.Role)
            .HasMaxLength(255)
            .HasColumnName("Role");

        builder.HasIndex(e => e.ActionDate, "IX_Core_LogPageGenerationTime_ActionDate");
        builder.HasIndex(e => e.CurrentPage, "IX_Core_LogPageGenerationTime_CurrentPage");
    }
}
