using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreWorkStatusOption"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_work_status_option table,
/// which defines the available action options for each workflow status, including the display text shown to users,
/// the target status when the action is performed, and component-specific options.
/// </remarks>
public class CoreWorkStatusOptionConfiguration : IEntityTypeConfiguration<CoreWorkStatusOption>
{
    /// <summary>
    /// Configures the CoreWorkStatusOption entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreWorkStatusOption> builder)
    {
        // Table mapping
        builder.ToTable("core_work_status_option", "dbo");

        // Primary key
        builder.HasKey(e => e.WsoId)
            .HasName("PK_core_work_status_option");

        // Property configurations
        builder.Property(e => e.WsoId)
            .HasColumnName("wso_id");

        builder.Property(e => e.WsId)
            .HasColumnName("ws_id");

        builder.Property(e => e.DisplayText)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("display_text");

        builder.Property(e => e.Active)
            .HasColumnName("active");

        builder.Property(e => e.SortOrder)
            .HasColumnName("sort_order");

        builder.Property(e => e.Template)
            .HasColumnName("template");

        builder.Property(e => e.WsIdOut)
            .HasColumnName("ws_id_out");

        builder.Property(e => e.Compo)
            .HasColumnName("compo");

        // Foreign key relationships
        builder.HasOne(d => d.Ws)
            .WithMany(p => p.CoreWorkStatusOptionWs)
            .HasForeignKey(d => d.WsId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_work_status_option_core_work_status");

        builder.HasOne(d => d.WsIdOutNavigation)
            .WithMany(p => p.CoreWorkStatusOptionWsIdOutNavigations)
            .HasForeignKey(d => d.WsIdOut)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_work_status_option_ws_id_out");

        // Indexes
        builder.HasIndex(e => e.WsId, "IX_core_work_status_option_ws_id");

        builder.HasIndex(e => e.WsIdOut, "IX_core_work_status_option_ws_id_out");

        builder.HasIndex(e => new { e.WsId, e.Active }, "IX_core_work_status_option_ws_active");

        builder.HasIndex(e => new { e.WsId, e.SortOrder }, "IX_core_work_status_option_ws_sort");
        
        builder.HasIndex(e => e.Active)
            .HasDatabaseName("IX_core_work_status_option_active")
            .HasFilter("active = 1");
        
        builder.HasIndex(e => e.SortOrder, "IX_core_work_status_option_sort_order");
        
        builder.HasIndex(e => e.Template, "IX_core_work_status_option_template");
    }
}
