using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;
using System.Linq;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace CalibrationSaaS.Domain.Aggregates.ValueObjects
{
    public class UncertaintyViewModel
    {
        //public int Id { get; set; }
        //public string WorkOrder { get; set; }
        //public string WorkOrderDetailId { get; set; }
        //public string EquipmentTypeId { get; set; }
        //public double FS { get; set; }
        //public double Nominal { get; set; }
        //public string UnitOfMeasure { get; set; }
        //public string? Due { get; set; }
        //public string? Address { get; set; }
        //public string? Model { get; set; }
        //public string? CompanyName { get; set; }
        //public string? City { get; set; }
        //public string? Country { get; set; }
        //public int CalibrationSubtypeId { get; set; }
        //public PieceOfEquipment pieceOfEquipment { get; set; }
        public List<Contributor> Contributors { get; set; }
        //public List<Contributor> ContributorsRep { get; set; }
        public Totales totales { get; set; }
        //public string CalibrationDate { get; set; }
        //public string WorkOrderDetail { get; set; }
        public List<Uncertainty> Uncertainties { get; set; }

        public string Message { get; set; }

        public string cmcValuesReplace { get; set; }
        //public double ExpUncertainty { get; set; }

    }




    public class Contributor
    {
        public string SerialNumber { get; set; }
        public string CalibrationRole { get; set; }
        public string Contributors { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public string Divisor { get; set; }
        public string Quotient { get; set; }
        public string Square { get; set; }
        public string Comment { get; set; }
        public string ControlNumber { get; set; }

        public static Contributor ConvertUncertainty(Uncertainty uncertainty, List<UnitOfMeasure> uomList, List<WeightDistribution> WeightDistributionList, List<WeightType> weightTypes, double? Magnitude = null, double? resolution = null, double? uncertaintyValue = null)
        {

            //var uncertainty2 = uncertainty.UnitOfMeasure; //Convert.ToDouble((string)obj1["Uncertanty"]);

            //var uom = uomList.Where(x => x.UnitOfMeasureID == uncertainty.UnitOfMeasureID).FirstOrDefault();

            //uncertainty.UnitOfMeasure.UncertaintyUnitOfMeasure.Abbreviation;

            var uomUncertainty = uncertainty.UnitOfMeasure.Abbreviation;
            //double resolution = genericCalibration.WeightSets.FirstOrDefault().Resolution;//Convert.ToDouble((string)obj1["StepResol"]);
            string unit = "";

            switch (uomUncertainty)
            {
                case "lb":
                    uomUncertainty = "lbf";
                    break;
                case "kg":
                    uomUncertainty = "kgf";
                    break;
                case "oz":
                    uomUncertainty = "ozf";
                    break;
                case "g":
                    uomUncertainty = "gf";
                    break;


            }



            Contributor contributor = new Contributor();
            var divRes = uncertainty.Divisor;

            double magnitude;

            if (Magnitude.HasValue)
            {
                magnitude = Magnitude.Value;
            }
            else if (uncertainty.Value.HasValue)
            {
                magnitude = uncertainty.Value.Value;
            }
            else
            {
                magnitude = 0;
            }


            double quotient = magnitude / divRes;


            double square = Math.Round(quotient * quotient, 3);
            contributor = new Contributor();
            contributor.Magnitude = Math.Round(magnitude, 3).ToString();

            var distriName = WeightDistributionList.Where(x => x.WeightDistributionID == Convert.ToUInt32(uncertainty.Distribution)).FirstOrDefault();

            if (distriName != null)
            {
                contributor.Distribution = distriName.Name;// "Resolution";
            }
            else
            {
                contributor.Distribution = "";// "Resolution";
            }



            contributor.Quotient = Math.Round(quotient, 3).ToString();
            contributor.Square = Math.Round(square, 3).ToString();
            contributor.Units = uomUncertainty;

            var typex = weightTypes.Where(x => x.WeightTypeID == Convert.ToUInt32(uncertainty.Type)).FirstOrDefault();

            if (typex != null)
            {
                contributor.Type = typex.Name;
            }
            else
            {
                contributor.Type = "";
            }






            contributor.SerialNumber = "";
            contributor.CalibrationRole = "";
            contributor.Comment = uncertainty.Comment;
            contributor.Contributors = uncertainty.Contributors;
            contributor.Divisor = divRes.ToString();
            if (!string.IsNullOrEmpty(uncertainty.PieceOfEquipmentID))
            {
                contributor.ControlNumber = uncertainty.PieceOfEquipmentID;
            }
            else if (uncertainty.EquipmentTemplateID.HasValue && uncertainty.EquipmentTemplateID.Value > 0)
            {
                contributor.ControlNumber = uncertainty.EquipmentTemplateID.ToString();
            }


           
            return contributor;
        }

        public static List<Contributor> ConvertUncertainties(List<Uncertainty> uncertainties,List<UnitOfMeasure> uomList, List<WeightDistribution> WeightDistributionList, List<WeightType> weightTypes, double? Magnitude = null, double? resolution = null, double? uncertaintyValue = null)
        {
            List<Contributor> contributors = new List<Contributor>();


            foreach (var iten in uncertainties)
            {
                Contributor contributor = new Contributor();


                contributor= Contributor.ConvertUncertainty(iten, uomList, WeightDistributionList,  weightTypes, Magnitude, resolution ,  uncertaintyValue );

                contributors.Add( contributor );    


            }

           



            return contributors;    


        }





        }

        public class Totales
    {
        public string SumOfSquares { get; set; }
        public string TotalUncerainty { get; set; }
        public string ExpandedUncertainty { get; set; }
        public string stringCMC { get; set; }
    }
}
