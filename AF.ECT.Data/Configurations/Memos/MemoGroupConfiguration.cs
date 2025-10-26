using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Memos;

/// <summary>
/// Entity Framework Core configuration for the MemoGroup entity.
/// Configures memo template group-based permissions defining which groups can create,
/// edit, delete, and view specific memo templates.
/// </summary>
public class MemoGroupConfiguration : IEntityTypeConfiguration<MemoGroup>
{
    /// <summary>
    /// Configures the MemoGroup entity with table mapping and properties for
    /// group-based memo template access control.
    /// </summary>
    /// <param name="builder">The entity type builder for MemoGroup.</param>
    public void Configure(EntityTypeBuilder<MemoGroup> builder)
    {
        builder.HasNoKey();

        builder.ToTable("MEMO_GROUP", "dbo");

        builder.Property(e => e.CanCreate)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("CAN_CREATE");
        builder.Property(e => e.CanDelete)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("CAN_DELETE");
        builder.Property(e => e.CanEdit)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("CAN_EDIT");
        builder.Property(e => e.CanView)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("CAN_VIEW");
        builder.Property(e => e.GroupId)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("GROUP_ID");
        builder.Property(e => e.Id)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("ID");
        builder.Property(e => e.TemplateId)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("TEMPLATE_ID");
    }
}
