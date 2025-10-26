using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Permissions;

/// <summary>
/// Entity Framework configuration for the CorePermissionDocGroup entity.
/// </summary>
public class CorePermissionDocGroupConfiguration : IEntityTypeConfiguration<CorePermissionDocGroup>
{
    /// <summary>
    /// Configures the CorePermissionDocGroup entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CorePermissionDocGroup> builder)
    {
        builder.ToTable("Core_PermissionDocGroup", "dbo");

        builder.HasKey(e => new { e.PermId, e.DocGroupId })
            .HasName("PK_Core_PermissionDocGroup");

        builder.Property(e => e.PermId).HasColumnName("PermID");
        builder.Property(e => e.DocGroupId).HasColumnName("DocGroupID");

        builder.HasIndex(e => e.PermId, "IX_Core_PermissionDocGroup_PermID");
        builder.HasIndex(e => e.DocGroupId, "IX_Core_PermissionDocGroup_DocGroupID");
    }
}
