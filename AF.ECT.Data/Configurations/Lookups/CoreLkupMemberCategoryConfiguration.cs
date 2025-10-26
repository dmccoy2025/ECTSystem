using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupMemberCategory"/>.
/// </summary>
public class CoreLkupMemberCategoryConfiguration : IEntityTypeConfiguration<CoreLkupMemberCategory>
{
    /// <summary>
    /// Configures the entity type for CoreLkupMemberCategory.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupMemberCategory> builder)
    {
        builder.ToTable("Core_Lkup_MemberCategory", "dbo");

        builder.HasKey(e => e.MemberStatusId)
            .HasName("PK_Core_Lkup_MemberCategory");

        builder.Property(e => e.MemberStatusId).HasColumnName("MemberStatusID");
        builder.Property(e => e.MemberStatusDesc).HasMaxLength(255);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_MemberCategory_SortOrder");
    }
}
