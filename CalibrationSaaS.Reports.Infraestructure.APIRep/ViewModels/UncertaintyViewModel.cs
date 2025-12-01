using System;
using System.Collections.Generic;
using System.Linq;
using CalibrationSaaS.Domain.Aggregates.Entities;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.ViewModels
{
    public class UncertaintyViewModel
    {
        public int Id { get; set; }
        public string WorkOrder { get; set; }
        public string WorkOrderDetailId { get; set; }
        public double FS { get; set; }
        public double Nominal { get; set; }
        public string UnitOfMeasure { get; set; }
        public string? Due { get; set; }
        public string? Address { get; set; }
        public string? Model { get; set; }
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public int CalibrationSubtypeId { get; set; }
        public PieceOfEquipment pieceOfEquipment { get; set; }
        public List<Contributor> Contributors { get; set; }
        public List<Contributor> ContributorsRep { get; set; }
        public List<Contributor> ContributorsASTM { get; set; }
        public Totales totales { get; set; }
        public Totales totalesAstm { get; set; }
        public bool IncludeASTM { get; set; }
        public string Title { get; set; }
        public double UncertaintyNew { get; set; }
        public string cmcValuesReplace { get; set; }
        
    }

    public class Contributor
    {
        public string ControlNumber { get; set; }
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
    }
    

    public class Totales
    {
        public string SumOfSquares { get; set; }
        public string TotalUncerainty { get; set; }
        public string ExpandedUncertainty { get; set; }
        public string cmcValuesReplace { get; set; }


    }

    
}