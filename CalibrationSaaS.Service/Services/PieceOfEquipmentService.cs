using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CalibrationSaaS.Domain.Aggregates.Querys;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
   
    public class PieceOfEquipmentService : IPieceOfEquipmentService<CallContext>
    {
        private readonly PieceOfEquipmentUseCases pieceOfEquipmentLogic;
        private readonly ValidatorHelper modelValidator;
        private readonly ILogger _logger;

        public PieceOfEquipmentService(PieceOfEquipmentUseCases pieceOfEquipmentLogic, ILogger<PieceOfEquipment> logger, ValidatorHelper modelValidator)
        {
            this.pieceOfEquipmentLogic = pieceOfEquipmentLogic;
            this._logger = logger;
            this.modelValidator = modelValidator;
        }

        public string GetComponent(CallContext context)
        {
            var header = context.RequestHeaders.Where(x => x.Key.ToLower() == "component").FirstOrDefault();
            var user = context.ServerCallContext.GetHttpContext();

            string com = null;

            if (header != null)
            {
                com = header.Value;


            }

            return com;
        }


        public async ValueTask<PieceOfEquipment> UpdateChildPieceOfEquipment(PieceOfEquipment pieceOfEquipmentDTO, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.UpdateChildPieceOfEquipment(pieceOfEquipmentDTO);


            return result;
        }

        public async ValueTask<PieceOfEquipment> PieceOfEquipmentCreate(PieceOfEquipment pieceOfEquipmentDTO, CallContext context)
        {

            if ( pieceOfEquipmentDTO.Valid())
            {
                var com = GetComponent(context);
                var result = await pieceOfEquipmentLogic.CreatePieceOfEquipment(pieceOfEquipmentDTO,com );

                return await GetPieceOfEquipmentXId(result, context);
            }
            else
            {
                throw new Exception("Not Valid Model");
            }
        }

        public async ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipment(Pagination<PieceOfEquipment> pagination, CallContext context)
        {

            var user = context.ServerCallContext.GetHttpContext().User;

            var result = await pieceOfEquipmentLogic.GetPieceOfEquipment(pagination);

            foreach (var item in result.List)
            {
                item.EquipmentTemplate.EquipmentTypeObject.EquipmentTemplates = null;
            }
            return new PieceOfEquipmentResultSet { PieceOfEquipments = result.List, Rows=result.Count };
            
        }

        


        public async ValueTask<PieceOfEquipment> GetPieceOfEquipmentXId(PieceOfEquipment PieceOfEquipmentDTO, CallContext context)
        {
            var header = context.RequestHeaders.Where(x => x.Key.ToLower() == "component").FirstOrDefault();
            var user = context.ServerCallContext.GetHttpContext();

            string com = "";

            if (header != null) 
            {
                com = header.Value;
            }
           

            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentByID(PieceOfEquipmentDTO,user.User.Identity.Name, GetComponent(context)) ;
            return result;

        }
        public async ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentXDueDate(Pagination<PieceOfEquipment> pagination, CallContext context)
        {

            var user =  context.ServerCallContext.GetHttpContext().User;

            //var a = user.IsInRole("Owner");
            //var b = user.IsInRole("admin");
            //var d = user.IsInRole("pepe");


            var result = await pieceOfEquipmentLogic.GetPieceOfEquipment(pagination);
            return result;//new PieceOfEquipmentResultSet { PieceOfEquipments = result.List, Rows=result.Count };

        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentChildren(Pagination<PieceOfEquipment> pagination, CallContext context)
        {

            var user = context.ServerCallContext.GetHttpContext().User;

            //var a = user.IsInRole("Owner");
            //var b = user.IsInRole("admin");
            //var d = user.IsInRole("pepe");


            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentChildren(pagination);
            return result;//new PieceOfEquipmentResultSet { PieceOfEquipments = result.List, Rows=result.Count };

        }


        public async ValueTask<PieceOfEquipment> DeletePieceOfEquipment(PieceOfEquipment PieceOfEquipmentDTO, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.DeletePieceOfEquipment(PieceOfEquipmentDTO);
            return PieceOfEquipmentDTO;
        }

        public async ValueTask<PieceOfEquipment> UpdatePieceOfEquipment(PieceOfEquipment DTO, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.UpdatePieceOfEquipment(DTO, GetComponent(context));
            return DTO;
        }

        public async  ValueTask<PieceOfEquipmentResultSet> GetAllWeightSets(PieceOfEquipment DTO ,CallContext context)
        {
            var result = await pieceOfEquipmentLogic.GetAllWeightSets(DTO);

            if(result != null)
            {
                return new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };
            }

            return null;
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentByCustomer(Pagination<PieceOfEquipment> Pagination, CallContext context)
        {
            //var DTO = Pagination.Entity.Customer;

            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentByCustomer(Pagination);
            return result; //new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };

        }

       
        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentIndicator(Pagination<PieceOfEquipment> DTO, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmenIndicator(DTO);
            //if (result != null)
            //{
            //    return new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };
            //}
            return result;
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByIndicator(Pagination<PieceOfEquipment> DTO, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentBalanceByIndicator(DTO);
            //if (result != null)
            //{
            //    return new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };
            //}
            return result;
        }


        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByPer(Pagination<PieceOfEquipment> DTO, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentBalanceByPer(DTO);
            //if (result != null)
            //{
            //    return new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };
            //}
            return result;
        }



        //YPPP
        public async ValueTask<WorkOrderDetailResultSet> GetPieceOfEquipmentHistory(PieceOfEquipment DTO, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentHistory(DTO.PieceOfEquipmentID);
            if (result != null)
            {
                return new WorkOrderDetailResultSet { WorkOrderDetails = result.ToList() };
            }
            return null;
        }

        public async ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByET(EquipmentTemplate DTO, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentByET(DTO.EquipmentTemplateID);
            if (result != null)
            {
                return new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };
            }
            return null;
        }

        public async ValueTask<ICollection<PieceOfEquipment>> GetTemperatureStandard(CallContext context)
        {
            var result = await pieceOfEquipmentLogic.GetTemperatureStandard();
            if (result != null)
            {
                return result.ToList(); //new PieceOfEquipmentResultSet { PieceOfEquipments = result.ToList() };
            }
            return null;
        }

        public async ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByCustomerId(Customer DTO, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentByCustomer(DTO.CustomerID);
            PieceOfEquipmentResultSet ret = new PieceOfEquipmentResultSet();
            if (result != null)
            {
                ret.PieceOfEquipments = result.ToList();
                return ret;
            }
            return null;
        }

        public async ValueTask<PieceOfEquipment> UpdateIndicator(PieceOfEquipment DTO, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.UpdateIndicator(DTO);
            return DTO;
        }

        public async  Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.GetAllWeightSetsPag(pagination);

            return result;
        }

        public async Task<ResultSet<PieceOfEquipment>> GetAllPeripheralsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.GetAllPeripheralsPag(pagination);

            return result;
        }

        public async Task<ResultSet<WeightSet>> SaveWeights(ICollection<WeightSet> W, CallContext context = default)
        {

            var result = await pieceOfEquipmentLogic.SaveWeights(W);

            return result;

        }

        public async ValueTask<Uncertainty> CreateUncertainty(TableChanges<Uncertainty> PieceOfEquipmentDTO, CallContext context)
        {
             var result = await pieceOfEquipmentLogic.CreateUncertainty(PieceOfEquipmentDTO);

            return result;
        }

        public async ValueTask<ResultSet<Uncertainty>> GetUncertainty(Pagination<Uncertainty> Uncertainty, CallContext context)
        {
             var result = await pieceOfEquipmentLogic.GetUncertainty(Uncertainty);

            return result;
        }

        public async ValueTask<ICollection<Force>> GetCalculatesForISOandASTM(ISOandASTM ISOandASTM, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.GetCalculatesForISOandASTM( ISOandASTM);
            return await Task.FromResult(result);
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPOEByTestCodePag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
             var result = await pieceOfEquipmentLogic.GetPOEByTestCodePag(pagination);
            return await Task.FromResult(result);
        }

        public async ValueTask<ResultSet<POE_Scale>> GetPOEScale(Pagination<POE_Scale> pagination, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.GetPOEScale(pagination);
            return await Task.FromResult(result);
        
        
        }

        public async ValueTask<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByScale(string id, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentByScale(id);
            return await Task.FromResult(result);
        }

        public async ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByMass(IEnumerable<PieceOfEquipment> DTO, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.GetResolutionByMass(DTO);
            return await Task.FromResult(result);
        }


        public async ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByLenght(IEnumerable<PieceOfEquipment> DTO, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.GetResolutionByLenght(DTO);
            return await Task.FromResult(result);
        }


        public async ValueTask<CalibrationType> CreateConfiguration(CalibrationType DTO, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.CreateConfiguration(DTO);
            return await Task.FromResult(result);
        }

        public async ValueTask<CalibrationSubType> DeleteConfiguration(CalibrationSubType DTO, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.DeleteConfiguration(DTO);
            return await Task.FromResult(result);
        }

        public async ValueTask<CalibrationSubType> EnableConfiguration(CalibrationSubType DTO, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.EnableConfiguration(DTO);
            return await Task.FromResult(result);
        }

        public async ValueTask<CalibrationType>  GetDynamicConfiguration(CalibrationType CalibrationType, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.GetDynamicConfiguration(CalibrationType.CalibrationTypeId,GetComponent(context));
            return await Task.FromResult(result);
        }

        public async ValueTask<ICollection<Force>> CalculateUncertainty(List<Force> forces, int iso, CallContext context)
        {
            var result = await pieceOfEquipmentLogic.CalculateUncertainty(forces, iso, null);
            return result;
        }


        public async ValueTask<ResultSet<PieceOfEquipment>> GetSelectPOEChildren(Pagination<PieceOfEquipment> pagination, CallContext context)
        {

            var result = await pieceOfEquipmentLogic.GetSelectPOEChildren(pagination);
            return await Task.FromResult(result);

        }

        public async ValueTask<ICollection<PieceOfEquipment>> GetPieceOfEquipmentChildrenAll(PieceOfEquipment PieceOfEquipmentDTO, CallContext context = default)
        {
            var result = await pieceOfEquipmentLogic.GetPieceOfEquipmentChildrenAll(PieceOfEquipmentDTO);

            return result.ToArray();
        }
    }
}
