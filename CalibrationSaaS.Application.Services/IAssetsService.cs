using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;

namespace CalibrationSaaS.Application.Services
{

    [ServiceContract(Name = "CalibrationSaaS.Application.Services.AssetsServices")]
    public interface IAssetsServices<T>
    {
        #region WorkOrder

        ValueTask<ResultSet<WorkOrder>> GetWorkOrdersOff(Pagination<WorkOrder> pagination, T context);

        ValueTask<WorkOrder> CreateWorkOrder(WorkOrder workOrderDTO, T context);

        ValueTask<ResultSet<WorkOrder>> GetWorkOrders(Pagination<WorkOrder> pagination,T context);

        ValueTask<WorkOrder> DeleteWorkOrder(WorkOrder workOrder, T context);
        ValueTask<WorkOrder> UpdateWorkOrder(WorkOrder DTO, T context);
        ValueTask<WorkOrder> GetWorkOrderByID(WorkOrder workOrder, T context);
        #endregion
        ValueTask<AddressResultSet> GetAddressByCustomerId(Customer customerId);
        
        ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByCustomerId(Customer customerId);

        ValueTask<UserResultSet> GetUsersByCustomerId(Customer customerId);

        ValueTask<ContactResultSet> GetContactsByCustomerId(Customer customerId);
        ValueTask<UserResultSet> GetUsers();

        ValueTask<CalibrationTypeResultSet> GetCalibrationType();

      
        Task<ICollection<CertificatePoE>> GetCertificateXPoE(PieceOfEquipment DTO);
        ValueTask<WeightSet> CreateWeightSet(WeightSet DTO);

        //Task<ICollection<WeightSet>> GetWeightSetXPoE(PieceOfEquipment DTO);

     
        ValueTask<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByStatus(Pagination<WorkOrderDetailByStatus> pagination, T context);

        ValueTask<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByEquipment(Pagination<WorkOrderDetailByStatus> pagination, T context);

        ValueTask<ResultSet<WorkOrderDetailByCustomer>> GetWorkOrderDetailByCustomer(Pagination<WorkOrderDetailByCustomer> pagination, T context);

        Task<ICollection<Certificate>> GetCertificateXWod(WorkOrderDetail DTO);

        ValueTask<WeightSet> DeleteWeightSet(WeightSet DTO, T context);

        ValueTask<IEnumerable<Domain.Aggregates.Entities.WOStatus>> GetStatus(T context = default);

    }
}
