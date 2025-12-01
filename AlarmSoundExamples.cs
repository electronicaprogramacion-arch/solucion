using System;
using System.Threading.Tasks;
using CalibrationSaaS.Utilities;

namespace CalibrationSaaS.Examples
{
    /// <summary>
    /// Examples of how to use alarm sounds in the CalibrationSaaS application
    /// </summary>
    public class AlarmSoundExamples
    {
        /// <summary>
        /// Demonstrates all available alarm sounds
        /// </summary>
        public static async Task RunAllAlarmExamples()
        {
            Console.WriteLine("CalibrationSaaS Alarm Sound Examples");
            Console.WriteLine("====================================");

            // Example 1: Simple system beep
            Console.WriteLine("\n1. Playing system beep...");
            AlarmSoundUtilities.PlaySystemBeep();
            await Task.Delay(1000);

            // Example 2: Custom frequency beep
            Console.WriteLine("\n2. Playing custom beep (1000Hz, 500ms)...");
            AlarmSoundUtilities.PlayCustomBeep(1000, 500);
            await Task.Delay(1000);

            // Example 3: System alert sound
            Console.WriteLine("\n3. Playing system alert sound...");
            AlarmSoundUtilities.PlaySystemAlert();
            await Task.Delay(1000);

            // Example 4: System warning sound
            Console.WriteLine("\n4. Playing system warning sound...");
            AlarmSoundUtilities.PlaySystemWarning();
            await Task.Delay(1000);

            // Example 5: Calibration-specific alarm
            Console.WriteLine("\n5. Playing calibration alarm...");
            await AlarmSoundUtilities.PlayCalibrationAlarm();
            await Task.Delay(1000);

            // Example 6: Urgent alarm
            Console.WriteLine("\n6. Playing urgent alarm...");
            await AlarmSoundUtilities.PlayUrgentAlarm();
            await Task.Delay(1000);

            // Example 7: Completion sound
            Console.WriteLine("\n7. Playing completion sound...");
            await AlarmSoundUtilities.PlayCompletionSound();
            await Task.Delay(1000);

            // Example 8: Error sound
            Console.WriteLine("\n8. Playing error sound...");
            await AlarmSoundUtilities.PlayErrorSound();
            await Task.Delay(1000);

            // Example 9: Alarm pattern
            Console.WriteLine("\n9. Playing alarm pattern (3 beeps)...");
            await AlarmSoundUtilities.PlayAlarmPattern(3, 1200, 300, 200);

            Console.WriteLine("\nAll alarm examples completed!");
        }

        /// <summary>
        /// Example of using alarms in calibration scenarios
        /// </summary>
        public static async Task CalibrationScenarioExamples()
        {
            Console.WriteLine("\nCalibration Scenario Examples");
            Console.WriteLine("=============================");

            // Scenario 1: Calibration completed successfully
            Console.WriteLine("\nScenario 1: Calibration completed successfully");
            await AlarmManager.PlayAlarm(AlarmType.CompletionSound);
            await Task.Delay(1000);

            // Scenario 2: Calibration error detected
            Console.WriteLine("\nScenario 2: Calibration error detected");
            await AlarmManager.PlayAlarm(AlarmType.ErrorSound);
            await Task.Delay(1000);

            // Scenario 3: Urgent calibration required
            Console.WriteLine("\nScenario 3: Urgent calibration required");
            await AlarmManager.PlayAlarm(AlarmType.UrgentAlarm);
            await Task.Delay(1000);

            // Scenario 4: General calibration alert
            Console.WriteLine("\nScenario 4: General calibration alert");
            await AlarmManager.PlayAlarm(AlarmType.CalibrationAlarm);
            await Task.Delay(1000);

            // Scenario 5: Warning - calibration due soon
            Console.WriteLine("\nScenario 5: Warning - calibration due soon");
            await AlarmManager.PlayAlarm(AlarmType.SystemWarning);
            await Task.Delay(1000);

            Console.WriteLine("\nCalibration scenario examples completed!");
        }

