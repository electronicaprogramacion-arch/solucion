using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;

namespace CalibrationSaaS.Domain.Aggregates.ValueObjects
{


    //[DataContract]
    //public class ResultSet<T>
    //{
    //    [DataMember(Order = 1)]
    //    public List<T> List { get; set; }

    //    [DataMember(Order = 2)]
    //    public int Count { get; set; }

    //    [DataMember(Order = 3)]
    //    public int PageTotal { get; set; }

    //    [DataMember(Order = 4)]
    //    public int CurrentPage { get; set; }

    //    [DataMember(Order = 5)]
    //    public string ColumnName { get; set; }

    //    [DataMember(Order = 6)]
    //    public bool SortingAscending { get; set; }

    //    [DataMember(Order = 7)]
    //    public int Shown { get; set; }


    //    [DataMember(Order = 8)]
    //    public bool ClientPagination { get; set; }

    //    [DataMember(Order = 9)]
    //    public string  Message { get; set; }


    //}




    [DataContract]
    public class ReportResultSet
    {
        [DataMember(Order = 1)]
        public string  JsonObject { get; set; }
        [DataMember(Order = 2)]
        public string PdfString { get; set; }

        [DataMember(Order = 3)]
        public string Serial { get; set; }

    }


    [DataContract]
    public class TestPointResultSet
    {
        [DataMember(Order = 1)]
        public List<TestPoint> TestPointList { get; set; }


    }



    [DataContract]
    public class TestPointGroupResultSet
    {
        [DataMember(Order = 1)]
        public List<TestPointGroup> TestPointGroupList { get; set; }



    }


    [DataContract]
    public class UOMResultSet
    {
        [DataMember(Order = 1)]
        public List<UnitOfMeasure> UnitOfMeasureList { get; set; }
    }


    [DataContract]
    public class WorkOrderDetailHistoryResultSet
    {
        [DataMember(Order = 1)]
        public List<WorkDetailHistory> WorkOrderDetailsHistory { get; set; }
    }

    [DataContract]
    public class StatusResultSet
    {
        [DataMember(Order = 1)]
        public List<Status> Status { get; set; }
    }

    [DataContract]
    public class RolResultSet
    {
        [DataMember(Order = 1)]
        public List<Rol> Roles { get; set; }
    }


    [DataContract]
    public class WorkOrderDetailResultSet
    {
        [DataMember(Order = 1)]
        public List<WorkOrderDetail> WorkOrderDetails { get; set; }
    }

   

    [DataContract]
    public class AddressResultSet
    {
        [DataMember(Order = 1)]
        public List<Address> Addresses { get; set; }
    }

    [DataContract]
    public class PhoneNumbersResultSet
    {
        [DataMember(Order = 1)]
        public List<PhoneNumber> PhoneNumbers { get; set; }
    }


    [DataContract]
    public class UserResultSet
    {
        [DataMember(Order = 1)]
        public List<User> Users { get; set; }
    }

    


    [DataContract]
    public class ContactResultSet
    {
        [DataMember(Order = 1)]
        public List<Contact> Contacts { get; set; }
    }


    [DataContract]
    public class EquipmentResultSet
    {
        [DataMember(Order = 1)]
        public List<EquipmentTemplate> EquipmentTemplates { get; set; }
    }



    [DataContract]
    public class ManufacturerResultSet
    {
        [DataMember(Order = 1)]
        public List<Manufacturer> Manufacturers { get; set; }
    }

    [DataContract]
    public class ProcedureResultSet
    {
        [DataMember(Order = 1)]
        public List<Procedure> Procedures { get; set; }
    }


    [DataContract]
    public class EquipmentTypeResultSet
    {
        [DataMember(Order = 1)]
        public List<EquipmentType> EquipmentTypes { get; set; }
    }

    [DataContract]
    public class CustomerResultSet
    {
        [DataMember(Order = 1)]
        public List<Customer> Customers { get; set; }
    }

    [DataContract]
    public class TenantDTO
    {
        [DataMember(Order = 1)]
        public int TenantID { get; set; }
    }

    [DataContract]
    public class CustomerAddressResultSet
    {
        [DataMember(Order = 1)]
        public List<CustomerAddress> CustomerAddress { get; set; }
    }

    [DataContract]
    public class PieceOfEquipmentResultSet
    {
        [DataMember(Order = 1)]
        public List<PieceOfEquipment> PieceOfEquipments { get; set; }

        [DataMember(Order = 2)]

        public int Rows { get; set; }

    }

    [DataContract]
    public class WorkOrderResultSet
    {
        [DataMember(Order = 1)]
        public List<WorkOrder> WorkOrders { get; set; }
    }

    [DataContract]
    public class WeightSetResultSet
    {
        [DataMember(Order = 1)]
        public List<WeightSet> WeightSets { get; set; }
    }



    [DataContract]
    public class CertificateResultSet
    {
        [DataMember(Order = 1)]
        public List<Certificate> Certificates { get; set; }
    }

    [DataContract]
    public class CalibrationTypeResultSet
    {
        [DataMember(Order = 1)]
        public List<CalibrationType> CalibrationTypes { get; set; }
    }

    [DataContract]
    public class WorkOrderDetailByStatusResultSet
    {
        [DataMember(Order = 1)]
        public List<WorkOrderDetailByStatus> WorkOrderDetailByStatuses { get; set; }
    }

    [DataContract]
    public class ISOandASTM
    {
        [DataMember(Order = 1)]
        public List<Force> Forces { get; set; }

        [DataMember(Order = 2)]
        public WorkOrderDetail WorkOrderDetail { get; set; }

        



    }

    




}
