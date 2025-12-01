using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Application.Services;
using GrpcSample.Tests.Fixture;
using ProtoBuf.Grpc.Client;
using Shouldly;
using Xunit;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CallContext = ProtoBuf.Grpc.CallContext;
using System.Collections.Generic;
using Grpc.Core;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using System.IO;

namespace GrpcSample.Tests
{
    [Collection(TestCollections.ApiIntegration)]
    public class POEServiceShould: IDisposable
    {
        ScriptClass sc = new ScriptClass();
        PieceOfEquipment requestScale = new PieceOfEquipment();
        Pagination<PieceOfEquipment> pagScale = new Pagination<PieceOfEquipment>();
        Pagination<PieceOfEquipment> pagIndicator = new Pagination<PieceOfEquipment>();

        public string Customer { get; set; } = "ReMed Recovery Care Center, LLC";
        public POEServiceShould(TestServerFixture testServerFixture)
        {
            var channel = testServerFixture.GrpcChannel;
            _clientService = channel.CreateGrpcService<IPieceOfEquipmentService<CallContext>>();

            _assetsService = channel.CreateGrpcService<IAssetsServices<CallContext>>();


            PieceOfEquipment filter = new PieceOfEquipment();

            filter.EquipmentTemplate = new EquipmentTemplate();

            filter.EquipmentTemplate.EquipmentTypeID = 3;

            PieceOfEquipment filter2 = new PieceOfEquipment();

            filter2.EquipmentTemplate = new EquipmentTemplate();

            filter2.EquipmentTemplate.EquipmentTypeID = 1;

            pagIndicator.Object = new FilterObject<PieceOfEquipment>();
            pagScale.Object= new FilterObject<PieceOfEquipment>();           

            pagIndicator.Object.EntityFilter = filter2;
            pagScale.Object.EntityFilter = filter;
            pagIndicator.Object.EntityFilter.Customer = new Customer();
            pagScale.Object.EntityFilter.Customer = new Customer();

            pagIndicator.Show = 2;
            pagIndicator.Page = 1;
           
            pagScale.Show = 2;
            pagScale.Page = 1;


            

            if (sc.SetupTestData("totalss.sql"))
            {
                sc.SetupTestData("datass.sql");
            }


        }

        private readonly IPieceOfEquipmentService<CallContext> _clientService;

        private readonly IAssetsServices<CallContext> _assetsService;

        private static PieceOfEquipment POE;

        public async Task<PieceOfEquipment> GetPOEByID(PieceOfEquipment request)
        {

            //var request = new PieceOfEquipment();

            //request.PieceOfEquipmentID = 1899.ToString();

            var result = await _clientService.GetPieceOfEquipmentXId(request, CallContext.Default);


            return result;

        }


        public async Task<List<PieceOfEquipment>> GetPOES(Pagination<PieceOfEquipment> pag, string filter = null)
        {

            pag.Object.EntityFilter.Customer.Name=filter;

            var result = await _clientService.GetPiecesOfEquipmentXDueDate(pag, CallContext.Default);


            return result.List;

        }

        //[Fact]
        //public async void GetPOEByIDTest()
        //{
            
          

        //}

        //    [Fact]
        //public async void AddIndicatorToPoe()
        //{
            
        //}


