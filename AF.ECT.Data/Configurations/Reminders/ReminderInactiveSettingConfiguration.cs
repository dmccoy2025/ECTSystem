using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reminders;

/// <summary>
/// Entity Framework Core configuration for the ReminderInactiveSetting entity.
/// Configures reminder settings for inactive cases including notification intervals,
/// template assignments, and active status for automated reminder generation.
/// </summary>
public class ReminderInactiveSettingConfiguration : IEntityTypeConfiguration<ReminderInactiveSetting>
{
    /// <summary>
    /// Configures the ReminderInactiveSetting entity with table mapping, primary key,
    /// and properties for controlling inactive case reminder behavior and timing.
    /// </summary>
    /// <param name="builder">The entity type builder for ReminderInactiveSetting.</param>
    public void Configure(EntityTypeBuilder<ReminderInactiveSetting> builder)
    {
        builder.HasKey(e => e.IId).HasName("PK__REMINDER__DC501F7953A266AC");

        builder.ToTable("REMINDER_INACTIVE_SETTINGS", "dbo");

        builder.Property(e => e.IId).HasColumnName("I_ID");
        builder.Property(e => e.Active).HasColumnName("ACTIVE");
        builder.Property(e => e.Interval).HasColumnName("INTERVAL");
        builder.Property(e => e.NotificationInterval).HasColumnName("NOTIFICATION_INTERVAL");
        builder.Property(e => e.TemplateId).HasColumnName("TEMPLATE_ID");

        builder.HasIndex(e => e.TemplateId, "IX_REMINDER_INACTIVE_SETTINGS_TEMPLATE");
        builder.HasIndex(e => e.Active, "IX_REMINDER_INACTIVE_SETTINGS_ACTIVE");
    }
}
