using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AspstateTempApplication"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the ASPStateTempApplications table,
/// which is part of the ASP.NET session state SQL Server mode infrastructure.
/// Stores application identifiers and names for session management.
/// This is a standard ASP.NET system table used by the session state provider
/// to manage application-scoped session data. Typically managed by ASP.NET runtime.
/// </remarks>
public class AspstateTempApplicationConfiguration : IEntityTypeConfiguration<AspstateTempApplication>
{
    /// <summary>
    /// Configures the entity of type <see cref="AspstateTempApplication"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AspstateTempApplication> builder)
    {
        // Table mapping
        builder.ToTable("ASPStateTempApplications", "dbo");

        // Primary key
        builder.HasKey(e => e.AppId)
            .HasName("PK_ASPStateTempApplications");

        // Properties configuration
        builder.Property(e => e.AppId)
            .HasColumnName("AppId")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.AppName)
            .IsRequired()
            .HasMaxLength(280)
            .HasColumnName("AppName");

        // Indexes
        builder.HasIndex(e => e.AppName)
            .IsUnique()
            .HasDatabaseName("IX_aspstate_temp_application_app_name");
        
        builder.HasIndex(e => new { e.AppId, e.AppName }, "IX_aspstate_temp_application_id_name");
    }
}
