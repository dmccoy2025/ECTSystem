# PowerShell script to fix missing property documentation
# This script adds XML documentation to properties that are missing it

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
        "^.*Ssn$|^Ssan$" { return "$doc Social Security Number." }
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
        "^.*Order$" { return "$doc$($PropertyName -creplace '([A-Z])', ' $1' -replace '^ ')." }
        "^Title$" { return "$doc title." }
        default { return "$doc$($PropertyName -creplace '([A-Z])', ' $1' -replace '^ ')." }
    }
}

function Add-PropertyDocumentation {
    param(
        [string]$FilePath
    )
    
    try {
        $content = Get-Content $FilePath -Raw
        $originalContent = $content
        
        # Find all properties without documentation
        # Pattern: property declaration NOT preceded by /// <summary>
        $lines = $content -split "`r?`n"
        $newLines = @()
        $modified = $false
        
        for ($i = 0; $i -lt $lines.Count; $i++) {
            $line = $lines[$i]
            
            # Check if this is a property declaration
            if ($line -match '^\s+public\s+(virtual\s+)?(\S+)\s+(\w+)\s*{\s*get;\s*set;\s*}') {
                $indent = [regex]::Match($line, '^\s+').Value
                $propertyName = $Matches[3]
                
                # Check if previous line is NOT a summary tag
                $prevLine = if ($i -gt 0) { $lines[$i - 1] } else { "" }
                
                if ($prevLine -notmatch '^\s*/// </summary>') {
                    # Add documentation
                    $propDoc = Get-PropertyDocumentation -PropertyName $propertyName
                    $newLines += "$indent/// <summary>"
                    $newLines += "$indent/// $propDoc"
                    $newLines += "$indent/// </summary>"
                    $modified = $true
                }
            }
            
            $newLines += $line
        }
        
        if ($modified) {
            $newContent = $newLines -join "`r`n"
            
            if (!$WhatIf) {
                Set-Content -Path $FilePath -Value $newContent -NoNewline
                Write-Host "Fixed documentation in $FilePath" -ForegroundColor Green
                return $true
            } else {
                Write-Host "Would fix documentation in $FilePath" -ForegroundColor Cyan
                return $false
            }
        } else {
            Write-Host "No missing property documentation in $FilePath" -ForegroundColor Gray
            return $false
        }
    }
    catch {
        Write-Host "Error processing $FilePath : $_" -ForegroundColor Red
        return $false
    }
}

# Main execution
Write-Host "Starting property documentation fix process..." -ForegroundColor Cyan
Write-Host "Entities folder: $EntitiesFolder" -ForegroundColor Cyan

if ($WhatIf) {
    Write-Host "Running in WhatIf mode - no files will be modified" -ForegroundColor Yellow
}

