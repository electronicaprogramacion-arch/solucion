using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract] 
    public partial class BalanceAndScaleCalibration: IGenericCalibrationCollection<GenericCalibrationResult2>
    {
        public BalanceAndScaleCalibration()
        {
            //Eccentricity = new HashSet<Eccentricity>();
            //Linearity = new HashSet<Linearity>();
            //Repeatability = new HashSet<Repeatability>();
        }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 1)]
        public int CalibrationTypeId { get; set; } = 1;      

        [Key]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 2)]
        public int WorkOrderDetailId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 3)]
        public int TenantId { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 4)]
        public bool HasLinearity { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 5)]
        public bool HasEccentricity { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 6)]
        public bool HasRepeatability { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 7)]
        public virtual ICollection<Linearity>? Linearities { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Required")]
        
        public virtual ICollection<Force>? Tensions { get; set; }




        [NotMapped]
        [IgnoreDataMember]
        //[DataMember(Order = 8)]
        public virtual WorkOrderDetail WorkOrderDetail { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 8)]
        public virtual Eccentricity Eccentricity { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 9)]
        public virtual Repeatability Repeatability { get; set; }

         [NotMapped]
        [Required(ErrorMessage = "Required")]
         [DataMember(Order = 10)]
        public virtual ICollection<Force> Forces { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "Required")]
        [DataMember(Order = 11)]
        public virtual ICollection<Rockwell> Rockwells { get; set; }

        [NotMapped]      
        [DataMember(Order = 12)]
        public virtual ICollection<GenericCalibration> GenericCalibration { get; set; }


        [NotMapped]
        [DataMember(Order = 13)]
        public virtual List<GenericCalibrationResult2> TestPointResult { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[DataMember(Order = 7)]
        //public virtual ICollection<Linearity> Linearities { get; set; }

        //Environmental Values
        public double EnvironmentalFactor { get; set; }
        public int EnvironmentalUncertaintyValueUOMID { get; set; }
        public string EnvironmentalUncertaintyType { get; set; }
        public string EnvironmentalUncertaintyDistribution { get; set; }
        public double EnvironmentalUncertaintyDivisor { get; set; }
        [NotMapped]
        public double EnvironmentalQuotient
        {
            get
            {
                if (EnvironmentalUncertaintyDivisor == 0)
                {
                    return 0;
                }

                return EnvironmentalFactor / EnvironmentalUncertaintyDivisor;
            }
        }
        [NotMapped]
        public double EnvironmentalSquare
        {
            get
            {
                return EnvironmentalQuotient * EnvironmentalQuotient;
            }
        }
        [NotMapped]
        public double EnvironmentalTotalUncertainty
        {
            get
            {
                return Math.Sqrt(EnvironmentalSquare);
            }
        }
        [NotMapped]
        public double EnvironmentalExpandedUncertainty
        {
            get
            {
                return EnvironmentalTotalUncertainty * 2;
            }
        }

        //Resolution Values
        public double Resolution { get; set; }
        public double ResolutionFormatted { get; set; }
        public double ResolutionNumber { get; set; }
        public int ResolutionUncertaintyValueUOMID { get; set; }
        public string ResolutionUncertaintyType { get; set; }
        public string ResolutionUncertaintyDistribution { get; set; }
        public double ResolutionUncertaintyDivisor { get; set; }
        [NotMapped]
        public double ResolutionQuotient
        {
            get
            {
                if (ResolutionUncertaintyDivisor == 0)
                {
                    return 0;
                }

                return ResolutionFormatted / ResolutionUncertaintyDivisor;


            }
        }
        [NotMapped]
        public double ResolutionSquare
        {
            get
            {

                if (ResolutionUncertaintyDivisor == 0)
                {
                    return 0;
                }

                return ResolutionFormatted / ResolutionUncertaintyDivisor;
            }
        }
        [NotMapped]
        public double ResolutionTotalUncertainty
        {
            get
            {
                return Math.Sqrt(ResolutionSquare);
            }
        }
        [NotMapped]
        public double ResolutionExpandedUncertainty
        {
            get
            {
                return ResolutionTotalUncertainty * 2;
            }
        }

        //public List<GenericCalibrationResult2> BasicCalibrationResult { get ; set ; }
    }
}
