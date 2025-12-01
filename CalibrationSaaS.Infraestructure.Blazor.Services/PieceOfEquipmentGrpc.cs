using Blazor.IndexedDB.Framework;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    public class PieceOfEquipmentGRPC : IDisposable, IPieceOfEquipmentService<CallContext>
    {


        private CallContext _context;

#pragma warning disable CS0169 // El campo 'PieceOfEquipmentGRPC.pieceOfEquipment' nunca se usa
        private readonly IPieceOfEquipmentService<CallContext> pieceOfEquipment;
#pragma warning restore CS0169 // El campo 'PieceOfEquipmentGRPC.pieceOfEquipment' nunca se usa


        private readonly dynamic DbFactory;

        public PieceOfEquipmentGRPC(Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>> _poe, dynamic _DbFactory)
        {

            _context = new CallOptions();
            Client2 = _poe(_DbFactory);
        }

        public PieceOfEquipmentGRPC(Func<dynamic, CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext>> _poe
            , dynamic _DbFactory, CallOptions callOptions)
        {

            _context = callOptions;
            Client2 = _poe(_DbFactory);


        }

        public PieceOfEquipmentGRPC(IPieceOfEquipmentService<CallContext> _poe)
        {
            _context = new CallOptions();
            Client2 = _poe;
        }

        public PieceOfEquipmentGRPC(IPieceOfEquipmentService<CallContext> _poe, CallContext context = default)
        {
            _context = context;
            Client2 = _poe;
        }

        public IPieceOfEquipmentService<CallContext> Client2 { get; set; }

        public async ValueTask<PieceOfEquipment> PieceOfEquipmentCreate(PieceOfEquipment DTO, CallContext context = default)
        {
            if (Client2 != null)
            {
                var result = await Client2.PieceOfEquipmentCreate(DTO, _context);


                return result;

            }
            else
            {

                throw new Exception("Error");
            }


        }

        public void Dispose()
        {
            //Client2 = null;
        }

        public ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipment(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            if (Client2 != null)
            {

                return Client2.GetPieceOfEquipment(pagination, _context);
            }
            else
            {
                return new ValueTask<PieceOfEquipmentResultSet>();

            }
        }

         public async ValueTask<PieceOfEquipment> GetPieceOfEquipmentXId(string ID)
        {
            var DTO= new PieceOfEquipment();    
            DTO.PieceOfEquipmentID=ID;
            var result = await Client2.GetPieceOfEquipmentXId(DTO, _context);
            return result;
        }

        public async ValueTask<PieceOfEquipment> GetPieceOfEquipmentXId(PieceOfEquipment DTO, CallContext context = default)
        {
            var result = await Client2.GetPieceOfEquipmentXId(DTO, _context);
            return result;
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async ValueTask<PieceOfEquipment> DeletePieceOfEquipment(PieceOfEquipment PieceOfEquipmentDTO, CallContext context = default)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            var result = Client2.DeletePieceOfEquipment(PieceOfEquipmentDTO, _context);
            return PieceOfEquipmentDTO;
        }

        public async ValueTask<PieceOfEquipment> UpdatePieceOfEquipment(PieceOfEquipment DTO, CallContext context = default)
        {
            var result = await Client2.UpdatePieceOfEquipment(DTO, _context);
            return DTO;
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentXDueDate(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            var result = await Client2.GetPiecesOfEquipmentXDueDate(pagination, context);
            return result;//new PieceOfEquipmentResultSet { PieceOfEquipments = result.PieceOfEquipments };

        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPiecesOfEquipmentChildren(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            var result = await Client2.GetPiecesOfEquipmentChildren(pagination, context);
            return result;//new PieceOfEquipmentResultSet { PieceOfEquipments = result.PieceOfEquipments };

        }

        public ValueTask<PieceOfEquipmentResultSet> GetAllWeightSets(PieceOfEquipment DTO, CallContext context = default)
        {
            return Client2.GetAllWeightSets(DTO, _context);
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentIndicator(Pagination<PieceOfEquipment> DTO, CallContext context = default)
        {
            return await Client2.GetPieceOfEquipmentIndicator(DTO, _context);
        }


        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByIndicator(Pagination<PieceOfEquipment> DTO, CallContext context = default)
        {
            return await Client2.GetPieceOfEquipmentBalanceByIndicator(DTO, _context);
        }



        //YPPP
        public async ValueTask<WorkOrderDetailResultSet> GetPieceOfEquipmentHistory(PieceOfEquipment DTO, CallContext context = default)
        {
            return await Client2.GetPieceOfEquipmentHistory(DTO, _context);
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentByCustomer(Pagination<PieceOfEquipment> DTO, CallContext context = default)
        {
            var result = await Client2.GetPieceOfEquipmentByCustomer(DTO, _context);
            return result;//new PieceOfEquipmentResultSet { PieceOfEquipments = result.PieceOfEquipments };
        }

        public async ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByET(EquipmentTemplate DTO, CallContext context = default)
        {
            var result = await Client2.GetPieceOfEquipmentByET(DTO, _context);
            return result; //new PieceOfEquipmentResultSet { PieceOfEquipments = result.PieceOfEquipments };
        }

        public async ValueTask<PieceOfEquipmentResultSet> GetPieceOfEquipmentByCustomerId(Customer DTO, CallContext context = default)
        {
            var result = await Client2.GetPieceOfEquipmentByCustomerId(DTO, _context);
            return result; //new PieceOfEquipmentResultSet { PieceOfEquipments = result.PieceOfEquipments };
        }

        public async ValueTask<PieceOfEquipment> UpdateIndicator(PieceOfEquipment DTO, CallContext context = default)
        {
            var result = await Client2.UpdateIndicator(DTO, _context);
            return DTO;
        }

        public async Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            var result = await Client2.GetAllWeightSetsPag(pagination, _context);
            return result;
        }

        public async Task<ResultSet<PieceOfEquipment>> GetAllPeripheralsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            var result = await Client2.GetAllPeripheralsPag(pagination, context);
            return result;
        }

        public async Task<ResultSet<WeightSet>> SaveWeights(ICollection<WeightSet> W, CallContext context = default)
        {
            var result = await Client2.SaveWeights(W, _context);
            return result;
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPieceOfEquipmentBalanceByPer(Pagination<PieceOfEquipment> DTO, CallContext context = default)
        {
             return await Client2.GetPieceOfEquipmentBalanceByPer(DTO, _context);
        }

        public async ValueTask<Uncertainty> CreateUncertainty(TableChanges<Uncertainty> PieceOfEquipmentDTO, CallContext context = default)
        {
            return await Client2.CreateUncertainty(PieceOfEquipmentDTO, _context);
        }

        public async ValueTask<ResultSet<Uncertainty>> GetUncertainty(Pagination<Uncertainty> Uncertainty, CallContext context = default)
        {
            return await Client2.GetUncertainty(Uncertainty, _context);
        }

        public async ValueTask<ICollection<Force>> GetCalculatesForISOandASTM(ISOandASTM ISOandASTM, CallContext context = default)
        {
            return await Client2.GetCalculatesForISOandASTM( ISOandASTM, _context);
        }

        public async ValueTask<ICollection<PieceOfEquipment>> GetTemperatureStandard(CallContext context = default)
        {
            return await Client2.GetTemperatureStandard(_context);
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetPOEByTestCodePag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
             return await Client2.GetPOEByTestCodePag(pagination,_context);
        }

        public async ValueTask<ResultSet<POE_Scale>> GetPOEScale(Pagination<POE_Scale> pagination, CallContext context = default)
        {
            return await Client2.GetPOEScale(pagination, _context);
        }

        public async ValueTask<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByScale(string id, CallContext context = default)
        {
            return await Client2.GetPieceOfEquipmentByScale(id, _context);
        }

        public async ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByMass(IEnumerable<PieceOfEquipment> DTO, CallContext context = default)
        {
            return await Client2.GetResolutionByMass(DTO, _context);
        }

        public async ValueTask<IEnumerable<PieceOfEquipment>> GetResolutionByLenght(IEnumerable<PieceOfEquipment> DTO, CallContext context = default)
        {
            return await Client2.GetResolutionByLenght(DTO, _context);
        }

        public async ValueTask<CalibrationType> CreateConfiguration(CalibrationType DTO, CallContext context = default)
        {
            return await Client2.CreateConfiguration(DTO, _context);
        }

        public async ValueTask<CalibrationSubType> DeleteConfiguration(CalibrationSubType DTO, CallContext context = default)
        {
            return await Client2.DeleteConfiguration(DTO, _context);
        }

        public async ValueTask<CalibrationSubType> EnableConfiguration(CalibrationSubType DTO, CallContext context = default)
        {
            return await Client2.EnableConfiguration(DTO, _context);
        }

        public async ValueTask<CalibrationType> GetDynamicConfiguration(CalibrationType CalibrationType, CallContext context = default)
        {
            return await Client2.GetDynamicConfiguration(CalibrationType, _context);
        }

       public async ValueTask<ICollection<Force>> CalculateUncertainty(List<Force> forces, int iso, CallContext context = default)
        {
            return await Client2.CalculateUncertainty(forces, iso, _context);
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> GetSelectPOEChildren(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            return await Client2.GetSelectPOEChildren(pagination, _context);
        }

        public async ValueTask<PieceOfEquipment> UpdateChildPieceOfEquipment(PieceOfEquipment pieceOfEquipmentDTO, CallContext context = default)
        {
            return await Client2.UpdateChildPieceOfEquipment(pieceOfEquipmentDTO, _context);
        }

        public async ValueTask<ICollection<PieceOfEquipment>> GetPieceOfEquipmentChildrenAll(PieceOfEquipment PieceOfEquipmentDTO, CallContext context = default)
        {
            return await Client2.GetPieceOfEquipmentChildrenAll(PieceOfEquipmentDTO, _context);
        }
    }
}
