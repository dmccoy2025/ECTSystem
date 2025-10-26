using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupFindingsText"/>.
/// </summary>
public class CoreLkupFindingsTextConfiguration : IEntityTypeConfiguration<CoreLkupFindingsText>
{
    /// <summary>
    /// Configures the entity type for CoreLkupFindingsText.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupFindingsText> builder)
    {
        builder.ToTable("Core_Lkup_FindingsText", "dbo");

        builder.HasKey(e => e.FindingId)
            .HasName("PK_Core_Lkup_FindingsText");

        builder.Property(e => e.FindingId).HasColumnName("FindingID");
        builder.Property(e => e.Description).HasMaxLength(1000);

        builder.HasIndex(e => e.Workflow, "IX_Core_Lkup_FindingsText_Workflow");
        builder.HasIndex(e => e.GroupId, "IX_Core_Lkup_FindingsText_GroupId");
    }
}
