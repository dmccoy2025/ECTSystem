using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreUserRole"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema and relationships for the core_user_role table,
/// which represents the many-to-many relationship between users and user groups (roles).
/// Users can have multiple roles, and each role assignment can be active or inactive.
/// Roles determine what pages users can access, what actions they can perform, and what
/// data they can view within the ECT system. Role assignments can be temporarily deactivated
/// without deletion to support leave periods, role changes, and access reviews.
/// </remarks>
public class CoreUserRoleConfiguration : IEntityTypeConfiguration<CoreUserRole>
{
    /// <summary>
    /// Configures the entity of type <see cref="CoreUserRole"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreUserRole> builder)
    {
        // Table mapping
        builder.ToTable("core_user_role", "dbo");

        // Primary key
        builder.HasKey(e => e.UserRoleId)
            .HasName("PK_core_user_role");

        // Properties configuration
        builder.Property(e => e.UserRoleId)
            .HasColumnName("user_role_id")
            .HasComment("userRoleID identifies UserRoles");

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id")
            .HasComment("roleID from table Roles");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .HasComment("UserId from table Users");

        builder.Property(e => e.Status)
            .HasColumnName("status");

        builder.Property(e => e.Active)
            .HasColumnName("active");

        // Relationships
        builder.HasOne(d => d.Group)
            .WithMany(p => p.CoreUserRoles)
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_role_core_user_group");

        builder.HasOne(d => d.StatusNavigation)
            .WithMany(p => p.CoreUserRoles)
            .HasForeignKey(d => d.Status)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_role_core_lkup_access_status");

        builder.HasOne(d => d.User)
            .WithMany(p => p.CoreUserRoles)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_role_core_user");

        // Indexes
        builder.HasIndex(e => new { e.UserId, e.GroupId })
            .HasDatabaseName("IX_core_user_role_user_group")
            .IsUnique();

        builder.HasIndex(e => e.UserId, "IX_core_user_role_user_id");

        builder.HasIndex(e => e.GroupId, "IX_core_user_role_group_id");

        builder.HasIndex(e => e.Active, "IX_core_user_role_active");

        builder.HasIndex(e => new { e.UserId, e.Active }, "IX_core_user_role_user_active");
        
        builder.HasIndex(e => e.Status, "IX_core_user_role_status");
        
        builder.HasIndex(e => new { e.GroupId, e.Active }, "IX_core_user_role_group_active");
    }
}
