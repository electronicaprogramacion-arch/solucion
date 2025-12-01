# PowerShell script to delete excluded files from CalibrationSaaS.Infraestructure.Blazor project
param(
    [string]$ProjectPath = "src/CalibrationSaaS/BlazorUpgrade/LTI"
)

Write-Host "DELETING EXCLUDED FILES FROM BLAZOR PROJECT" -ForegroundColor Yellow
Write-Host "===========================================" -ForegroundColor Yellow
Write-Host ""

$projectDir = $ProjectPath

# List of individual files to delete (from Content Remove and Compile Remove)
$filesToDelete = @(
    "GenericMethods\StandardComponent.razor",
    "NavMenu.razor",
    "Pages\AssetsBasics\Certificate_Create.razor",
    "Pages\AssetsBasics\Certificate_CreatePoE.razor", 
    "Pages\AssetsBasics\PieceOfEquipment_Create.razor",
    "Pages\AssetsBasics\PieceOfEquipment_SearchChildren.razor",
    "Pages\AssetsBasics\WorkOrderDetailChildren_Search.razor",
    "Pages\AssetsBasics\WorkOrderDetail_Search.razor",
    "Pages\AssetsBasics\WorkOrder_Create.razor",
    "Pages\Basics\BasicInformationComponent.razor",
    "Pages\Basics\Equipment_Create.razor",
    "Pages\Basics\ResolutionComponent.razor",
    "Pages\Basics\ResolutionComponentModal.razor",
    "Pages\Basics\TestCode_Create.razor",
    "Pages\Calculator.razor",
    "Pages\Index.razor",
    "Pages\Offline\ViewDataBase.razor",
    "Pages\Offline\WorkOrderDetailOff2_Search.razor",
    "Pages\Offline\WorkOrderDetailOff_Search.razor",
    "Pages\Order\BasicInformation.razor",
    "Pages\Order\CalibrationInstructions.razor",
    "Pages\Order\EnviromentComponent.razor",
    "Pages\Order\EquipmentConditionComponent.razor",
    "Pages\Order\WeightSetComponent.razor",
    "Pages\Order\WorkOrderItemCreate.razor",
    "Shared\Component\Indicator.razor",
    "Shared\DynamicReport.razor",
    "Shared\GenericTestComponent.razor",
    "Shared\GetReportView.razor",
    "Shared\LoadConfiguration.razor",
    "Shared\MainLayout.razor",
    "Shared\NavMenu.razor",
    "Shared\RedirectToLogin.razor",
    "Shared\TopMenu.razor",
    "Shared\version1\GenericTestComponent.razor",
    "Shared\WeightSetComponent.razor",
    "NavMenu.razor.css",
    "Shared\MainLayout.razor.css",
    "Shared\NavMenu.razor.css",
    "Shared\TopMenu.razor.bak",
    "GenericMethods\Create1000-6.cs",
    "GenericMethods\CreateET.cs",
    "GenericMethods\CreatePOE1000-6.cs",
    "GenericMethods\Get1000-6.cs",
    "GenericMethods\StandardComponent_Base.cs",
    "GlobalSuppressions.cs",
    "Helper\Create.cs",
    "ICalibrationInstructionsLTIBase.cs",
    "IWorkOrderItemCreate.cs",
    "KavokuComponentBase.cs",
    "KavokuComponentBase2.cs",
    "Pages\AssetsBasics\Certificate_Create.cs",
    "Pages\AssetsBasics\Certificate_CreatePoE.cs",
    "Pages\AssetsBasics\PieceOfEquipment_CreateBase.cs",
    "Pages\AssetsBasics\PieceOfEquipment_CreateBase.old.cs",
    "Pages\AssetsBasics\PieceOfEquipment_SearchBase.old.cs",
    "Pages\AssetsBasics\POEWeightSets_SearchBase.cs",
    "Pages\AssetsBasics\WorkOrderDetailChildren_SearchBase.cs",
    "Pages\AssetsBasics\WorkOrderDetail_SearchBase.cs",
    "Pages\AssetsBasics\WorkOrder_CreateBase.cs",
    "Pages\Basics\BasicInformationComponent.cs",
    "Pages\Basics\Equipment_CreateBase.cs",
    "Pages\Basics\ResolutionComponentBase.cs",
    "Pages\Basics\TestCode_CreateBase.cs",
    "Pages\Offline\WorkOrderDetailOff2_SearchBase.cs",
    "Pages\Offline\WorkOrderDetailOff_SearchBase.cs",
    "Pages\Order\BasicInformationBase.cs",
    "Pages\Order\CalibrationInstructionsBase.cs",
    "Pages\Order\Certified_Search.cs",
    "Pages\Order\EccentricityComponent.cs",
    "Pages\Order\LinearityComponentBase.cs",
    "Pages\Order\OffLineData_SearchBase.cs",
    "Pages\Order\PieceOfEquipment_SearchBase.cs",
    "Pages\Order\RepeabilityComponent.cs",
    "Pages\Order\WeightComponent.cs",
    "Pages\Order\WeightSetComponent_Base.cs",
    "Pages\Order\WorkOrderItemCreateBase.cs",
    "Pages\PieceOfEquipment\PieceOfEquipment_CreateBase.cs",
    "Pages\Security\Security_Search.cs",
    "ResponsiveTableService.cs",
    "Security\ArrayClaimsPrincipalFactory.cs",
    "Shared\CalibrationInstructionsBase.cs",
    "Shared\Component\Indicator_Base.cs",
    "Shared\DynamicModalHandler.cs",
    "Shared\DynamicReport.cs",
    "Shared\GenericTestComponentBase.cs",
    "Shared\ModalHandler.cs",
    "Shared\version1\GenericTestComponentBase.cs",
    "Shared\WeightSetBase.cs",
    "Shared\WeightSetComponent_Base.cs",
    "Pages\AssetsBasics\PieceOfEquipmentDueDate_Search.razor",
    "Pages\AssetsBasics\PieceOfEquipment_Create.old.razor",
    "Pages\AssetsBasics\PieceOfEquipment_Search.old.razor",
    "Pages\AssetsBasics\POEWeightSets_Search.razor",
    "Pages\AssetsBasics\Technician_Create.razor",
    "Pages\Customer\Address_Search_Test.razor",
    "Pages\Customer\Contact-Create.razor",
    "Pages\Order\Eccentricity.razor",
    "Pages\Order\EccentricityComponent.razor",
    "Pages\Order\LinearityComponent.razor",
    "Pages\Order\OffLineData_Search.razor",
    "Pages\Order\PieceOfEquipment_Search.razor",
    "Pages\Order\RepeabilityComponent.razor",
    "Pages\Order\WeightComponent.razor",
    "Pages\PieceOfEquipment\PieceOfEquipment_Create.razor",
    "Pages\Security\Security_Search.razor",
    "Pages\Tools\Customer_Details.razor",
    "Pages\Tools\GetReportByTestPoint.razor",
    "Pages\Tools\GetReportView.razor",
    "Pages\Tools\ReportUncertBudgetComp.razor",
    "wwwroot\Files\ExamenOCA.jpeg",
    "wwwroot\Files\Rut Personal Yenny Papamija.pdf",
    "wwwroot\js\BlazorControls.css",
    "wwwroot\js\BlazorControls.js",
    "wwwroot\js\Chart.min.js",
    "Pages\Basics\Equipment_Create.razor.old",
    "Program.cs.bak"
)

