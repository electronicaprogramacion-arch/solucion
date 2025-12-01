using BlazorInputFile;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using CalibrationSaaS.Application.UseCases;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class AssetsServices : IAssetsServices<CallContext>
    {
        private readonly AssetsUseCases assetsLogic;

        private readonly ValidatorHelper modelValidator;
        private readonly ILogger _logger;
        public AssetsServices(AssetsUseCases assetsLogic,ValidatorHelper _modelValidator)
        {
            this.assetsLogic = assetsLogic;

            this.modelValidator = _modelValidator;

        }

        #region WorkOrder
        public async ValueTask<WorkOrder> CreateWorkOrder(WorkOrder workOrderDTO, CallContext context)
        {

             var user = context.ServerCallContext.GetHttpContext().User;

            var aa= user.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault();

            var result = await assetsLogic.CreateWorkOrder(workOrderDTO,aa?.Value);

            //return  result;

            if (result.WorkOrderDetails != null)
            {
                foreach (var item in result.WorkOrderDetails)
                {
                    item.WorkOder = null;
                    //item.PieceOfEquipment.WorOrderDetails = null;
                    //item.PieceOfEquipment.EquipmentTemplate = null;

                }

            }

            if (result.Users != null)
            {
                foreach (var item in workOrderDTO.Users)
                {
                    item.WorkOrderDetails = null;

                }
            }


            //foreach (var item in result?.UserWorkOrders)
            //{
            //    item.User.UserWorkOrders = null;
            //    item.WorkOrder.UserWorkOrders = null;
            //}

            //result.UserWorkOrders = null;

            result.WorkOrderDetails.ToList().ForEach(item =>
            {
                item.WorkOder = null;
                if (item.PieceOfEquipment != null)
                {
                    item.PieceOfEquipment.WorOrderDetails = null;
                }


            });

            return result; 
        }

        public async ValueTask<ResultSet<WorkOrder>> GetWorkOrders(Pagination<WorkOrder> pagination,CallContext context)
        {
            var result = await assetsLogic.GeWorkOrder(pagination);
            return result;//new WorkOrderResultSet { WorkOrders = result.ToList() };
        }


        public async ValueTask<ResultSet<WorkOrder>> GetWorkOrdersOff(Pagination<WorkOrder> pagination, CallContext context)
        {
            var result = await assetsLogic.GeWorkOrderOff(pagination);
            return result;//new WorkOrderResultSet { WorkOrders = result.ToList() };
        }



        public async ValueTask<WorkOrder> DeleteWorkOrder(WorkOrder workOrder, CallContext context)
        {
            var result = await assetsLogic.DeleteWorkOrder(workOrder.WorkOrderId);
            return result;
        }

        public async ValueTask<WorkOrder> UpdateWorkOrder(WorkOrder DTO, CallContext context)
        {
            var result = await this.assetsLogic.UpdateWorkOrder(DTO);

            return result;
        }

        public async ValueTask<WorkOrder> GetWorkOrderByID(WorkOrder workOrder, CallContext context)
        {


            var result = await assetsLogic.GetWorkOrderByID(workOrder);

            result.WorkOrderDetails.ToList().ForEach(item =>
            {
                item.WorkOder = null;
                if(item.PieceOfEquipment !=null)
                {
                    item.PieceOfEquipment.WorOrderDetails = null;
                }
              

            });


            return result;
        }

        #endregion

        public async ValueTask<AddressResultSet> GetAddressByCustomerId(Customer customerId)
        {
            var result = await assetsLogic.GetAddressByCustomerId(customerId.CustomerID);

            //List<Address> result = new List<Address>
            //{
            //    new Address
            //    {
            //       AddressId = 1,
            //       StreetAddress1 = "Anaheim West Tower 201 S. Anaheim Blvd",
            //       City = "Anaheim",
            //       State = "California",
            //       ZipCode = "92800",
            //       AggregateID = 1

            //    }

            //    //new Address
            //    //{
            //    //   AddressId = 2,
            //    //   StreetAddress1 = "201 E Broadway, Anaheim",
            //    //   City = "Anaheim",
            //    //   State = "California",
            //    //   ZipCode = "92899",
            //    //   AggregateID = 1
            //    //},
            //    //new Address
            //    //{
            //    //   AddressId = 3,
            //    //   StreetAddress1 = "1313 Dr, Anaheim",
            //    //   City = "Anaheim",
            //    //   State = "California",
            //    //   ZipCode = "92802"

            //    //}
            //};

            if (result != null)
            {
                return new AddressResultSet { Addresses = result.ToList() };
            }
            else
            {
                return new AddressResultSet { Addresses = new List<Address>() };
            }

        }


        public async ValueTask<ContactResultSet> GetContactsByCustomerId(Customer customerId)
        {
            var result = await assetsLogic.GetContactsByCustomerId(customerId.CustomerID);
            return new ContactResultSet { Contacts = result.ToList() };

        }

        public async ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByCustomerId(Customer customerId)
        {
            var result = await assetsLogic.GetPieceOfEquipmentByCustomerId(customerId.CustomerID);
            return new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };
        }
        

        public async ValueTask<UserResultSet> GetUsersByCustomerId(Customer customerId)
        {
            List<User> result = new List<User>
            {
                new User
                {
                 UserID = 567,
                 Name = "Greg",
                 UserName = "Greg"
                
                },
                new User
                {
                 UserID = 876,
                 Name = "Joseph",
                 UserName = "Joseph",
              
                }

            };

            return new UserResultSet {Users = result };
        }


        public async ValueTask<UserResultSet> GetUsers()
        {
            var result = await assetsLogic.GetUsers();
            return new UserResultSet { Users = result.ToList() };
        }

        //public async ValueTask<Certificate> CreateCertificate(Certificate DTO)
        //{

        //    return new Certificate
        //    {
        //        //PieceOfEquipmentId = 7777,
        //        AffectDueDate = true,
        //        CalibrationDate = DateTime.Now,
        //        CertificateNumber = "123456789-ABCDEFGHI",
        //        Company = "test3",
        //        DueDate = DateTime.Now.AddDays(60)

        //    };

        //}

        public async Task<ICollection<CertificatePoE>> GetCertificateXPoE(PieceOfEquipment DTO)
        {
           var result = await  assetsLogic.GetCertificateXPoE(DTO);

            return result;


            //List<CertificatePoE> result = new List<CertificatePoE>
            //{
            //    new CertificatePoE
            //    {
            //     //PieceOfEquipmentId = 7777,
            //     AffectDueDate = true,
            //     CalibrationDate = DateTime.Now,
            //     CertificateNumber = "123456789-ABCDEFGHI",
            //     Company = "test",
            //     DueDate = DateTime.Now.AddDays(60)
                 
            //    },
            //   new CertificatePoE
            //    {
            //     //PieceOfEquipmentId = 7777,
            //     AffectDueDate = true,
            //     CalibrationDate = DateTime.Now,
            //     CertificateNumber = "123456789-JKLMNOPQ",
            //     Company = "Test2",
            //     DueDate = DateTime.Now.AddDays(40)

            //    },

            //};

            //return result;
        }


        public async ValueTask<WeightSet> CreateWeightSet(WeightSet DTO)
        {

            return new WeightSet();
            //{
            //    PieceOfEquipmentId = 7777,
            //    WeightValue = 50,
            //    UnitofMeasureID = "1",
            //    Note = "070"
            //};

        }

        public async Task<ICollection<WeightSet>> GetWeightSetXPoE(PieceOfEquipment DTO)
        {
            List<WeightSet> result = new List<WeightSet>
            {
            //   new WeightSet
            //    {
            //        PieceOfEquipmentId = 7777,
            //        WeightValue = 50,
            //        UnitofMeasureID = "1",
            //        Note = "070"
            //    },
            //   new WeightSet
            //    {
            //        PieceOfEquipmentId = 7777,
            //        WeightValue = 50,
            //        UnitofMeasureID = "2",
            //        Note = "071"
            //    },
            //   new WeightSet
            //    {
            //        PieceOfEquipmentId = 7777,
            //        WeightValue = 50,
            //        UnitofMeasureID = "1",
            //        Note = "072"
            //    }

            };
            return result; //new WeightSetResultSet { WeightSets = result };
        }

        public async ValueTask<CalibrationTypeResultSet> GetCalibrationType()
        {
           
           
            var result1 = await assetsLogic.GetCalibrationType();
           


            return new CalibrationTypeResultSet { CalibrationTypes = result1.ToList() };
        }

        public async ValueTask<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByStatus(Pagination<WorkOrderDetailByStatus> pagination, CallContext context = default)
        {
            var result = await assetsLogic.GetWorkOrderDetailByStatus(pagination);
            return result;

          
        }

        public async ValueTask<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByEquipment(Pagination<WorkOrderDetailByStatus> pagination, CallContext context = default)
        {
            var result = await assetsLogic.GetWorkOrderDetailByEquipment(pagination);
            return result;


        }

        public async ValueTask<ResultSet<WorkOrderDetailByCustomer>> GetWorkOrderDetailByCustomer(Pagination<WorkOrderDetailByCustomer> pagination, CallContext context = default)
        {
            var result = await assetsLogic.GetWorkOrderDetailByCustomer(pagination);
            return result;


        }

        public async Task<ICollection<Certificate>> GetCertificateXWod(WorkOrderDetail DTO)
        {
            var result = await assetsLogic.GetCertificateByWod(DTO.WorkOrderDetailID);
            
            foreach(var item in result)
            {
                item.WorkOrderDetailSerialized = "";
                //item.WorkOrderDetail = null;

            }
            return result;
        }

        public async ValueTask<WeightSet> DeleteWeightSet(WeightSet DTO, CallContext _context)
        {
            var result = await assetsLogic.DeleteWeightSet(DTO);

            return result;
        }


        public async ValueTask<IEnumerable<Domain.Aggregates.Entities.WOStatus>> GetStatus(CallContext context)
        {
            return await assetsLogic.GetStatus();
        }
    }
}
