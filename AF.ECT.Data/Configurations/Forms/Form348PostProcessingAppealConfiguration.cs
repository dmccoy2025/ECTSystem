using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348PostProcessingAppeal"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents appeal contact information for LOD post-processing notifications.
/// </remarks>
public class Form348PostProcessingAppealConfiguration : IEntityTypeConfiguration<Form348PostProcessingAppeal>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348PostProcessingAppeal> builder)
    {
        // Table mapping
        builder.ToTable("Form348PostProcessingAppeal", "dbo");

        // Primary key
        builder.HasKey(e => e.AppealId)
            .HasName("PK_Form348PostProcessingAppeal");

        // Properties
        builder.Property(e => e.AppealId).HasColumnName("AppealID");
        builder.Property(e => e.InitialLodId).HasColumnName("InitialLodID");
        builder.Property(e => e.AppealStreet).HasColumnName("AppealStreet").HasMaxLength(255);
        builder.Property(e => e.AppealCity).HasColumnName("AppealCity").HasMaxLength(100);
        builder.Property(e => e.AppealState).HasColumnName("AppealState").HasMaxLength(50);
        builder.Property(e => e.AppealZip).HasColumnName("AppealZip").HasMaxLength(20);
        builder.Property(e => e.AppealCountry).HasColumnName("AppealCountry").HasMaxLength(100);
        builder.Property(e => e.MemberNotificationDate).HasColumnName("MemberNotificationDate");
        builder.Property(e => e.HelpExtensionNumber).HasColumnName("HelpExtensionNumber").HasMaxLength(50);
        builder.Property(e => e.Email).HasColumnName("Email").HasMaxLength(255);

        // Indexes for query performance
        builder.HasIndex(e => e.InitialLodId, "IX_Form348PostProcessingAppeal_InitialLodID");
        builder.HasIndex(e => e.MemberNotificationDate, "IX_Form348PostProcessingAppeal_NotificationDate");
    }
}
