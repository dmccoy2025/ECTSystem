using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Configuration for the <see cref="CoreUserRoleRequest"/> entity.
/// </summary>
public class CoreUserRoleRequestConfiguration : IEntityTypeConfiguration<CoreUserRoleRequest>
{
    public void Configure(EntityTypeBuilder<CoreUserRoleRequest> builder)
    {
        // Table mapping
        builder.ToTable("core_user_role_request");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_user_role_request");

        // Properties
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Status)
            .HasColumnName("status");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.RequestedGroupId)
            .HasColumnName("requested_group_id");

        builder.Property(e => e.ExistingGroupId)
            .HasColumnName("existing_group_id");

        builder.Property(e => e.NewRole)
            .HasColumnName("new_role");

        builder.Property(e => e.RequestorComment)
            .HasMaxLength(2000)
            .HasColumnName("requestor_comment");

        builder.Property(e => e.RequestedDate)
            .HasColumnType("datetime")
            .HasColumnName("requested_date");

        builder.Property(e => e.CompletedBy)
            .HasColumnName("completed_by");

        builder.Property(e => e.CompletedDate)
            .HasColumnType("datetime")
            .HasColumnName("completed_date");

        builder.Property(e => e.CompletedComment)
            .HasMaxLength(2000)
            .HasColumnName("completed_comment");

        // Indexes
        builder.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_core_user_role_request_user_id");

        builder.HasIndex(e => e.Status)
            .HasDatabaseName("IX_core_user_role_request_status");

        builder.HasIndex(e => e.RequestedDate)
            .HasDatabaseName("IX_core_user_role_request_requested_date");
    }
}
