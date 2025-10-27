# PowerShell script to add XML documentation comments to all entity files
# This script processes C# entity files and adds comprehensive XML documentation

param(
    [string]$EntitiesFolder = "d:\source\repos\dmccoy2025\ECTSystem\AF.ECT.Data\Entities",
    [switch]$WhatIf
)

function Get-PropertyDocumentation {
    param(
        [string]$PropertyName,
        [string]$PropertyType
    )
    
    # Generate contextual documentation based on property name patterns
    $doc = "Gets or sets the "
    
    # Common patterns
    switch -Regex ($PropertyName) {
        "^.*Id$" { return "$doc$($PropertyName -creplace '([A-Z])', ' $1' -replace '^ ') identifier." }
        "^.*Yn$" { return "$doc value indicating whether $($PropertyName -replace 'Yn$', '' -creplace '([A-Z])', ' $1' -replace '^ ') (Y/N)." }
        "^.*Date$|^.*Dob$|^.*Doa$|^.*Doe$|^.*Dor$|^.*Ets$" { return "$doc$($PropertyName -creplace '([A-Z])', ' $1' -replace '^ ') date." }
        "^Created(By|Date)$" { 
            if ($PropertyName -eq "CreatedBy") { return "$doc user identifier who created this record." }
            else { return "$doc date when this record was created." }
        }
        "^Modified(By|Date)$" { 
            if ($PropertyName -eq "ModifiedBy") { return "$doc user identifier who last modified this record." }
            else { return "$doc date when this record was last modified." }
        }
        "^.*Name$" { return "$doc$($PropertyName -creplace '([A-Z])', ' $1' -replace '^ ')." }
        "^.*Ssn$" { return "$doc Social Security Number." }
        "^.*Grade$|^.*Rank$" { return "$doc military grade/rank." }
        "^.*Email$|^.*EMail.*$" { return "$doc email address." }
        "^.*Address.*$" { return "$doc$($PropertyName -creplace '([A-Z])', ' $1' -replace '^ ')." }
        "^.*City$" { return "$doc city." }
        "^.*State$" { return "$doc state." }
        "^.*Country$" { return "$doc country." }
        "^.*Zip$|^.*PostalCode$" { return "$doc postal/ZIP code." }
        "^.*Phone$" { return "$doc phone number." }
        "^.*Status$" { return "$doc$($PropertyName -creplace '([A-Z])', ' $1' -replace '^ ')." }
        "^.*Code$" { return "$doc$($PropertyName -creplace '([A-Z])', ' $1' -replace '^ ')." }
        "^.*Desc$|^.*Description$" { return "$doc description." }
        "^.*Comment.*$|^.*Remark.*$" { return "$doc comments/remarks." }
        "^.*Explanation$" { return "$doc explanation." }
        "^.*Decision$" { return "$doc decision." }
        "^.*Findings$|^.*Finding$" { return "$doc findings." }
        "^.*Reason.*$" { return "$doc reason(s)." }
        "^Username$" { return "$doc username." }
        "^Password$" { return "$doc encrypted password value." }
        "^Active$" { return "$doc value indicating whether this record is active." }
        "^Deleted$" { return "$doc value indicating whether this record is deleted." }
        default { return "$doc$($PropertyName -creplace '([A-Z])', ' $1' -replace '^ ')." }
    }
}

function Get-ClassDocumentation {
    param(
        [string]$ClassName
    )
    
    # Generate contextual class documentation based on class name
    switch -Regex ($ClassName) {
        "^AfrcOracle" { return "Represents $($ClassName -replace '^AfrcOracle', '' -replace 'Datum$', '' -creplace '([A-Z])', ' $1' -replace '^ ') data imported from AFRC Oracle." }
        "^Imp" { return "Represents imported $($ClassName -replace '^Imp', '' -creplace '([A-Z])', ' $1' -replace '^ ') data for system migration/integration." }
        "^CoreLkup" { return "Represents a lookup table for $($ClassName -replace '^CoreLkup', '' -creplace '([A-Z])', ' $1' -replace '^ ')." }
        "^CoreLog" { return "Represents a log entry for $($ClassName -replace '^CoreLog', '' -creplace '([A-Z])', ' $1' -replace '^ ')." }
        "^Core" { return "Represents a $($ClassName -replace '^Core', '' -creplace '([A-Z])', ' $1' -replace '^ ') in the Electronic Case Tracking (ECT) system." }
        "^Form348" { return "Represents Form 348 $($ClassName -replace '^Form348', '' -creplace '([A-Z])', ' $1' -replace '^ ') data." }
        "^Alod" { return "Represents ALOD $($ClassName -replace '^Alod', '' -creplace '([A-Z])', ' $1' -replace '^ ') data." }
        "^Rpt" { return "Represents a reporting $($ClassName -replace '^Rpt', '' -creplace '([A-Z])', ' $1' -replace '^ ')." }
        "^Ph" { return "Represents a placeholder/form $($ClassName -replace '^Ph', '' -creplace '([A-Z])', ' $1' -replace '^ ')." }
        default { return "Represents a $($ClassName -creplace '([A-Z])', ' $1' -replace '^ ') entity." }
    }
}

