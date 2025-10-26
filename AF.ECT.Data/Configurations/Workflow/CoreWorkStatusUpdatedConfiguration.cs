using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework configuration for the CoreWorkStatusUpdated entity.
/// </summary>
public class CoreWorkStatusUpdatedConfiguration : IEntityTypeConfiguration<CoreWorkStatusUpdated>
{
    /// <summary>
    /// Configures the CoreWorkStatusUpdated entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreWorkStatusUpdated> builder)
    {
        builder.ToTable("Core_WorkStatusUpdated");

        builder.HasKey(e => new { e.WsId, e.WorkflowId, e.StatusId });

        builder.Property(e => e.WsId).HasColumnName("wsID");
        builder.Property(e => e.WorkflowId).HasColumnName("WorkflowID");
        builder.Property(e => e.StatusId).HasColumnName("StatusID");
        builder.Property(e => e.SortOrder).HasColumnName("SortOrder");

        builder.HasIndex(e => e.WsId, "IX_Core_WorkStatusUpdated_wsID");
        builder.HasIndex(e => e.WorkflowId, "IX_Core_WorkStatusUpdated_WorkflowID");
    }
}
