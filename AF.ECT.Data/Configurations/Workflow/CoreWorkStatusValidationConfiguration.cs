using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework configuration for the CoreWorkStatusValidation entity.
/// </summary>
public class CoreWorkStatusValidationConfiguration : IEntityTypeConfiguration<CoreWorkStatusValidation>
{
    /// <summary>
    /// Configures the CoreWorkStatusValidation entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreWorkStatusValidation> builder)
    {
        builder.ToTable("Core_WorkStatusValidation", "dbo");

        builder.HasKey(e => e.WsvId)
            .HasName("PK_Core_WorkStatusValidation");

        builder.Property(e => e.WsvId).HasColumnName("wsvID");
        builder.Property(e => e.WsId).HasColumnName("wsID");
        builder.Property(e => e.ValidationType).HasColumnName("ValidationType");
        builder.Property(e => e.Data).HasColumnName("Data");
        builder.Property(e => e.Active).HasColumnName("Active");

        builder.HasIndex(e => e.WsId, "IX_Core_WorkStatusValidation_wsID");
        builder.HasIndex(e => e.ValidationType, "IX_Core_WorkStatusValidation_ValidationType");
        builder.HasIndex(e => e.Active, "IX_Core_WorkStatusValidation_Active")
            .HasFilter("Active = 1");
        builder.HasIndex(e => new { e.WsId, e.Active }, "IX_Core_WorkStatusValidation_wsID_Active");
    }
}
