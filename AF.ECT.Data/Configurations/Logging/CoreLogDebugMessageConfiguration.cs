using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Logging;

/// <summary>
/// Entity Framework configuration for the CoreLogDebugMessage entity.
/// </summary>
public class CoreLogDebugMessageConfiguration : IEntityTypeConfiguration<CoreLogDebugMessage>
{
    /// <summary>
    /// Configures the CoreLogDebugMessage entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLogDebugMessage> builder)
    {
        builder.ToTable("Core_LogDebugMessage", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_LogDebugMessage");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.Message).HasColumnName("Message");

        builder.HasIndex(e => e.CreatedDate, "IX_Core_LogDebugMessage_CreatedDate");
    }
}
