using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Configuration for the <see cref="CoreWitness"/> entity.
/// </summary>
public class CoreWitnessConfiguration : IEntityTypeConfiguration<CoreWitness>
{
    public void Configure(EntityTypeBuilder<CoreWitness> builder)
    {
        // Table mapping
        builder.ToTable("core_witness", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_core_witness");

        // Properties
        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.LodId)
            .HasColumnName("lod_id");

        builder.Property(e => e.WitnessType)
            .IsRequired()
            .HasColumnName("witness_type");

        builder.Property(e => e.OtherType)
            .IsRequired()
            .HasColumnName("other_type");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnName("name");

        builder.Property(e => e.Address)
            .HasMaxLength(2000)
            .HasColumnName("address");

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("modified_by");

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime")
            .HasColumnName("modified_date");

        // Indexes
        builder.HasIndex(e => e.LodId, "IX_core_witness_lod_id");
    }
}
