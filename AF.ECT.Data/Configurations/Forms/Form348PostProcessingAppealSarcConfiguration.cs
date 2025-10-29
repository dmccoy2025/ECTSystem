using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Form348PostProcessingAppealSarc"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Form_348_Post_Processing_Appeal_SARC table,
/// which stores post-processing appeal information for SARC (Sexual Assault Response Coordinator)
/// cases on Form 348. Contains member contact information (street address, city, state, zip, country),
/// notification date, help extension number, and email. This entity extends Form348ApSarc
/// with additional contact and notification details for SARC appeal processing.
/// Has a one-to-one relationship with Form348ApSarc.
/// </remarks>
public class Form348PostProcessingAppealSarcConfiguration : IEntityTypeConfiguration<Form348PostProcessingAppealSarc>
{
    /// <summary>
    /// Configures the entity of type <see cref="Form348PostProcessingAppealSarc"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Form348PostProcessingAppealSarc> builder)
    {
        // Table mapping
        builder.ToTable("Form_348_Post_Processing_Appeal_SARC", "dbo");

        // Primary key
        builder.HasKey(e => e.AppealId)
            .HasName("PK_Form_348_Post_Processing_Appeal_SARC");

        // Properties configuration
        builder.Property(e => e.AppealId)
            .HasColumnName("AppealID")
            .ValueGeneratedNever();

        builder.Property(e => e.AppealStreet)
            .HasMaxLength(200)
            .HasColumnName("AppealStreet");

        builder.Property(e => e.AppealCity)
            .HasMaxLength(100)
            .HasColumnName("AppealCity");

        builder.Property(e => e.AppealState)
            .HasMaxLength(50)
            .HasColumnName("AppealState");

        builder.Property(e => e.AppealZip)
            .HasMaxLength(20)
            .HasColumnName("AppealZip");

        builder.Property(e => e.AppealCountry)
            .HasMaxLength(100)
            .HasColumnName("AppealCountry");

        builder.Property(e => e.MemberNotificationDate)
            .HasColumnName("MemberNotificationDate")
            .HasColumnType("date");

        builder.Property(e => e.HelpExtensionNumber)
            .HasMaxLength(50)
            .HasColumnName("HelpExtensionNumber");

        builder.Property(e => e.Email)
            .HasMaxLength(200)
            .HasColumnName("Email");

        // Relationships
        builder.HasOne(e => e.Appeal)
            .WithOne()
            .HasForeignKey<Form348PostProcessingAppealSarc>(e => e.AppealId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Form_348_Post_Processing_Appeal_SARC");

        // Indexes
        builder.HasIndex(e => e.MemberNotificationDate, "IX_form_348_post_processing_appeal_sarc_notification_date");

        builder.HasIndex(e => e.Email, "IX_form_348_post_processing_appeal_sarc_email");
    }
}
