using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Configures the <see cref="RptUserQuery"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents saved user queries for reporting purposes, including query title, output fields,
/// sort fields, and sharing settings.
/// </remarks>
public class RptUserQueryConfiguration : IEntityTypeConfiguration<RptUserQuery>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<RptUserQuery> builder)
    {
        // Table mapping
        builder.ToTable("Rpt_UserQuery", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Rpt_UserQuery");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.UserId).HasColumnName("UserID");
        builder.Property(e => e.Title).HasColumnName("Title");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.Transient).HasColumnName("Transient");
        builder.Property(e => e.OutputFields).HasColumnName("OutputFields");
        builder.Property(e => e.SortFields).HasColumnName("SortFields");
        builder.Property(e => e.Shared).HasColumnName("Shared");

        // Indexes for query performance
        builder.HasIndex(e => e.UserId, "IX_Rpt_UserQuery_UserID");
        builder.HasIndex(e => new { e.UserId, e.Shared }, "IX_Rpt_UserQuery_UserID_Shared");
        builder.HasIndex(e => e.Transient, "IX_Rpt_UserQuery_Transient");
    }
}
