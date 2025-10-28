# Database Indexing Standards for ECTSystem

## Overview
This document establishes consistent indexing standards for Entity Framework Core configuration files across the ECTSystem solution. Following these standards ensures optimal database performance, maintainability, and consistency.

## Index Naming Conventions

### Standard Index Naming Pattern
All indexes should use explicit naming with the `.HasDatabaseName()` method following this pattern:

```csharp
builder.HasIndex(e => e.ColumnName)
    .HasDatabaseName("IX_TableName_ColumnName");
```

### Naming Rules

#### 1. Regular Indexes
- **Prefix:** `IX_`
- **Format:** `IX_TableName_Column1_Column2_...`
- **Case:** Use the exact table name case (PascalCase or snake_case as defined in schema)
- **Column Order:** List columns in the same order as the composite index definition

**Examples:**
```csharp
// Single column index
builder.HasIndex(e => e.Active)
    .HasDatabaseName("IX_core_user_Active");

// Composite index
builder.HasIndex(e => new { e.RefId, e.Deleted })
    .HasDatabaseName("IX_core_memo_RefId_Deleted");

// Foreign key index
builder.HasIndex(e => e.UserId)
    .HasDatabaseName("IX_core_workflow_UserId");
```

#### 2. Unique Indexes
- **Prefix:** `UQ_`
- **Format:** `UQ_TableName_Column1_Column2_...`
- **Include:** `.IsUnique()` fluent API call

**Examples:**
```csharp
builder.HasIndex(e => e.Name)
    .IsUnique()
    .HasDatabaseName("UQ_core_case_type_Name");

builder.HasIndex(e => new { e.Title, e.Compo })
    .IsUnique()
    .HasDatabaseName("UQ_core_email_template_Title_Compo");
```

#### 3. Primary Keys
- **Prefix:** `PK_`
- **Format:** `PK_TableName`

**Example:**
```csharp
builder.HasKey(e => e.Id)
    .HasName("PK_core_user");
```

#### 4. Foreign Keys
- **Prefix:** `FK_`
- **Format:** `FK_TableName_ReferencedTable` or `FK_TableName_ColumnName`

**Example:**
```csharp
builder.HasOne(d => d.CreatedByNavigation)
    .WithMany(p => p.CoreMemos)
    .HasForeignKey(d => d.CreatedBy)
    .HasConstraintName("FK_core_memo_created_by");
```

## When to Create Indexes

### Required Indexes

#### 1. Foreign Key Columns
**Always index foreign key columns** to optimize JOIN operations.

```csharp
builder.HasIndex(e => e.UserId)
    .HasDatabaseName("IX_TableName_UserId");

builder.HasIndex(e => e.GroupId)
    .HasDatabaseName("IX_TableName_GroupId");

builder.HasIndex(e => e.WorkflowId)
    .HasDatabaseName("IX_TableName_WorkflowId");
```

#### 2. Audit Tracking Fields
Index audit fields for reporting and compliance queries:

```csharp
builder.HasIndex(e => e.CreatedDate)
    .HasDatabaseName("IX_TableName_CreatedDate");

builder.HasIndex(e => e.CreatedBy)
    .HasDatabaseName("IX_TableName_CreatedBy");

builder.HasIndex(e => e.ModifiedDate)
    .HasDatabaseName("IX_TableName_ModifiedDate");

builder.HasIndex(e => e.ModifiedBy)
    .HasDatabaseName("IX_TableName_ModifiedBy");
```

#### 3. Status and Flag Columns
Index boolean flags and status columns frequently used in WHERE clauses:

```csharp
builder.HasIndex(e => e.Active)
    .HasDatabaseName("IX_TableName_Active");

builder.HasIndex(e => e.Deleted)
    .HasDatabaseName("IX_TableName_Deleted");

builder.HasIndex(e => e.IsEnabled)
    .HasDatabaseName("IX_TableName_IsEnabled");
```

#### 4. Natural Key Columns
Index columns used for business key lookups:

```csharp
builder.HasIndex(e => e.Name)
    .HasDatabaseName("IX_TableName_Name");

builder.HasIndex(e => e.Code)
    .HasDatabaseName("IX_TableName_Code");

builder.HasIndex(e => e.Email)
    .HasDatabaseName("IX_TableName_Email");
```

### Composite Indexes

Create composite indexes for common multi-column queries:

