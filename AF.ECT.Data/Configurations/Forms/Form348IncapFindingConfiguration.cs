using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Form348IncapFinding"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Form_348_Incap_Finding table,
/// which stores incapacitation (INCAP) findings for Form 348 cases.
/// Contains initial incapacitation period dates, medical report type, ability to perform determination,
/// investigating commander recommendation, Wing JA concurrence, financial impact assessments
/// (income lost, self-employed status), WCC initial approval, and late submission flag.
/// This is the parent entity for incapacitation appeals and extensions.
/// Used for tracking initial INCAP determinations and subsequent workflow actions.
/// </remarks>
public class Form348IncapFindingConfiguration : IEntityTypeConfiguration<Form348IncapFinding>
{
    /// <summary>
    /// Configures the entity of type <see cref="Form348IncapFinding"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Form348IncapFinding> builder)
    {
        // Table mapping
        builder.ToTable("Form_348_Incap_Finding", "dbo");

        // Primary key
        builder.HasKey(e => e.ScId)
            .HasName("PK_Form_348_Incap_Finding");

        // Properties configuration
        builder.Property(e => e.ScId)
            .HasColumnName("SC_ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.InitStartDate)
            .HasColumnName("Init_Start_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.InitEndDate)
            .HasColumnName("Init_End_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.InitExtOrComplete)
            .HasColumnName("Init_Ext_Or_Complete");

        builder.Property(e => e.InitAppealOrComplete)
            .HasColumnName("Init_Appeal_Or_Complete");

        builder.Property(e => e.MedReportType)
            .HasMaxLength(100)
            .HasColumnName("Med_Report_Type");

        builder.Property(e => e.MedAbilityToPreform)
            .HasColumnName("Med_Ability_To_Preform");

        builder.Property(e => e.ICRecommendation)
            .HasColumnName("IC_Recommendation");

        builder.Property(e => e.WingJaConcur)
            .HasColumnName("Wing_JA_Concur");

        builder.Property(e => e.FinIncomeLost)
            .HasColumnName("Fin_Income_Lost");

        builder.Property(e => e.WccInitApproval)
            .HasColumnName("WCC_Init_Approval");

        builder.Property(e => e.InitLateSubmission)
            .HasColumnName("Init_Late_Submission");

        builder.Property(e => e.FinSelfEmployed)
            .HasColumnName("Fin_Self_Employed");

        // Relationships
        builder.HasMany(e => e.Form348IncapAppeals)
            .WithOne(e => e.Sc)
            .HasForeignKey(e => e.ScId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Form_348_Incap_Appeal_Finding");

        builder.HasMany(e => e.Form348IncapExts)
            .WithOne(e => e.Sc)
            .HasForeignKey(e => e.ScId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Form_348_Incap_Ext_Finding");

        // Indexes
        builder.HasIndex(e => e.InitStartDate, "IX_form_348_incap_finding_init_start_date");

        builder.HasIndex(e => e.InitEndDate, "IX_form_348_incap_finding_init_end_date");

        builder.HasIndex(e => e.WccInitApproval, "IX_form_348_incap_finding_wcc_init_approval");
        
        builder.HasIndex(e => e.ICRecommendation, "IX_form_348_incap_finding_ic_recommendation");
        
        builder.HasIndex(e => e.WingJaConcur, "IX_form_348_incap_finding_wing_ja_concur");
        
        builder.HasIndex(e => e.MedAbilityToPreform, "IX_form_348_incap_finding_med_ability");
    }
}
