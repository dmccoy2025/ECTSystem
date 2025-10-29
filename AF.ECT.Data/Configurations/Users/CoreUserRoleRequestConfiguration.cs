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
        builder.ToTable("core_user_role_request", "dbo");

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

        // Relationships
        builder.HasOne(d => d.CompletedByNavigation)
            .WithMany()
            .HasForeignKey(d => d.CompletedBy)
            .HasConstraintName("FK_core_user_role_request_core_user_completed_by");

        builder.HasOne(d => d.ExistingGroup)
            .WithMany()
            .HasForeignKey(d => d.ExistingGroupId)
            .HasConstraintName("FK_core_user_role_request_core_user_group_existing");

        builder.HasOne(d => d.RequestedGroup)
            .WithMany()
            .HasForeignKey(d => d.RequestedGroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_role_request_core_user_group_requested");

        builder.HasOne(d => d.StatusNavigation)
            .WithMany()
            .HasForeignKey(d => d.Status)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_role_request_core_lkup_access_status");

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_user_role_request_core_user");

        // Indexes
        builder.HasIndex(e => e.UserId, "IX_core_user_role_request_user_id");

        builder.HasIndex(e => e.Status, "IX_core_user_role_request_status");

        builder.HasIndex(e => e.RequestedDate, "IX_core_user_role_request_requested_date");
    }
}
