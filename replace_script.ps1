# PowerShell script to replace all remaining occurrences
$filePath = "d:\source\repos\dmccoy2025\ECTSystem\AF.ECT.Server\Services\WorkflowServiceImpl.cs"
$content = Get-Content -Path $filePath -Raw
$oldPattern = 'throw new RpcException(new Status(StatusCode.Cancelled, "Operation was cancelled"));'
$newPattern = 'throw CreateCancelledException();'
$newContent = $content.Replace($oldPattern, $newPattern)
Set-Content -Path $filePath -Value $newContent -NoNewline
Write-Host "Replacement complete. Total replacements made."
