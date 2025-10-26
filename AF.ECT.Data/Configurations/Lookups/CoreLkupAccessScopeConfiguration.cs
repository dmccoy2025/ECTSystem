using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupAccessScope"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_access_scope table,
/// which defines data access scope levels (Own, Unit, Chain, Component, All). Determines how much data
/// users in specific groups can view and access based on organizational hierarchy.
/// </remarks>
public class CoreLkupAccessScopeConfiguration : IEntityTypeConfiguration<CoreLkupAccessScope>
{
    /// <summary>
    /// Configures the CoreLkupAccessScope entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupAccessScope> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_access_scope", "dbo");

        // Primary key
        builder.HasKey(e => e.ScopeId)
            .HasName("PK_core_lkup_access_scope");

        // Property configurations
        builder.Property(e => e.ScopeId)
            .HasColumnName("scope_id");

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("description");

        // Indexes
        builder.HasIndex(e => e.Description)
            .IsUnique()
            .HasDatabaseName("UQ_core_lkup_access_scope_description");
    }
}
