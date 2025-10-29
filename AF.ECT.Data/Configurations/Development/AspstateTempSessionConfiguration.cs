using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Development;

/// <summary>
/// Entity Framework Core configuration for the <see cref="AspstateTempSession"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the ASPStateTempSessions table,
/// which is part of the ASP.NET session state SQL Server mode infrastructure.
/// Stores session data including session ID, creation/expiration timestamps, lock information,
/// timeout settings, and serialized session items (short and long binary data).
/// This is a standard ASP.NET system table used by the session state provider
/// to persist user session data across web server restarts and load-balanced environments.
/// Typically managed by ASP.NET runtime.
/// </remarks>
public class AspstateTempSessionConfiguration : IEntityTypeConfiguration<AspstateTempSession>
{
    /// <summary>
    /// Configures the entity of type <see cref="AspstateTempSession"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<AspstateTempSession> builder)
    {
        // Table mapping
        builder.ToTable("ASPStateTempSessions", "dbo");

        // Primary key
        builder.HasKey(e => e.SessionId)
            .HasName("PK_ASPStateTempSessions");

        // Properties configuration
        builder.Property(e => e.SessionId)
            .IsRequired()
            .HasMaxLength(88)
            .HasColumnName("SessionId");

        builder.Property(e => e.Created)
            .IsRequired()
            .HasColumnName("Created")
            .HasColumnType("datetime");

        builder.Property(e => e.Expires)
            .IsRequired()
            .HasColumnName("Expires")
            .HasColumnType("datetime");

        builder.Property(e => e.LockDate)
            .IsRequired()
            .HasColumnName("LockDate")
            .HasColumnType("datetime");

        builder.Property(e => e.LockDateLocal)
            .IsRequired()
            .HasColumnName("LockDateLocal")
            .HasColumnType("datetime");

        builder.Property(e => e.LockCookie)
            .IsRequired()
            .HasColumnName("LockCookie");

        builder.Property(e => e.Timeout)
            .IsRequired()
            .HasColumnName("Timeout");

        builder.Property(e => e.Locked)
            .IsRequired()
            .HasColumnName("Locked");

        builder.Property(e => e.SessionItemShort)
            .HasMaxLength(7000)
            .HasColumnName("SessionItemShort")
            .HasColumnType("varbinary(7000)");

        builder.Property(e => e.SessionItemLong)
            .HasColumnName("SessionItemLong")
            .HasColumnType("image");

        builder.Property(e => e.Flags)
            .IsRequired()
            .HasColumnName("Flags");

        // Indexes
        builder.HasIndex(e => e.Expires, "IX_aspstate_temp_session_expires");

        builder.HasIndex(e => e.Locked, "IX_aspstate_temp_session_locked");
        
        builder.HasIndex(e => e.Created, "IX_aspstate_temp_session_created");
        
        builder.HasIndex(e => new { e.Expires, e.Locked }, "IX_aspstate_temp_session_expires_locked");
    }
}
