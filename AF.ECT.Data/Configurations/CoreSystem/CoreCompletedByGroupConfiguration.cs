using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreCompletedByGroup"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_completed_by_group table,
/// which defines groups or roles that can complete specific workflow stages or case types.
/// </remarks>
public class CoreCompletedByGroupConfiguration : IEntityTypeConfiguration<CoreCompletedByGroup>
{
    /// <summary>
    /// Configures the CoreCompletedByGroup entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreCompletedByGroup> builder)
    {
        // Table mapping
        builder.ToTable("core_completed_by_group", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_completed_by_group");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UQ_core_completed_by_group_name");
    }
}
