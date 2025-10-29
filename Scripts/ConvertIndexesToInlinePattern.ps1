# Script to convert .HasIndex().HasDatabaseName() pattern to inline string parameter pattern
# Following Microsoft's recommended best practice

$configPath = "AF.ECT.Data\Configurations"
$files = Get-ChildItem -Path $configPath -Filter "*Configuration.cs" -Recurse

$totalConversions = 0
$filesModified = 0

foreach ($file in $files) {
    $content = Get-Content -Path $file.FullName -Raw
    $originalContent = $content
    $conversionsInFile = 0
    
    # Pattern 1: Simple index with HasDatabaseName on separate line
    # builder.HasIndex(e => e.Column)
    #     .HasDatabaseName("IX_Name");
    $pattern1 = '(?m)builder\.HasIndex\(([^)]+)\)\r?\n\s+\.HasDatabaseName\("([^"]+)"\);'
    $replacement1 = 'builder.HasIndex($1, "$2");'
    $content = [regex]::Replace($content, $pattern1, $replacement1)
    
    # Pattern 2: Simple index with HasDatabaseName on same line
    # builder.HasIndex(e => e.Column).HasDatabaseName("IX_Name");
    $pattern2 = 'builder\.HasIndex\(([^)]+)\)\.HasDatabaseName\("([^"]+)"\);'
    $replacement2 = 'builder.HasIndex($1, "$2");'
    $content = [regex]::Replace($content, $pattern2, $replacement2)
    
    # Pattern 3: Index with HasDatabaseName followed by filter (keep HasDatabaseName for chaining)
    # This one we DON'T convert because it needs chaining
    # Already handled by not matching if followed by .HasFilter or .IsUnique
    
    # Pattern 4: Unique index - keep HasDatabaseName for chaining with IsUnique
    # builder.HasIndex(e => e.Column)
    #     .IsUnique()
    #     .HasDatabaseName("UQ_Name");
    # These stay as-is
    
    if ($content -ne $originalContent) {
        # Count conversions
        $conversionsInFile = ([regex]::Matches($originalContent, '\.HasDatabaseName\(')).Count - ([regex]::Matches($content, '\.HasDatabaseName\(')).Count
        $totalConversions += $conversionsInFile
        $filesModified++
        
        # Backup original
        # Copy-Item -Path $file.FullName -Destination "$($file.FullName).bak"
        
        # Write changes
        Set-Content -Path $file.FullName -Value $content -NoNewline
        
        Write-Host "✓ $($file.Name): $conversionsInFile conversions" -ForegroundColor Green
    }
}

Write-Host "`n============================================" -ForegroundColor Cyan
Write-Host "Conversion Summary:" -ForegroundColor Cyan
Write-Host "Files modified: $filesModified" -ForegroundColor Yellow
Write-Host "Total conversions: $totalConversions" -ForegroundColor Yellow
Write-Host "============================================`n" -ForegroundColor Cyan

Write-Host "NOTE: Indexes with .IsUnique() or .HasFilter() were NOT converted" -ForegroundColor Magenta
Write-Host "as they require .HasDatabaseName() for method chaining.`n" -ForegroundColor Magenta
