using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupModule"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_module table,
/// which defines the available modules in the ECT system (LOD, IDES, PDES, etc.). Each module represents
/// a distinct workflow type with specific forms, processes, and business rules.
/// </remarks>
public class CoreLkupModuleConfiguration : IEntityTypeConfiguration<CoreLkupModule>
{
    /// <summary>
    /// Configures the CoreLkupModule entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupModule> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_module", "dbo");

        // Primary key
        builder.HasKey(e => e.ModuleId)
            .HasName("PK_core_lkup_module");

        // Property configurations
        builder.Property(e => e.ModuleId)
            .HasColumnName("module_id");

        builder.Property(e => e.ModuleName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("module_name");

        builder.Property(e => e.IsSpecialCase)
            .HasColumnName("is_special_case");

        // Indexes
        builder.HasIndex(e => e.ModuleName)
            .IsUnique()
            .HasDatabaseName("UQ_core_lkup_module_name");

        builder.HasIndex(e => e.IsSpecialCase, "IX_core_lkup_module_is_special_case");
    }
}
