using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreEmailTemplate"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_email_template table,
/// which stores email templates used for automated notifications throughout the ECT system. Templates support
/// component-specific customization and can be linked to data procedures for dynamic content generation.
/// </remarks>
public class CoreEmailTemplateConfiguration : IEntityTypeConfiguration<CoreEmailTemplate>
{
    /// <summary>
    /// Configures the CoreEmailTemplate entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreEmailTemplate> builder)
    {
        // Table mapping
        builder.ToTable("core_email_template", "dbo");

        // Primary key
        builder.HasKey(e => e.TemplateId)
            .HasName("PK__core_ema__1E85A0F3BC79E4A1");

        // Property configurations
        builder.Property(e => e.TemplateId)
            .HasColumnName("template_id");

        builder.Property(e => e.Compo)
            .HasMaxLength(4)
            .HasColumnName("compo");

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        builder.Property(e => e.Subject)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("subject");

        builder.Property(e => e.Body)
            .IsRequired()
            .HasColumnName("body");

        builder.Property(e => e.DataProc)
            .HasMaxLength(200)
            .HasColumnName("data_proc");

        builder.Property(e => e.Active)
            .HasColumnName("active");

        builder.Property(e => e.SysDate)
            .HasColumnType("datetime")
            .HasDefaultValueSql("getdate()")
            .HasColumnName("sys_date");

        // Indexes
        builder.HasIndex(e => e.Active)
            .HasDatabaseName("IX_core_email_template_active")
            .HasFilter("active = 1");

        builder.HasIndex(e => e.Compo, "IX_core_email_template_compo");

        builder.HasIndex(e => new { e.Title, e.Compo })
            .IsUnique()
            .HasDatabaseName("UQ_core_email_template_title_compo");
    }
}