# List of files identified as having missing property documentation
$filesToFix = @(
    "ApplicationWarmupProcess.cs",
    "ApplicationWarmupProcessLog.cs",
    "AspstateTempApplication.cs",
    "AspstateTempSession.cs",
    "CaseDialogueComment.cs",
    "CommandStructChain.cs",
    "CommandStructChainBackup.cs",
    "CommandStructHistory.cs",
    "CommandStructTree.cs",
    "CommandStructTreeTmp.cs",
    "ConversionRun.cs",
    "ConversionRunLog.cs",
    "CoreCaseType.cs",
    "CoreCaseTypeSubCaseTypeMap.cs",
    "CoreCertificationStamp.cs",
    "CoreCompletedByGroup.cs",
    "CoreLkupAccessScope.cs",
    "CoreLkupAccessStatus.cs",
    "CoreLkupChainType.cs",
    "CoreLkupCompo.cs",
    "CoreLkupCountry.cs",
    "CoreLkupDisposition.cs",
    "CoreLkupGrade.cs",
    "CoreLkupMajcom.cs",
    "CoreLkupModule.cs",
    "CoreLkupProcess.cs",
    "CoreLkupUnitLevelType.cs",
    "CoreLkupWorkflowAction.cs",
    "CoreLkupYearsSatisfactoryService.cs",
    "CoreMemo.cs",
    "CoreMessage.cs",
    "CorePage.cs",
    "CorePageAccess.cs",
    "CoreSignatureMetaDatum.cs",
    "CoreSubCaseType.cs",
    "CoreUserPermission.cs",
    "CoreUsersOnline.cs",
    "CoreWorkStatusAction.cs",
    "CoreWorkStatusOption.cs",
    "DataElementDetail.cs",
    "DevLogin.cs",
    "DevUnit.cs",
    "DocCategoryView.cs",
    "DocumentCategory2.cs",
    "DocumentView.cs",
    "Form261.cs",
    "Form348Ap.cs",
    "Form348ApFinding.cs",
    "Form348ApprovalAuthority.cs",
    "Form348ApSarc.cs",
    "Form348ApSarcFinding.cs",
    "Form348Audit.cs",
    "Form348Comment.cs",
    "Form348Finding.cs",
    "Form348IncapAppeal.cs",
    "Form348IncapExt.cs",
    "Form348IncapFinding.cs",
    "Form348Lesson.cs",
    "Form348Medical.cs",
    "Form348PostProcessing.cs",
    "Form348PostProcessingAppeal.cs",
    "Form348PostProcessingAppealSarc.cs",
    "Form348PscdFinding.cs",
    "Form348Rr.cs",
    "Form348RrFinding.cs",
    "Form348Sarc.cs",
    "Form348SarcFinding.cs",
    "Form348SarcIcd.cs",
    "Form348SarcPostProcessing.cs",
    "Form348Sc.cs",
    "Form348ScPeppType.cs",
    "Form348ScReassessment.cs",
    "Form348Unit.cs",
    "GradeAbbreviation.cs",
    "HyperLink.cs",
    "HyperLinkType.cs",
    "ImpLodmapping.cs",
    "ImpPermmapping.cs",
    "ImpProcmapping.cs",
    "MemberdataBackup.cs",
    "MemberDataChangeHistory.cs",
    "MilpdsrawDatum.cs",
    "PalDataTemp.cs",
    "PalDatum.cs",
    "PaldocumentLookup.cs",
    "PhField.cs",
    "PhFieldType.cs",
    "PhFormField.cs",
    "PhFormValue.cs",
    "PhSection.cs",
    "PkgLog.cs",
    "PreviousWeeksEctcase.cs",
    "PrintDocument.cs",
    "PrintDocumentFormFieldParser.cs",
    "PrintDocumentProperty.cs",
    "RecentlyUploadedDocument.cs",
    "ReminderEmailSetting.cs",
    "ReminderInactiveSetting.cs",
    "ReportUnitMetric.cs",
    "Return.cs",
    "RptPhUserQuery.cs",
    "RptPhUserQueryClause.cs",
    "RptPhUserQueryOutputField.cs",
    "RptPhUserQueryParam.cs",
    "RptQueryField.cs",
    "RptQuerySource.cs",
    "RptUserQuery.cs",
    "RptUserQueryClause.cs",
    "RptUserQueryParam.cs",
    "Rwoa.cs",
    "SsisConfiguration.cs",
    "SuicideMethod.cs",
    "TestCommandStruct.cs",
    "TestCommandStructChain.cs",
    "TmpRgXxCommandStruct.cs"
)

$totalFiles = $filesToFix.Count
$processedFiles = 0
$skippedFiles = 0

Write-Host "Found $totalFiles entity files to fix" -ForegroundColor Cyan

foreach ($fileName in $filesToFix) {
    $filePath = Join-Path $EntitiesFolder $fileName
    if (Test-Path $filePath) {
        $result = Add-PropertyDocumentation -FilePath $filePath
        if ($result) {
            $processedFiles++
        } else {
            $skippedFiles++
        }
    } else {
        Write-Host "File not found: $fileName" -ForegroundColor Red
    }
}

Write-Host "`nSummary:" -ForegroundColor Cyan
Write-Host "Total files: $totalFiles" -ForegroundColor White
Write-Host "Processed: $processedFiles" -ForegroundColor Green
Write-Host "Skipped: $skippedFiles" -ForegroundColor Yellow

if (!$WhatIf) {
    Write-Host "`nProperty documentation has been fixed successfully!" -ForegroundColor Green
} else {
    Write-Host "`nRun without -WhatIf to apply changes" -ForegroundColor Yellow
}
