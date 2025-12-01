using Blazor.IndexedDB.Framework;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Aggregates.Views;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    public class AssetsServiceGRPC : IDisposable, IAssetsServices<CallContext>
    {

        private CallContext context;

        private readonly IAssetsServices<CallContext> service;


        private readonly dynamic DbFactory;

        public AssetsServiceGRPC(IAssetsServices<CallContext> _service)
        {
            service = _service;
            context = new CallOptions();
        }

public AssetsServiceGRPC(Func<dynamic, Application.Services.IAssetsServices<CallContext>> _service, dynamic _DbFactor)
        {
            DbFactory = _DbFactor;
            service = _service(DbFactory);
            context = new CallOptions();
        }

        public AssetsServiceGRPC(IAssetsServices<CallContext> _service, CallContext _context)
        {
            service = _service;
            context = new CallOptions();
        }

        public void Dispose()
        {

        }

        public async ValueTask<WorkOrder> CreateWorkOrder(WorkOrder workOrderDTO, CallContext context = default)
        {
            return await service.CreateWorkOrder(workOrderDTO, context);
        }

        public async ValueTask<ResultSet<WorkOrder>> GetWorkOrders(Pagination<WorkOrder> pagination, CallContext _context = default)
        {
            return await service.GetWorkOrders(pagination, context);
        }

        public async ValueTask<WorkOrder> DeleteWorkOrder(WorkOrder workOrder, CallContext _context = default)
        {
            return await service.DeleteWorkOrder(workOrder, context);
        }

        public ValueTask<WorkOrder> UpdateWorkOrder(WorkOrder DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<WorkOrder> GetWorkOrderByID(WorkOrder workOrder, CallContext _context = default)
        {
            return await service.GetWorkOrderByID(workOrder, context);
        }

        public async ValueTask<AddressResultSet> GetAddressByCustomerId(Customer customerId = default)
        {
            return await service.GetAddressByCustomerId(customerId);
        }

        public ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByCustomerId(Customer customerId = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<UserResultSet> GetUsersByCustomerId(Customer customerId = default)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<ContactResultSet> GetContactsByCustomerId(Customer customerId)
        {
            return await service.GetContactsByCustomerId(customerId);
        }

        public async ValueTask<UserResultSet> GetUsers()
        {
            return await service.GetUsers();

        }

        public async ValueTask<CalibrationTypeResultSet> GetCalibrationType()
        {

            return await service.GetCalibrationType();


        }

        
        //public ValueTask<Certificate> CreateCertificate(Certificate DTO)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ICollection<CertificatePoE>> GetCertificateXPoE(PieceOfEquipment DTO)
        {
            return await service.GetCertificateXPoE(DTO);
        }

        public async Task<ICollection<Certificate>> GetCertificateXWod(WorkOrderDetail DTO)
        {
            return await service.GetCertificateXWod(DTO);
        }

        public ValueTask<WeightSet> CreateWeightSet(WeightSet DTO)
        {
            throw new NotImplementedException();
        }

        //public async Task<ICollection<WeightSet>> GetWeightSetXPoE(PieceOfEquipment DTO)
        //{
        //    List<WeightSet> result = new List<WeightSet>();
        //    return result;
        //}

        public ValueTask<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByStatus(Pagination<WorkOrderDetailByStatus> pagination, CallContext _context = default)
        {
            return service.GetWorkOrderDetailByStatus(pagination, context);
        }

        public ValueTask<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByEquipment(Pagination<WorkOrderDetailByStatus> pagination, CallContext _context = default)
        {
            return service.GetWorkOrderDetailByEquipment(pagination, context);
        }

        public ValueTask<ResultSet<WorkOrderDetailByCustomer>> GetWorkOrderDetailByCustomer(Pagination<WorkOrderDetailByCustomer> pagination, CallContext _context = default)
        {
            return service.GetWorkOrderDetailByCustomer(pagination, context);
        }

        public async ValueTask<WeightSet> DeleteWeightSet(WeightSet DTO, CallContext _context = default)
        {
            var result = await service.DeleteWeightSet(DTO, context);

            return result;
        }

        public async ValueTask<ResultSet<WorkOrder>> GetWorkOrdersOff(Pagination<WorkOrder> pagination, CallContext _context = default)
        {
            var result = await service.GetWorkOrdersOff(pagination, context);

            return result;
        }


        public async ValueTask<IEnumerable<Domain.Aggregates.Entities.WOStatus>> GetStatus(CallContext context = default)
        {
            return await service.GetStatus(context);
        }
    }
}
