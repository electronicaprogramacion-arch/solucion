using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.Domain.ReportViewModels
{
    public class UncertaintyViewModel
    {
        public int Id { get; set; }
        public double WeigthApplied { get; set; }
        public string EquipmentTypeId { get; set; }
        public string WorkOrderDetail { get; set; }
        public string CalibrationDate { get; set; }
        public List<UncertaintyOfRerence> UncertaintyOfRerences { get; set; }
        public List<CornerloadEccentricity> CornerloadEccentricities { get; set; }
        public List<RepeatabilityOfBalance> RepeatabilityOfBalances { get; set; }
        public EnvironmentalFactors EnvironmentalFactors { get; set; }
        public Resolution Resolutions { get; set; }
        public Totales Totales { get; set; }
        public string cmcValuesReplace { get; set; }
        public DriftStability DriftStabilities { get; set; }
        public AirBuoyancy AirBuoyancies { get; set; }
        public TemperatureEffect TemperatureEffects { get; set; }
        public List<UncertaintyMPE> UncertaintyMPEs { get; set; }
        public List<ContributorDynamic> ContributorsDynamic { get; set; }
    }
    public class ContributorDynamic
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double? Divisor { get; set; }
        public double? Quotient { get; set; }
        public string Square { get; set; }
        public string Contributor { get; set; }
    }
    public class UncertaintyOfRerence
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double Divisor { get; set; }
        public double? Quotient { get; set; }
        public string Square { get; set; }
        public string Variance { get; set; }
        public string Contributor { get; set; }
        public string Degrees { get; set; }
        public string df { get; set; }
        public string PercentContribution { get; set; }
        public string Comments { get; set; }

    }
    public class UncertaintyMPE
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double Divisor { get; set; }
        public double? Quotient { get; set; }
        public string Square { get; set; }
        public string Variance { get; set; }
        public string Contributor { get; set; }
        public string Degrees { get; set; }
        public string df { get; set; }
        public string PercentContribution { get; set; }
        public string Comments { get; set; }

    }

    public class RepeatabilityOfBalance
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public int? Divisor { get; set; }
        public double? Quotient { get; set; }
        public string Square { get; set; }
        public string Contributor { get; set; }
    }
    public class CornerloadEccentricity
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double Divisor { get; set; }
        public double? Quotient { get; set; }
        public string  Square { get; set; }
        public string Contributor { get; set; }
    }
    public class EnvironmentalFactors
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double Divisor { get; set; }
        public double? Quotient { get; set; }
        public string Square { get; set; }
        public string Contributor { get; set; }
    }
    public class Resolution
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double Divisor { get; set; }
        public double? Quotient { get; set; }
        public string Square { get; set; }
        public string Contributor { get; set; }
    }
    public class DriftStability 
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double Divisor { get; set; }
        public double? Quotient { get; set; }
        public string  Square { get; set; }
        public string Contributor { get; set; }
    }
    public class AirBuoyancy
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double Divisor { get; set; }
        public double? Quotient { get; set; }
        public string Square { get; set; }
        public string Contributor { get; set; }
    }
    public class TemperatureEffect
    {
        public int Id { get; set; }
        public string Weigth { get; set; }
        public string Magnitude { get; set; }
        public string Units { get; set; }
        public string Type { get; set; }
        public string Distribution { get; set; }
        public double Divisor { get; set; }
        public double? Quotient { get; set; }
        public string Square { get; set; }
        public string Contributor { get; set; }
    }

    public class Totales
    {
        public string SumOfSquares { get; set; }
        public double TotalUncerainty { get; set; }
        public double ExpandedUncertainty { get; set; }
        public double StdUnc { get; set; }
        public double Veff { get; set; }
        public double k { get; set; }
    }
}