function Add-XmlDocumentation {
    param(
        [string]$FilePath
    )
    
    try {
        $content = Get-Content $FilePath -Raw
        
        # Check if already has XML documentation on the class
        if ($content -match '/// <summary>.*?public partial class') {
            Write-Host "Skipping $FilePath - already has documentation" -ForegroundColor Yellow
            return $false
        }
        
        # Extract class name
        if ($content -match 'public partial class (\w+)') {
            $className = $Matches[1]
            $classDoc = Get-ClassDocumentation -ClassName $className
            
            # Add class-level documentation
            $content = $content -replace '(namespace AF\.ECT\.Data\.Entities;)\s+\npublic partial class', "`$1`n`n/// <summary>`n/// $classDoc`n/// </summary>`npublic partial class"
            
            # Find all properties and add documentation
            $propertyPattern = '(?m)^(\s+)(public\s+[\w?]+\s+(\w+)\s*{\s*get;\s*set;\s*})$'
            $matches = [regex]::Matches($content, $propertyPattern)
            
            # Process matches in reverse order to maintain positions
            for ($i = $matches.Count - 1; $i -ge 0; $i--) {
                $match = $matches[$i]
                $indent = $match.Groups[1].Value
                $propertyDeclaration = $match.Groups[2].Value
                $propertyName = $match.Groups[3].Value
                
                # Check if this property already has documentation
                $beforeProperty = $content.Substring(0, $match.Index)
                if ($beforeProperty -match '/// <summary>\s*$') {
                    continue
                }
                
                $propDoc = Get-PropertyDocumentation -PropertyName $propertyName
                $docComment = "$indent/// <summary>`n$indent/// $propDoc`n$indent/// </summary>`n"
                
                $content = $content.Substring(0, $match.Index) + $docComment + $propertyDeclaration + $content.Substring($match.Index + $match.Length)
            }
            
            if (!$WhatIf) {
                Set-Content -Path $FilePath -Value $content -NoNewline
                Write-Host "Added documentation to $FilePath" -ForegroundColor Green
                return $true
            } else {
                Write-Host "Would add documentation to $FilePath" -ForegroundColor Cyan
                return $false
            }
        }
    }
    catch {
        Write-Host "Error processing $FilePath : $_" -ForegroundColor Red
        return $false
    }
    
    return $false
}

# Main execution
Write-Host "Starting XML documentation addition process..." -ForegroundColor Cyan
Write-Host "Entities folder: $EntitiesFolder" -ForegroundColor Cyan

if ($WhatIf) {
    Write-Host "Running in WhatIf mode - no files will be modified" -ForegroundColor Yellow
}

$files = Get-ChildItem -Path $EntitiesFolder -Filter "*.cs" | Where-Object { $_.Name -notmatch '\.Extensions\.cs$' }
$totalFiles = $files.Count
$processedFiles = 0
$skippedFiles = 0

Write-Host "Found $totalFiles entity files to process" -ForegroundColor Cyan

foreach ($file in $files) {
    $result = Add-XmlDocumentation -FilePath $file.FullName
    if ($result) {
        $processedFiles++
    } else {
        $skippedFiles++
    }
}

Write-Host "`nSummary:" -ForegroundColor Cyan
Write-Host "Total files: $totalFiles" -ForegroundColor White
Write-Host "Processed: $processedFiles" -ForegroundColor Green
Write-Host "Skipped: $skippedFiles" -ForegroundColor Yellow

if (!$WhatIf) {
    Write-Host "`nXML documentation has been added successfully!" -ForegroundColor Green
} else {
    Write-Host "`nRun without -WhatIf to apply changes" -ForegroundColor Yellow
}
