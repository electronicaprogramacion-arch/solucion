using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions.Assets;
using CalibrationSaaS.Domain.BusinessExceptions.Customer;
using CalibrationSaaS.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;

namespace CalibrationSaaS.Application.UseCases
{
    public class AssetsUseCases
    {
        private readonly CalibrationSaaS.Domain.Repositories.IAssetsRepository assetsRepository;

        private readonly WorkOrderDetailUseCase Repository;

       

        public AssetsUseCases(IAssetsRepository assetsRepository, WorkOrderDetailUseCase _Repository)
        {
            this.assetsRepository = assetsRepository;
            Repository = _Repository;

        }

        
        public async Task<ResultSet<WorkOrder>> GeWorkOrder(Pagination<WorkOrder> Pagination)
        {
            return await assetsRepository.GetWorkOrder(Pagination);
        }


        public async Task<ResultSet<WorkOrder>> GeWorkOrderOff(Pagination<WorkOrder> Pagination)
        {
            return await assetsRepository.GetWorkOrderOff(Pagination);
        }

       
        public async Task<WorkOrder> DeleteWorkOrder(int workOrderId)
        {
            var result = await assetsRepository.DeleteWorkOrder(workOrderId);
            return result;
        }

        public async Task<WeightSet> DeleteWeightSet(WeightSet DTO)
        {
            var result = await assetsRepository.DeleteWeightSet(DTO);
            return result;
        }
        public async Task<WorkOrder> UpdateWorkOrder(WorkOrder DTO)
        {
            var result = await this.assetsRepository.UpdateWorkOrder(DTO);

           

            return result;
        }

        public async Task<WorkOrder> CreateWorkOrder(WorkOrder DTO,string UserName)
        {

          

            if (DTO.WorkOrderDetails == null || DTO?.WorkOrderDetails?.Count == 0)
            {
                DTO.WorkOrderDetails = null;

            }
            else
            {
                var s = await assetsRepository.GetDefaultStatus();
                if (s == null)
                {
                    //throw new AssetsNotConfiguredStatus();
                    await Repository.GetStatus();

                    s = await assetsRepository.GetDefaultStatus();

                    if (s == null)
                    {
                        throw new AssetsNotConfiguredStatus();
                    }

                }
                DTO.WorkOrderDetails.ToList().ForEach(item =>
               {
                   item.CurrentStatus = s;
                   item.CurrentStatusID = s.StatusId;
                   item.PieceOfEquipmentId = item.PieceOfEquipment.PieceOfEquipmentID;
                   item.PieceOfEquipment = null;
                   item.WorkOder = null;
               });
            }

            if (!string.IsNullOrEmpty(DTO.Invoice) && !DTO.IsInternal)
            {
                var aa = await this.assetsRepository.GetWorkOrderByInvoice(DTO);

                //if (aa == null)
                //{
                //    throw new AlreadyInUseException();
                //}

                aa.Address = null;

               

                DTO = aa;

            }

            var result = await this.assetsRepository.InsertWokOrder(DTO);

            if (result?.WorkOrderDetails?.Count > 0)
            {
                foreach (var hist in result.WorkOrderDetails)
                {

                    var a = await Repository.GetHistory(hist);

                    if (a == null || a?.Count() == 0)
                    {
                        await Repository.SaveHistory(hist, 1, "Create WorkOrderDetail", UserName);
                    }

                }
            }
            
            result = await GetWorkOrderByID(DTO); 

            return result;
        }

        public async Task<WorkOrder> GetWorkOrderByID(WorkOrder DTO,int? status=null)
        {
          
            if (!status.HasValue)
            {
                status=0;
            }
            var result = await assetsRepository.GetWorkOrderByID(DTO.WorkOrderId, status.Value);
            return result;
        }


        public async Task<IEnumerable<Address>> GetAddressByCustomerId(int customerId)
        {
            return await assetsRepository.GetAddressByCustomerId(customerId);
        }

        public async Task<IEnumerable<Contact>> GetContactsByCustomerId(int customerId)
        {
            return await assetsRepository.GetContactsByCustomerId(customerId);
        }

        public async Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByCustomerId(int customerId)
        {
            return await assetsRepository.GetPieceOfEquipmentByCustomerId(customerId);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await assetsRepository.GetUsers();
        }

        public async Task<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByStatus(Pagination<WorkOrderDetailByStatus> pagination)
        {
            return await assetsRepository.GetWorkOrderDetailByStatus(pagination);
        }

        public async Task<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByEquipment(Pagination<WorkOrderDetailByStatus> pagination)
        {
            return await assetsRepository.GetWorkOrderDetailByEquipment(pagination);
        }
        public async Task<ResultSet<WorkOrderDetailByCustomer>> GetWorkOrderDetailByCustomer(Pagination<WorkOrderDetailByCustomer> pagination)
        {
            return await assetsRepository.GetWorkOrderDetailByCustomer(pagination);
        }

        public async Task<ICollection<Certificate>> GetCertificateByWod(int id)
        {
            var result = await assetsRepository.GetCertificateByWod(id);
            return result;
        }


        public async Task<ICollection<CertificatePoE>> GetCertificateXPoE(PieceOfEquipment DTO)
        { 
        

            var result = await assetsRepository.GetCertificateXPoE( DTO);
            return result;

        }

       public async Task<IEnumerable<CalibrationType>> GetCalibrationType()
        {
            return await assetsRepository.GetCalibrationTypes();
        }

        public async Task<IEnumerable<WOStatus>> GetStatus()
        {
            var a = await assetsRepository.GetStatus();


            

            return a;

        }

    }
}
