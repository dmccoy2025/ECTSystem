using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Configuration for the <see cref="CoreUserGroupsViewBy"/> entity.
/// </summary>
public class CoreUserGroupsViewByConfiguration : IEntityTypeConfiguration<CoreUserGroupsViewBy>
{
    public void Configure(EntityTypeBuilder<CoreUserGroupsViewBy> builder)
    {
        // Table mapping
        builder.ToTable("core_user_groups_view_by", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.ViewerId, e.MemberId })
            .HasName("PK_core_user_groups_view_by");

        // Properties
        builder.Property(e => e.ViewerId)
            .HasColumnName("viewer_id");

        builder.Property(e => e.MemberId)
            .HasColumnName("member_id");

        // Indexes
        builder.HasIndex(e => e.ViewerId, "IX_core_user_groups_view_by_viewer_id");

        builder.HasIndex(e => e.MemberId, "IX_core_user_groups_view_by_member_id");
    }
}
