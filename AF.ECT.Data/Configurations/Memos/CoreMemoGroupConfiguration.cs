using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Memos;

/// <summary>
/// Configuration for the <see cref="CoreMemoGroup"/> entity.
/// </summary>
public class CoreMemoGroupConfiguration : IEntityTypeConfiguration<CoreMemoGroup>
{
    public void Configure(EntityTypeBuilder<CoreMemoGroup> builder)
    {
        // Table mapping
        builder.ToTable("core_memo_group", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_memo_group");

        // Properties
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.TemplateId)
            .HasColumnName("template_id");

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.CanCreate)
            .HasColumnName("can_create");

        builder.Property(e => e.CanEdit)
            .HasColumnName("can_edit");

        builder.Property(e => e.CanDelete)
            .HasColumnName("can_delete");

        builder.Property(e => e.CanView)
            .HasColumnName("can_view");

        // Indexes
        builder.HasIndex(e => e.TemplateId, "IX_core_memo_group_template_id");

        builder.HasIndex(e => e.GroupId, "IX_core_memo_group_group_id");

        builder.HasIndex(e => new { e.TemplateId, e.GroupId })
            .IsUnique()
            .HasDatabaseName("UX_core_memo_group_template_group");
    }
}