# List of folders to delete completely (from Compile Remove with **)
$foldersToDelete = @(
    "GenericMethods\NewFolder",
    "LTI",
    "Pages\Assets",
    "Pages\Base", 
    "Services",
    "Shared\OfflineModal",
    "Shared\ReportView",
    "Store",
    "Upload"
)

Write-Host "DELETING INDIVIDUAL FILES:" -ForegroundColor Cyan
Write-Host "==========================" -ForegroundColor Cyan

$deletedFiles = 0
$errorFiles = 0
$totalSizeFreed = 0

foreach ($file in $filesToDelete) {
    $fullPath = Join-Path $projectDir $file
    
    if (Test-Path $fullPath) {
        try {
            $fileInfo = Get-Item $fullPath
            $fileSize = $fileInfo.Length
            Remove-Item -Path $fullPath -Force
            Write-Host "  DELETED: $file (Size: $([math]::Round($fileSize/1024, 2)) KB)" -ForegroundColor Green
            $deletedFiles++
            $totalSizeFreed += $fileSize
        } catch {
            Write-Host "  ERROR deleting: $file - $($_.Exception.Message)" -ForegroundColor Red
            $errorFiles++
        }
    } else {
        Write-Host "  NOT FOUND: $file" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "DELETING FOLDERS:" -ForegroundColor Cyan
Write-Host "=================" -ForegroundColor Cyan

$deletedFolders = 0
$errorFolders = 0

foreach ($folder in $foldersToDelete) {
    $fullPath = Join-Path $projectDir $folder
    
    if (Test-Path $fullPath) {
        try {
            # Get folder size before deletion
            $folderSize = (Get-ChildItem -Path $fullPath -Recurse -File | Measure-Object -Property Length -Sum).Sum
            if ($folderSize -eq $null) { $folderSize = 0 }
            
            Remove-Item -Path $fullPath -Recurse -Force
            Write-Host "  DELETED FOLDER: $folder (Size: $([math]::Round($folderSize/1024, 2)) KB)" -ForegroundColor Green
            $deletedFolders++
            $totalSizeFreed += $folderSize
        } catch {
            Write-Host "  ERROR deleting folder: $folder - $($_.Exception.Message)" -ForegroundColor Red
            $errorFolders++
        }
    } else {
        Write-Host "  FOLDER NOT FOUND: $folder" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "DELETION SUMMARY:" -ForegroundColor Green
Write-Host "=================" -ForegroundColor Green
Write-Host "  Files deleted: $deletedFiles" -ForegroundColor White
Write-Host "  File errors: $errorFiles" -ForegroundColor $(if ($errorFiles -gt 0) { "Red" } else { "White" })
Write-Host "  Folders deleted: $deletedFolders" -ForegroundColor White
Write-Host "  Folder errors: $errorFolders" -ForegroundColor $(if ($errorFolders -gt 0) { "Red" } else { "White" })
Write-Host "  Total space freed: $([math]::Round($totalSizeFreed/1024, 2)) KB ($([math]::Round($totalSizeFreed/(1024*1024), 2)) MB)" -ForegroundColor White

Write-Host ""
Write-Host "CLEANUP COMPLETE!" -ForegroundColor Green
