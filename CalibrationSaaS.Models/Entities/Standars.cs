using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    public class Standard:IGeneric
    {
        public Standard(string name, int _StandarID, string _Field1, string _Field2, string _Field3)
        {
            Name = name;
            StandarID = _StandarID;
            Field1 = _Field1;
            Field2 = _Field2;
            Field3 = _Field3;

        }

        public string Name { get; set; }
        public int StandarID { get; set; }
        public string Description { get ; set ; }

        public string Field1 { get; set; }

        public string Field2 { get; set; }

        public string Field3 { get; set; }


    }
}
