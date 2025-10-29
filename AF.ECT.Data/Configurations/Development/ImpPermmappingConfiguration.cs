using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpPermmapping"/> entity.
/// Configures a staging table for importing permission name to group ID mappings from legacy systems.
/// </summary>
/// <remarks>
/// ImpPermmapping is a temporary staging table used during data migration processes to map
/// legacy permission names to their corresponding group IDs in the new system. This entity has
/// no primary key (keyless entity) as it represents transient import data used for permission
/// mapping and group assignment during migration.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - Required permission name (non-nullable)
/// - Nullable group ID to accommodate unmapped permissions
/// - Used for permission group assignment during migration
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful permission migration and validation
/// </remarks>
public class ImpPermmappingConfiguration : IEntityTypeConfiguration<ImpPermmapping>
{
    /// <summary>
    /// Configures the ImpPermmapping entity as a keyless staging table with permission mapping fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpPermmapping.</param>
    public void Configure(EntityTypeBuilder<ImpPermmapping> builder)
    {
        builder.ToTable("ImpPERMMapping", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // Permission mapping properties
        builder.Property(e => e.PermName)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("PERM_NAME");

        builder.Property(e => e.GroupId)
            .HasColumnName("GROUP_ID");
        
        // Indexes for common queries
        builder.HasIndex(e => e.PermName, "IX_imp_perm_mapping_perm_name");
        
        builder.HasIndex(e => e.GroupId, "IX_imp_perm_mapping_group_id");
    }
}