```csharp
// Optimize queries filtering by RefId and Workflow
builder.HasIndex(e => new { e.RefId, e.Workflow })
    .HasDatabaseName("IX_TableName_RefId_Workflow");

// Optimize date range queries with status filter
builder.HasIndex(e => new { e.StartDate, e.EndDate, e.Status })
    .HasDatabaseName("IX_TableName_StartDate_EndDate_Status");

// Optimize foreign key with sort order
builder.HasIndex(e => new { e.WorkflowId, e.SortOrder })
    .HasDatabaseName("IX_TableName_WorkflowId_SortOrder");
```

#### Column Order in Composite Indexes
Order columns by **selectivity** (most selective first) and **query patterns**:

1. **Equality columns** (WHERE col = value) come first
2. **Range columns** (WHERE col > value) come last
3. **Most selective columns** (fewer distinct values) first for filtering

**Example:**
```csharp
// Good: UserId is selective, Status has few values
builder.HasIndex(e => new { e.UserId, e.Status, e.CreatedDate })
    .HasDatabaseName("IX_TableName_UserId_Status_CreatedDate");
```

### Filtered Indexes

Use filtered indexes to optimize queries on subsets of data:

```csharp
// Index only active records
builder.HasIndex(e => e.RefId)
    .HasDatabaseName("IX_core_memo_RefId_Active")
    .HasFilter("[active] = 1");

// Index only non-deleted records
builder.HasIndex(e => new { e.RefId, e.Deleted })
    .HasDatabaseName("IX_TableName_RefId_Deleted")
    .HasFilter("[deleted] = 0");

// Index only pending records
builder.HasIndex(e => e.CreatedDate)
    .HasDatabaseName("IX_TableName_CreatedDate_Pending")
    .HasFilter("[status] = 'Pending'");
```

**When to use filtered indexes:**
- When queries consistently filter by the same predicate
- When the filtered subset is significantly smaller than the full table
- For soft-delete patterns (deleted = 0)
- For active/inactive patterns (active = 1)

## Special Cases

### 1. Keyless Entities (Views, Query Results)
For entities without primary keys (views, stored procedure results):

```csharp
public void Configure(EntityTypeBuilder<MyView> builder)
{
    // Keyless entities cannot have indexes
    builder.HasNoKey();
    builder.ToView("vw_MyView");
    
    // No indexes needed - indexing handled on underlying tables
}
```

### 2. Temporary/Staging Tables
Staging tables used for bulk imports may have minimal indexes:

```csharp
// Option 1: No indexes for pure staging tables (drop/recreate pattern)
builder.HasKey(e => e.Id)
    .HasName("PK_TempTable");
// No additional indexes for temporary data

// Option 2: Index import status for processing queries
builder.HasIndex(e => e.Imported)
    .HasDatabaseName("IX_TempTable_Imported");
```

### 3. Small Lookup Tables
Very small lookup tables (< 100 rows) may only need primary key:

```csharp
// Minimal indexing for small reference data
builder.HasKey(e => e.Id)
    .HasName("PK_LookupTable");

// Optional: Unique constraint on name/code
builder.HasIndex(e => e.Name)
    .IsUnique()
    .HasDatabaseName("UQ_LookupTable_Name");
```

### 4. Large Text Columns
Avoid indexing large text columns (nvarchar(max), text):

```csharp
// Don't index these:
builder.Property(e => e.Description).HasColumnType("nvarchar(max)");
builder.Property(e => e.Notes).HasColumnType("text");

// Use full-text search instead for searching large text
```

## Index Configuration Patterns

