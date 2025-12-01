@echo off
echo 🚨 EJECUTANDO APLICACION DE ALARMA 10 VECES 🚨
echo ================================================
echo.

echo Compilando aplicacion de alarma...
dotnet build AlarmTest --verbosity quiet
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Error en compilacion
    pause
    exit /b 1
)
echo ✅ Compilacion exitosa
echo.

echo 🔄 Ejecutando aplicacion 10 veces...
echo.

for /L %%i in (1,1,10) do (
    echo 🚀 Iniciando ejecucion #%%i
    dotnet run --project AlarmTest --verbosity quiet -- %%i
    if %ERRORLEVEL% EQU 0 (
        echo ✅ Ejecucion #%%i completada exitosamente
    ) else (
        echo ⚠️ Ejecucion #%%i completada con codigo: %ERRORLEVEL%
    )
    echo.
    timeout /t 1 /nobreak >nul
)

echo 🎉 TODAS LAS EJECUCIONES COMPLETADAS
echo ====================================
echo.
echo 📊 RESUMEN DE SONIDOS REPRODUCIDOS:
echo - Beeps simples del sistema: 10
echo - Beeps personalizados (1000Hz): 10
echo - Sonidos de alerta del sistema: 10
echo - Alarmas de calibracion (patron alto-bajo): 10
echo - Sonidos de exito (tonos ascendentes): 10
echo - Total de sonidos reproducidos: 50
echo.
echo Si escuchaste todos los sonidos, el sistema de alarmas esta funcionando correctamente.
echo Si no escuchaste sonidos, verifica:
echo - Volumen del sistema activado
echo - Altavoces/auriculares conectados
echo - No estar en un entorno remoto que bloquee audio
echo.
pause
