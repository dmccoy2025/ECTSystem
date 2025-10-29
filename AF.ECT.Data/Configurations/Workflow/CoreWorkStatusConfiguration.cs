using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreWorkStatus"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema and relationships for the core_work_status table,
/// which represents individual statuses within workflow definitions. Each workflow consists
/// of multiple statuses that define the stages a case passes through from initiation to
/// completion. Statuses determine available actions, required validations, routing rules,
/// and user permissions at each stage of case processing. Board statuses indicate review
/// levels, holding statuses pause automatic progression, and consult statuses enable
/// subject matter expert review.
/// </remarks>
public class CoreWorkStatusConfiguration : IEntityTypeConfiguration<CoreWorkStatus>
{
    /// <summary>
    /// Configures the entity of type <see cref="CoreWorkStatus"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreWorkStatus> builder)
    {
        // Table mapping
        builder.ToTable("core_work_status", "dbo");

        // Primary key
        builder.HasKey(e => e.WsId)
            .HasName("PK_core_work_status");

        // Properties configuration
        builder.Property(e => e.WsId)
            .HasColumnName("ws_id");

        builder.Property(e => e.WorkflowId)
            .HasColumnName("workflow_id");

        builder.Property(e => e.StatusId)
            .HasColumnName("status_id");

        builder.Property(e => e.SortOrder)
            .HasColumnName("sort_order");

        builder.Property(e => e.IsBoardStatus)
            .HasColumnName("is_board_status");

        builder.Property(e => e.DisplayText)
            .HasMaxLength(200)
            .HasColumnName("display_text");

        builder.Property(e => e.IsHolding)
            .HasColumnName("is_holding");

        builder.Property(e => e.Compo)
            .HasMaxLength(10)
            .HasColumnName("compo");

        builder.Property(e => e.IsConsult)
            .HasColumnName("is_consult");

        // Relationships
        builder.HasOne(d => d.Status)
            .WithMany(p => p.CoreWorkStatuses)
            .HasForeignKey(d => d.StatusId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_work_status_core_status_code");

        builder.HasOne(d => d.Workflow)
            .WithMany(p => p.CoreWorkStatuses)
            .HasForeignKey(d => d.WorkflowId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_work_status_core_workflow");

        // Indexes
        builder.HasIndex(e => new { e.WorkflowId, e.StatusId })
            .HasDatabaseName("IX_core_work_status_workflow_status")
            .IsUnique();

        builder.HasIndex(e => new { e.WorkflowId, e.SortOrder }, "IX_core_work_status_workflow_sort");

        builder.HasIndex(e => e.IsBoardStatus, "IX_core_work_status_is_board");

        builder.HasIndex(e => e.IsHolding, "IX_core_work_status_is_holding");
        
        builder.HasIndex(e => e.IsConsult, "IX_core_work_status_is_consult");
    }
}
