using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Logging;

/// <summary>
/// Entity Framework configuration for the CoreLogChangeSet entity.
/// </summary>
public class CoreLogChangeSetConfiguration : IEntityTypeConfiguration<CoreLogChangeSet>
{
    /// <summary>
    /// Configures the CoreLogChangeSet entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLogChangeSet> builder)
    {
        builder.ToTable("Core_LogChangeSet", "dbo");

        builder.HasKey(e => new { e.LogId, e.Section, e.Field })
            .HasName("PK_Core_LogChangeSet");

        builder.Property(e => e.LogId).HasColumnName("LogID");
        builder.Property(e => e.Section)
            .HasMaxLength(255)
            .HasColumnName("Section");
        builder.Property(e => e.Field)
            .HasMaxLength(255)
            .HasColumnName("Field");
        builder.Property(e => e.Old).HasColumnName("Old");
        builder.Property(e => e.New).HasColumnName("New");

        builder.HasIndex(e => e.LogId, "IX_Core_LogChangeSet_LogID");
        builder.HasIndex(e => e.Section, "IX_Core_LogChangeSet_Section");
    }
}
