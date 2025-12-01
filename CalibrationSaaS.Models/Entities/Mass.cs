using System.ComponentModel.DataAnnotations;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    public class Mass
    {
        [Key]
        public int MassID { get; set; }

        public int Metric { get; set; }


        public int UnitOfMeasure { get; set; }


        public double? Class1 { get; set; }


        public int? Class1UoM { get; set; }


        public double? Class2 { get; set; }


        public int? Class2UoM { get; set; }


        public double? Class3 { get; set; }


        public int? Class3UoM { get; set; }

        public double? Class4 { get; set; }


        public int? Class4UoM { get; set; }


        public double? Class5 { get; set; }


        public int? Class5UoM { get; set; }

        public double?   Class6 { get; set; }


        public int? Class6UoM { get; set; }


        public double? Class7 { get; set; }


        public int?  Class7UoM { get; set; }



    }
}
