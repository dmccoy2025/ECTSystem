using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Reminders;

/// <summary>
/// Entity Framework Core configuration for the <see cref="ReminderEmail"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the reminder_email table,
/// which tracks when reminder emails were last sent for specific cases. Prevents duplicate reminders
/// and tracks reminder frequency per case.
/// </remarks>
public class ReminderEmailConfiguration : IEntityTypeConfiguration<ReminderEmail>
{
    /// <summary>
    /// Configures the ReminderEmail entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<ReminderEmail> builder)
    {
        // Table mapping
        builder.ToTable("reminder_email", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_reminder_email");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.SettingId)
            .HasColumnName("setting_id");

        builder.Property(e => e.CaseId)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("case_id");

        builder.Property(e => e.LastSentDate)
            .HasColumnName("last_sent_date");

        builder.Property(e => e.LastModifiedDate)
            .HasColumnName("last_modified_date");

        builder.Property(e => e.SentCount)
            .HasColumnName("sent_count");

        builder.Property(e => e.MemberUnitId)
            .HasColumnName("member_unit_id");

        // Relationships
        builder.HasOne(d => d.Setting)
            .WithMany(p => p.ReminderEmails)
            .HasForeignKey(d => d.SettingId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_reminder_email_reminder_email_setting");

        // Indexes
        builder.HasIndex(e => new { e.SettingId, e.CaseId })
            .IsUnique()
            .HasDatabaseName("UQ_reminder_email_setting_case");

        builder.HasIndex(e => e.LastSentDate, "IX_reminder_email_last_sent_date");

        builder.HasIndex(e => e.CaseId, "IX_reminder_email_case_id");
    }
}
