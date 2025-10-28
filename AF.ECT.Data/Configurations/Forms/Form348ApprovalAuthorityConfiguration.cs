using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348ApprovalAuthority"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents approval authority definitions with signature blocks and effective dates.
/// </remarks>
public class Form348ApprovalAuthorityConfiguration : IEntityTypeConfiguration<Form348ApprovalAuthority>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348ApprovalAuthority> builder)
    {
        // Table mapping
        builder.ToTable("Form348ApprovalAuthority", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Form348ApprovalAuthority");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Name).HasColumnName("Name").HasMaxLength(200).IsRequired();
        builder.Property(e => e.SigBlock).HasColumnName("SigBlock").IsRequired();
        builder.Property(e => e.Title).HasColumnName("Title").HasMaxLength(200).IsRequired();
        builder.Property(e => e.EffectiveDate).HasColumnName("EffectiveDate");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");

        // Indexes for query performance
        builder.HasIndex(e => e.EffectiveDate, "IX_Form348ApprovalAuthority_EffectiveDate");
        builder.HasIndex(e => e.Name, "IX_Form348ApprovalAuthority_Name");
        builder.HasIndex(e => e.CreatedBy, "IX_Form348ApprovalAuthority_CreatedBy");
        builder.HasIndex(e => e.CreatedDate, "IX_Form348ApprovalAuthority_CreatedDate");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form348ApprovalAuthority_ModifiedBy");
        builder.HasIndex(e => e.ModifiedDate, "IX_Form348ApprovalAuthority_ModifiedDate");
    }
}
