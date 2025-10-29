using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Logging;

/// <summary>
/// Entity Framework configuration for the CoreLogAction entity.
/// </summary>
public class CoreLogActionConfiguration : IEntityTypeConfiguration<CoreLogAction>
{
    /// <summary>
    /// Configures the CoreLogAction entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLogAction> builder)
    {
        builder.ToTable("Core_LogAction", "dbo");

        builder.HasKey(e => e.LogId)
            .HasName("PK_Core_LogAction");

        builder.Property(e => e.LogId).HasColumnName("LogID");
        builder.Property(e => e.ModuleId).HasColumnName("ModuleID");
        builder.Property(e => e.ActionId).HasColumnName("ActionID");
        builder.Property(e => e.ActionDate).HasColumnName("ActionDate");
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.ReferenceId).HasColumnName("ReferenceID");
        builder.Property(e => e.Notes).HasColumnName("Notes");
        builder.Property(e => e.Status).HasColumnName("Status");
        builder.Property(e => e.NewStatus).HasColumnName("NewStatus");
        builder.Property(e => e.Address)
            .HasMaxLength(50)
            .HasColumnName("Address");

        builder.HasIndex(e => e.ActionDate, "IX_Core_LogAction_ActionDate");
        builder.HasIndex(e => e.UserId, "IX_Core_LogAction_UserID");
        builder.HasIndex(e => e.ReferenceId, "IX_Core_LogAction_ReferenceID");

        builder.HasIndex(e => e.ModuleId, "IX_Core_LogAction_ModuleID");

        builder.HasIndex(e => e.ActionId, "IX_Core_LogAction_ActionID");

        builder.HasIndex(e => new { e.ModuleId, e.ActionId }, "IX_Core_LogAction_Module_Action");

        builder.HasIndex(e => new { e.UserId, e.ActionDate }, "IX_Core_LogAction_User_Date");

        builder.HasIndex(e => new { e.ReferenceId, e.ModuleId }, "IX_Core_LogAction_Ref_Module");

        builder.HasIndex(e => e.Status, "IX_Core_LogAction_Status");

        builder.HasIndex(e => e.NewStatus, "IX_Core_LogAction_NewStatus");
    }
}
