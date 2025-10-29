using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Configuration for the <see cref="CoreUserGroupsManagedBy"/> entity.
/// </summary>
public class CoreUserGroupsManagedByConfiguration : IEntityTypeConfiguration<CoreUserGroupsManagedBy>
{
    public void Configure(EntityTypeBuilder<CoreUserGroupsManagedBy> builder)
    {
        // Table mapping
        builder.ToTable("core_user_groups_managed_by", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.GroupId, e.ManagedBy })
            .HasName("PK_core_user_groups_managed_by");

        // Properties
        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.ManagedBy)
            .HasColumnName("managed_by");

        builder.Property(e => e.Notify)
            .HasColumnName("notify");

        builder.Property(e => e.ViewBy)
            .HasColumnName("view_by");

        // Indexes
        builder.HasIndex(e => e.GroupId, "IX_core_user_groups_managed_by_group_id");

        builder.HasIndex(e => e.ManagedBy, "IX_core_user_groups_managed_by_managed_by");
    }
}
