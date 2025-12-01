using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.Serialization;
using System.Text;

namespace Helpers.Controls
{


    [DataContract]
    public class ResultsTable
    {
        [DataMember(Order = 1)]
        public string? WodId { get; set; }

        [DataMember(Order = 2)]
        public string? JsonResults { get; set; }

        [DataMember(Order = 3)]
        public string? FileName { get; set; }

    }

}
