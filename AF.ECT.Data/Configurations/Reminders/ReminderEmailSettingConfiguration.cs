using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Reminders;

/// <summary>
/// Entity Framework Core configuration for the <see cref="ReminderEmailSetting"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the reminder_email_setting table,
/// which defines automated email reminder rules for specific workflow statuses. Controls when and how often
/// reminder emails are sent to responsible parties.
/// </remarks>
public class ReminderEmailSettingConfiguration : IEntityTypeConfiguration<ReminderEmailSetting>
{
    /// <summary>
    /// Configures the ReminderEmailSetting entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<ReminderEmailSetting> builder)
    {
        // Table mapping
        builder.ToTable("reminder_email_setting", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_reminder_email_setting");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.WsId)
            .HasColumnName("ws_id");

        builder.Property(e => e.Compo)
            .HasColumnName("compo");

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.TemplateId)
            .HasColumnName("template_id");

        builder.Property(e => e.IntervalTime)
            .HasColumnName("interval_time");

        // Relationships
        builder.HasOne(d => d.Workflow)
            .WithMany()
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_reminder_email_setting_core_workflow");

        builder.HasOne(d => d.Ws)
            .WithMany()
            .HasForeignKey(d => d.WsId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_reminder_email_setting_core_work_status");

        builder.HasOne(d => d.Group)
            .WithMany()
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_reminder_email_setting_core_user_group");

        builder.HasOne(d => d.Template)
            .WithMany(p => p.ReminderEmailSettings)
            .HasForeignKey(d => d.TemplateId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_reminder_email_setting_core_email_template");

        // Indexes
        builder.HasIndex(e => new { e.WorkflowId, e.WsId, e.Compo })
            .IsUnique()
            .HasDatabaseName("UQ_reminder_email_setting_workflow_status_compo");

        builder.HasIndex(e => e.TemplateId, "IX_reminder_email_setting_template_id");

        builder.HasIndex(e => e.GroupId, "IX_reminder_email_setting_group_id");
    }
}
