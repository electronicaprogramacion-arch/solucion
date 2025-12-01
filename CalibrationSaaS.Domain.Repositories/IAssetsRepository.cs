using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IAssetsRepository
    {
        #region workOrder

        Task<WorkOrder> GetWorkOrderByIDHeader(int workOrderId, int CurrentStatus);

        Task<WorkOrder> InsertWokOrder(WorkOrder newWorkOrder);

        Task<ResultSet<WorkOrder>> GetWorkOrder(Pagination<WorkOrder> pagination);


        Task<ResultSet<WorkOrder>> GetWorkOrderOff(Pagination<WorkOrder> pagination);

        Task<WorkOrder> GetWorkOrderByID(int newWorkOrder,int CurrentStatus =0);

        Task<WorkOrder> GetWorkOrderByInvoice(WorkOrder  DTO);

        //Task<WorkOrder> GetCustomerAddressByName(string name, int tenantID);

        Task<WorkOrder> DeleteWorkOrder(int newWorkOrder);

        Task<WorkOrder> UpdateWorkOrder(WorkOrder newWorkOrder);


        Task<bool> Save();
        #endregion


        Task<IEnumerable<WOStatus>> GetStatus();
        Task LoadUserWorkOrder(ICollection<User_WorkOrder> pag);
         Task<IEnumerable<CalibrationType>> GetCalibrationTypes();

        Task<IEnumerable<Address>> GetAddressByCustomerId(int customerId);

        Task<IEnumerable<Contact>> GetContactsByCustomerId(int customerId);

        Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByCustomerId(int id);

        //Task<IEnumerable<User>> GetUserByCustomerId(int customerId);

        Task<IEnumerable<User>> GetUsers();


        Task<Status> GetDefaultStatus();
        Task<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByStatus(Pagination<WorkOrderDetailByStatus> pagination);
        Task<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByEquipment(Pagination<WorkOrderDetailByStatus> pagination);

        Task<ResultSet<WorkOrderDetailByCustomer>> GetWorkOrderDetailByCustomer(Pagination<WorkOrderDetailByCustomer> pagination);

        Task<ICollection<Certificate>> GetCertificateByWod(int id);

        Task<ICollection<CertificatePoE>> GetCertificateXPoE(PieceOfEquipment DTO);


        //ValueTask<Certificate> CreateCertificate(CertificatePoE DTO);
        

        Task<WeightSet> DeleteWeightSet(WeightSet id);
    }   
        
}
