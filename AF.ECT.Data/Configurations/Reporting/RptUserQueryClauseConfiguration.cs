using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Configures the <see cref="RptUserQueryClause"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents WHERE clause definitions for saved user queries in the reporting system.
/// </remarks>
public class RptUserQueryClauseConfiguration : IEntityTypeConfiguration<RptUserQueryClause>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<RptUserQueryClause> builder)
    {
        // Table mapping
        builder.ToTable("Rpt_UserQueryClause", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Rpt_UserQueryClause");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.QueryId).HasColumnName("QueryID");
        builder.Property(e => e.WhereType).HasColumnName("WhereType").HasMaxLength(50).IsRequired();

        // Indexes for query performance
        builder.HasIndex(e => e.QueryId, "IX_Rpt_UserQueryClause_QueryID");
    }
}
