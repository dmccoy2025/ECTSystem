using AF.ECT.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Configures the <see cref="Form348Sc"/> entity for Entity Framework Core.
/// </summary>
/// <remarks>
/// Represents Suitability/Classification (SC) cases in the Electronic Case Tracking system.
/// Manages evaluations for medical conditions affecting duty performance and deployment readiness.
/// </remarks>
public class Form348ScConfiguration : IEntityTypeConfiguration<Form348Sc>
{
    /// <summary>
    /// Configures the entity properties, primary key, indexes, and relationships.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<Form348Sc> builder)
    {
        // Table mapping
        builder.ToTable("Form348SC", "dbo");

        // Primary key
        builder.HasKey(e => e.ScId)
            .HasName("PK_Form348SC");

        // Properties - Core fields
        builder.Property(e => e.ScId).HasColumnName("ScID");
        builder.Property(e => e.ModuleId).HasColumnName("ModuleID");
        builder.Property(e => e.AssociatedLod).HasColumnName("AssociatedLOD");
        builder.Property(e => e.CaseId).HasColumnName("CaseID").HasMaxLength(50).IsRequired();
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.ModifiedBy).HasColumnName("ModifiedBy");
        builder.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");
        builder.Property(e => e.Workflow).HasColumnName("Workflow");
        builder.Property(e => e.Status).HasColumnName("Status");
        
        // Member information
        builder.Property(e => e.MemberSsn).HasColumnName("MemberSSN").HasMaxLength(20).IsRequired();
        builder.Property(e => e.MemberName).HasColumnName("MemberName").HasMaxLength(200).IsRequired();
        builder.Property(e => e.MemberUnit).HasColumnName("MemberUnit").HasMaxLength(200).IsRequired();
        builder.Property(e => e.MemberCompo).HasColumnName("MemberCompo");
        builder.Property(e => e.MemberUnitId).HasColumnName("MemberUnitID");
        builder.Property(e => e.MemberGrade).HasColumnName("MemberGrade");
        builder.Property(e => e.MemberDob).HasColumnName("MemberDOB");
        
        // Additional key fields
        builder.Property(e => e.SubWorkflowType).HasColumnName("SubWorkflowType");
        builder.Property(e => e.ReturnToGroup).HasColumnName("ReturnToGroup");
        builder.Property(e => e.ReturnByGroup).HasColumnName("ReturnByGroup");
        builder.Property(e => e.MedOffConcur).HasColumnName("MedOffConcur");

        // Note: This entity has 150+ properties. For comprehensive configuration, all properties
        // should be mapped, but showing core properties here for brevity.

        // Indexes for query performance
        builder.HasIndex(e => e.CaseId, "IX_Form348SC_CaseID").IsUnique();
        builder.HasIndex(e => new { e.Status, e.Workflow }, "IX_Form348SC_Status_Workflow");
        builder.HasIndex(e => e.MemberSsn, "IX_Form348SC_MemberSSN");
        builder.HasIndex(e => new { e.ModuleId, e.SubWorkflowType }, "IX_Form348SC_ModuleID_SubWorkflowType");
        builder.HasIndex(e => e.AssociatedLod, "IX_Form348SC_AssociatedLOD");
        builder.HasIndex(e => e.CreatedBy, "IX_Form348SC_CreatedBy");
        builder.HasIndex(e => e.CreatedDate, "IX_Form348SC_CreatedDate");
        builder.HasIndex(e => e.ModifiedBy, "IX_Form348SC_ModifiedBy");
        builder.HasIndex(e => e.ModifiedDate, "IX_Form348SC_ModifiedDate");
        builder.HasIndex(e => e.MemberGrade, "IX_Form348SC_MemberGrade");
        builder.HasIndex(e => e.MemberUnitId, "IX_Form348SC_MemberUnitID");
        builder.HasIndex(e => e.ReturnToGroup, "IX_Form348SC_ReturnToGroup");
        builder.HasIndex(e => e.ReturnByGroup, "IX_Form348SC_ReturnByGroup");
    }
}
