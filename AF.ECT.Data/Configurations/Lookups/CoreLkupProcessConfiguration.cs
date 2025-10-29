using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupProcess"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_process table,
/// which defines business processes or procedural steps in the case management workflow.
/// </remarks>
public class CoreLkupProcessConfiguration : IEntityTypeConfiguration<CoreLkupProcess>
{
    /// <summary>
    /// Configures the CoreLkupProcess entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupProcess> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_process", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_lkup_process");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("title");

        // Indexes
        builder.HasIndex(e => e.Title, "IX_core_lkup_process_title");
    }
}
