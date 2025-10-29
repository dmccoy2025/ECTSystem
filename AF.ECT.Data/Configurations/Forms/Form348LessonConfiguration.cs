using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AF.ECT.Data.Entities;

namespace AF.ECT.Data.Configurations.Forms;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Form348Lesson"/> entity.
/// </summary>
/// <remarks>
/// This configuration defines the schema for the Form_348_Lesson table,
/// which stores lessons learned from Line of Duty (LOD) investigations on Form 348.
/// Contains LOD ID, comments/lessons learned text, creator information, creation date,
/// and soft delete flag. Used for capturing important insights, process improvements,
/// or preventive measures identified during case investigations.
/// Supports organizational learning and continuous improvement.
/// </remarks>
public class Form348LessonConfiguration : IEntityTypeConfiguration<Form348Lesson>
{
    /// <summary>
    /// Configures the entity of type <see cref="Form348Lesson"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Form348Lesson> builder)
    {
        // Table mapping
        builder.ToTable("Form_348_Lesson", "dbo");

        // Primary key
        builder.HasKey(e => e.Id)
            .HasName("PK_Form_348_Lesson");

        // Properties configuration
        builder.Property(e => e.Id)
            .HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Lodid)
            .IsRequired()
            .HasColumnName("LODID");

        builder.Property(e => e.Comments)
            .IsRequired()
            .HasMaxLength(4000)
            .HasColumnName("Comments");

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasColumnName("Created_By");

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasColumnName("Created_Date")
            .HasColumnType("datetime");

        builder.Property(e => e.Deleted)
            .IsRequired()
            .HasColumnName("Deleted")
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(e => e.Lodid, "IX_form_348_lesson_lodid");

        builder.HasIndex(e => e.Deleted, "IX_form_348_lesson_deleted");

        builder.HasIndex(e => e.CreatedDate, "IX_form_348_lesson_created_date");
        
        builder.HasIndex(e => e.CreatedBy, "IX_form_348_lesson_created_by");
    }
}
