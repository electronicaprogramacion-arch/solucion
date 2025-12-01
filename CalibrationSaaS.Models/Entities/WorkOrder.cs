using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace CalibrationSaaS.Domain.Aggregates.Entities
{
    [DataContract]
    public partial class WorkOrder:IGeneric
    {
        public WorkOrder()
        {
            //WorkOrderDetails = new HashSet<WorkOrderDetail>();
          //  Customer = new Customer();
        }
        [Key]
        [Required(ErrorMessage = "Required WorkOrderId")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [DataMember(Order = 1)]
        public int WorkOrderId { get; set; }


        
        [Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [Required(ErrorMessage = "Required CustomerId")]
        
        [DataMember(Order = 2)]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Required WorkOrderDate")]
        [DataMember(Order = 3)]
        public DateTime WorkOrderDate { get; set; }
        [DataMember(Order = 4)]
        public string Description { get; set; }

        [DataMember(Order = 5)]
        public string ControlNumber { get; set; }
        [DataMember(Order = 6)]
        public int TenantId { get; set; }

        //[Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [Required(ErrorMessage = "Required CalibrationType")]
        [DataMember(Order = 7)]
        public int CalibrationType { get; set; }

        [Range(1, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [Required(ErrorMessage = "Required AddressId")]
        [DataMember(Order = 8)]
        public int AddressId { get; set; }

        //[Required(ErrorMessage = "Required UserId")]
        [DataMember(Order = 9)]
        public int ContactId { get; set; }

        [DataMember(Order = 10)]
        public string Name { get; set; }

        [DataMember(Order = 11)]
        public virtual Customer Customer { get; set; }

        [DataMember(Order = 12)]
        public virtual Address Address { get; set; }

        [DataMember(Order = 13)]
        public virtual IList<User> Users { get; set; }

        [DataMember(Order = 14)]
        public virtual ICollection<WorkOrderDetail> WorkOrderDetails { get; set; }


        //[DataMember(Order = 15)]
      //  public virtual ICollection<PieceOfEquipment> PieceOfEquipments { get; set; }
        [DataMember(Order = 15)]
        public virtual ICollection<POE_WorkOrder> POE_WorkOrders { get; set; }

        [NotMapped]
        [DataMember(Order = 16)]
        public virtual ICollection<User_WorkOrder> UserWorkOrders { get; set; }


        [NotMapped]
        [DataMember(Order = 17)]
        public virtual bool IsAccredited { get; set; }

        [DataMember(Order = 18)]
        public  string Invoice { get; set; }

       
         [DataMember(Order = 19)]
        public  string CustomerInvoice { get; set; }

        [NotMapped]
          [DataMember(Order = 20)]
        public  bool IsInternal { get; set; }


        [NotMapped]
        [DataMember(Order = 21)]
        public bool IsOffline { get; set; }

        [DataMember(Order = 22)]
        public string? PurchaseOrder { get; set; }


        [DataMember(Order = 23)]
        public DateTime? ScheduledDate { get; set; }

        [DataMember(Order = 24)]
        public virtual ICollection<WO_Standard> WO_Standard { get; set; }

        [DataMember(Order = 25)]
        public DateTime? CalibrationDate { get; set; }

        [DataMember(Order = 26)]
        public DateTime? NextDueDate { get; set; }

        [DataMember(Order = 27)]
        public virtual ICollection<WO_Weight> WO_Weights { get; set; }



        [DataMember(Order = 28)]
        public int? StatusID { get; set; }

        // Alias properties for compatibility
        //[NotMapped]
        //public int WorkOrderID
        //{
        //    get => WorkOrderId;
        //    set => WorkOrderId = value;
        //}

        //[NotMapped]
        //public int CustomerID
        //{
        //    get => CustomerId;
        //    set => CustomerId = value;
        //}

    }
}
