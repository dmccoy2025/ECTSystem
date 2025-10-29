using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Configuration for the <see cref="CoreUsersAltTitle"/> entity.
/// </summary>
public class CoreUsersAltTitleConfiguration : IEntityTypeConfiguration<CoreUsersAltTitle>
{
    public void Configure(EntityTypeBuilder<CoreUsersAltTitle> builder)
    {
        // Table mapping
        builder.ToTable("core_users_alt_title", "dbo");

        // Composite primary key
        builder.HasKey(e => new { e.UserId, e.GroupId })
            .HasName("PK_core_users_alt_title");

        // Properties
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .HasColumnName("title");

        // Indexes
        builder.HasIndex(e => e.UserId, "IX_core_users_alt_title_user_id");

        builder.HasIndex(e => e.GroupId, "IX_core_users_alt_title_group_id");
    }
}
