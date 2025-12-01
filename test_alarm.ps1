Write-Host "🚨 PRUEBA DE SONIDO DE ALARMA 🚨" -ForegroundColor Yellow
Write-Host "================================" -ForegroundColor Yellow
Write-Host ""
Write-Host "Reproduciendo sonidos de alarma..." -ForegroundColor Green
Write-Host ""

try {
    # Primer sonido - Beep básico
    Write-Host "🔊 Reproduciendo primer beep (800Hz, 500ms)..." -ForegroundColor Cyan
    [console]::beep(800, 500)
    Start-Sleep -Seconds 1
    
    # Segundo sonido - Tono alto
    Write-Host "🔊 Reproduciendo segundo beep (1200Hz, 300ms)..." -ForegroundColor Cyan
    [console]::beep(1200, 300)
    Start-Sleep -Seconds 1
    
    # Tercer sonido - Tono bajo
    Write-Host "🔊 Reproduciendo tercer beep (600Hz, 700ms)..." -ForegroundColor Cyan
    [console]::beep(600, 700)
    Start-Sleep -Seconds 1
    
    # Patrón de alarma
    Write-Host "🔊 Reproduciendo patrón de alarma..." -ForegroundColor Cyan
    for ($i = 0; $i -lt 3; $i++) {
        [console]::beep(1000, 200)
        Start-Sleep -Milliseconds 100
    }
    
    Write-Host ""
    Write-Host "✅ ¡Sonidos de alarma completados!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Si escuchaste los sonidos, la funcionalidad de alarma está funcionando." -ForegroundColor White
    Write-Host "Si no escuchaste nada, verifica:" -ForegroundColor Yellow
    Write-Host "- Que los altavoces/auriculares estén conectados" -ForegroundColor White
    Write-Host "- Que el volumen del sistema esté activado" -ForegroundColor White
    Write-Host "- Que no estés en un entorno remoto que bloquee sonidos" -ForegroundColor White
}
catch {
    Write-Host "❌ Error reproduciendo sonidos: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Nota: Los sonidos pueden no funcionar en todos los entornos" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Presiona cualquier tecla para continuar..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