        [Fact]
        public async void UpdateIndicatorFromNoIndicator()
        {


            return;

            var ID = "";

            try
            {

                var ScaleList = await GetPOES(pagScale, Customer);

                ScaleList.ShouldNotBeNull();

                var IndicatorList = await GetPOES(pagIndicator, Customer);

                IndicatorList.ShouldNotBeNull();               



                int contador = 0;
                
                foreach (var item in ScaleList)
                {
                    if (1==1 && contador < IndicatorList.Count && item.CustomerId == IndicatorList[contador].CustomerId)
                    {
                        var request = await GetPOEByID(item);

                        ID = request.PieceOfEquipmentID;
                        //////////////////////////////////////////////////////////////
                        CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();
                        _custId.CustomerID = request.CustomerId;
                        var _addressFiltered = await _assetsService.GetAddressByCustomerId(_custId);

                        if(_addressFiltered == null || _addressFiltered?.Addresses==null || _addressFiltered?.Addresses?.Count == 0)
                        {
                            throw new NotImplementedException("Failed address"); 
                        }

                        request.AddressId = _addressFiltered.Addresses[0].AddressId;
                        //////////////////////////////////////////////////////////////////////////////////



                        request.IndicatorPieceOfEquipmentID = null;

                        request.Indicator = null;

                        var result = await _clientService.PieceOfEquipmentCreate(request, CallContext.Default);

                        result.ShouldNotBeNull("Save null error");


                        if (item.CustomerId== IndicatorList[contador].CustomerId)
                        {
                            request.IndicatorPieceOfEquipmentID = IndicatorList[contador].PieceOfEquipmentID;
                        }

                        // act
                        result = await _clientService.PieceOfEquipmentCreate(request, CallContext.Default);

                        result.ShouldNotBeNull("Save Indicator");

                        // assert

                        var resultList = result;
                        resultList.ShouldNotBeNull();
                        resultList.PieceOfEquipmentID.ShouldNotBeNullOrEmpty();

                        if (!string.IsNullOrEmpty(request.IndicatorPieceOfEquipmentID) || request.Indicator != null)
                        {
                            resultList.IndicatorPieceOfEquipmentID.ShouldNotBeNullOrEmpty();
                        }

                        Assert.Equal(resultList.IndicatorPieceOfEquipmentID, IndicatorList[contador].PieceOfEquipmentID);
                        contador++;
                        

                    }


                }
               


               



            }
            catch(NotImplementedException ex)
            {
                throw new Exception("Fail Precondition " + ID);
            }
            catch(RpcException ex)
            {
                if (ex.StatusCode == StatusCode.Unavailable)
                {
                    throw new Exception("connection problem");
                }
               
                if (ex.StatusCode == StatusCode.InvalidArgument && ex.Status.Detail.Contains("Required"))
                {
                   


                    throw new Exception("Fail Precondition " + ex.Status.Detail + " " +  ID );
                }

                throw ex;

            }
            catch (Exception ex)
            {
                throw new Exception(ID, ex);
            }




            //resultList.Count().ShouldBe(10);
            //resultList.First().Value.ShouldBe(1);
            //resultList.Last().Value.ShouldBe(10);

            //var resultList = result.ToList();
            //resultList.ShouldNotBeNull();
            //resultList.Count().ShouldBe(10);
            //resultList.First().Value.ShouldBe(1);
            //resultList.Last().Value.ShouldBe(10);
        }

        public void Dispose()
        {
            
            sc=null;    
            
            //var route = Environment.CurrentDirectory;

            //if (File.Exists(@route + @"\LocalDb.mdf"))
            //{
                 
            //    File.Delete(@route + @"\LocalDb.mdf");

              
            //}
            //if (File.Exists(@route + @"\LocalDb_log.ldf"))
            //{

            //    File.Delete(@route + @"\LocalDb_log.ldf");


            //}

            //throw new NotImplementedException();
        }

        //[Fact]
        //public async void UpdateIndicatorUseValidation()
        //{

        //    try
        //    {

        //        var ScaleList = await GetPOES(pagScale, Customer);

        //        ScaleList.ShouldNotBeNull();

        //        var IndicatorList = await GetPOES(pagIndicator, Customer);

        //        IndicatorList.ShouldNotBeNull();

        //        int contador = 0;

        //        foreach (var item in ScaleList)
        //        {
        //            if (1 == 1 && contador < IndicatorList.Count)
        //            {
        //                var request = await GetPOEByID(item);

        //                request.IndicatorPieceOfEquipmentID = null;

        //                request.Indicator = null;

        //                var result = await _clientService.PieceOfEquipmentCreate(request, CallContext.Default);

        //                if (item.CustomerId == IndicatorList[0].CustomerId)
        //                {
        //                    request.IndicatorPieceOfEquipmentID = IndicatorList[0].PieceOfEquipmentID;
        //                }

