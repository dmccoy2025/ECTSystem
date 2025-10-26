using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity Framework Core configuration for the Return entity.
/// Configures case return tracking when cases are sent back to previous workflow steps
/// with reasons, explanations, sender/recipient information, and routing details.
/// </summary>
public class ReturnConfiguration : IEntityTypeConfiguration<Return>
{
    /// <summary>
    /// Configures the Return entity with table mapping, primary key, properties,
    /// and indexes for tracking case returns and workflow reversals.
    /// </summary>
    /// <param name="builder">The entity type builder for Return.</param>
    public void Configure(EntityTypeBuilder<Return> builder)
    {
        builder.HasKey(e => e.ReturnId).HasName("PK__RETURN__F445E9485FA1564B");

        builder.ToTable("RETURN", "dbo");

        builder.Property(e => e.ReturnId).HasColumnName("RETURN_ID");
        builder.Property(e => e.BoardReturn).HasColumnName("BOARD_RETURN");
        builder.Property(e => e.CommentsBackToSender)
            .HasMaxLength(1000)
            .IsUnicode(false)
            .HasColumnName("COMMENTS_BACK_TO_SENDER");
        builder.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");
        builder.Property(e => e.CreatedDate).HasColumnName("CREATED_DATE");
        builder.Property(e => e.DateSent).HasColumnName("DATE_SENT");
        builder.Property(e => e.DateSentBack).HasColumnName("DATE_SENT_BACK");
        builder.Property(e => e.ExplanationForSendingBack)
            .HasMaxLength(1000)
            .IsUnicode(false)
            .HasColumnName("EXPLANATION_FOR_SENDING_BACK");
        builder.Property(e => e.ReasonSentBack).HasColumnName("REASON_SENT_BACK");
        builder.Property(e => e.RefId).HasColumnName("REF_ID");
        builder.Property(e => e.Rerouting).HasColumnName("REROUTING");
        builder.Property(e => e.SentTo).HasColumnName("SENT_TO");
        builder.Property(e => e.Sender).HasColumnName("SENDER");
        builder.Property(e => e.Workflow).HasColumnName("WORKFLOW");
        builder.Property(e => e.WorkstatusFrom).HasColumnName("WORKSTATUS_FROM");
        builder.Property(e => e.WorkstatusTo).HasColumnName("WORKSTATUS_TO");

        builder.HasIndex(e => e.RefId, "IX_RETURN_REF_ID");
        builder.HasIndex(e => e.Workflow, "IX_RETURN_WORKFLOW");
        builder.HasIndex(e => e.DateSent, "IX_RETURN_DATE_SENT");
    }
}
