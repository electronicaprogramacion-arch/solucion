using System;
using System.Media;
using System.Threading.Tasks;

namespace CalibrationSaaS.Utilities
{
    /// <summary>
    /// Utility class for playing alarm sounds in the CalibrationSaaS application
    /// </summary>
    public static class AlarmSoundUtilities
    {
        /// <summary>
        /// Plays the system beep sound (simple alarm)
        /// </summary>
        public static void PlaySystemBeep()
        {
            try
            {
                Console.Beep();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing system beep: {ex.Message}");
            }
        }

        /// <summary>
        /// Plays a custom frequency beep for specified duration
        /// </summary>
        /// <param name="frequency">Frequency in Hz (37-32767)</param>
        /// <param name="duration">Duration in milliseconds</param>
        public static void PlayCustomBeep(int frequency = 800, int duration = 500)
        {
            try
            {
                Console.Beep(frequency, duration);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing custom beep: {ex.Message}");
            }
        }

        /// <summary>
        /// Plays an alarm pattern with multiple beeps
        /// </summary>
        /// <param name="beepCount">Number of beeps</param>
        /// <param name="frequency">Frequency in Hz</param>
        /// <param name="beepDuration">Duration of each beep in ms</param>
        /// <param name="pauseDuration">Pause between beeps in ms</param>
        public static async Task PlayAlarmPattern(int beepCount = 3, int frequency = 1000, int beepDuration = 300, int pauseDuration = 200)
        {
            try
            {
                for (int i = 0; i < beepCount; i++)
                {
                    Console.Beep(frequency, beepDuration);
                    if (i < beepCount - 1) // Don't pause after the last beep
                    {
                        await Task.Delay(pauseDuration);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing alarm pattern: {ex.Message}");
            }
        }

        /// <summary>
        /// Plays the Windows system sound for critical alerts
        /// </summary>
        public static void PlaySystemAlert()
        {
            try
            {
                SystemSounds.Hand.Play(); // Critical stop sound
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing system alert: {ex.Message}");
            }
        }

        /// <summary>
        /// Plays the Windows system sound for warnings
        /// </summary>
        public static void PlaySystemWarning()
        {
            try
            {
                SystemSounds.Exclamation.Play(); // Warning sound
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing system warning: {ex.Message}");
            }
        }

        /// <summary>
        /// Plays the Windows system sound for information
        /// </summary>
        public static void PlaySystemInfo()
        {
            try
            {
                SystemSounds.Asterisk.Play(); // Information sound
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing system info: {ex.Message}");
            }
        }

        /// <summary>
        /// Plays a calibration-specific alarm sound
        /// High-low pattern suitable for calibration alerts
        /// </summary>
        public static async Task PlayCalibrationAlarm()
        {
            try
            {
                // High-low alarm pattern
                Console.Beep(1200, 400); // High tone
                await Task.Delay(100);
                Console.Beep(800, 400);  // Low tone
                await Task.Delay(100);
                Console.Beep(1200, 400); // High tone
                await Task.Delay(100);
                Console.Beep(800, 400);  // Low tone
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing calibration alarm: {ex.Message}");
            }
        }

        /// <summary>
        /// Plays an urgent alarm with rapid beeps
        /// </summary>
        public static async Task PlayUrgentAlarm()
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.Beep(1500, 150); // Short, high-pitched beeps
                    await Task.Delay(50);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing urgent alarm: {ex.Message}");
            }
        }

        /// <summary>
        /// Plays a completion sound (success)
        /// </summary>
        public static async Task PlayCompletionSound()
        {
            try
            {
                // Ascending tone pattern for success
                Console.Beep(600, 200);
                await Task.Delay(50);
                Console.Beep(800, 200);
                await Task.Delay(50);
                Console.Beep(1000, 300);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing completion sound: {ex.Message}");
            }
        }

        /// <summary>
        /// Plays an error sound
        /// </summary>
        public static async Task PlayErrorSound()
        {
            try
            {
                // Descending tone pattern for errors
                Console.Beep(1000, 200);
                await Task.Delay(50);
                Console.Beep(800, 200);
                await Task.Delay(50);
                Console.Beep(600, 300);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing error sound: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Enum for different alarm types
    /// </summary>
    public enum AlarmType
    {
        SystemBeep,
        SystemAlert,
        SystemWarning,
        SystemInfo,
        CalibrationAlarm,
        UrgentAlarm,
        CompletionSound,
        ErrorSound,
        CustomBeep
    }

    /// <summary>
    /// Main alarm manager class
    /// </summary>
    public static class AlarmManager
    {
        /// <summary>
        /// Plays the specified alarm type
        /// </summary>
        /// <param name="alarmType">Type of alarm to play</param>
        /// <param name="frequency">Frequency for custom beep (ignored for other types)</param>
        /// <param name="duration">Duration for custom beep (ignored for other types)</param>
        public static async Task PlayAlarm(AlarmType alarmType, int frequency = 800, int duration = 500)
        {
            switch (alarmType)
            {
                case AlarmType.SystemBeep:
                    AlarmSoundUtilities.PlaySystemBeep();
                    break;
                case AlarmType.SystemAlert:
                    AlarmSoundUtilities.PlaySystemAlert();
                    break;
                case AlarmType.SystemWarning:
                    AlarmSoundUtilities.PlaySystemWarning();
                    break;
                case AlarmType.SystemInfo:
                    AlarmSoundUtilities.PlaySystemInfo();
                    break;
                case AlarmType.CalibrationAlarm:
                    await AlarmSoundUtilities.PlayCalibrationAlarm();
                    break;
                case AlarmType.UrgentAlarm:
                    await AlarmSoundUtilities.PlayUrgentAlarm();
                    break;
                case AlarmType.CompletionSound:
                    await AlarmSoundUtilities.PlayCompletionSound();
                    break;
                case AlarmType.ErrorSound:
                    await AlarmSoundUtilities.PlayErrorSound();
                    break;
                case AlarmType.CustomBeep:
                    AlarmSoundUtilities.PlayCustomBeep(frequency, duration);
                    break;
                default:
                    AlarmSoundUtilities.PlaySystemBeep();
                    break;
            }
        }
    }
}
