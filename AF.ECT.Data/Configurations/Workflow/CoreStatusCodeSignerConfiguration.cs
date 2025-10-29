using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework configuration for the CoreStatusCodeSigner entity.
/// </summary>
public class CoreStatusCodeSignerConfiguration : IEntityTypeConfiguration<CoreStatusCodeSigner>
{
    /// <summary>
    /// Configures the CoreStatusCodeSigner entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreStatusCodeSigner> builder)
    {
        builder.ToTable("Core_StatusCodeSigner", "dbo");

        builder.HasKey(e => new { e.Status, e.GroupId })
            .HasName("PK_Core_StatusCodeSigner");

        builder.Property(e => e.Status).HasColumnName("Status");
        builder.Property(e => e.GroupId).HasColumnName("GroupID");

        // Relationships
        builder.HasOne(d => d.Group)
            .WithMany()
            .HasForeignKey(d => d.GroupId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_core_status_code_signer_core_user_group");

        builder.HasOne(d => d.StatusNavigation)
            .WithMany()
            .HasForeignKey(d => d.Status)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Core_StatusCodeSigner_core_work_status");

        builder.HasIndex(e => e.Status, "IX_Core_StatusCodeSigner_Status");
        builder.HasIndex(e => e.GroupId, "IX_Core_StatusCodeSigner_GroupID");
    }
}
