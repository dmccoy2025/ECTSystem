using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Workflow;

/// <summary>
/// Entity type configuration for the <see cref="Rwoa"/> entity.
/// Configures the schema, table name, primary key, properties, and relationships for RWOA (Return Without Action) tracking.
/// </summary>
public class RwoaConfiguration : IEntityTypeConfiguration<Rwoa>
{
    /// <summary>
    /// Configures the entity of type <see cref="Rwoa"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Rwoa> builder)
    {
        builder.ToTable("RWOA", "dbo");

        // Primary Key
        builder.HasKey(e => e.RwoaId)
            .HasName("PK_RWOA");

        // Properties
        builder.Property(e => e.RwoaId)
            .HasColumnName("RWOAId");

        builder.Property(e => e.RefId)
            .HasColumnName("RefID");

        builder.Property(e => e.Workstatus)
            .HasColumnName("workstatus");

        builder.Property(e => e.Workflow)
            .HasColumnName("workflow");

        builder.Property(e => e.SentTo)
            .HasMaxLength(50)
            .HasColumnName("sentTo");

        builder.Property(e => e.ReasonSentBack)
            .HasColumnName("reasonSentBack");

        builder.Property(e => e.ExplanationForSendingBack)
            .HasColumnType("text")
            .HasColumnName("explanationForSendingBack");

        builder.Property(e => e.Sender)
            .HasMaxLength(50)
            .HasColumnName("sender");

        builder.Property(e => e.DateSent)
            .HasColumnType("datetime")
            .HasColumnName("dateSent");

        builder.Property(e => e.CommentsBackToSender)
            .HasColumnType("text")
            .HasColumnName("commentsBackToSender");

        builder.Property(e => e.DateSentBack)
            .HasColumnType("datetime")
            .HasColumnName("dateSentBack");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("createdBy");

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime")
            .HasColumnName("createdDate");

        builder.Property(e => e.Rerouting)
            .HasColumnName("rerouting");

        // Indexes
        builder.HasIndex(e => e.RefId)
            .HasDatabaseName("IX_RWOA_RefID");

        builder.HasIndex(e => e.Workflow)
            .HasDatabaseName("IX_RWOA_Workflow");

        builder.HasIndex(e => e.DateSent)
            .HasDatabaseName("IX_RWOA_DateSent");

        builder.HasIndex(e => e.CreatedBy)
            .HasDatabaseName("IX_RWOA_CreatedBy");
    }
}
