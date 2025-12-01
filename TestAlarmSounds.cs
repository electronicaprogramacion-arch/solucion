using System;
using System.Threading.Tasks;
using CalibrationSaaS.Utilities;

namespace CalibrationSaaS
{
    /// <summary>
    /// Simple console application to test alarm sounds
    /// Run this to hear the different alarm sounds available
    /// </summary>
    class TestAlarmSounds
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("🚨 ALARM SOUND TESTER 🚨");
            Console.WriteLine("========================");
            Console.WriteLine("This will play various alarm sounds for testing.");
            Console.WriteLine("Make sure your speakers/headphones are on!");
            Console.WriteLine();

            Console.WriteLine("Press any key to start testing alarm sounds...");
            Console.ReadKey();
            Console.WriteLine();

            try
            {
                // Test 1: Simple beep
                Console.WriteLine("🔊 Test 1: Simple System Beep");
                AlarmSoundUtilities.PlaySystemBeep();
                await Task.Delay(1500);

                // Test 2: Custom beep
                Console.WriteLine("🔊 Test 2: Custom Beep (1000Hz)");
                AlarmSoundUtilities.PlayCustomBeep(1000, 500);
                await Task.Delay(1500);

                // Test 3: System alert
                Console.WriteLine("🔊 Test 3: System Alert (Critical)");
                AlarmSoundUtilities.PlaySystemAlert();
                await Task.Delay(1500);

                // Test 4: System warning
                Console.WriteLine("🔊 Test 4: System Warning");
                AlarmSoundUtilities.PlaySystemWarning();
                await Task.Delay(1500);

                // Test 5: Calibration alarm
                Console.WriteLine("🔊 Test 5: Calibration Alarm (High-Low Pattern)");
                await AlarmSoundUtilities.PlayCalibrationAlarm();
                await Task.Delay(1500);

                // Test 6: Urgent alarm
                Console.WriteLine("🔊 Test 6: Urgent Alarm (Rapid Beeps)");
                await AlarmSoundUtilities.PlayUrgentAlarm();
                await Task.Delay(1500);

                // Test 7: Success sound
                Console.WriteLine("🔊 Test 7: Success/Completion Sound");
                await AlarmSoundUtilities.PlayCompletionSound();
                await Task.Delay(1500);

                // Test 8: Error sound
                Console.WriteLine("🔊 Test 8: Error Sound");
                await AlarmSoundUtilities.PlayErrorSound();
                await Task.Delay(1500);

                Console.WriteLine();
                Console.WriteLine("✅ All alarm tests completed!");
                Console.WriteLine();

                // Interactive mode
                Console.WriteLine("Would you like to test specific alarms? (y/n)");
                var response = Console.ReadLine();
                
                if (response?.ToLower() == "y" || response?.ToLower() == "yes")
                {
                    await InteractiveMode();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error playing sounds: {ex.Message}");
                Console.WriteLine("Note: Sound may not work in all environments (e.g., some cloud/remote systems)");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static async Task InteractiveMode()
        {
            Console.WriteLine();
            Console.WriteLine("🎵 Interactive Alarm Mode");
            Console.WriteLine("=========================");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Choose an alarm to play:");
                Console.WriteLine("1 - System Beep");
                Console.WriteLine("2 - Custom Beep");
                Console.WriteLine("3 - System Alert");
                Console.WriteLine("4 - System Warning");
                Console.WriteLine("5 - Calibration Alarm");
                Console.WriteLine("6 - Urgent Alarm");
                Console.WriteLine("7 - Success Sound");
                Console.WriteLine("8 - Error Sound");
                Console.WriteLine("9 - Alarm Pattern (3 beeps)");
                Console.WriteLine("0 - Exit");
                Console.WriteLine();
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("🔊 Playing System Beep...");
                            AlarmSoundUtilities.PlaySystemBeep();
                            break;

                        case "2":
                            Console.Write("Enter frequency (Hz, 37-32767) [default 800]: ");
                            var freqInput = Console.ReadLine();
                            Console.Write("Enter duration (ms) [default 500]: ");
                            var durInput = Console.ReadLine();

                            int freq = 800, dur = 500;
                            if (!string.IsNullOrEmpty(freqInput) && int.TryParse(freqInput, out int f))
                                freq = Math.Max(37, Math.Min(32767, f));
                            if (!string.IsNullOrEmpty(durInput) && int.TryParse(durInput, out int d))
                                dur = Math.Max(1, d);

                            Console.WriteLine($"🔊 Playing Custom Beep ({freq}Hz, {dur}ms)...");
                            AlarmSoundUtilities.PlayCustomBeep(freq, dur);
                            break;

                        case "3":
                            Console.WriteLine("🔊 Playing System Alert...");
                            AlarmSoundUtilities.PlaySystemAlert();
                            break;

                        case "4":
                            Console.WriteLine("🔊 Playing System Warning...");
                            AlarmSoundUtilities.PlaySystemWarning();
                            break;

                        case "5":
                            Console.WriteLine("🔊 Playing Calibration Alarm...");
                            await AlarmSoundUtilities.PlayCalibrationAlarm();
                            break;

                        case "6":
                            Console.WriteLine("🔊 Playing Urgent Alarm...");
                            await AlarmSoundUtilities.PlayUrgentAlarm();
                            break;

                        case "7":
                            Console.WriteLine("🔊 Playing Success Sound...");
                            await AlarmSoundUtilities.PlayCompletionSound();
                            break;

                        case "8":
                            Console.WriteLine("🔊 Playing Error Sound...");
                            await AlarmSoundUtilities.PlayErrorSound();
                            break;

                        case "9":
                            Console.WriteLine("🔊 Playing Alarm Pattern...");
                            await AlarmSoundUtilities.PlayAlarmPattern(3, 1000, 300, 200);
                            break;

                        case "0":
                            Console.WriteLine("Exiting interactive mode...");
                            return;

                        default:
                            Console.WriteLine("❌ Invalid choice. Please enter 0-9.");
                            continue;
                    }

                    await Task.Delay(500); // Brief pause after each sound
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error playing sound: {ex.Message}");
                }
            }
        }
    }
}