        //                // act
        //                result = await _clientService.PieceOfEquipmentCreate(request, CallContext.Default);

        //                contador++;
        //                // assert

        //                var resultList = result;
        //                resultList.ShouldNotBeNull();
        //                resultList.PieceOfEquipmentID.ShouldNotBeNullOrEmpty();

        //                if (!string.IsNullOrEmpty(request.IndicatorPieceOfEquipmentID) || request.Indicator != null)
        //                {
        //                    resultList.IndicatorPieceOfEquipmentID.ShouldNotBeNullOrEmpty();
        //                }

        //            }


        //        }







        //    }
        //    catch (Exception ex)
        //    {

        //    }




        //    //resultList.Count().ShouldBe(10);
        //    //resultList.First().Value.ShouldBe(1);
        //    //resultList.Last().Value.ShouldBe(10);

        //    //var resultList = result.ToList();
        //    //resultList.ShouldNotBeNull();
        //    //resultList.Count().ShouldBe(10);
        //    //resultList.First().Value.ShouldBe(1);
        //    //resultList.Last().Value.ShouldBe(10);
        //}





        //[Fact]
        //public async void UpdateIndicator()
        //{

        //    try
        //    {

        //        var ScaleList = await GetPOES(pagScale, Customer);

        //        ScaleList.ShouldNotBeNull();

        //        var IndicatorList = await GetPOES(pagIndicator, Customer);

        //        IndicatorList.ShouldNotBeNull();

        //        int contador = 0;

        //        foreach (var item in ScaleList)
        //        {
        //            if (1 == 1 && contador < IndicatorList.Count)
        //            {
        //                var request = await GetPOEByID(item);

        //                //request.IndicatorPieceOfEquipmentID = null;

        //                //request.Indicator = null

        //                //var result = await _clientService.PieceOfEquipmentCreate(request, CallContext.Default);

        //                if (item.CustomerId == IndicatorList[contador].CustomerId)
        //                {
        //                    request.IndicatorPieceOfEquipmentID = IndicatorList[contador].PieceOfEquipmentID;
        //                }

        //                // act
        //                var result = await _clientService.PieceOfEquipmentCreate(request, CallContext.Default);

        //                contador++;
        //                // assert

        //                var resultList = result;
        //                resultList.ShouldNotBeNull();
        //                resultList.PieceOfEquipmentID.ShouldNotBeNullOrEmpty();

        //                if (!string.IsNullOrEmpty(request.IndicatorPieceOfEquipmentID) || request.Indicator != null)
        //                {
        //                    resultList.IndicatorPieceOfEquipmentID.ShouldNotBeNullOrEmpty();
        //                }

        //            }


        //        }







        //    }
        //    catch (Exception ex)
        //    {

        //    }




        //    //resultList.Count().ShouldBe(10);
        //    //resultList.First().Value.ShouldBe(1);
        //    //resultList.Last().Value.ShouldBe(10);

        //    //var resultList = result.ToList();
        //    //resultList.ShouldNotBeNull();
        //    //resultList.Count().ShouldBe(10);
        //    //resultList.First().Value.ShouldBe(1);
        //    //resultList.Last().Value.ShouldBe(10);
        //}





        //[Fact]
        //public async Task SlowCountFromLowToHighAsync()
        //{
        //    // arrange
        //    //var counter = 1;
        //    //var timer = new Stopwatch();
        //    //var request = new CountRequest {LowerBound = 1, UpperBound = 5};

        //    //// act
        //    //timer.Start();
        //    //var result = _clientService.SlowCountAsync(request, CallContext.Default);

        //    //// assert
        //    //await foreach (var value in result)
        //    //{
        //    //    value.Value.ShouldBe(counter++);
        //    //}

        //    //timer.Stop();
        //    //counter.ShouldBe(6);
        //    //timer.Elapsed.ShouldBeGreaterThan(TimeSpan.FromSeconds(5));
        //}
    }
}