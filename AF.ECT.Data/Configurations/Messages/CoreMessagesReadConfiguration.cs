using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Messages;

/// <summary>
/// Entity Framework configuration for the CoreMessagesRead entity.
/// </summary>
public class CoreMessagesReadConfiguration : IEntityTypeConfiguration<CoreMessagesRead>
{
    /// <summary>
    /// Configures the CoreMessagesRead entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreMessagesRead> builder)
    {
        builder.ToTable("Core_MessagesRead", "dbo");

        builder.HasKey(e => new { e.MessageId, e.UserId })
            .HasName("PK_Core_MessagesRead");

        builder.Property(e => e.MessageId).HasColumnName("MessageID");
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.DateRead).HasColumnName("DateRead");

        builder.HasIndex(e => e.MessageId, "IX_Core_MessagesRead_MessageID");
        builder.HasIndex(e => e.UserId, "IX_Core_MessagesRead_UserID");
    }
}
