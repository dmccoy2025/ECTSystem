using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Lookups;

/// <summary>
/// Configures the entity mapping for <see cref="CoreLkupMemberStatus"/>.
/// </summary>
public class CoreLkupMemberStatusConfiguration : IEntityTypeConfiguration<CoreLkupMemberStatus>
{
    /// <summary>
    /// Configures the entity type for CoreLkupMemberStatus.
    /// </summary>
    /// <param name="builder">The builder to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<CoreLkupMemberStatus> builder)
    {
        builder.ToTable("Core_Lkup_MemberStatus", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Lkup_MemberStatus");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.MemberType).HasMaxLength(100);
        builder.Property(e => e.MemberDescr).HasMaxLength(255);

        builder.HasIndex(e => e.SortOrder, "IX_Core_Lkup_MemberStatus_SortOrder");
    }
}
