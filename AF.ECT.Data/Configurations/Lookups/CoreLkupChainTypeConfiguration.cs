using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreLkupChainType"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_lkup_chain_type table,
/// which defines the types of command chains used in the military organizational structure (Admin, Operational,
/// Medical, etc.). Chain types determine how approval authorities are structured and how cases are routed
/// through the command hierarchy.
/// </remarks>
public class CoreLkupChainTypeConfiguration : IEntityTypeConfiguration<CoreLkupChainType>
{
    /// <summary>
    /// Configures the CoreLkupChainType entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLkupChainType> builder)
    {
        // Table mapping
        builder.ToTable("core_lkup_chain_type", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_lkup_chain_type");

        // Property configurations
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(e => e.Active)
            .HasColumnName("active");

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("created_by");

        builder.Property(e => e.ModifiedDate)
            .HasColumnName("modified_date")
            .HasDefaultValueSql("(getdate())");

        builder.Property(e => e.ModifiedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("modified_by");

        builder.Property(e => e.Description)
            .HasMaxLength(500)
            .HasColumnName("description");

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("UQ_core_lkup_chain_type_name");

        builder.HasIndex(e => e.Active, "IX_core_lkup_chain_type_active");
    }
}
