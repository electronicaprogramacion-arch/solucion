using System;
using System.Media;
using System.Threading.Tasks;

/// <summary>
/// Simple alarm test that can be run directly
/// </summary>
public class SimpleAlarmTest
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("=== PRUEBA DE SONIDOS DE ALARMA ===");
        Console.WriteLine("Iniciando pruebas de sonido...");
        
        try
        {
            // Test 1: Simple beep
            Console.WriteLine("Prueba 1: Beep simple");
            Console.Beep();
            await Task.Delay(1000);
            
            // Test 2: Custom beep
            Console.WriteLine("Prueba 2: Beep personalizado (1000Hz)");
            Console.Beep(1000, 500);
            await Task.Delay(1000);
            
            // Test 3: System sounds
            Console.WriteLine("Prueba 3: Sonido de alerta del sistema");
            SystemSounds.Hand.Play();
            await Task.Delay(1000);
            
            Console.WriteLine("Prueba 4: Sonido de advertencia");
            SystemSounds.Exclamation.Play();
            await Task.Delay(1000);
            
            // Test 5: Alarm pattern
            Console.WriteLine("Prueba 5: Patrón de alarma");
            for (int i = 0; i < 3; i++)
            {
                Console.Beep(800 + (i * 200), 300);
                await Task.Delay(200);
            }
            
            Console.WriteLine("✅ Todas las pruebas completadas!");
            Console.WriteLine("Si escuchaste los sonidos, las alarmas funcionan correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine("Los sonidos pueden no funcionar en entornos remotos o sin audio.");
        }
        
        Console.WriteLine("Presiona Enter para continuar...");
        Console.ReadLine();
    }
}
