using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Reporting;

/// <summary>
/// Configures the <see cref="RptUserQueryParam"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents parameter values for query clause conditions in the reporting system.
/// </remarks>
public class RptUserQueryParamConfiguration : IEntityTypeConfiguration<RptUserQueryParam>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<RptUserQueryParam> builder)
    {
        // Table mapping
        builder.ToTable("Rpt_UserQueryParam", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Rpt_UserQueryParam");

        // Properties
        builder.Property(e => e.Id).HasColumnName("ID");
        builder.Property(e => e.ClauseId).HasColumnName("ClauseID");
        builder.Property(e => e.SourceId).HasColumnName("SourceID");
        builder.Property(e => e.OperatorType).HasColumnName("OperatorType").HasMaxLength(50).IsRequired();
        builder.Property(e => e.WhereType).HasColumnName("WhereType").HasMaxLength(50).IsRequired();
        builder.Property(e => e.StartValue).HasColumnName("StartValue").HasMaxLength(255).IsRequired();
        builder.Property(e => e.StartDisplay).HasColumnName("StartDisplay").HasMaxLength(255).IsRequired();
        builder.Property(e => e.EndValue).HasColumnName("EndValue").HasMaxLength(255);
        builder.Property(e => e.EndDisplay).HasColumnName("EndDisplay").HasMaxLength(255);
        builder.Property(e => e.ExecuteOrder).HasColumnName("ExecuteOrder");

        // Indexes for query performance
        builder.HasIndex(e => e.ClauseId, "IX_Rpt_UserQueryParam_ClauseID");
        builder.HasIndex(e => e.SourceId, "IX_Rpt_UserQueryParam_SourceID");
        builder.HasIndex(e => e.ExecuteOrder, "IX_Rpt_UserQueryParam_ExecuteOrder");
    }
}
