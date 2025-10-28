using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework configuration for the CoreSignOnlySignature entity.
/// </summary>
public class CoreSignOnlySignatureConfiguration : IEntityTypeConfiguration<CoreSignOnlySignature>
{
    /// <summary>
    /// Configures the CoreSignOnlySignature entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreSignOnlySignature> builder)
    {
        builder.ToTable("Core_SignOnlySignature", "dbo");

        builder.HasKey(e => new { e.RefId, e.Workflow })
            .HasName("PK_Core_SignOnlySignature");

        builder.Property(e => e.RefId).HasColumnName("RefID");
        builder.Property(e => e.Workflow).HasColumnName("Workflow");
        builder.Property(e => e.Signature).HasColumnName("Signature");
        builder.Property(e => e.SigDate)
            .HasMaxLength(50)
            .HasColumnName("SigDate");
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.Ptype).HasColumnName("PType");

        builder.HasIndex(e => e.RefId, "IX_Core_SignOnlySignature_RefID");
        builder.HasIndex(e => e.Workflow, "IX_Core_SignOnlySignature_Workflow");
        builder.HasIndex(e => e.UserId, "IX_Core_SignOnlySignature_UserID");
        builder.HasIndex(e => e.Ptype, "IX_Core_SignOnlySignature_PType");
        builder.HasIndex(e => new { e.RefId, e.Workflow, e.UserId }, "IX_Core_SignOnlySignature_Ref_Workflow_User");
    }
}
