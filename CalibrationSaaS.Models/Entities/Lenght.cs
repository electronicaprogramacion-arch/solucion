using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    public class Lenght
    {
        [Key]
        public int LenghtID { get; set; }

        public double From { get; set; }

        public double To { get; set; }


        public int UnitOfMeasure { get; set; }


        public double Tolerance { get; set; }


        public int CertificationID { get; set; }





    }
}
