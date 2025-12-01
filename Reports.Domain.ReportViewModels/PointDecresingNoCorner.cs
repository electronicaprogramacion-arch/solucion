using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class PointDecresingNoCorner
    {
        public int RepeatabilityId { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public double Standard { get; set; }
        public double AsFound { get; set; }
        public double AsLeft { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
       
    }
}
