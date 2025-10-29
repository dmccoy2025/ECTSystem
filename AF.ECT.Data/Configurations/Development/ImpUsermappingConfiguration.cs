using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpUsermapping"/> entity.
/// Configures a staging table for mapping legacy user IDs to new person IDs and usernames during migration.
/// </summary>
/// <remarks>
/// ImpUsermapping is a temporary staging table used during data migration processes to establish
/// the mapping between legacy user IDs from old systems and new person IDs and usernames in the
/// RCPHA system. This entity facilitates the translation of user references during migration from
/// legacy authentication systems to the new user management system. This entity has no primary key
/// (keyless entity) as it represents transient mapping data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for staging
/// - All nullable properties to accommodate incomplete mappings
/// - Legacy user ID to new person ID mapping
/// - Username association for account linking
/// - Used for user reference translation during migration
/// - Supports reconciliation of user accounts across systems
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful user migration and validation
/// </remarks>
public class ImpUsermappingConfiguration : IEntityTypeConfiguration<ImpUsermapping>
{
    /// <summary>
    /// Configures the ImpUsermapping entity as a keyless staging table with user mapping fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpUsermapping.</param>
    public void Configure(EntityTypeBuilder<ImpUsermapping> builder)
    {
        builder.ToTable("ImpUSERMapping", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // User mapping properties
        builder.Property(e => e.UserId)
            .HasColumnName("USER_ID");

        builder.Property(e => e.PersonId)
            .HasColumnName("PERSON_ID");

        builder.Property(e => e.Username)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("USERNAME");
        
        // Indexes for common queries
        builder.HasIndex(e => e.UserId, "IX_imp_user_mapping_user_id");
        
        builder.HasIndex(e => e.PersonId, "IX_imp_user_mapping_person_id");
        
        builder.HasIndex(e => e.Username, "IX_imp_user_mapping_username");
    }
}
