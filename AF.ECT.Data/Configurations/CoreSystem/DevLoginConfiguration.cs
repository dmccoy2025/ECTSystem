using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.CoreSystem;

/// <summary>
/// Configures the <see cref="DevLogin"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents development login credentials for testing purposes.
/// </remarks>
public class DevLoginConfiguration : IEntityTypeConfiguration<DevLogin>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<DevLogin> builder)
    {
        // Table mapping
        builder.ToTable("DevLogin", "dbo");

        // Primary key
        builder.HasKey(e => e.UserId)
            .HasName("PK_DevLogin");

        // Properties
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.Compo).HasColumnName("Compo").HasMaxLength(50).IsRequired();
        builder.Property(e => e.RoleId).HasColumnName("RoleID");

        // Indexes for query performance
        builder.HasIndex(e => e.Compo, "IX_DevLogin_Compo");
    }
}
