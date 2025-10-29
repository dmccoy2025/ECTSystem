using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreUsersOnline"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_users_online table,
/// which tracks active user sessions for monitoring, analytics, and concurrent access management.
/// </remarks>
public class CoreUsersOnlineConfiguration : IEntityTypeConfiguration<CoreUsersOnline>
{
    /// <summary>
    /// Configures the CoreUsersOnline entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreUsersOnline> builder)
    {
        // Table mapping
        builder.ToTable("core_users_online", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.UserId, e.GroupId, e.LoginTime })
            .HasName("PK_core_users_online");

        // Property configurations
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.LoginTime)
            .HasColumnName("login_time");

        builder.Property(e => e.LastAccess)
            .HasColumnName("last_access");

        builder.Property(e => e.SessionId)
            .HasMaxLength(100)
            .HasColumnName("session_id");

        builder.Property(e => e.RemoteAddress)
            .HasMaxLength(50)
            .HasColumnName("remote_address");

        // Indexes
        builder.HasIndex(e => e.UserId, "IX_core_users_online_user_id");

        builder.HasIndex(e => e.LastAccess, "IX_core_users_online_last_access");

        builder.HasIndex(e => e.SessionId, "IX_core_users_online_session_id");
        
        builder.HasIndex(e => e.GroupId, "IX_core_users_online_group_id");
        
        builder.HasIndex(e => e.LoginTime, "IX_core_users_online_login_time");
        
        builder.HasIndex(e => new { e.UserId, e.LastAccess }, "IX_core_users_online_user_last_access");
    }
}
