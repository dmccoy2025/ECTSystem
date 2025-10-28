using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348PostProcessing"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents post-processing data for LOD cases including member notification information
/// and appeal contact details.
/// </remarks>
public class Form348PostProcessingConfiguration : IEntityTypeConfiguration<Form348PostProcessing>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348PostProcessing> builder)
    {
        // Table mapping
        builder.ToTable("Form348PostProcessing", "dbo");

        // Primary key
        builder.HasKey(e => e.LodId)
            .HasName("PK_Form348PostProcessing");

        // Properties
        builder.Property(e => e.LodId).HasColumnName("LodID");
        builder.Property(e => e.HelpExtensionNumber).HasColumnName("HelpExtensionNumber").HasMaxLength(50);
        builder.Property(e => e.AppealStreet).HasColumnName("AppealStreet").HasMaxLength(255);
        builder.Property(e => e.AppealCity).HasColumnName("AppealCity").HasMaxLength(100);
        builder.Property(e => e.AppealState).HasColumnName("AppealState").HasMaxLength(50);
        builder.Property(e => e.AppealZip).HasColumnName("AppealZip").HasMaxLength(20);
        builder.Property(e => e.AppealCountry).HasColumnName("AppealCountry").HasMaxLength(100);
        builder.Property(e => e.NokFirstName).HasColumnName("NokFirstName").HasMaxLength(100);
        builder.Property(e => e.NokLastName).HasColumnName("NokLastName").HasMaxLength(100);
        builder.Property(e => e.NokMiddleName).HasColumnName("NokMiddleName").HasMaxLength(100);
        builder.Property(e => e.NotificationDate).HasColumnName("NotificationDate");
        builder.Property(e => e.Email).HasColumnName("Email").HasMaxLength(255);
        builder.Property(e => e.AddressFlag).HasColumnName("AddressFlag");
        builder.Property(e => e.EmailFlag).HasColumnName("EmailFlag");
        builder.Property(e => e.PhoneFlag).HasColumnName("PhoneFlag");

        // Indexes for query performance
        builder.HasIndex(e => e.NotificationDate, "IX_Form348PostProcessing_NotificationDate");
        builder.HasIndex(e => e.AddressFlag, "IX_Form348PostProcessing_AddressFlag");
        builder.HasIndex(e => e.EmailFlag, "IX_Form348PostProcessing_EmailFlag");
        builder.HasIndex(e => e.PhoneFlag, "IX_Form348PostProcessing_PhoneFlag");
    }
}
