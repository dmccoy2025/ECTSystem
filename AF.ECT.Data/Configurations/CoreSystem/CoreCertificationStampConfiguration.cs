using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreCertificationStamp"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_certification_stamp table,
/// which stores certification statement templates used for official approvals and signatures on case documents.
/// </remarks>
public class CoreCertificationStampConfiguration : IEntityTypeConfiguration<CoreCertificationStamp>
{
    /// <summary>
    /// Configures the CoreCertificationStamp entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreCertificationStamp> builder)
    {
        // Table mapping
        builder.ToTable("core_certification_stamp", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_certification_stamp");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("name");

        builder.Property(e => e.Body)
            .IsRequired()
            .HasColumnName("body");

        builder.Property(e => e.IsQualified)
            .HasColumnName("is_qualified");

        // Indexes
        builder.HasIndex(e => e.Name, "IX_core_certification_stamp_name");

        builder.HasIndex(e => e.IsQualified, "IX_core_certification_stamp_is_qualified");
    }
}
