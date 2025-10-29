using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework configuration for the CoreWorkStatusTracking entity.
/// </summary>
public class CoreWorkStatusTrackingConfiguration : IEntityTypeConfiguration<CoreWorkStatusTracking>
{
    /// <summary>
    /// Configures the CoreWorkStatusTracking entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreWorkStatusTracking> builder)
    {
        builder.ToTable("Core_WorkStatusTracking", "dbo");

        builder.HasKey(e => e.WstId)
            .HasName("PK_Core_WorkStatusTracking");

        builder.Property(e => e.WstId).HasColumnName("wstID");
        builder.Property(e => e.WsId).HasColumnName("wsID");
        builder.Property(e => e.RefId).HasColumnName("RefID");
        builder.Property(e => e.Module).HasColumnName("Module");
        builder.Property(e => e.StartDate).HasColumnName("StartDate");
        builder.Property(e => e.EndDate).HasColumnName("EndDate");
        builder.Property(e => e.CompletedBy).HasColumnName("CompletedBy");
        builder.Property(e => e.WorkflowId).HasColumnName("WorkflowID");
        builder.Property(e => e.Rank).HasColumnName("Rank");
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("Name");

        builder.HasIndex(e => e.WsId, "IX_Core_WorkStatusTracking_wsID");
        builder.HasIndex(e => e.RefId, "IX_Core_WorkStatusTracking_RefID");
        builder.HasIndex(e => e.StartDate, "IX_Core_WorkStatusTracking_StartDate");

        builder.HasIndex(e => e.EndDate, "IX_Core_WorkStatusTracking_EndDate");

        builder.HasIndex(e => new { e.RefId, e.Module, e.WsId }, "IX_Core_WorkStatusTracking_Ref_Module_Ws");

        builder.HasIndex(e => new { e.StartDate, e.EndDate }, "IX_Core_WorkStatusTracking_StartDate_EndDate");

        builder.HasIndex(e => e.WorkflowId, "IX_Core_WorkStatusTracking_WorkflowID");

        builder.HasIndex(e => e.CompletedBy, "IX_Core_WorkStatusTracking_CompletedBy");

        builder.HasIndex(e => e.Module, "IX_Core_WorkStatusTracking_Module");
    }
}
