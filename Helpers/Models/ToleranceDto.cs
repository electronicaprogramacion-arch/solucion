using System.Runtime.Serialization;
namespace Helpers.Models
{
    [DataContract]
    public class ToleranceDto
    {
        [DataMember(Order = 1)]
        public int ToleranceTypeID { get; set; }

        [DataMember(Order = 2)]
        public bool? FullScale { get; set; }

        [DataMember(Order = 3)]
        public double AccuracyPercentage { get; set; }

        [DataMember(Order = 4)]
        public double ToleranceFixedValue { get; set; }

        [DataMember(Order = 5)]
        public double ResolutionValue { get; set; }

        [DataMember(Order = 6)]
        public double MaxValue { get; set; }

        [DataMember(Order = 7)]
        public double Resolution { get; set; }
    }
}
