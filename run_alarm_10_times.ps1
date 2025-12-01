Write-Host "🚨 EJECUTANDO APLICACIÓN DE ALARMA 10 VECES 🚨" -ForegroundColor Yellow
Write-Host "================================================" -ForegroundColor Yellow
Write-Host ""

$startTime = Get-Date
Write-Host "Inicio: $($startTime.ToString('HH:mm:ss'))" -ForegroundColor Green
Write-Host ""

# Compilar la aplicación primero
Write-Host "📦 Compilando aplicación de alarma..." -ForegroundColor Cyan
try {
    $buildResult = dotnet build AlarmTest --verbosity quiet
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Compilación exitosa" -ForegroundColor Green
    } else {
        Write-Host "❌ Error en compilación" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "❌ Error compilando: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "🔄 Ejecutando aplicación 10 veces..." -ForegroundColor Cyan
Write-Host ""

# Ejecutar la aplicación 10 veces
for ($i = 1; $i -le 10; $i++) {
    $runStart = Get-Date
    Write-Host "🚀 Iniciando ejecución #$i - $($runStart.ToString('HH:mm:ss.fff'))" -ForegroundColor White
    
    try {
        # Ejecutar la aplicación pasando el número de ejecución como argumento
        $output = dotnet run --project AlarmTest --verbosity quiet -- $i
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Ejecución #$i completada exitosamente" -ForegroundColor Green
        } else {
            Write-Host "⚠️ Ejecución #$i completada con código: $LASTEXITCODE" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "❌ Error en ejecución #$i : $($_.Exception.Message)" -ForegroundColor Red
    }
    
    $runEnd = Get-Date
    $runDuration = ($runEnd - $runStart).TotalSeconds
    Write-Host "⏱️ Duración ejecución #$i : $($runDuration.ToString('F2')) segundos" -ForegroundColor Gray
    Write-Host ""
    
    # Pausa breve entre ejecuciones
    if ($i -lt 10) {
        Start-Sleep -Milliseconds 500
    }
}

$endTime = Get-Date
$totalDuration = ($endTime - $startTime).TotalSeconds

Write-Host "🎉 TODAS LAS EJECUCIONES COMPLETADAS" -ForegroundColor Green
Write-Host "====================================" -ForegroundColor Green
Write-Host "Inicio: $($startTime.ToString('HH:mm:ss'))" -ForegroundColor White
Write-Host "Fin: $($endTime.ToString('HH:mm:ss'))" -ForegroundColor White
Write-Host "Duración total: $($totalDuration.ToString('F2')) segundos" -ForegroundColor White
Write-Host "Promedio por ejecución: $(($totalDuration / 10).ToString('F2')) segundos" -ForegroundColor White
Write-Host ""

# Resumen de sonidos reproducidos
Write-Host "📊 RESUMEN DE SONIDOS REPRODUCIDOS:" -ForegroundColor Cyan
Write-Host "- Beeps simples del sistema: 10" -ForegroundColor White
Write-Host "- Beeps personalizados (1000Hz): 10" -ForegroundColor White
Write-Host "- Sonidos de alerta del sistema: 10" -ForegroundColor White
Write-Host "- Alarmas de calibración (patrón alto-bajo): 10" -ForegroundColor White
Write-Host "- Sonidos de éxito (tonos ascendentes): 10" -ForegroundColor White
Write-Host "- Total de sonidos reproducidos: 50" -ForegroundColor Yellow
Write-Host ""

Write-Host "Si escuchaste todos los sonidos, el sistema de alarmas está funcionando correctamente." -ForegroundColor Green
Write-Host "Si no escuchaste sonidos, verifica:" -ForegroundColor Yellow
Write-Host "- Volumen del sistema activado" -ForegroundColor White
Write-Host "- Altavoces/auriculares conectados" -ForegroundColor White
Write-Host "- No estar en un entorno remoto que bloquee audio" -ForegroundColor White
Write-Host ""

Write-Host "Presiona cualquier tecla para continuar..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
