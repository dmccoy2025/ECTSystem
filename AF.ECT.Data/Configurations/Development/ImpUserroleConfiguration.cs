using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpUserrole"/> entity.
/// Configures a staging table for importing user role assignments and permissions from legacy systems.
/// </summary>
/// <remarks>
/// ImpUserrole is a temporary staging table used during data migration processes to load user role
/// assignments, permissions, and group memberships from legacy access control systems. This entity
/// captures the relationship between users, their permissions, permission groups, and default role
/// settings for migrating security configurations. This entity has no primary key (keyless entity)
/// as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - Nullable properties to accommodate incomplete role data
/// - Username identification for account association
/// - Permission assignment (permission name or code)
/// - Group membership tracking (GroupID)
/// - Default role flag (IsDefault as boolean)
/// - Used for security configuration migration
/// - Supports role-based access control (RBAC) setup
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful role migration to production permission tables
/// </remarks>
public class ImpUserroleConfiguration : IEntityTypeConfiguration<ImpUserrole>
{
    /// <summary>
    /// Configures the ImpUserrole entity as a keyless staging table with user role import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpUserrole.</param>
    public void Configure(EntityTypeBuilder<ImpUserrole> builder)
    {
        builder.ToTable("ImpUSERRole", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // User identification
        builder.Property(e => e.Username)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("USERNAME");

        // Permission assignment
        builder.Property(e => e.Permission)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("PERMISSION");

        // Group membership
        builder.Property(e => e.Groupid)
            .HasColumnName("GROUPID");

        // Default role flag
        builder.Property(e => e.IsDefault)
            .HasColumnName("IS_DEFAULT");
        
        // Indexes for common queries
        builder.HasIndex(e => e.Username, "IX_imp_user_role_username");
        
        builder.HasIndex(e => e.Permission, "IX_imp_user_role_permission");
        
        builder.HasIndex(e => e.Groupid, "IX_imp_user_role_groupid");
    }
}
