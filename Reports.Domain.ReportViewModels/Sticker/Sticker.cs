using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class Sticker
    {

        public string Model { get; set; }

        public string Serial { get; set; }
        public string CalibrationDate { get; set; }
        public string CalibrationDue { get; set; }
        public string Tech { get; set; }

        public string Url { get; set; }

        public string Path { get; set; }

        public Byte[] Image { get; set; }

        public string ImageStr { get; set; }

        

    }
}
