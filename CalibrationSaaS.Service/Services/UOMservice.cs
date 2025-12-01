using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Reports.Domain.ReportViewModels;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using CalibrationSaaS.Domain.Repositories;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class UOMServices : IUOMService<CallContext>
    {
        private readonly SampleUseCases LogicSample;
     //   private readonly IWorkOrderDetailRepository wodRepository;

        private readonly UOMUseCases Logic;
        private readonly WorkOrderDetailUseCase LogicWod;
        private readonly PieceOfEquipmentUseCases Logicpoe;
        private readonly ValidatorHelper modelValidator;
        private readonly ILogger logger;
        private readonly IConfiguration Configuration;
        public System.Net.Http.HttpClient Http { get; set; }
       

        public UOMServices(UOMUseCases _Logic, SampleUseCases _LogicSample, WorkOrderDetailUseCase _LogicWod, PieceOfEquipmentUseCases _Logicpoe, ILogger<UOMServices> _logger, ValidatorHelper _modelValidator, IConfiguration _Configuration)
        {
            Logic = _Logic;
            _LogicSample = LogicSample;
            logger = _logger;
            modelValidator = _modelValidator;
           this.LogicWod = _LogicWod;
            this.Logicpoe = _Logicpoe;
            Configuration = _Configuration;

        }

    //    private static double convertValueInternal(String initialUnitOfMeasureId, double value, String finalUnitOfMeasureId, Delegator delegator) throws Exception
    //    {
    //        UnitOfMeasure initialUnitOfMeasure = UnitOfMeasure.getNewUnitOfMeasure(delegator.findOne("UnitOfMeasure", false, UtilMisc.toMap("unitOfMeasureId", initialUnitOfMeasureId)));
    //    UnitOfMeasure finalUnitOfMeasure = UnitOfMeasure.getNewUnitOfMeasure(delegator.findOne("UnitOfMeasure", false, UtilMisc.toMap("unitOfMeasureId", finalUnitOfMeasureId)));
    //    if(!initialUnitOfMeasure.unitOfMeasureTypeId.equals(finalUnitOfMeasure.unitOfMeasureTypeId)){
    //        throw new Exception("The units of measure " + initialUnitOfMeasure.name + " and " + finalUnitOfMeasure.name +  " are from different unit types, a conversion can't be done");
    //}
    //    //Round removed 7/29/2020
    //    //return roundValue((value * initialUnitOfMeasure.conversionValue) * (1/finalUnitOfMeasure.conversionValue), 3);
    //    return (value* initialUnitOfMeasure.conversionValue) * (1/finalUnitOfMeasure.conversionValue);
    //}


       //public async ValueTask<UnitOfMeasure> Conversion(UnitOfMeasure _Source, 
       //    UnitOfMeasure _Target, 
       //    UnitOfMeasure _Base, 
       //    UnitOfMeasure uncertain, 
       //    CallContext context)
       // {
       //     return _Source;
       // }

        public async  ValueTask<UnitOfMeasure> Create(UnitOfMeasure DTO , CallContext context)
        {
            return await Logic.Create(DTO);
        }

        public async ValueTask<UnitOfMeasure> Delete(UnitOfMeasure DTO, CallContext context)
        {
          var a= await Logic.Delete(DTO);

           
            a.UncertaintyUnitOfMeasure = null;
            a.UncertaintyWeightSets = null;
            a.WeightSets = null;

          return a;
        }

        public async ValueTask<UOMResultSet> GetAllEnabled(CallContext context)
        {
            IEnumerable<UnitOfMeasure> UnitofMeasureList;

            //UnitofMeasureList.Add(new UnitOfMeasure { Name = "Pounds", Abbreviation = "Pd", UnitOfMeasureID = 1, TypeID=3 });

            //UnitofMeasureList.Add(new UnitOfMeasure { Name = "Kilogramo", Abbreviation = "Kg", UnitOfMeasureID = 2, TypeID = 3 });

            UnitofMeasureList = await Logic.GetAllEnabled();

            return new UOMResultSet { UnitOfMeasureList = UnitofMeasureList.ToList() };
        }

        public async ValueTask<UOMResultSet> GetAll(CallContext context)
        {

            IEnumerable<UnitOfMeasure> UnitofMeasureList  ;

            //UnitofMeasureList.Add(new UnitOfMeasure { Name = "Pounds", Abbreviation = "Pd", UnitOfMeasureID = 1, TypeID=3 });

            //UnitofMeasureList.Add(new UnitOfMeasure { Name = "Kilogramo", Abbreviation = "Kg", UnitOfMeasureID = 2, TypeID = 3 });

            UnitofMeasureList = await Logic.GetAll();

            return new UOMResultSet { UnitOfMeasureList = UnitofMeasureList.ToList() };


        }

        public async ValueTask<ResultSet<UnitOfMeasure>> GetAllPag(Pagination<UnitOfMeasure> Pagination,CallContext context)
        {

            //IEnumerable<UnitOfMeasure> UnitofMeasureList;

            //UnitofMeasureList.Add(new UnitOfMeasure { Name = "Pounds", Abbreviation = "Pd", UnitOfMeasureID = 1, TypeID=3 });

            //UnitofMeasureList.Add(new UnitOfMeasure { Name = "Kilogramo", Abbreviation = "Kg", UnitOfMeasureID = 2, TypeID = 3 });

            var UnitofMeasureList = await Logic.GetAllPag(Pagination);

            return UnitofMeasureList;//new UOMResultSet { UnitOfMeasureList = UnitofMeasureList.ToList() };


        }

        public async ValueTask<UnitOfMeasure> GetByID(UnitOfMeasure DTO, CallContext context)
        {
           return  await Logic.GetByID(DTO);
        }

        public async ValueTask<UOMResultSet> GetByType(UnitOfMeasureType Type, CallContext context)
        {
            var a= await Logic.GetByType(Type);

            return new UOMResultSet { UnitOfMeasureList = a.ToList() };
        }

        public async ValueTask<ICollection<UnitOfMeasureType>> GetTypes(CallContext context)
        {
            //List<UnitOfMeasureType> t = new List<UnitOfMeasureType>();

            //t.Add(new UnitOfMeasureType { Name = "Temperature", Value = 1 });

            //t.Add(new UnitOfMeasureType { Name = "Humidity", Value = 2 });

            //t.Add(new UnitOfMeasureType { Name = "Weight", Value = 3 });

            var t = await Logic.GetTypes();


            return t.ToArray();

        }

        public async  ValueTask<UnitOfMeasure> Update(UnitOfMeasure DTO, CallContext context)
        {
            return  await Logic.Update(DTO);
        }


        public async ValueTask<UnitOfMeasureType> UpdateType(UnitOfMeasureType DTO, CallContext context)
        {
            return await Logic.UpdateType(DTO);
        }



        public async ValueTask<UnitOfMeasureType> CreateType(UnitOfMeasureType DTO, CallContext context)
        {
            return await Logic.CreateType(DTO);
        }

       
    }
}
