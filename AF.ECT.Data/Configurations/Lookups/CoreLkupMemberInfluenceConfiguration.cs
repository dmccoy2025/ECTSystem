using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupMemberInfluence"/>.
/// </summary>
public class CoreLkupMemberInfluenceConfiguration : IEntityTypeConfiguration<CoreLkupMemberInfluence>
{
    /// <summary>
    /// Configures the entity type for CoreLkupMemberInfluence.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupMemberInfluence> builder)
    {
        builder.ToTable("Core_Lkup_MemberInfluence", "dbo");

        builder.HasKey(e => e.InfluenceId)
            .HasName("PK_Core_Lkup_MemberInfluence");

        builder.Property(e => e.InfluenceId).HasColumnName("InfluenceID");
        builder.Property(e => e.InfluenceDescription).HasMaxLength(255);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_MemberInfluence_SortOrder");
    }
}
