using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    public class FormulaResult()
    {

        public double uncertainty { get; set; }
        public double? uncertaintyCMC { get; set; }

}

    
}
