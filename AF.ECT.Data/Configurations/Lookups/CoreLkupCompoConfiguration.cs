using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupCompo"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_compo table,
/// which defines military components (Army, Navy, Air Force, Marines, Coast Guard, etc.). Components
/// determine organizational hierarchies, workflow rules, and component-specific configurations.
/// </remarks>
public class CoreLkupCompoConfiguration : IEntityTypeConfiguration<CoreLkupCompo>
{
    /// <summary>
    /// Configures the CoreLkupCompo entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupCompo> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_compo", "dbo");

        // Primary key
        builder.HasKey(e => e.Compo)
            .HasName("PK_core_lkup_compo");

        // Property configurations
        builder.Property(e => e.Compo)
            .HasMaxLength(4)
            .HasColumnName("compo");

        builder.Property(e => e.CompoDescr)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("compo_descr");

        builder.Property(e => e.Abbreviation)
            .HasMaxLength(10)
            .HasColumnName("abbreviation");

        // Indexes
        builder.HasIndex(e => e.CompoDescr, "IX_core_lkup_compo_descr");

        builder.HasIndex(e => e.Abbreviation, "IX_core_lkup_compo_abbr");
    }
}
