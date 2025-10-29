using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AfrcOracleLodRwoaDatum"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the AfrcOracle_LOD_RWOA_Data table,
/// which stores Line of Duty Returned Without Action (RWOA) tracking data from AFRC Oracle migration.
/// Contains RWOA workflow information including sender, recipient, reasons for return,
/// explanations, dates, and comments for cases sent back for corrections or additional information.
/// All properties are nullable strings for Oracle import staging.
/// </remarks>
public class AfrcOracleLodRwoaDatumConfiguration : IEntityTypeConfiguration<AfrcOracleLodRwoaDatum>
{
    /// <summary>
    /// Configures the entity of type <see cref="AfrcOracleLodRwoaDatum"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AfrcOracleLodRwoaDatum> builder)
    {
        // Table mapping
        builder.ToTable("AfrcOracle_LOD_RWOA_Data", "dbo");

        // No primary key - this is a staging/import table
        builder.HasNoKey();

        // Properties configuration
        builder.Property(e => e.RwoaId)
            .HasColumnName("rwoa_id");

        builder.Property(e => e.LodId)
            .HasColumnName("lod_id");

        builder.Property(e => e.SentTo)
            .HasColumnName("sent_to");

        builder.Property(e => e.ReasonSentBack)
            .HasColumnName("reason_sent_back");

        builder.Property(e => e.ExplanationForSendingBack)
            .HasColumnName("explanation_for_sending_back");

        builder.Property(e => e.Sender)
            .HasColumnName("sender");

        builder.Property(e => e.DateSent)
            .HasColumnName("date_sent");

        builder.Property(e => e.CommentsBackToSender)
            .HasColumnName("comments_back_to_sender");

        builder.Property(e => e.DateSentBack)
            .HasColumnName("date_sent_back");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.CreatedDate)
            .HasColumnName("created_date");

        // Indexes for common queries
        builder.HasIndex(e => e.LodId, "IX_afrc_oracle_lod_rwoa_lod_id");

        builder.HasIndex(e => e.RwoaId, "IX_afrc_oracle_lod_rwoa_rwoa_id");

        builder.HasIndex(e => e.DateSent, "IX_afrc_oracle_lod_rwoa_date_sent");
    }
}
