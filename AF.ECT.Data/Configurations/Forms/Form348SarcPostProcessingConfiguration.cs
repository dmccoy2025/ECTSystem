using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348SarcPostProcessing"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents post-processing data for SARC cases including member notification and appeal information.
/// </remarks>
public class Form348SarcPostProcessingConfiguration : IEntityTypeConfiguration<Form348SarcPostProcessing>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348SarcPostProcessing> builder)
    {
        // Table mapping
        builder.ToTable("Form348SARCPostProcessing");

        // Primary key
        builder.HasKey(e => e.SarcId);

        // Properties
        builder.Property(e => e.SarcId).HasColumnName("SarcID");
        builder.Property(e => e.MemberNotified).HasColumnName("MemberNotified");
        builder.Property(e => e.MemberNotificationDate).HasColumnName("MemberNotificationDate");
        builder.Property(e => e.HelpExtensionNumber).HasColumnName("HelpExtensionNumber").HasMaxLength(50);
        builder.Property(e => e.AppealStreet).HasColumnName("AppealStreet").HasMaxLength(255);
        builder.Property(e => e.AppealCity).HasColumnName("AppealCity").HasMaxLength(100);
        builder.Property(e => e.AppealState).HasColumnName("AppealState").HasMaxLength(50);
        builder.Property(e => e.AppealZip).HasColumnName("AppealZip").HasMaxLength(20);
        builder.Property(e => e.AppealCountry).HasColumnName("AppealCountry").HasMaxLength(100);
        builder.Property(e => e.Email).HasColumnName("Email").HasMaxLength(255);

        // Indexes for query performance
        builder.HasIndex(e => e.MemberNotificationDate, "IX_Form348SARCPostProcessing_NotificationDate");
    }
}