        /// <summary>
        /// Interactive alarm tester
        /// </summary>
        public static async Task InteractiveAlarmTester()
        {
            Console.WriteLine("\nInteractive Alarm Tester");
            Console.WriteLine("========================");
            Console.WriteLine("Choose an alarm to play:");
            Console.WriteLine("1. System Beep");
            Console.WriteLine("2. System Alert");
            Console.WriteLine("3. System Warning");
            Console.WriteLine("4. Calibration Alarm");
            Console.WriteLine("5. Urgent Alarm");
            Console.WriteLine("6. Completion Sound");
            Console.WriteLine("7. Error Sound");
            Console.WriteLine("8. Custom Beep");
            Console.WriteLine("0. Exit");

            while (true)
            {
                Console.Write("\nEnter your choice (0-8): ");
                var input = Console.ReadLine();

                if (input == "0")
                {
                    Console.WriteLine("Exiting alarm tester...");
                    break;
                }

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Playing System Beep...");
                        await AlarmManager.PlayAlarm(AlarmType.SystemBeep);
                        break;
                    case "2":
                        Console.WriteLine("Playing System Alert...");
                        await AlarmManager.PlayAlarm(AlarmType.SystemAlert);
                        break;
                    case "3":
                        Console.WriteLine("Playing System Warning...");
                        await AlarmManager.PlayAlarm(AlarmType.SystemWarning);
                        break;
                    case "4":
                        Console.WriteLine("Playing Calibration Alarm...");
                        await AlarmManager.PlayAlarm(AlarmType.CalibrationAlarm);
                        break;
                    case "5":
                        Console.WriteLine("Playing Urgent Alarm...");
                        await AlarmManager.PlayAlarm(AlarmType.UrgentAlarm);
                        break;
                    case "6":
                        Console.WriteLine("Playing Completion Sound...");
                        await AlarmManager.PlayAlarm(AlarmType.CompletionSound);
                        break;
                    case "7":
                        Console.WriteLine("Playing Error Sound...");
                        await AlarmManager.PlayAlarm(AlarmType.ErrorSound);
                        break;
                    case "8":
                        Console.Write("Enter frequency (Hz, 37-32767): ");
                        if (int.TryParse(Console.ReadLine(), out int freq) && freq >= 37 && freq <= 32767)
                        {
                            Console.Write("Enter duration (ms): ");
                            if (int.TryParse(Console.ReadLine(), out int dur) && dur > 0)
                            {
                                Console.WriteLine($"Playing Custom Beep ({freq}Hz, {dur}ms)...");
                                await AlarmManager.PlayAlarm(AlarmType.CustomBeep, freq, dur);
                            }
                            else
                            {
                                Console.WriteLine("Invalid duration. Using default (500ms).");
                                await AlarmManager.PlayAlarm(AlarmType.CustomBeep, freq, 500);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid frequency. Using default (800Hz, 500ms).");
                            await AlarmManager.PlayAlarm(AlarmType.CustomBeep);
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 0-8.");
                        break;
                }

                await Task.Delay(500); // Small delay between sounds
            }
        }

        /// <summary>
        /// Example of integrating alarms with CalibrationSaaS components
        /// </summary>
        public static class CalibrationAlarmIntegration
        {
            /// <summary>
            /// Simulates a calibration process with sound notifications
            /// </summary>
            public static async Task SimulateCalibrationWithAlarms()
            {
                Console.WriteLine("\nSimulating Calibration Process with Alarms");
                Console.WriteLine("==========================================");

                // Start calibration
                Console.WriteLine("Starting calibration process...");
                await AlarmManager.PlayAlarm(AlarmType.SystemInfo);
                await Task.Delay(1000);

                // Calibration in progress
                Console.WriteLine("Calibration in progress...");
                await Task.Delay(2000);

                // Simulate different outcomes
                var random = new Random();
                var outcome = random.Next(1, 4);

                switch (outcome)
                {
                    case 1:
                        Console.WriteLine("✓ Calibration completed successfully!");
                        await AlarmManager.PlayAlarm(AlarmType.CompletionSound);
                        break;
                    case 2:
                        Console.WriteLine("⚠ Calibration completed with warnings!");
                        await AlarmManager.PlayAlarm(AlarmType.SystemWarning);
                        break;
                    case 3:
                        Console.WriteLine("✗ Calibration failed!");
                        await AlarmManager.PlayAlarm(AlarmType.ErrorSound);
                        break;
                }
            }

            /// <summary>
            /// Example of equipment status monitoring with alarms
            /// </summary>
            public static async Task MonitorEquipmentStatus()
            {
                Console.WriteLine("\nMonitoring Equipment Status");
                Console.WriteLine("===========================");

                var equipmentStatuses = new[]
                {
                    "Equipment A: Normal operation",
                    "Equipment B: Calibration due in 7 days",
                    "Equipment C: URGENT - Calibration overdue!",
                    "Equipment D: Error detected",
                    "Equipment E: Calibration completed"
                };

                foreach (var status in equipmentStatuses)
                {
                    Console.WriteLine(status);

                    if (status.Contains("Normal"))
                    {
                        // No alarm for normal operation
                    }
                    else if (status.Contains("due in"))
                    {
                        await AlarmManager.PlayAlarm(AlarmType.SystemWarning);
                    }
                    else if (status.Contains("URGENT"))
                    {
                        await AlarmManager.PlayAlarm(AlarmType.UrgentAlarm);
                    }
                    else if (status.Contains("Error"))
                    {
                        await AlarmManager.PlayAlarm(AlarmType.ErrorSound);
                    }
                    else if (status.Contains("completed"))
                    {
                        await AlarmManager.PlayAlarm(AlarmType.CompletionSound);
                    }

                    await Task.Delay(1500);
                }
            }
        }

        /// <summary>
        /// Main method to run all examples
        /// </summary>
        public static async Task Main(string[] args)
        {
            Console.WriteLine("CalibrationSaaS Alarm Sound System");
            Console.WriteLine("===================================");

            try
            {
                // Run all examples
                await RunAllAlarmExamples();
                await CalibrationScenarioExamples();
                await CalibrationAlarmIntegration.SimulateCalibrationWithAlarms();
                await CalibrationAlarmIntegration.MonitorEquipmentStatus();

                // Interactive tester (uncomment to use)
                // await InteractiveAlarmTester();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running alarm examples: {ex.Message}");
                await AlarmManager.PlayAlarm(AlarmType.ErrorSound);
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
