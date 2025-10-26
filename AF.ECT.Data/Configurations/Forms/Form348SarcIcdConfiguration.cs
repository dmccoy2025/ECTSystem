using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348SarcIcd"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents ICD (International Classification of Diseases) codes associated with SARC cases.
/// </remarks>
public class Form348SarcIcdConfiguration : IEntityTypeConfiguration<Form348SarcIcd>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348SarcIcd> builder)
    {
        // Table mapping
        builder.ToTable("Form348SARC_ICD", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Form348SARC_ICD");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Sarcid).HasColumnName("SARCID");
        builder.Property(e => e.IcdcodeId).HasColumnName("ICDCodeID");
        builder.Property(e => e.Icd7thChar).HasColumnName("ICD7thChar").HasMaxLength(1);

        // Indexes for query performance
        builder.HasIndex(e => e.Sarcid, "IX_Form348SARC_ICD_SARCID");
        builder.HasIndex(e => new { e.Sarcid, e.IcdcodeId }, "IX_Form348SARC_ICD_SARCID_ICDCodeID");
    }
}
