using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity type configuration for the <see cref="TestCommandStructChain"/> entity.
/// Configures the schema, table name, primary key, properties, relationships, and indexes for test command structure chain data.
/// </summary>
public class TestCommandStructChainConfiguration : IEntityTypeConfiguration<TestCommandStructChain>
{
    /// <summary>
    /// Configures the entity of type <see cref="TestCommandStructChain"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TestCommandStructChain> builder)
    {
        builder.ToTable("Test_CommandStructChain", "dbo");

        // Primary Key
        builder.HasKey(e => e.CscId)
            .HasName("PK_Test_CommandStructChain");

        // Properties
        builder.Property(e => e.CscId)
            .HasColumnName("CSC_ID");

        builder.Property(e => e.CsId)
            .HasColumnName("CS_ID");

        builder.Property(e => e.ChainType)
            .HasMaxLength(20)
            .HasColumnName("CHAIN_TYPE");

        builder.Property(e => e.CscIdParent)
            .HasColumnName("CSC_ID_PARENT");

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(50);

        builder.Property(e => e.CreatedDate)
            .HasColumnType("datetime");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(50);

        builder.Property(e => e.ModifiedDate)
            .HasColumnType("datetime");

        builder.Property(e => e.ViewType)
            .HasColumnName("viewType");

        // Relationships
        builder.HasOne(d => d.Cs)
            .WithMany(p => p.TestCommandStructChains)
            .HasForeignKey(d => d.CsId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Test_CommandStructChain_Test_CommandStruct");

        builder.HasOne(d => d.ViewTypeNavigation)
            .WithMany(p => p.TestCommandStructChains)
            .HasForeignKey(d => d.ViewType)
            .HasConstraintName("FK_Test_CommandStructChain_CoreLkupChainType");

        // Indexes
        builder.HasIndex(e => e.CsId, "IX_Test_CommandStructChain_CS_ID");

        builder.HasIndex(e => e.CscIdParent, "IX_Test_CommandStructChain_CSC_ID_PARENT");

        builder.HasIndex(e => e.ViewType, "IX_Test_CommandStructChain_viewType");
    }
}
