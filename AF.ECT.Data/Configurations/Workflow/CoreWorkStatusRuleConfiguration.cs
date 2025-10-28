using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework configuration for the CoreWorkStatusRule entity.
/// </summary>
public class CoreWorkStatusRuleConfiguration : IEntityTypeConfiguration<CoreWorkStatusRule>
{
    /// <summary>
    /// Configures the CoreWorkStatusRule entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreWorkStatusRule> builder)
    {
        builder.ToTable("Core_WorkStatusRule", "dbo");

        builder.HasKey(e => e.WsrId)
            .HasName("PK_Core_WorkStatusRule");

        builder.Property(e => e.WsrId).HasColumnName("wsrID");
        builder.Property(e => e.WsoId).HasColumnName("wsoID");
        builder.Property(e => e.RuleId).HasColumnName("RuleID");
        builder.Property(e => e.RuleData).HasColumnName("RuleData");
        builder.Property(e => e.CheckAll).HasColumnName("CheckAll");

        builder.HasIndex(e => e.WsoId, "IX_Core_WorkStatusRule_wsoID");
        builder.HasIndex(e => e.RuleId, "IX_Core_WorkStatusRule_RuleID");
        builder.HasIndex(e => e.CheckAll, "IX_Core_WorkStatusRule_CheckAll");
        builder.HasIndex(e => new { e.WsoId, e.RuleId }, "IX_Core_WorkStatusRule_wsoID_RuleID");
    }
}
