using System;
using System.Media;
using System.Threading.Tasks;

namespace AlarmTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("🚨 PRUEBA DE SONIDOS DE ALARMA 🚨");
            Console.WriteLine("=================================");
            Console.WriteLine("Esta aplicación reproducirá varios sonidos de alarma.");
            Console.WriteLine("¡Asegúrate de que tus altavoces/auriculares estén encendidos!");
            Console.WriteLine();

            // Check if we have a run number argument
            int runNumber = 1;
            if (args.Length > 0 && int.TryParse(args[0], out int argRun))
            {
                runNumber = argRun;
            }

            Console.WriteLine($"Ejecución #{runNumber} - Iniciando pruebas de sonido automáticamente...");
            Console.WriteLine();

            try
            {
                // Prueba 1: Beep simple del sistema
                Console.WriteLine($"🔊 Ejecución {runNumber} - Prueba 1: Beep Simple del Sistema");
                Console.Beep();
                await Task.Delay(800);

                // Prueba 2: Beep personalizado
                Console.WriteLine($"🔊 Ejecución {runNumber} - Prueba 2: Beep Personalizado (1000Hz, 300ms)");
                Console.Beep(1000, 300);
                await Task.Delay(800);

                // Prueba 3: Sonido de alerta del sistema
                Console.WriteLine($"🔊 Ejecución {runNumber} - Prueba 3: Sonido de Alerta del Sistema");
                SystemSounds.Hand.Play();
                await Task.Delay(800);

                // Prueba 4: Alarma de calibración (patrón alto-bajo)
                Console.WriteLine($"🔊 Ejecución {runNumber} - Prueba 4: Alarma de Calibración");
                await PlayCalibrationAlarm();
                await Task.Delay(800);

                // Prueba 5: Sonido de éxito
                Console.WriteLine($"🔊 Ejecución {runNumber} - Prueba 5: Sonido de Éxito");
                await PlaySuccessSound();
                await Task.Delay(800);

                Console.WriteLine();
                Console.WriteLine($"✅ Ejecución #{runNumber} completada!");
                Console.WriteLine();

                // Skip interactive mode for automated runs
                Console.WriteLine("Ejecución automática completada. Saliendo...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error reproduciendo sonidos: {ex.Message}");
                Console.WriteLine("Nota: Los sonidos pueden no funcionar en todos los entornos (ej. sistemas remotos/nube)");
            }

            Console.WriteLine();
            Console.WriteLine($"Ejecución #{runNumber} finalizada.");
            // Auto-exit for automated runs
        }

        static async Task PlayCalibrationAlarm()
        {
            try
            {
                // Patrón alto-bajo para alarmas de calibración
                Console.Beep(1200, 400); // Tono alto
                await Task.Delay(100);
                Console.Beep(800, 400);  // Tono bajo
                await Task.Delay(100);
                Console.Beep(1200, 400); // Tono alto
                await Task.Delay(100);
                Console.Beep(800, 400);  // Tono bajo
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en alarma de calibración: {ex.Message}");
            }
        }

        static async Task PlayUrgentAlarm()
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.Beep(1500, 150); // Beeps cortos y agudos
                    await Task.Delay(50);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en alarma urgente: {ex.Message}");
            }
        }

        static async Task PlaySuccessSound()
        {
            try
            {
                // Patrón ascendente para éxito
                Console.Beep(600, 200);
                await Task.Delay(50);
                Console.Beep(800, 200);
                await Task.Delay(50);
                Console.Beep(1000, 300);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en sonido de éxito: {ex.Message}");
            }
        }

        static async Task PlayErrorSound()
        {
            try
            {
                // Patrón descendente para errores
                Console.Beep(1000, 200);
                await Task.Delay(50);
                Console.Beep(800, 200);
                await Task.Delay(50);
                Console.Beep(600, 300);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en sonido de error: {ex.Message}");
            }
        }

        static async Task InteractiveMode()
        {
            Console.WriteLine();
            Console.WriteLine("🎵 Modo Interactivo de Alarmas");
            Console.WriteLine("==============================");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Elige una alarma para reproducir:");
                Console.WriteLine("1 - Beep del Sistema");
                Console.WriteLine("2 - Beep Personalizado");
                Console.WriteLine("3 - Alerta del Sistema");
                Console.WriteLine("4 - Advertencia del Sistema");
                Console.WriteLine("5 - Alarma de Calibración");
                Console.WriteLine("6 - Alarma Urgente");
                Console.WriteLine("7 - Sonido de Éxito");
                Console.WriteLine("8 - Sonido de Error");
                Console.WriteLine("9 - Patrón de Alarma (3 beeps)");
                Console.WriteLine("0 - Salir");
                Console.WriteLine();
                Console.Write("Ingresa tu elección: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("🔊 Reproduciendo Beep del Sistema...");
                            Console.Beep();
                            break;

                        case "2":
                            Console.Write("Ingresa frecuencia (Hz, 37-32767) [por defecto 800]: ");
                            var freqInput = Console.ReadLine();
                            Console.Write("Ingresa duración (ms) [por defecto 500]: ");
                            var durInput = Console.ReadLine();

                            int freq = 800, dur = 500;
                            if (!string.IsNullOrEmpty(freqInput) && int.TryParse(freqInput, out int f))
                                freq = Math.Max(37, Math.Min(32767, f));
                            if (!string.IsNullOrEmpty(durInput) && int.TryParse(durInput, out int d))
                                dur = Math.Max(1, d);

                            Console.WriteLine($"🔊 Reproduciendo Beep Personalizado ({freq}Hz, {dur}ms)...");
                            Console.Beep(freq, dur);
                            break;

                        case "3":
                            Console.WriteLine("🔊 Reproduciendo Alerta del Sistema...");
                            SystemSounds.Hand.Play();
                            break;

                        case "4":
                            Console.WriteLine("🔊 Reproduciendo Advertencia del Sistema...");
                            SystemSounds.Exclamation.Play();
                            break;

                        case "5":
                            Console.WriteLine("🔊 Reproduciendo Alarma de Calibración...");
                            await PlayCalibrationAlarm();
                            break;

                        case "6":
                            Console.WriteLine("🔊 Reproduciendo Alarma Urgente...");
                            await PlayUrgentAlarm();
                            break;

                        case "7":
                            Console.WriteLine("🔊 Reproduciendo Sonido de Éxito...");
                            await PlaySuccessSound();
                            break;

                        case "8":
                            Console.WriteLine("🔊 Reproduciendo Sonido de Error...");
                            await PlayErrorSound();
                            break;

                        case "9":
                            Console.WriteLine("🔊 Reproduciendo Patrón de Alarma...");
                            for (int i = 0; i < 3; i++)
                            {
                                Console.Beep(1000, 300);
                                if (i < 2) await Task.Delay(200);
                            }
                            break;

                        case "0":
                            Console.WriteLine("Saliendo del modo interactivo...");
                            return;

                        default:
                            Console.WriteLine("❌ Elección inválida. Por favor ingresa 0-9.");
                            continue;
                    }

                    await Task.Delay(500); // Breve pausa después de cada sonido
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error reproduciendo sonido: {ex.Message}");
                }
            }
        }
    }
}
