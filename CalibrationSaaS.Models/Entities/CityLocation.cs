using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    public class CityLocation :IGeneric
    {
        public CityLocation(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get ; set ; }
    }
}
