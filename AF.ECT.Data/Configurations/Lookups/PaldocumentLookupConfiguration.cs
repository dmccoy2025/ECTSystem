using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Entity Framework Core configuration for the PaldocumentLookup entity.
/// Configures PAL document lookup reference table linking document URLs to member
/// information for quick PAL document retrieval and archival access.
/// </summary>
public class PaldocumentLookupConfiguration : IEntityTypeConfiguration<PaldocumentLookup>
{
    /// <summary>
    /// Configures the PaldocumentLookup entity with table mapping, primary key, properties,
    /// and indexes for efficient PAL document URL lookups by member and date.
    /// </summary>
    /// <param name="builder">The entity type builder for PaldocumentLookup.</param>
    public void Configure(EntityTypeBuilder<PaldocumentLookup> builder)
    {
        builder.HasKey(e => e.PalDocId).HasName("PK__PALDOCUM__41E3003001E7F7B6");

        builder.ToTable("PALDOCUMENT_LOOKUP", "dbo");

        builder.Property(e => e.PalDocId).HasColumnName("PAL_DOC_ID");
        builder.Property(e => e.DocMonth).HasColumnName("DOC_MONTH");
        builder.Property(e => e.DocYear).HasColumnName("DOC_YEAR");
        builder.Property(e => e.Last4Ssn)
            .HasMaxLength(4)
            .IsUnicode(false)
            .HasColumnName("LAST_4_SSN");
        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LAST_NAME");
        builder.Property(e => e.Url)
            .HasMaxLength(300)
            .IsUnicode(false)
            .HasColumnName("URL");

        builder.HasIndex(e => new { e.LastName, e.Last4Ssn }, "IX_PALDOCUMENT_LOOKUP_NAME_SSN");
        builder.HasIndex(e => new { e.DocYear, e.DocMonth }, "IX_PALDOCUMENT_LOOKUP_DATE");
    }
}
