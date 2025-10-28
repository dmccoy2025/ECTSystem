using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework configuration for the <see cref="Form348RrFinding"/> entity.
/// Configures findings for Form 348 Reinvestigation Requests (RR), tracking personnel information,
/// investigation outcomes, and approval workflow for reinvestigation processes.
/// </summary>
/// <remarks>
/// Form348RrFinding represents individual findings within a reinvestigation request, capturing
/// details about personnel under investigation including their identity, grade, component,
/// PAS code, finding outcomes, and concurrence status. Each finding is linked to a parent
/// reinvestigation request and tracks creation/modification audit information.
/// 
/// Key characteristics:
/// - Child entity of Form348Rr (many findings per reinvestigation request)
/// - Personnel identity tracking (name, grade, rank, component)
/// - Finding outcome and explanation documentation
/// - Legacy finding migration support
/// - Concurrence tracking for approval workflow
/// - Audit trail via CreatedBy/ModifiedBy foreign keys
/// - Personnel type classification via Ptype
/// </remarks>
public class Form348RrFindingConfiguration : IEntityTypeConfiguration<Form348RrFinding>
{
    /// <summary>
    /// Configures the Form348RrFinding entity with table mapping, primary key, required fields,
    /// foreign key relationships, and indexes.
    /// </summary>
    /// <param name="builder">The entity type builder for Form348RrFinding.</param>
    public void Configure(EntityTypeBuilder<Form348RrFinding> builder)
    {
        builder.ToTable("Form348_RR_Finding", "dbo");

        builder.HasKey(e => e.Id)
            .HasName("PK_Form348_RR_Finding");

        // Primary key
        builder.Property(e => e.Id)
            .HasColumnName("ID");

        // Foreign keys
        builder.Property(e => e.RequestId)
            .HasColumnName("RequestID");

        builder.Property(e => e.Ptype)
            .HasColumnName("PTYPE");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("CreatedBy");

        builder.Property(e => e.ModifiedBy)
            .HasColumnName("ModifiedBy");

        // Personnel identity properties
        builder.Property(e => e.LastName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("LastName");

        builder.Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("FirstName");

        builder.Property(e => e.MiddleName)
            .HasMaxLength(50)
            .IsUnicode(false)
            .HasColumnName("MiddleName");

        builder.Property(e => e.Grade)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("Grade");

        builder.Property(e => e.Rank)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("Rank");

        builder.Property(e => e.Compo)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("Compo");

        builder.Property(e => e.Pascode)
            .HasMaxLength(10)
            .IsUnicode(false)
            .HasColumnName("PASCODE");

        // Finding properties
        builder.Property(e => e.Finding)
            .HasColumnName("Finding");

        builder.Property(e => e.Explanation)
            .HasColumnType("ntext")
            .HasColumnName("Explanation");

        builder.Property(e => e.IsLegacyFinding)
            .HasColumnName("IsLegacyFinding");

        builder.Property(e => e.Concur)
            .HasMaxLength(1)
            .IsUnicode(false)
            .IsFixedLength()
            .HasColumnName("Concur");

        // Audit properties
        builder.Property(e => e.CreatedDate)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime")
            .HasColumnName("CreatedDate");

        builder.Property(e => e.ModifiedDate)
            .HasDefaultValueSql("(getdate())")
            .HasColumnType("datetime")
            .HasColumnName("ModifiedDate");

        // Foreign key relationships
        builder.HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.Form348RrFindingCreatedByNavigations)
            .HasForeignKey(d => d.CreatedBy)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Form348_RR_Finding_Core_User_CreatedBy");

        builder.HasOne(d => d.ModifiedByNavigation)
            .WithMany(p => p.Form348RrFindingModifiedByNavigations)
            .HasForeignKey(d => d.ModifiedBy)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Form348_RR_Finding_Core_User_ModifiedBy");

        builder.HasOne(d => d.PtypeNavigation)
            .WithMany(p => p.Form348RrFindings)
            .HasForeignKey(d => d.Ptype)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Form348_RR_Finding_Core_Lkup_PersonnelType");

        builder.HasOne(d => d.Request)
            .WithMany(p => p.Form348RrFindings)
            .HasForeignKey(d => d.RequestId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Form348_RR_Finding_Form348_RR");

        // Indexes
        builder.HasIndex(e => e.RequestId, "IX_Form348_RR_Finding_RequestID");
        builder.HasIndex(e => e.Ptype, "IX_Form348_RR_Finding_PTYPE");
        builder.HasIndex(e => e.CreatedBy, "IX_Form348_RR_Finding_CreatedBy");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form348_RR_Finding_ModifiedBy");
        builder.HasIndex(e => e.CreatedDate, "IX_Form348_RR_Finding_CreatedDate");
        builder.HasIndex(e => e.ModifiedDate, "IX_Form348_RR_Finding_ModifiedDate");
        builder.HasIndex(e => e.Finding, "IX_Form348_RR_Finding_Finding");
        builder.HasIndex(e => e.Grade, "IX_Form348_RR_Finding_Grade");
        builder.HasIndex(e => e.Concur, "IX_Form348_RR_Finding_Concur");
    }
}