### Basic Pattern
```csharp
public void Configure(EntityTypeBuilder<MyEntity> builder)
{
    // 1. Table mapping
    builder.ToTable("table_name", "dbo");

    // 2. Primary key
    builder.HasKey(e => e.Id)
        .HasName("PK_table_name");

    // 3. Properties
    builder.Property(e => e.Name)
        .IsRequired()
        .HasMaxLength(100);

    // 4. Foreign key relationships
    builder.HasOne(d => d.Parent)
        .WithMany(p => p.Children)
        .HasForeignKey(d => d.ParentId)
        .HasConstraintName("FK_table_name_parent");

    // 5. Indexes (grouped by type)
    
    // Foreign key indexes
    builder.HasIndex(e => e.ParentId)
        .HasDatabaseName("IX_table_name_ParentId");

    // Audit field indexes
    builder.HasIndex(e => e.CreatedDate)
        .HasDatabaseName("IX_table_name_CreatedDate");

    builder.HasIndex(e => e.CreatedBy)
        .HasDatabaseName("IX_table_name_CreatedBy");

    // Business key indexes
    builder.HasIndex(e => e.Name)
        .HasDatabaseName("IX_table_name_Name");

    // Status/flag indexes
    builder.HasIndex(e => e.Active)
        .HasDatabaseName("IX_table_name_Active");

    // Composite indexes
    builder.HasIndex(e => new { e.ParentId, e.SortOrder })
        .HasDatabaseName("IX_table_name_ParentId_SortOrder");

    // Unique constraints
    builder.HasIndex(e => new { e.Name, e.ParentId })
        .IsUnique()
        .HasDatabaseName("UQ_table_name_Name_ParentId");

    // Filtered indexes
    builder.HasIndex(e => e.RefId)
        .HasDatabaseName("IX_table_name_RefId_Active")
        .HasFilter("[active] = 1");
}
```

### Organization Best Practices
Within the `Configure` method, organize indexes by:

1. **Foreign key indexes** - Group all FK indexes together
2. **Audit field indexes** - CreatedDate, CreatedBy, ModifiedDate, ModifiedBy
3. **Business key indexes** - Name, Code, Email, etc.
4. **Status/flag indexes** - Active, Deleted, IsEnabled, etc.
5. **Composite indexes** - Multi-column indexes
6. **Unique constraints** - Business rules requiring uniqueness
7. **Filtered indexes** - Indexes with filter predicates

## Performance Considerations

### Index Overhead
Remember that indexes have costs:
- **Storage:** Each index requires disk space
- **Write Performance:** Indexes slow down INSERT, UPDATE, DELETE operations
- **Maintenance:** Indexes require rebuilding/reorganization

**Guidelines:**
- Don't create indexes "just in case"
- Base indexes on actual query patterns
- Monitor index usage via DMVs (Dynamic Management Views)
- Remove unused indexes identified through monitoring

### Index Selectivity
Good indexes have high selectivity (many distinct values):
- ✅ **High selectivity:** UserId, Email, OrderNumber (good for indexing)
- ⚠️ **Medium selectivity:** StatusId, CategoryId (useful in composite indexes)
- ❌ **Low selectivity:** IsActive (boolean), Gender (consider filtered indexes)

### Covering Indexes
Consider adding INCLUDE columns for frequently accessed columns:

```csharp
// SQL Server specific - use raw SQL for advanced features
migrationBuilder.Sql(@"
    CREATE NONCLUSTERED INDEX IX_table_name_UserId_Including
    ON table_name (UserId)
    INCLUDE (Name, Email, CreatedDate)
");
```

## Examples from ECTSystem

### Example 1: User Entity Configuration
```csharp
public class CoreUserConfiguration : IEntityTypeConfiguration<CoreUser>
{
    public void Configure(EntityTypeBuilder<CoreUser> builder)
    {
        builder.ToTable("core_user", "dbo");

        builder.HasKey(e => e.UserId)
            .HasName("PK_core_user");

        // Properties...

        // Foreign key indexes
        builder.HasIndex(e => e.RankId)
            .HasDatabaseName("IX_core_user_RankId");

        builder.HasIndex(e => e.UnitId)
            .HasDatabaseName("IX_core_user_UnitId");

        // Business key indexes
        builder.HasIndex(e => e.Edipi)
            .HasDatabaseName("IX_core_user_Edipi");

        builder.HasIndex(e => e.Email)
            .HasDatabaseName("IX_core_user_Email");

        // Status indexes
        builder.HasIndex(e => e.Active)
            .HasDatabaseName("IX_core_user_Active");

        builder.HasIndex(e => e.Deleted)
            .HasDatabaseName("IX_core_user_Deleted");

        // Audit indexes
        builder.HasIndex(e => e.CreatedDate)
            .HasDatabaseName("IX_core_user_CreatedDate");

        // Composite indexes for common queries
        builder.HasIndex(e => new { e.Active, e.Deleted })
            .HasDatabaseName("IX_core_user_Active_Deleted")
            .HasFilter("[active] = 1 AND [deleted] = 0");

        // Unique constraints
        builder.HasIndex(e => e.Edipi)
            .IsUnique()
            .HasDatabaseName("UQ_core_user_Edipi");
    }
}
```

