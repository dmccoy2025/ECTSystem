using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Configuration for the <see cref="CoreWorkflowLock"/> entity.
/// </summary>
public class CoreWorkflowLockConfiguration : IEntityTypeConfiguration<CoreWorkflowLock>
{
    public void Configure(EntityTypeBuilder<CoreWorkflowLock> builder)
    {
        // Table mapping
        builder.ToTable("core_workflow_lock", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_workflow_lock");

        // Properties
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.RefId)
            .HasColumnName("ref_id");

        builder.Property(e => e.Module)
            .HasColumnName("module");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.LockTime)
            .HasColumnType("datetime")
            .HasColumnName("lock_time");

        // Indexes
        builder.HasIndex(e => new { e.RefId, e.Module }, "IX_core_workflow_lock_ref_module");

        builder.HasIndex(e => e.UserId, "IX_core_workflow_lock_user_id");

        builder.HasIndex(e => e.LockTime, "IX_core_workflow_lock_lock_time");
    }
}
