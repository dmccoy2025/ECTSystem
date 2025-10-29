using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Messages;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreMessage"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_message table,
/// which stores system-wide messages, announcements, and alerts displayed to users.
/// </remarks>
public class CoreMessageConfiguration : IEntityTypeConfiguration<CoreMessage>
{
    /// <summary>
    /// Configures the CoreMessage entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreMessage> builder)
    {
        // Table mapping
        builder.ToTable("core_message", "dbo");

        // Primary key
        builder.HasKey(e => e.MessageId)
            .HasName("PK_core_message");

        // Property configurations
        builder.Property(e => e.MessageId)
            .HasColumnName("message_id");

        builder.Property(e => e.Message)
            .IsRequired()
            .HasColumnName("message");

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .HasColumnName("title");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.Popup)
            .HasColumnName("popup");

        builder.Property(e => e.StartTime)
            .HasColumnName("start_time");

        builder.Property(e => e.EndTime)
            .HasColumnName("end_time");

        builder.Property(e => e.IsAdminMessage)
            .HasColumnName("is_admin_message");

        // Indexes
        builder.HasIndex(e => e.StartTime, "IX_core_message_start_time");

        builder.HasIndex(e => e.EndTime, "IX_core_message_end_time");

        builder.HasIndex(e => new { e.StartTime, e.EndTime, e.IsAdminMessage }, "IX_core_message_active");

        builder.HasIndex(e => e.Popup, "IX_core_message_popup");
    }
}
