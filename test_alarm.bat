@echo off
echo 🚨 PRUEBA DE SONIDO DE ALARMA 🚨
echo ================================
echo.
echo Reproduciendo sonido de alarma...
echo.

REM Usar PowerShell para reproducir el sonido
powershell -Command "[console]::beep(800,500)"
echo Primer beep reproducido!

timeout /t 1 /nobreak >nul

powershell -Command "[console]::beep(1200,300)"
echo Segundo beep reproducido!

timeout /t 1 /nobreak >nul

powershell -Command "[console]::beep(600,700)"
echo Tercer beep reproducido!

echo.
echo ✅ Sonidos de alarma completados!
echo.
echo Si escuchaste los sonidos, la funcionalidad de alarma está funcionando.
echo Si no escuchaste nada, verifica:
echo - Que los altavoces/auriculares estén conectados
echo - Que el volumen del sistema esté activado
echo - Que no estés en un entorno remoto que bloquee sonidos
echo.
pause
