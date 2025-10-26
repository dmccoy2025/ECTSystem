using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupIcd9"/>.
/// </summary>
public class CoreLkupIcd9Configuration : IEntityTypeConfiguration<CoreLkupIcd9>
{
    /// <summary>
    /// Configures the entity type for CoreLkupIcd9.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupIcd9> builder)
    {
        builder.ToTable("Core_Lkup_ICD9", "dbo");

        builder.HasKey(e => e.Icd9Id)
            .HasName("PK_Core_Lkup_ICD9");

        builder.Property(e => e.Icd9Id).HasColumnName("ICD9_ID");
        builder.Property(e => e.Value).HasMaxLength(50);
        builder.Property(e => e.Text).HasMaxLength(500);
        builder.Property(e => e.ParentId).HasColumnName("ParentID");
        builder.Property(e => e.Icdversion).HasColumnName("ICDVersion");

        builder.HasIndex(e => e.Value, "IX_Core_Lkup_ICD9_Value");
        builder.HasIndex(e => e.ParentId, "IX_Core_Lkup_ICD9_ParentID");
        builder.HasIndex(e => e.Active, "IX_Core_Lkup_ICD9_Active");
    }
}
