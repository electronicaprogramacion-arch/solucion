# PowerShell script to analyze excluded files in CalibrationSaaS.Infraestructure.Blazor project
param(
    [string]$ProjectPath = "src/CalibrationSaaS/BlazorUpgrade/LTI",
    [string]$ProjectFile = "CalibrationSaaS.Infraestructure.Blazor.csproj"
)

Write-Host "ANALYZING EXCLUDED FILES IN BLAZOR PROJECT" -ForegroundColor Yellow
Write-Host "===========================================" -ForegroundColor Yellow
Write-Host ""

$projectDir = $ProjectPath
$csprojPath = Join-Path $projectDir $ProjectFile

if (-not (Test-Path $csprojPath)) {
    Write-Host "ERROR: Project file not found: $csprojPath" -ForegroundColor Red
    exit 1
}

Write-Host "Project Directory: $projectDir" -ForegroundColor Green
Write-Host "Project File: $csprojPath" -ForegroundColor Green
Write-Host ""

# Read and parse the project file
$projectContent = Get-Content $csprojPath -Raw
[xml]$projectXml = $projectContent

# Extract excluded files from different sections
$excludedFiles = @()
$excludedFolders = @()

# Get Content Remove items (individual files)
$contentRemoveItems = $projectXml.Project.ItemGroup.Content | Where-Object { $_.Remove }
foreach ($item in $contentRemoveItems) {
    if ($item.Remove) {
        $excludedFiles += $item.Remove
    }
}

# Get None Remove items
$noneRemoveItems = $projectXml.Project.ItemGroup.None | Where-Object { $_.Remove }
foreach ($item in $noneRemoveItems) {
    if ($item.Remove) {
        $excludedFiles += $item.Remove
    }
}

# Get Compile Remove items (individual files)
$compileRemoveItems = $projectXml.Project.ItemGroup.Compile | Where-Object { $_.Remove }
foreach ($item in $compileRemoveItems) {
    if ($item.Remove) {
        $excludedFiles += $item.Remove
    }
}

# Get excluded folders (from Compile Remove with **)
$compileRemoveFolders = $projectXml.Project.ItemGroup.Compile | Where-Object { $_.Remove -and $_.Remove.Contains("**") }
foreach ($item in $compileRemoveFolders) {
    if ($item.Remove) {
        $excludedFolders += $item.Remove.Replace("\**", "").Replace("/**", "")
    }
}

# Get Content Remove folders
$contentRemoveFolders = $projectXml.Project.ItemGroup.Content | Where-Object { $_.Remove -and $_.Remove.Contains("**") }
foreach ($item in $contentRemoveFolders) {
    if ($item.Remove) {
        $excludedFolders += $item.Remove.Replace("\**", "").Replace("/**", "")
    }
}

Write-Host "EXCLUDED INDIVIDUAL FILES:" -ForegroundColor Cyan
Write-Host "===========================" -ForegroundColor Cyan
$excludedFiles | Sort-Object | ForEach-Object {
    Write-Host "  - $_" -ForegroundColor White
}
Write-Host ""

Write-Host "EXCLUDED FOLDERS:" -ForegroundColor Cyan
Write-Host "=================" -ForegroundColor Cyan
$excludedFolders | Sort-Object | ForEach-Object {
    Write-Host "  - $_" -ForegroundColor White
}
Write-Host ""

# Get all physical files in the project directory
Write-Host "Scanning physical files..." -ForegroundColor Yellow
$allFiles = Get-ChildItem -Path $projectDir -Recurse -File | Where-Object {
    $_.FullName -notmatch "\\bin\\" -and
    $_.FullName -notmatch "\\obj\\" -and
    $_.FullName -notmatch "\\.git\\" -and
    $_.FullName -notmatch "\\.vs\\" -and
    $_.Name -ne $ProjectFile
}

Write-Host "Total physical files found: $($allFiles.Count)" -ForegroundColor Green
Write-Host ""

# Check which files are excluded
$filesToDelete = @()

foreach ($file in $allFiles) {
    $relativePath = $file.FullName.Substring($projectDir.Length + 1).Replace("/", "\")
    $isExcluded = $false
    
    # Check if file is in excluded individual files list
    foreach ($excludedFile in $excludedFiles) {
        if ($relativePath -eq $excludedFile -or $relativePath -eq $excludedFile.Replace("/", "\")) {
            $isExcluded = $true
            break
        }
    }
    
    # Check if file is in an excluded folder
    if (-not $isExcluded) {
        foreach ($excludedFolder in $excludedFolders) {
            $normalizedFolder = $excludedFolder.Replace("/", "\")
            if ($relativePath.StartsWith($normalizedFolder + "\") -or $relativePath.StartsWith($normalizedFolder + "/")) {
                $isExcluded = $true
                break
            }
        }
    }
    
    if ($isExcluded) {
        $filesToDelete += @{
            FullPath = $file.FullName
            RelativePath = $relativePath
            Size = $file.Length
            LastModified = $file.LastWriteTime
        }
    }
}

Write-Host "FILES TO BE DELETED:" -ForegroundColor Red
Write-Host "====================" -ForegroundColor Red

if ($filesToDelete.Count -eq 0) {
    Write-Host "No excluded files found to delete!" -ForegroundColor Green
} else {
    $totalSize = 0
    foreach ($file in $filesToDelete) {
        $sizeKB = [math]::Round($file.Size / 1024, 2)
        $totalSize += $file.Size
        Write-Host "  FILE: $($file.RelativePath)" -ForegroundColor White
        Write-Host "     Size: $sizeKB KB | Modified: $($file.LastModified)" -ForegroundColor Gray
    }

    $totalSizeKB = [math]::Round($totalSize / 1024, 2)
    $totalSizeMB = [math]::Round($totalSize / (1024 * 1024), 2)

    Write-Host ""
    Write-Host "SUMMARY:" -ForegroundColor Yellow
    Write-Host "========" -ForegroundColor Yellow
    Write-Host "  Files to delete: $($filesToDelete.Count)" -ForegroundColor White
    Write-Host "  Total size: $totalSizeKB KB ($totalSizeMB MB)" -ForegroundColor White
    Write-Host ""
    
    # Ask for confirmation
    $confirmation = Read-Host "WARNING: Do you want to DELETE these files? (yes/no)"

    if ($confirmation -eq "yes" -or $confirmation -eq "y") {
        Write-Host ""
        Write-Host "DELETING FILES..." -ForegroundColor Red
        Write-Host "=================" -ForegroundColor Red

        $deletedCount = 0
        $errorCount = 0

        foreach ($file in $filesToDelete) {
            try {
                Remove-Item -Path $file.FullPath -Force
                Write-Host "  DELETED: $($file.RelativePath)" -ForegroundColor Green
                $deletedCount++
            } catch {
                Write-Host "  ERROR deleting: $($file.RelativePath) - $($_.Exception.Message)" -ForegroundColor Red
                $errorCount++
            }
        }

        Write-Host ""
        Write-Host "DELETION COMPLETE!" -ForegroundColor Green
        Write-Host "==================" -ForegroundColor Green
        Write-Host "  Successfully deleted: $deletedCount files" -ForegroundColor Green
        Write-Host "  Errors: $errorCount files" -ForegroundColor $(if ($errorCount -gt 0) { "Red" } else { "Green" })
        Write-Host "  Total space freed: $totalSizeKB KB ($totalSizeMB MB)" -ForegroundColor Green

    } else {
        Write-Host ""
        Write-Host "Deletion cancelled by user." -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "Analysis complete!" -ForegroundColor Green
