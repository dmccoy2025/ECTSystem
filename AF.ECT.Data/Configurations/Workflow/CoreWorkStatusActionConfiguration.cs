using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework Core configuration for the <see cref="CoreWorkStatusAction"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema, relationships, and constraints for the core_work_status_action table,
/// which maps workflow actions to workflow status options, defining which actions are available for each
/// status option and what the target state is when that action is performed.
/// </remarks>
public class CoreWorkStatusActionConfiguration : IEntityTypeConfiguration<CoreWorkStatusAction>
{
    /// <summary>
    /// Configures the CoreWorkStatusAction entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreWorkStatusAction> builder)
    {
        // Table mapping
        builder.ToTable("core_work_status_action", "dbo");

        // Primary key
        builder.HasKey(e => e.WsaId)
            .HasName("PK_core_work_status_action");

        // Property configurations
        builder.Property(e => e.WsaId)
            .HasColumnName("wsa_id");

        builder.Property(e => e.WsoId)
            .HasColumnName("wso_id");

        builder.Property(e => e.ActionType)
            .HasColumnName("action_type");

        builder.Property(e => e.Target)
            .HasColumnName("target");

        builder.Property(e => e.Data)
            .HasColumnName("data");

        // Foreign key relationships
        builder.HasOne(d => d.ActionTypeNavigation)
            .WithMany(p => p.CoreWorkStatusActions)
            .HasForeignKey(d => d.ActionType)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_work_status_action_action_type");

        builder.HasOne(d => d.Wso)
            .WithMany(p => p.CoreWorkStatusActions)
            .HasForeignKey(d => d.WsoId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_work_status_action_wso");

        // Indexes
        builder.HasIndex(e => e.WsoId, "IX_core_work_status_action_wso_id");

        builder.HasIndex(e => e.ActionType, "IX_core_work_status_action_action_type");

        builder.HasIndex(e => new { e.WsoId, e.ActionType }, "IX_core_work_status_action_wso_action");
        
        builder.HasIndex(e => e.Target, "IX_core_work_status_action_target");
    }
}
