using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class Enviroment
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }

        public ICollection<Condition> Conditions { get; set; }
    }
}