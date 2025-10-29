using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Users;

/// <summary>
/// Configuration for the <see cref="CoreUserGroupLevel"/> entity.
/// </summary>
public class CoreUserGroupLevelConfiguration : IEntityTypeConfiguration<CoreUserGroupLevel>
{
    public void Configure(EntityTypeBuilder<CoreUserGroupLevel> builder)
    {
        // Table mapping
        builder.ToTable("core_user_group_level", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_user_group_level");

        // Properties
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        // Indexes
        builder.HasIndex(e => e.Name, "IX_core_user_group_level_name");
    }
}
