using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    public static class ConnectionStatusService
    {
        private static bool isOnline = true;
        private static bool isNotgrpc;
        public static string grpcUrl;
        private static bool isConnected;
        private static bool isChangeStatus;
        private static bool isinstall;
        public static event Func<bool, Task> OnChange;
        public static event Action<bool> OnChangeConnection;

      

        public static bool GetChangeStatus()
        {
            return isChangeStatus;
        }

        

        public static bool GetCurrentStatus()
        {

            

            if (isNotgrpc)
            {
                return false;
            }

            return isOnline;
        }

        public static bool GetCurrentConnected()
        {
            return isConnected;
        }

        public static string GetCurrentStatusLabel()
        {
            return isOnline ? "Online" : "Offline";
        }

        public static async Task ChangeConnectionStatus(bool connectionStus)
        {
            if (!isChangeStatus)
            {
                isChangeStatus = true;
            }
            else
            {
                isChangeStatus = false;
            }


            isOnline = connectionStus;

            await OnChange?.Invoke(connectionStus);
        }

        public static void ConnectedChangeStatus( bool isgrpc)
        {
            isNotgrpc = isgrpc;
            
        }
        public static void ConnectedChangeStatus(bool connectionStus, bool install, bool isgrpc)
        {
            isNotgrpc = isgrpc;
            isinstall = install;
            isConnected = connectionStus;
            OnChangeConnection?.Invoke(connectionStus);
        }

        public static async Task<bool> IsInstalled()
        {
            //try
            //{
            //    if(JSRuntime != null)
            //    {
            //         var _displayMode = await JSRuntime.InvokeAsync<bool>("isinstalled"); // ? "Standalone" : "Not standalone";

            //        return _displayMode;
            //    }
            //    return false;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return false;
            //}
            return isinstall;



        }
    }
}
