using System.Collections.Generic;

namespace CalibrationSaaS.Infraestructure.Blazor.Controls
{
    public class ResultSet<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
    }
}
