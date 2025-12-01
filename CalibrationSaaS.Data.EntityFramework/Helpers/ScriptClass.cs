using CalibrationSaaS.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.Helpers
{
    public class ScriptClass
    {
        CalibrationSaaSDBContext context = new CalibrationSaaSDBContext(); 
        public bool SetupTestData(string Path)
        {

            bool result = false;
            int line = 0;
            var sql = System.IO.File.ReadAllText(Path);
            string[] commands = sql.Split(new string[] { "GO" + Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            DbConnection conn = context.Database.GetDbConnection(); // Get Database connection
            var initialConnectionState = conn.State;
            try
            {
                if (initialConnectionState != ConnectionState.Open)
                    conn.Open();  // open connection if not already open

                using (DbCommand cmd = conn.CreateCommand())
                {
                    // Iterate the string array and execute each one.
                    foreach (string thisCommand in commands)
                    {
                        cmd.CommandText = @thisCommand;
                        cmd.ExecuteNonQuery();
                        line = line + 1;
                    }
                }
                result=true;
                
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if (initialConnectionState != ConnectionState.Open)
                    conn.Close(); // only close connection if not initially open
            }

            return result;
        }


       

    }
}