### Example 2: Workflow Configuration
```csharp
public class CoreWorkflowConfiguration : IEntityTypeConfiguration<CoreWorkflow>
{
    public void Configure(EntityTypeBuilder<CoreWorkflow> builder)
    {
        builder.ToTable("core_workflow", "dbo");

        builder.HasKey(e => e.WorkflowId)
            .HasName("PK_core_workflow");

        // Properties...

        // Foreign key indexes
        builder.HasIndex(e => e.CaseTypeId)
            .HasDatabaseName("IX_core_workflow_CaseTypeId");

        builder.HasIndex(e => e.CreatedBy)
            .HasDatabaseName("IX_core_workflow_CreatedBy");

        // Status indexes
        builder.HasIndex(e => e.Active)
            .HasDatabaseName("IX_core_workflow_Active");

        // Audit indexes
        builder.HasIndex(e => e.CreatedDate)
            .HasDatabaseName("IX_core_workflow_CreatedDate");

        builder.HasIndex(e => e.ModifiedDate)
            .HasDatabaseName("IX_core_workflow_ModifiedDate");

        // Composite indexes
        builder.HasIndex(e => new { e.CaseTypeId, e.Active })
            .HasDatabaseName("IX_core_workflow_CaseTypeId_Active")
            .HasFilter("[active] = 1");
    }
}
```

### Example 3: Memo Configuration with Filtered Indexes
```csharp
public class CoreMemoConfiguration : IEntityTypeConfiguration<CoreMemo>
{
    public void Configure(EntityTypeBuilder<CoreMemo> builder)
    {
        builder.ToTable("core_memo", "dbo");

        builder.HasKey(e => e.MemoId)
            .HasName("PK_core_memo");

        // Properties...

        // Foreign key relationships
        builder.HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.CoreMemos)
            .HasForeignKey(d => d.CreatedBy)
            .HasConstraintName("FK_core_memo_created_by");

        // Foreign key indexes
        builder.HasIndex(e => e.TemplateId)
            .HasDatabaseName("IX_core_memo_TemplateId");

        builder.HasIndex(e => e.CreatedBy)
            .HasDatabaseName("IX_core_memo_CreatedBy");

        // Reference ID index
        builder.HasIndex(e => e.RefId)
            .HasDatabaseName("IX_core_memo_RefId");

        // Filtered composite index for active memos
        builder.HasIndex(e => new { e.RefId, e.Deleted })
            .HasDatabaseName("IX_core_memo_RefId_Deleted")
            .HasFilter("[deleted] = 0");

        // Audit indexes
        builder.HasIndex(e => e.CreatedDate)
            .HasDatabaseName("IX_core_memo_CreatedDate");
    }
}
```

## Validation Checklist

Before committing configuration changes, verify:

- [ ] All foreign key columns have indexes
- [ ] Audit fields (CreatedDate, CreatedBy, ModifiedDate, ModifiedBy) are indexed
- [ ] Status/flag columns (Active, Deleted, IsEnabled) are indexed
- [ ] Business keys (Name, Code, Email) are indexed
- [ ] All indexes use `.HasDatabaseName()` with consistent naming
- [ ] Index names follow `IX_TableName_Column` pattern
- [ ] Unique constraints use `UQ_` prefix
- [ ] Filtered indexes have appropriate `.HasFilter()` predicates
- [ ] Composite index column order optimizes query patterns
- [ ] No unnecessary indexes on low-selectivity columns
- [ ] No indexes on large text columns (nvarchar(max))

## Migration Considerations

When adding indexes to existing tables:

1. **Test Performance Impact:** Measure query performance before/after
2. **Monitor Index Usage:** Use SQL Server DMVs to track index usage
3. **Plan Maintenance:** Schedule index rebuilds during low-usage periods
4. **Document Rationale:** Comment why specific composite indexes were added

## References

- [EF Core Indexes Documentation](https://learn.microsoft.com/en-us/ef/core/modeling/indexes)
- [SQL Server Index Design Guidelines](https://learn.microsoft.com/en-us/sql/relational-databases/sql-server-index-design-guide)
- [Microsoft Best Practices for Indexing](https://learn.microsoft.com/en-us/sql/relational-databases/indexes/indexes)

---

**Document Version:** 1.0  
**Last Updated:** October 27, 2025  
**Maintained By:** ECTSystem Development Team
