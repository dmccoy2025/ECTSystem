using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Logging;

/// <summary>
/// Entity Framework configuration for the CoreLogEmail entity.
/// </summary>
public class CoreLogEmailConfiguration : IEntityTypeConfiguration<CoreLogEmail>
{
    /// <summary>
    /// Configures the CoreLogEmail entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreLogEmail> builder)
    {
        builder.ToTable("Core_LogEmail", "dbo");

        builder.HasKey(e => e.EId)
            .HasName("PK_Core_LogEmail");

        builder.Property(e => e.EId).HasColumnName("eID");
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.DateSent).HasColumnName("DateSent");
        builder.Property(e => e.ETo).HasColumnName("eTo");
        builder.Property(e => e.ECc).HasColumnName("eCC");
        builder.Property(e => e.EBcc).HasColumnName("eBCC");
        builder.Property(e => e.Subject).HasColumnName("Subject");
        builder.Property(e => e.Body).HasColumnName("Body");
        builder.Property(e => e.Failed).HasColumnName("Failed");
        builder.Property(e => e.TemplateId).HasColumnName("TemplateID");

        builder.HasIndex(e => e.DateSent, "IX_Core_LogEmail_DateSent");
        builder.HasIndex(e => e.UserId, "IX_Core_LogEmail_UserID");
        builder.HasIndex(e => e.TemplateId, "IX_Core_LogEmail_TemplateID");
    }
}
