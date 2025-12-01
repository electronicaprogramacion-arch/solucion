using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace CalibrationSaaS.Domain.Aggregates.Entities
{

    [DataContract]
    public class EquipmentTypeGroup : IGeneric
    {

        public EquipmentTypeGroup()
        {
            
        }

        [Key]
        [Required]
        [DataMember(Order = 1)]
        public int EquipmentTypeGroupID { get; set; }

       
        [Required]
        [DataMember(Order = 2)]
        public string Name { get; set; }

       
        [DataMember(Order = 3)]
        public virtual ICollection<EquipmentType> EquipmentTypes { get; set; }

        //[Required]
        //[DataMember(Order = 4)]
        //public string EquipmentTypeCollectionJSON { get; set; }


        [Required]
        [DataMember(Order = 4)]
        public bool IsDelete { get; set; }


        
        [DataMember(Order = 5)]
        public string Children { get; set; }

        private List<int> _ChildrenList;

        [NotMapped]       
        public List<int> ChildrenList { get 
            { 
                return _ChildrenList; 
            
            }}

    }


    [DataContract]
    public class EquipmentType : IGeneric, INote
    {
        public EquipmentType()
        {
            //PieceOfEquipment = new HashSet<PieceOfEquipment>();
        }

        [Key]
        [Required]       
        [DataMember(Order = 1)]
        public int EquipmentTypeID { get; set; }

        [DataMember(Order = 2)]        
        [Required(AllowEmptyStrings =false,ErrorMessage = "Required")]
        [StringLength(500, ErrorMessage = "Name is too long.")]
        public string Name { get; set; }


        [DataMember(Order = 3)]
        public bool IsEnabled { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<PieceOfEquipment> PieceOfEquipment { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<EquipmentTemplate> EquipmentTemplates { get; set; }
        [DataMember(Order = 5)]
        [NotMapped]
        public string Description { get; set; } = "";

        [DataMember(Order = 6)]       
        public bool IsBalance { get; set; } 

        [DataMember(Order = 7)]       
        public bool HasIndicator { get; set; } 

        [DataMember(Order = 8)]       
        public bool HasPeripheral { get; set; } 

         [DataMember(Order = 9)]       
        public bool HasAccredited { get; set; } 

        [DataMember(Order = 10)]   
        public bool HasClass { get; set; } 

         [DataMember(Order = 11)]   
        public bool HasTestpoint { get; set; } 

        
         [DataMember(Order = 12)]   
        public bool HasTolerance { get; set; } 


         [DataMember(Order = 13)]   
        public bool HasCertificate { get; set; } 

         [DataMember(Order = 14)]   
        public bool HasStandard { get; set; } 

        [DataMember(Order = 15)]   
        public int CalibrationTypeID { get; set; }

        
        [DataMember(Order = 16)]   
        public  virtual CalibrationType CalibrationType { get; set; } 

        [DataMember(Order = 17)]   
        public bool HasStandardRange { get; set; } 

        [DataMember(Order = 18)]   
        public bool HasUncertanity { get; set; } 

         [DataMember(Order = 19)]   
        public bool HasResolution { get; set; } 

         [DataMember(Order = 20)]   
        public bool HasCapacity { get; set; } 

         [DataMember(Order = 21)]   
        public bool HasWorkOrderDetail { get; set; }

       

        [DataMember(Order = 23)]
        public bool HasScales { get; set; }

        [DataMember(Order = 24)]
        public bool HardnessTestBlockInformation { get; set; }

       
        [DataMember(Order = 25)]
        public virtual ICollection<Note> Notes { get; set; }

        /// <summary>
        /// say if a POE no standard is load like standard
        /// </summary>
        [DataMember(Order = 26)]
        public bool UseWorkOrderDetailStandard { get; set; }

        //TODO
        //[DataMember(Order = 27)]

        //public bool HasWorkOrdeDetailStandard { get; set; }


        [DataMember(Order = 28)]
        public bool HasStandardConfiguration { get; set; }


        [DataMember(Order = 29)]
        public bool DynamicConfiguration { get; set; }


        [DataMember(Order = 30)]
        public bool DynamicConfiguration2 { get; set; }


        [DataMember(Order = 31)]
        public string CalibrationInstrucctions { get; set; }


        [DataMember(Order = 32)]
        public string StandardComponent { get; set; }


        [DataMember(Order = 33)]
        public bool HasRepeateabilityAndEccentricity { get; set; }


        [DataMember(Order = 34)]
        public bool HasReturnToZero { get; set; }


        [DataMember(Order = 35)]
        public string DefaultCustomer { get; set; }

        [DataMember(Order = 36)]
        public string JSONConfiguration { get; set; }

        [DataMember(Order = 37)]
        public string ETCalculatedAlgorithm { get; set; }

        [DataMember(Order = 38)]
        public int? EquipmentTypeGroupID { get; set; }


        [NotMapped]
        [DataMember(Order = 39)]
        public virtual EquipmentTypeGroup? EquipmentTypeGroup { get; set; }

        [DataMember(Order = 40)]
        public bool? IsBalanceAndScale { get; set; } = false;

    }
}
