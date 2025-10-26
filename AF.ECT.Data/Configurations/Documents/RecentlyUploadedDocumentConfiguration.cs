using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Documents;

/// <summary>
/// Entity Framework Core configuration for the RecentlyUploadedDocument entity.
/// Configures view for tracking recently uploaded documents with references to document
/// groups, document IDs, and document types for quick access to recent file uploads.
/// </summary>
public class RecentlyUploadedDocumentConfiguration : IEntityTypeConfiguration<RecentlyUploadedDocument>
{
    /// <summary>
    /// Configures the RecentlyUploadedDocument entity as a keyless view for read-only
    /// access to recent document upload information across the system.
    /// </summary>
    /// <param name="builder">The entity type builder for RecentlyUploadedDocument.</param>
    public void Configure(EntityTypeBuilder<RecentlyUploadedDocument> builder)
    {
        builder.HasNoKey();

        builder.ToView("vw_RecentlyUploadedDocuments");

        builder.Property(e => e.DocGroupId).HasColumnName("DOC_GROUP_ID");
        builder.Property(e => e.DocId).HasColumnName("DOC_ID");
        builder.Property(e => e.DocTypeId).HasColumnName("DOC_TYPE_ID");
        builder.Property(e => e.RefId).HasColumnName("REF_ID");
    }
}
