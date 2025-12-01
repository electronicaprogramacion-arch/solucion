using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Runtime.Serialization;
using System.Text;

namespace Helpers.Controls
{


    [DataContract]
    public class InstrumentThread
    {
        [DataMember(Order = 1)]
        public string? InstrumentNumber { get; set; }

        [DataMember(Order = 2)]
        public string? CustomerNumber { get; set; }

        [DataMember(Order = 3)]
        public string? DefaultTestCode { get; set; }

        [DataMember(Order = 4)]
        public string? Department { get; set; }

        [DataMember(Order = 5)]
        public string? InstrumentType { get; set; }
        
        [DataMember(Order = 6)]
        public string? InstrumentDescription { get; set; }
        
        [DataMember(Order = 7)]
        public string? Manufacturer { get; set; }
        
        [DataMember(Order = 8)]
        public string? Model { get; set; }
        
        [DataMember(Order = 9)]
        public string? SizeOrRange { get; set; }
        
        [DataMember(Order = 10)]
        public string? Accuracy { get; set; }

        [DataMember(Order = 11)]
        public string? SerialNumber { get; set; }

        [DataMember(Order = 12)]
        public string? CustomerReference { get; set; }

        [DataMember(Order = 13)]
        public string? CalibrationFrequency { get; set; }

        [DataMember(Order = 14)]
        public string? CalibrationDate { get; set; }

        [DataMember(Order = 15)]
        public string? CalibrationDueDate { get; set; }

        [DataMember(Order = 16)]
        public string? CalibratedBy { get; set; }

        [DataMember(Order = 17)]
        public string? ProcedureName { get; set; }

        [DataMember(Order = 18)]
        public string? ProcedureRevision { get; set; }

        [DataMember(Order = 19)]
        public string? ProcedureAddenda { get; set; }

        [DataMember(Order = 20)]
        public string? UncertaintyCalibration { get; set; }

        [DataMember(Order = 21)]
        public string? UncertaintyKFactor { get; set; }

        [DataMember(Order = 22)]
        public string? Notes { get; set; }

        [DataMember(Order = 23)]    
        public string? LTI_Active { get; set; }

        [DataMember(Order = 24)]
        public string? LTI_InServiceDate { get; set; }

        [DataMember(Order = 25)]
        public string? LTI_RetiredDate { get; set; }

        [DataMember(Order = 26)]
        public string? LTI_Department { get; set; }

        [DataMember(Order = 27)]
        public string? LTI_WorkCenter { get; set; }

        [DataMember(Order = 28)]
        public string? LTI_NIST { get; set; }

        [DataMember(Order = 29)]
        public string? LTI_TYPE { get; set; }

        [DataMember(Order = 30)]
        public string? LTI_SetPlugPitchDiam { get; set; }

        [DataMember(Order = 31)]    
        public string? type { get; set; }

        [DataMember(Order = 32)]
        public string? series { get; set; }

        [DataMember(Order = 33)]
        public string? class_   { get; set; }

        [DataMember(Order = 34)]
        public string? Gage_Function { get; set; }

        [DataMember(Order = 35)]
        public string? Pitch_Diam_Size { get; set; }

        [DataMember(Order = 36)]
        public string? Size_Range { get; set; }

        [DataMember(Order = 37)]
        public string? Description { get; set; }

        [DataMember(Order = 38)]
        public string? Calibrate_With_Type { get; set; }

        [DataMember(Order = 39)]
        public string? NoGo_Pitch_Diam { get; set; }

        [DataMember(Order = 40)]
        public string? TestCode { get; set; }

        [DataMember(Order = 41)]
        public string? Cal_With_Instrument_Number { get; set; }

        [DataMember(Order = 42)]
        public string? Cal_With_Cal_Due_Date	{ get; set; }

        [DataMember(Order = 43)]
        public string? Cal_With_Set_Plug_Pitch_Diameter	{ get; set; }

        [DataMember(Order = 44)]
        public string? WodId { get; set; }

        [DataMember(Order = 45)]
        public string? FileName { get; set; }

        [DataMember(Order = 46)]
        public string? Id { get; set; }

        [DataMember(Order = 47)]
        public string? UrlWebFile  { get; set; }
    }

}
