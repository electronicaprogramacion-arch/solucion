using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Helpers.Models
{
    public class CreateModelMin
    {
        public int Version { get; set; }

        /// <summary>
        /// Type or Namespace
        /// </summary>
        public string ClassName { get; set; }

        public int? CalibrationSubtypeID { get; set; }

        public int? CalibrationSubtypeID2 { get; set; }

        public string? NumberTestpoints { get; set; }

        /// <summary>
        /// Put Json of Array of key-value (dictionary) to inicialized the create
        /// </summary>
        //public string MapArray { get; set; }

        /// <summary>
        /// Put Json of Array of key-value (dictionary) to inicialized the create
        /// </summary>
        //public string HeaderMapArray { get; set; }


        //public bool? IsZero { get; set; }

        //public Dictionary<string, string> Map { get; set; }


        //public List<string> Data { get; set; }

        public ICollection<Dictionary<string, object>> Data { get; set; }

        [JsonIgnore]
        public Dictionary<string, object> DefaultData { get; set; }

        public Range Range { get; set; }


        public Header Header { get; set; }

        public string? row0 { get; set; }



        ///////////////////////////////////////////
        /// <summary>
        /// 1- Modal-Form . 2 Modal-Grid . 3 Panel-Form 4-Panel-Grid /////5 Panel-Form-1row 4-Panel-Grid-1row
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 1- one row 2- multiple rows
        /// </summary>
        public string Component { get; set; }


        /// <summary>
        /// 1- one row 2- multiple rows
        /// </summary>
        //public bool? NewDeleteGrid { get; set; }


        /// <summary>
        /// 1- one row 2- multiple rows
        /// </summary>
        //public bool? NewDeleteRow { get; set; }


        public string FilterField { get; set; }


        public string ErrorMessage { get; set; }

        public string Title { get; set; }

        public Dictionary<string, string> Parameter { get; set; }

        public string Ihnerit { get; set; }


        public string Property { get; set; }

        //public bool Mandatory { get; set; } = true;

        public string Operator { get; set; }

        public string Poe { get; set; }

    }

    public class Range
    {
        public double Initial { get; set; }

        public double Final { get; set; }

        public double Step { get; set; }

    }

    public class Header
    {
        public Dictionary<string, object> Data { get; set; }

        [JsonIgnore]
        public Dictionary<string, object> DefaultData { get; set; }


        [JsonIgnore]
        public bool? DefaultHeader { get; set; }


    }





}
