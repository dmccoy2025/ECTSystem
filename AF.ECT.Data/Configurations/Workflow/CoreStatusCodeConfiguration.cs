using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreStatusCode"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_status_code table,
/// which represents workflow status codes used throughout the ECT system. Status codes define the state
/// of cases at various points in their lifecycle, including draft, in-review, approved, canceled, and final states.
/// </remarks>
public class CoreStatusCodeConfiguration : IEntityTypeConfiguration<CoreStatusCode>
{
    /// <summary>
    /// Configures the CoreStatusCode entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreStatusCode> builder)
    {
        // Table mapping
        builder.ToTable("core_status_code", "dbo");

        // Primary key
        builder.HasKey(e => e.StatusId)
            .HasName("PK__core_sta__3683B531B6D1A2E3");

        // Property configurations
        builder.Property(e => e.StatusId)
            .HasColumnName("status_id");

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("description");

        builder.Property(e => e.ModuleId)
            .HasColumnName("module_id");

        builder.Property(e => e.Compo)
            .IsRequired()
            .HasMaxLength(4)
            .HasColumnName("compo");

        builder.Property(e => e.GroupId)
            .HasColumnName("group_id");

        builder.Property(e => e.IsFinal)
            .HasColumnName("is_final");

        builder.Property(e => e.IsApproved)
            .HasColumnName("is_approved");

        builder.Property(e => e.CanAppeal)
            .HasColumnName("can_appeal");

        builder.Property(e => e.Filter)
            .HasMaxLength(50)
            .HasColumnName("filter");

        builder.Property(e => e.DisplayOrder)
            .HasColumnName("display_order");

        builder.Property(e => e.IsCancel)
            .HasColumnName("is_cancel");

        builder.Property(e => e.IsDisposition)
            .HasColumnName("is_disposition");

        builder.Property(e => e.IsFormal)
            .HasColumnName("is_formal");

        // Relationships
        builder.HasOne(d => d.Module)
            .WithMany()
            .HasForeignKey(d => d.ModuleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_status_code_core_lkup_module");

        builder.HasOne(d => d.Group)
            .WithMany()
            .HasForeignKey(d => d.GroupId)
            .HasConstraintName("FK_core_status_code_core_user_group");

        // Indexes
        builder.HasIndex(e => e.ModuleId, "IX_core_status_code_module_id");

        builder.HasIndex(e => e.GroupId, "IX_core_status_code_group_id");

        builder.HasIndex(e => new { e.Compo, e.ModuleId }, "IX_core_status_code_compo_module");

        builder.HasIndex(e => e.IsFinal)
            .HasDatabaseName("IX_core_status_code_is_final")
            .HasFilter("is_final = 1");

        builder.HasIndex(e => e.DisplayOrder, "IX_core_status_code_display_order");
        
        builder.HasIndex(e => e.IsApproved)
            .HasDatabaseName("IX_core_status_code_is_approved")
            .HasFilter("is_approved = 1");
        
        builder.HasIndex(e => e.IsCancel)
            .HasDatabaseName("IX_core_status_code_is_cancel")
            .HasFilter("is_cancel = 1");
        
        builder.HasIndex(e => e.IsDisposition)
            .HasDatabaseName("IX_core_status_code_is_disposition")
            .HasFilter("is_disposition = 1");
    }
}
