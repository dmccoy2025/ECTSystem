using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupAccessStatus"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_access_status table,
/// which defines user and user-role assignment statuses (Active, Inactive, Suspended, etc.). Controls whether
/// users can access the system and which roles are currently effective.
/// </remarks>
public class CoreLkupAccessStatusConfiguration : IEntityTypeConfiguration<CoreLkupAccessStatus>
{
    /// <summary>
    /// Configures the CoreLkupAccessStatus entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupAccessStatus> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_access_status", "dbo");

        // Primary key
        builder.HasKey(e => e.StatusId)
            .HasName("PK_core_lkup_access_status");

        // Property configurations
        builder.Property(e => e.StatusId)
            .HasColumnName("status_id");

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("description");

        // Indexes
        builder.HasIndex(e => e.Description)
            .IsUnique()
            .HasDatabaseName("UQ_core_lkup_access_status_description");
    }
}
