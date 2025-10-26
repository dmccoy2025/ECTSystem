using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Entity Framework configuration for the CoreTest entity.
/// </summary>
public class CoreTestConfiguration : IEntityTypeConfiguration<CoreTest>
{
    /// <summary>
    /// Configures the CoreTest entity.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CoreTest> builder)
    {
        builder.ToTable("Core_Test", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Core_Test");

        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("Name");
        builder.Property(e => e.Active).HasColumnName("Active");
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(255)
            .HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDaye).HasColumnName("CreatedDaye");

        builder.HasIndex(e => e.Name, "IX_Core_Test_Name");
        builder.HasIndex(e => e.CreatedDaye, "IX_Core_Test_CreatedDaye");
    }
}
