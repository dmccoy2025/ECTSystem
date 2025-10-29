using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework configuration for the <see cref="ImpLodRwoa"/> entity.
/// Configures a staging table for importing LOD (Line of Duty) RWOA (Return Without Authority)
/// tracking data from legacy systems.
/// </summary>
/// <remarks>
/// ImpLodRwoa is a temporary staging table used during data migration processes to load
/// RWOA (Return Without Authority) case information associated with LOD cases from legacy systems.
/// This entity tracks when cases are sent back for corrections and the workflow of returns.
/// This entity has no primary key (keyless entity) as it represents transient import data.
/// 
/// Key characteristics:
/// - Keyless entity (HasNoKey) for import staging
/// - All nullable string properties to accommodate raw import data
/// - RWOA and LOD identifiers
/// - Return workflow tracking (sent to, sender, dates sent/returned)
/// - Reason and explanation for sending back
/// - Comments from recipient back to sender
/// - String-based audit fields for flexible import
/// - No foreign key relationships (staging isolation)
/// - Temporary data cleared after successful migration to production tables
/// </remarks>
public class ImpLodRwoaConfiguration : IEntityTypeConfiguration<ImpLodRwoa>
{
    /// <summary>
    /// Configures the ImpLodRwoa entity as a keyless staging table with RWOA tracking import fields.
    /// </summary>
    /// <param name="builder">The entity type builder for ImpLodRwoa.</param>
    public void Configure(EntityTypeBuilder<ImpLodRwoa> builder)
    {
        builder.ToTable("ImpLodRWOA", "dbo");

        // Keyless entity for staging
        builder.HasNoKey();

        // RWOA and LOD identifiers
        builder.Property(e => e.RwoaId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("RWOA_ID");

        builder.Property(e => e.LodId)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LOD_ID");

        // Return workflow properties
        builder.Property(e => e.SentTo)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("SENT_TO");

        builder.Property(e => e.Sender)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("SENDER");

        builder.Property(e => e.DateSent)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DATE_SENT");

        builder.Property(e => e.DateSentBack)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("DATE_SENT_BACK");

        // Reason and explanation properties
        builder.Property(e => e.ReasonSentBack)
            .HasMaxLength(200)
            .IsUnicode(false)
            .HasColumnName("REASON_SENT_BACK");

        builder.Property(e => e.ExplanationForSendingBack)
            .HasColumnType("ntext")
            .HasColumnName("EXPLANATION_FOR_SENDING_BACK");

        builder.Property(e => e.CommentsBackToSender)
            .HasColumnType("ntext")
            .HasColumnName("COMMENTS_BACK_TO_SENDER");

        // Audit properties (string-based for import staging)
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_BY");

        builder.Property(e => e.CreatedDate)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("CREATED_DATE");
        
        // Indexes for common queries
        builder.HasIndex(e => e.RwoaId, "IX_imp_lod_rwoa_rwoa_id");
        
        builder.HasIndex(e => e.LodId, "IX_imp_lod_rwoa_lod_id");
        
        builder.HasIndex(e => e.DateSent, "IX_imp_lod_rwoa_date_sent");
        
        builder.HasIndex(e => e.SentTo, "IX_imp_lod_rwoa_sent_to");
    }
}
