using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Messages;

/// <summary>
/// Entity Framework configuration for the CoreMessagesGroup entity.
/// </summary>
public class CoreMessagesGroupConfiguration : IEntityTypeConfiguration<CoreMessagesGroup>
{
    /// <summary>
    /// Configures the CoreMessagesGroup entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreMessagesGroup> builder)
    {
        builder.ToTable("Core_MessagesGroup");

        builder.HasKey(e => new { e.MessageId, e.GroupId });

        builder.Property(e => e.MessageId).HasColumnName("MessageID");
        builder.Property(e => e.GroupId).HasColumnName("GroupID");

        builder.HasIndex(e => e.MessageId, "IX_Core_MessagesGroup_MessageID");
        builder.HasIndex(e => e.GroupId, "IX_Core_MessagesGroup_GroupID");
    }
}
