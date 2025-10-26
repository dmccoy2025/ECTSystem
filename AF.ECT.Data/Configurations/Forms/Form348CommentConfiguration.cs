using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348Comment"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents comments added to Form 348 LOD cases with soft delete capability.
/// </remarks>
public class Form348CommentConfiguration : IEntityTypeConfiguration<Form348Comment>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348Comment> builder)
    {
        // Table mapping
        builder.ToTable("Form348Comment", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Form348Comment");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Lodid).HasColumnName("LODID");
        builder.Property(e => e.Comments).HasColumnName("Comments").IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.Deleted).HasColumnName("Deleted");

        // Indexes for query performance
        builder.HasIndex(e => e.Lodid, "IX_Form348Comment_LODID");
        builder.HasIndex(e => new { e.Lodid, e.Deleted }, "IX_Form348Comment_LODID_Deleted");
        builder.HasIndex(e => e.CreatedDate, "IX_Form348Comment_CreatedDate");
    }
}
