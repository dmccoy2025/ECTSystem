using Microsoft.EntityFrameworkCore;
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

        builder.HasIndex(e => e.Status, "IX_Core_StatusCodeSigner_Status");
        builder.HasIndex(e => e.GroupId, "IX_Core_StatusCodeSigner_GroupID");
    }
}
