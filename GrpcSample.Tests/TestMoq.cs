
using Bogus;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.GrpcServices.Services;
using Grpc.Core;
using GrpcSample.Tests.Fixture;
using Helpers.Controls.ValueObjects;
using Moq;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GrpcSample.Prueba
{
    [Collection(TestCollections.ApiIntegration)]
    public class TestMoq
    {
        //private readonly IPieceOfEquipmentService<CallContext> _clientService;


        PieceOfEquipment POEPrinc = new PieceOfEquipment();
        PieceOfEquipment requestScale = new PieceOfEquipment();
        Pagination<PieceOfEquipment> pagScale = new Pagination<PieceOfEquipment>();
        Pagination<PieceOfEquipment> pagIndicator = new Pagination<PieceOfEquipment>();
        private readonly IPieceOfEquipmentService<CallContext> _clientService;

        private readonly IAssetsServices<CallContext> _assetsService;

        private  PieceOfEquipment POE;
        public TestMoq(TestServerFixture testServerFixture)
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
            pagScale.Object = new FilterObject<PieceOfEquipment>();

            pagIndicator.Object.EntityFilter = filter2;
            pagScale.Object.EntityFilter = filter;
            pagIndicator.Object.EntityFilter.Customer = new Customer();
            pagScale.Object.EntityFilter.Customer = new Customer();

            pagIndicator.Show = 2;
            pagIndicator.Page = 1;

            pagScale.Show = 2;
            pagScale.Page = 1;

            POE = new PieceOfEquipment();
            POE.AddressId = 1;
            POE.PieceOfEquipmentID = "poetest";
            POE.EquipmentTemplate = new EquipmentTemplate();

        }

        //[Fact]
        public async void Test()
        {
            //var BasicsServiceMock = new Mock<IBasicsRepository>();

            //var ETMock = new Mock<EquipmentTemplate>();

           

            //  EquipmentTemplate et = new EquipmentTemplate();

            //  BasicsServiceMock.Setup(p => p.CreateEquipment(ETMock.Object)).ReturnsAsync(et);
            //var ET = new EquipmentTemplate();

            //var p = new PieceOfEquipment(ET);
            //p.Resolution = 100;
            //ET.Resolution = 10;
            //var poe = new Faker<PieceOfEquipment>();
            //poe.CustomInstantiator(f => p);

            //poe.RuleFor(x => x.Notes, notesexample);
            //poe.RuleFor(x => x.Resolution, Resolutionexample);

            //var poe1 = poe.Generate();

            ////poe1.Valid();

            //var ob = poe.ViewModel.PropertiesViewModel.Where(x => x.Key == "Resolution").FirstOrDefault();
            ////var obj =(ViewProperty<double>) ob.Value;
            //var res = ob.Value.IsVisible();
            


        }


        ViewProperty<string> notesexample(Faker f, PieceOfEquipment poe)
        {
            ViewProperty<string> r = new ViewProperty<string>();

            r.Value = "test";

            return r;
        }

        ViewProperty<double> Resolutionexample(Faker f, PieceOfEquipment poe)
        {

            if (poe.EquipmentTemplate != null && poe.IsToleranceImport)
            {
                poe.Resolution = poe.EquipmentTemplate.Resolution;
            }
            f.IsVisible = false;
            f.IsEnable = false;
            ViewProperty<double> r = new ViewProperty<double>();
            r.IsDisabled = true;
            r.IsVisible = true;
            r.Value = poe.Resolution;


            return r;
        }

        //[Fact]
        public async void PoeCreate()
        {
            //this.pieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader
            //    GetPieceOfEquipmentBySerial

            var POEServiceMock = new Mock<IPieceOfEquipmentService<CallContext>>();

            

           

            //poe.Setup(x => x.AddressId).Returns(1);


            var POEUseCaseMock = new Mock<PieceOfEquipmentUseCases>();

            var POERepoMock = new Mock<IPieceOfEquipmentRepository>();

            POERepoMock.Setup(x => x.GetPieceOfEquipmentByIDHeader(It.IsAny<string>())).ReturnsAsync(POE);

            POERepoMock.Setup(x => x.GetPieceOfEquipmentBySerial(It.IsAny<string>(),It.IsAny<int>())).ReturnsAsync(POE);

            //POERepoMock.Setup(x => x.InsertPieceOfEquipment(POE)).ReturnsAsync(POE);

            //POERepoMock.Setup(x => x.UpdatePieceOfEquipment(POE)).ReturnsAsync(POE);

            //var service = new PieceOfEquipmentUseCases(POERepoMock.Object,null);


            //var result = await service.CreatePieceOfEquipment(POE);


            //POERepoMock.Verify(s => s.GetPieceOfEquipmentByIDHeader(It.IsAny<string>()), Times.Never());


           // Assert.NotNull(result);

        }

        [Fact]
        public async void PoeCreateIDExistsSerialNoexist()
        {
            //this.pieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader
            //    GetPieceOfEquipmentBySerial

            var POEServiceMock = new Mock<IPieceOfEquipmentService<CallContext>>();

            var poe = new PieceOfEquipment();

            var poe2 = new PieceOfEquipment();

            poe2 = null;

            poe.AddressId = 1;
            poe.PieceOfEquipmentID = "poetest";
            //poe.Setup(x => x.AddressId).Returns(1);


            var POEUseCaseMock = new Mock<PieceOfEquipmentUseCases>();

            var POERepoMock = new Mock<IPieceOfEquipmentRepository>();

            POERepoMock.Setup(x => x.GetPieceOfEquipmentByIDHeader(It.IsAny<string>())).ReturnsAsync(poe);

            POERepoMock.Setup(x => x.GetPieceOfEquipmentBySerial(It.IsAny<string>(),It.IsAny<int>())).ReturnsAsync(poe2);

            //POERepoMock.Setup(x => x.InsertPieceOfEquipment(POE)).ReturnsAsync(POE);

            //POERepoMock.Setup(x => x.UpdatePieceOfEquipment(POE)).ReturnsAsync(POE);

            //var service = new PieceOfEquipmentUseCases(POERepoMock.Object,null);

             //Act
            //async Task act() => await service.CreatePieceOfEquipment(POE);


            //var result = await service.CreatePieceOfEquipment(poe);


            //POERepoMock.Verify(s => s.GetPieceOfEquipmentByIDHeader(It.IsAny<string>()), Times.Never());


            //await Assert.ThrowsAsync<ExistingException>(act);

        }


        [Fact]
        public async void PoeCreateIDNoExistsSerialexist()
        {
            //this.pieceOfEquipmentRepository.GetPieceOfEquipmentByIDHeader
            //    GetPieceOfEquipmentBySerial

            var POEServiceMock = new Mock<IPieceOfEquipmentService<CallContext>>();

            

            var poe2 = new PieceOfEquipment();

            poe2 = null;

           
            //poe.Setup(x => x.AddressId).Returns(1);


            var POEUseCaseMock = new Mock<PieceOfEquipmentUseCases>();

            var POERepoMock = new Mock<IPieceOfEquipmentRepository>();

            POERepoMock.Setup(x => x.GetPieceOfEquipmentByIDHeader(It.IsAny<string>())).ReturnsAsync(poe2);

            POERepoMock.Setup(x => x.GetPieceOfEquipmentBySerial(It.IsAny<string>(),It.IsAny<int>())).ReturnsAsync(POE);

            //POERepoMock.Setup(x => x.InsertPieceOfEquipment(POE)).ReturnsAsync(POE);

            //POERepoMock.Setup(x => x.UpdatePieceOfEquipment(POE)).ReturnsAsync(POE);

            //var service = new PieceOfEquipmentUseCases(POERepoMock.Object,null);


            //Act
           // async Task act() => await service.CreatePieceOfEquipment(POE);;


            //var result = await service.CreatePieceOfEquipment(poe);


            //POERepoMock.Verify(s => s.GetPieceOfEquipmentByIDHeader(It.IsAny<string>()), Times.Never());


            //await Assert.ThrowsAsync<ExistingException>(act);

        }

        [Fact]
        public async void PoeValidateN0InstallLocation()
        {

            var POEServiceMock = new Mock<IPieceOfEquipmentService<CallContext>>();



            var poe2 = new PieceOfEquipment();

            poe2 = null;

            POE.EquipmentTemplate.EquipmentTypeID = 3;
            //poe.Setup(x => x.AddressId).Returns(1);


            var POEUseCaseMock = new Mock<PieceOfEquipmentUseCases>();

            var POERepoMock = new Mock<IPieceOfEquipmentRepository>();

            POERepoMock.Setup(x => x.GetPieceOfEquipmentByIDHeader(It.IsAny<string>())).ReturnsAsync(poe2);

            POERepoMock.Setup(x => x.GetPieceOfEquipmentBySerial(It.IsAny<string>(),It.IsAny<int>())).ReturnsAsync(POE);

            //POERepoMock.Setup(x => x.InsertPieceOfEquipment(POE)).ReturnsAsync(POE);

            //POERepoMock.Setup(x => x.UpdatePieceOfEquipment(POE)).ReturnsAsync(POE);

            //var service = new PieceOfEquipmentUseCases(POERepoMock.Object,null);


            //Act
            //async Task act() => await service.CreatePieceOfEquipment(POE); 


            //var result = await service.CreatePieceOfEquipment(poe);


            //POERepoMock.Verify(s => s.GetPieceOfEquipmentByIDHeader(It.IsAny<string>()), Times.Never());


            //await Assert.ThrowsAsync<InvalidCalSaaSModel>(act);



        }
         [Fact]
        public async void PoeValidateInstallLocation()
        {

            var POEServiceMock = new Mock<IPieceOfEquipmentService<CallContext>>();



            var poe2 = new PieceOfEquipment();

            //poe2 = null;

            POE.EquipmentTemplate.EquipmentTypeID = 3;
            //poe.Setup(x => x.AddressId).Returns(1);
            POE.InstallLocation = "test";

            var POEUseCaseMock = new Mock<PieceOfEquipmentUseCases>();

            var POERepoMock = new Mock<IPieceOfEquipmentRepository>();

            POERepoMock.Setup(x => x.GetPieceOfEquipmentByIDHeader(It.IsAny<string>())).ReturnsAsync(poe2);

            POERepoMock.Setup(x => x.GetPieceOfEquipmentBySerial(It.IsAny<string>(),It.IsAny<int>())).ReturnsAsync(POE);

            //POERepoMock.Setup(x => x.InsertPieceOfEquipment(POE)).ReturnsAsync(POE);

            //POERepoMock.Setup(x => x.UpdatePieceOfEquipment(POE)).ReturnsAsync(POE);

            //var service = new PieceOfEquipmentUseCases(POERepoMock.Object,null);


            //Act
            //var result = await service.CreatePieceOfEquipment(POE); ;


            //var result = await service.CreatePieceOfEquipment(poe);


            //POERepoMock.Verify(s => s.GetPieceOfEquipmentByIDHeader(It.IsAny<string>()), Times.Never());
            //Assert.NotNull(result);

            //await Assert.ThrowsAsync<InvalidCalSaaSModel>(act);



        }

        [Fact]
        public async void PoeValidScaleCreate()
        {
            //var poe = new Faker<PieceOfEquipment>();
            //poe.CustomInstantiator(f => POE);

            //poe.RuleFor(x => x.Notes, notesexample);
            //poe.RuleFor(x => x.Resolution, Resolutionexample);
            //poe.RuleFor(x => x.InstallLocation, f=>f.Address.FullAddress());
            //poe.RuleFor(x => x.Notes, f=>f.Music.Genre());
            
            ////var poe1 = poe.Generate();

            //////poe1.Valid();

            ////var ob = poe.ViewModel.PropertiesViewModel.Where(x => x.Key == "Resolution").FirstOrDefault();
            //////var obj =(ViewProperty<double>) ob.Value;
            ////var res = ob.Value.IsVisible();

            ////Act
            //async Task act() => await _clientService.PieceOfEquipmentCreate(POE,CallContext.Default);

            
            //var b= await Assert.ThrowsAsync<RpcException>(act);



        
        }


    }
}
