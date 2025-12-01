using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using Grpc.Core;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Component = Helpers.Controls.Component;
using Microsoft.AspNetCore.Routing;
using CalibrationSaaS.Domain.Aggregates.Querys;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class BasicsServices :ServiceBase, IBasicsServices<CallContext>
    {
        private readonly BasicsUseCases  basicsLogic;
        private readonly ValidatorHelper modelValidator;
        private readonly ILogger _logger;
        public BasicsServices(BasicsUseCases basicsLogic, ILogger<BasicsServices> logger, ValidatorHelper modelValidator)
        {
            this.basicsLogic = basicsLogic;
            this.modelValidator = modelValidator;
            this._logger = logger;


        }


        #region EquipmentType




        public async  ValueTask<ResultSet<EquipmentType>> GetEquipmenTypesPag(Pagination<EquipmentType> Pagination, CallContext context = default)
        {
            var result = await basicsLogic.GetEquipmenTypesPag(Pagination);

            return result;
        }

        public async ValueTask<ResultSet<CalibrationType>> GetCalibrationTypesPag(Pagination<CalibrationType> Pagination, CallContext context = default)
        {
            var result = await basicsLogic.GetCalibrationTypesPag(Pagination);

            return result;
        }

        public async ValueTask<EquipmentTypeResultSet> GetEquipmenTypes(CallContext context = default)
        {

            var result = await basicsLogic.GetAllEquipmentType();

            return new EquipmentTypeResultSet { EquipmentTypes = result.ToList() };

           
        }

        public async ValueTask<EquipmentType> CreateEquipmentType(EquipmentType equipmentTypeDTO, CallContext context = default)
        {
            var result = await basicsLogic.CreateEquipmentType(equipmentTypeDTO);


            return equipmentTypeDTO;
        }

        public async ValueTask<EquipmentType> DeleteEquipmentType(EquipmentType equipmentTypeDTO, CallContext context = default)
        {
            var result = await basicsLogic.DeleteEquipmentType(equipmentTypeDTO);
            return equipmentTypeDTO;
        }

        public async Task UpdateEquipmentType(EquipmentType equipmentTypeDTO, CallContext context = default)
        {
            await basicsLogic.UpdateEquipmentType(equipmentTypeDTO);


        }

        public async Task<EquipmentTemplate> GetEquipmentXName(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            var result = await basicsLogic.GetEquipmentXName(EquipmentDTO);
            return result;
        }
        #endregion

        #region Manufacturer

        public async ValueTask<ManufacturerResultSet> GetAllManufacturers( CallContext context = default)
        {
            var ii = IsAuthenticate(context);
            
            var result = await basicsLogic.GetManufacturers();

            return new ManufacturerResultSet { Manufacturers = result.ToList() };

            //List<Manufacturer> result = new List<Manufacturer>();

            //var item = new Manufacturer { ManufacturerID = 1, Name = "Cyberdine", Abbreviation = "CD" };

            //result.Add(item);

            // item = new Manufacturer { ManufacturerID = 2, Name = "Hewlet Packard", Abbreviation = "HP" };

            //result.Add(item);

            //return new ManufacturerResultSet { Manufacturers = result.ToList() };
        }


        public async ValueTask<ResultSet<Manufacturer>> GetManufacturers(Pagination<Manufacturer> Pagination,CallContext context = default)
        {

             var ii = IsAuthenticate(context);           
            

            var result = await basicsLogic.GetAllManufacturers(Pagination);

            return result;//new ManufacturerResultSet { Manufacturers = result.ToList() };

            //List<Manufacturer> result = new List<Manufacturer>();

            //var item = new Manufacturer { ManufacturerID = 1, Name = "Cyberdine", Abbreviation = "CD" };

            //result.Add(item);

            // item = new Manufacturer { ManufacturerID = 2, Name = "Hewlet Packard", Abbreviation = "HP" };

            //result.Add(item);

            //return new ManufacturerResultSet { Manufacturers = result.ToList() };
        }

        public async ValueTask<Manufacturer> CreateManufacturer(Manufacturer manufacturerDTO, CallContext context= default)
        {
            var result = await basicsLogic.CreateManufacturer(manufacturerDTO);
           

            return result;
        }

        public async ValueTask<Manufacturer> DeleteManufacturer(Manufacturer manufacturerDTO, CallContext context = default)
        {
            var result = await basicsLogic.DeleteManufacturer(manufacturerDTO);
            return manufacturerDTO;
        }

        public async Task  UpdateManufacturer(Manufacturer manufacturerDTO, CallContext context = default)
        {
          await basicsLogic.UpdateManufacturer(manufacturerDTO);


        }


        #endregion

        #region Procedure

        public async ValueTask<ProcedureResultSet> GetAllProcedures(CallContext context = default)
        {
            var ii = IsAuthenticate(context);

            var result = await basicsLogic.GeProcedures();

            return new ProcedureResultSet { Procedures = result.ToList() };

            //List<Manufacturer> result = new List<Manufacturer>();

            //var item = new Manufacturer { ManufacturerID = 1, Name = "Cyberdine", Abbreviation = "CD" };

            //result.Add(item);

            // item = new Manufacturer { ManufacturerID = 2, Name = "Hewlet Packard", Abbreviation = "HP" };

            //result.Add(item);

            //return new ManufacturerResultSet { Manufacturers = result.ToList() };
        }


        public async ValueTask<ResultSet<Procedure>> GetProcedures(Pagination<Procedure> Pagination, CallContext context = default)
        {

            var ii = IsAuthenticate(context);


            var result = await basicsLogic.GetAllProcedures(Pagination);

            return result;
        }

        public async ValueTask<Procedure> CreateProcedure(Procedure ProcedureDTO, CallContext context = default)
        {
            var result = await basicsLogic.CreateProcedure(ProcedureDTO);


            return ProcedureDTO;
        }

        public async ValueTask<Procedure> DeleteProcedure(Procedure pocedureDTO, CallContext context = default)
        {
            var result = await basicsLogic.DeleteProcedure(pocedureDTO);
            return pocedureDTO;
        }

        public async Task UpdateProcedure(Procedure procedureDTO, CallContext context = default)
        {
            await basicsLogic.UpdateProcedure(procedureDTO);


        }

        public async Task<Procedure> GetProcedureXName(Procedure ProcedureDTO, CallContext context)
        {
            var result = await basicsLogic.GetProcedureXName(ProcedureDTO);
            return result;
        }

        #endregion

        #region EquipmetTemplate

        public async ValueTask<EquipmentTemplate> CreateEquipment(EquipmentTemplate EquipmentDTO, CallContext context = default)
        {
            

            if (modelValidator.IsValid(EquipmentDTO))
            {

                var result = await basicsLogic.CreateEquipment(EquipmentDTO);
                //var result = await customerLogic.CreateCustomer(customerDTO);
                return result;
            }
            return null;

        }

        public async ValueTask<ResultSet<EquipmentTemplate>> GetEquipment(Pagination<EquipmentTemplate> pagination,CallContext context = default)
        {
            var resultlist = await basicsLogic.GetEquipment(pagination);

            return resultlist;

        //    if (resultlist != null) 
        //    {
        //        EquipmentResultSet result = new EquipmentResultSet
        //        {
        //            EquipmentTemplates = resultlist.ToList()
        //      };

        //        return result;
        //    }           
       
        //else
        //    {
        
        //return resultlist;
        
        //}

            




            //List<TestPoint> list = new List<TestPoint>();
            //list.Add(new TestPoint { TestPointID = 1, UnitOfMeasurementID = "2" , NominalTestPoit = 3 });
            //list.Add(new TestPoint { TestPointID = 2, UnitOfMeasurementID = "1", NominalTestPoit = 10 });

            //TestPointGroup group = new TestPointGroup
            //{
            //    TestPoitGroupID = 1,
            //    TestPoints = list,
            //    Name = "test group name",
            //    OutUnitOfMeasurementID = "1",


            //};
            //List<TestPointGroup> listgroups = new List<TestPointGroup>();

            //listgroups.Add(group);

            //List<EquipmentTemplate> result = new List<EquipmentTemplate> {
            //    new EquipmentTemplate
            //    {
            //        EquipmentTemplateID = 23,
            //        Model="xsdfd",
            //        Name = "HP",
            //        Description= "Errhhhdh dddd  f  f f f tttttt",
            //        IsEnabled = true,
            //        StatusID="1",
            //        EquipmentTypeID=23,
            //        TestGroups = listgroups,
            //        ManufacturerID=1,
            //        Status="1",

            //    },
            //    new EquipmentTemplate
            //    {
            //        EquipmentTemplateID = 230,
            //        Description= "Errhhhdh dddd  f  f f f tttttt",
            //        Name = "DataLogic",
            //        IsEnabled = true,
            //        StatusID="1",
            //    },
            //     new EquipmentTemplate
            //    {
            //        EquipmentTemplateID = 250,
            //        Abbreviation="Term",
            //        Model="T-1000",
            //        Manufacturer="Cyberdine",
            //        Name = "Motorola",
            //        IsEnabled = false,
            //        StatusID=2.ToString(),
            //    },
            //    new EquipmentTemplate
            //    {
            //        EquipmentTemplateID = 2300,
            //        Description= "Errhhhdh dddd  f  f f f tttttt",
            //        Name = "Zebra",
            //        IsEnabled = true,
            //        StatusID=1.ToString(),
            //    },
            //    new EquipmentTemplate
            //    {
            //        EquipmentTemplateID = 231,
            //        Description= "descr",
            //        Name = "Zebra2",
            //        IsEnabled = true,
            //        StatusID=1.ToString(),
            //    }
            //    ,
            //    new EquipmentTemplate
            //    {
            //        EquipmentTemplateID = 233,
            //        Description= "descr",
            //        Name = "Zebra23",
            //        IsEnabled = true,
            //        StatusID=1.ToString(),
            //    }
            //};

            //return new EquipmentResultSet { EquipmentTemplates = result };
        }

        public async ValueTask<EquipmentTemplate> DeleteEquipment(EquipmentTemplate EquipmentDTO, CallContext context = default)
        {

            return await basicsLogic.DeleteEquipment(EquipmentDTO);

            //return new EquipmentTemplate
            //{
            //    EquipmentTemplateID = 23,
            //    Model = "xsdfd",
            //    Name = "HP",
            //    Description = "Errhhhdh dddd  f  f f f tttttt",
            //    IsEnabled = true,
            //    StatusID = "1",
            //    EquipmentTypeID = 23,


            //};
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
        public async ValueTask<EquipmentTemplate> GetEquipmentByID(EquipmentTemplate EquipmentDTO, CallContext context = default)
        {
        
                var result = await basicsLogic.GetEquipmentByID(EquipmentDTO, GetComponent( context));
                return result;
          
            

           
        }




        public async ValueTask<TestPointResultSet> GetTespoint(TestPointGroup DTO, CallContext context)
        {
            var res=  await basicsLogic.GetTespoint(DTO);

            TestPointResultSet result = new TestPointResultSet
            {

                TestPointList = res.ToList()
            };

            return result;


        }

        public async  ValueTask<TestPointGroupResultSet> GetTespointByEquipment(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            var res= await basicsLogic.GetTespointByEquipment(EquipmentDTO);
            TestPointGroupResultSet result = new TestPointGroupResultSet
            {

                TestPointGroupList = res.ToList()
            };

            return result;

        }

        public async ValueTask<TestPointGroupResultSet> GetTespointGroup(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            var res = await basicsLogic.GetTespointGroup(EquipmentDTO);

            TestPointGroupResultSet result = new TestPointGroupResultSet
            {

                TestPointGroupList = res.ToList()
            };

            return result;
        }

        public async ValueTask<TestPointGroup> InsertTestPointGroup(TestPointGroup DTO, CallContext context)
        {
            return await basicsLogic.InsertTestPointGroup(DTO);
        }






        #endregion



        #region Roles


        //public async ValueTask<RolResultSet> GetRoles(CallContext context = default)
        //{

        //    var result = await basicsLogic.GetAllRol();

        //    return new RolResultSet { Roles = result.ToList() };
        //}

        public async ValueTask<Rol> AddRol(Rol rol, CallContext context = default)
        {

            var result = await basicsLogic.AddRol(rol);

            

            return result;
        }


        public async ValueTask<Rol> DeleteRole(Rol rol, CallContext context = default)
        {

            var result = await basicsLogic.DeleteRole(rol);

            List<Rol> r = new List<Rol>();

            return result;
        }


        #endregion

        #region Technicians
        public async ValueTask<UserResultSet> GetUsers(CallContext context = default)
        {

            var result = await basicsLogic.GetAllUser();

            return new UserResultSet { Users = result.ToList() };
        }

        public async ValueTask<User> GetUserById(User userDTO, CallContext context = default)
        {

            var result = await basicsLogic.GetUserById(userDTO);
            return result;
        }

        public async ValueTask<User> GetUserById2(User userDTO, CallContext context = default)
        {

            var result = await basicsLogic.GetUserById2(userDTO);

            if(result == null)
            {
                return new User();
            }

            if(result?.UserRoles != null)
            {
                foreach (var item in result.UserRoles)
                {
                    item.User = null;
                }

            }
           
            if(result?.TechnicianCodes != null)
            {
                foreach (var item in result.TechnicianCodes)
                {
                    item.User = null;
                }
            }           

            return result;
        }


     

        public async ValueTask<User> CreateUser(User userDTO, CallContext context = default)
        {

            var result = await basicsLogic.CreateUser(userDTO, userDTO.IsFromAPI);

            if (result.UserRoles != null)
            {
                foreach (var item in result.UserRoles)
                {
                    item.User = null;
                }

            }

            if (result.TechnicianCodes != null)
            {
                foreach (var item in result.TechnicianCodes)
                {
                    item.User = null;
                }
            }



            return result;
        }

        public async ValueTask<User> DeleteUser(User userDTO, CallContext context = default)
        {
            var result = await basicsLogic.DeleteUser(userDTO);
            return userDTO;
        }

        public async Task UpdateUser(User userDTO, CallContext context = default)
        {
            await basicsLogic.UpdateUser(userDTO);


        }

        

        #endregion

        #region Contacts
        public async ValueTask<ContactResultSet> GetContacts(CallContext context = default)
        {

            var result = await basicsLogic.GetAllContact();

            return new ContactResultSet { Contacts = result.ToList() };
        }

        public async ValueTask<Contact> CreateContact(Contact contactDTO, CallContext context = default)
        {
            var result = await basicsLogic.CreateContact(contactDTO);


            return contactDTO;
        }

        public async ValueTask<Contact> DeleteContact(Contact contactDTO, CallContext context = default)
        {
            var result = basicsLogic.DeleteContact(contactDTO);
            return contactDTO;
        }

        public async Task UpdateContact(Contact contactDTO, CallContext context = default)
        {
            await basicsLogic.UpdateContact(contactDTO);


        }

        #endregion

        #region Address
        public async ValueTask<AddressResultSet> GetAddress(CallContext context = default)
        {

            var result = await basicsLogic.GetAllAddress();

            return new AddressResultSet { Addresses = result.ToList() };
        }

        public async ValueTask<Address> CreateAddress(Address addresDTO, CallContext context = default)
        {
            var result = await basicsLogic.CreateAddress(addresDTO);


            return addresDTO;
        }

        public async ValueTask<Address> DeleteAddress(int id, CallContext context = default)
        {
            var result = await basicsLogic.DeleteAddress(id);
            return result;
        }

        public async Task UpdateAddress(Address addressDTO, CallContext context = default)
        {
            await basicsLogic.UpdateAddress(addressDTO);

        }

        #endregion

        #region Status
        public async ValueTask<StatusResultSet> GetStatus(CallContext context = default)
        {

            var result = await basicsLogic.GetAllStatus();

            return new StatusResultSet { Status = result.ToList() };

           
        }
        #endregion

        #region Rol
        public async ValueTask<RolResultSet> GetRoles(CallContext context = default)
        {

            var result = await basicsLogic.GetAllRoles();

            return new RolResultSet { Roles = result.ToList() };



        }

        public async ValueTask<EquipmentType> GetEquipmentTypeByID(EquipmentType DTO)
        {
            var result = await basicsLogic.GetEquipmentTypeByID(DTO);

            return result;

        }

        public async ValueTask<EquipmentType> GetEquipmentTypeByID(EquipmentType DTO, CallContext context)
        {
            var result = await basicsLogic.GetEquipmentTypeByID(DTO);

            return result;
        }

        public async ValueTask<ResultSet<User>> GetUsersPag(Pagination<User> Pagination, CallContext _context)
        {
            var result = await basicsLogic.GetUserPag(Pagination);

            return result;
        }

        public async ValueTask<IEnumerable<Certification>> GetCertifications(CallContext context)
        {
            var result = await basicsLogic.GetCertifications();

            return result;
        }


        public async ValueTask<IEnumerable<ToleranceType>> GetToleranceTypes(CallContext context)
        {
            var result = await basicsLogic.GetToleranceTypes();

            return result;
        }

        public async ValueTask<ResultSet<TechnicianCode>> GetTechnicianCodePag(Pagination<TechnicianCode> Pagination, CallContext _context)
        {
            var result = await basicsLogic.GetTechnicianCodePag(Pagination);

            if(result?.List?.Count > 0)
            {
                foreach (var item in result.List)
                {
                   
                    item.User = null;
                    item.Certification.TechnicianCodes = null;
                }
            }
          
            return result;
        }

        public async ValueTask<TechnicianCode> CreateTechnicianCode(TechnicianCode DTO, CallContext _context)
        {
            var result = await basicsLogic.CreateTechnicianCode(DTO);

            return result;
        }

        public async ValueTask<TechnicianCode> DeleteTechnicianCode(TechnicianCode DTO, CallContext _context)
        {
            var result = await basicsLogic.DeleteTechnicianCode(DTO);

            return result;
        }

        public async Task<Component> DeleteComponent(Component Component, CallContext context)
        {
            var result = await basicsLogic.DeleteComponent(Component);

            return result;
        }

        public async  Task<ResultSet<Component>> CreateComponents(ICollection<Component> Component, CallContext context)
        {
            var result = await basicsLogic.CreateComponents(Component);

            return result;
        }

        public async Task<ResultSet<Component>> GetAllComponents(CallContext context)
        {
            var result = await basicsLogic.GetAllComponents();

            return result;
        }

        public async ValueTask<Component> GetComponentByRoute(string route)
        {
            var result = await basicsLogic.GetComponentByRoute(route);

            return result;
        }

        public async ValueTask<User> Enable(User userDTO, CallContext context = default)
        {
            var result = await basicsLogic.Enable(userDTO);

            if (result)
            {
                return userDTO;
            }
            else
            {
                return null;
            }
        }

        public async ValueTask<User> Reset(User userDTO, CallContext context = default)
        {
            var result = await basicsLogic.Reset(userDTO);
            if (result)
            {
                return userDTO;
            }
            else
            {
                return null;
            }
            
        }

        public async Task<Manufacturer> GetManufacturerXName(Manufacturer manufacturerDTO, CallContext context)
        {
            var result = await basicsLogic.GetManufacturerXName(manufacturerDTO);
            return result;
        }

        public async ValueTask<ResultSet<EquipmentTypeGroup>> GetEquipmentTypeGroups(CallContext context)
        {
            var result = await basicsLogic.GetEquipmentTypeGroups();

            return result;
        }

        #endregion

        #region CMCValues
        public async ValueTask<List<CMCValues>> GetCMCValuesByCalibrationType(CalibrationType DTO, CallContext context = default)
        {

            var result = await basicsLogic.GetCMCValuesByCalibrationType(DTO);

            return result;

        }

        public async ValueTask<CMCValues> GetCMCValuesById(CMCValues DTO, CallContext context = default)
        {

            var result = await basicsLogic.GetCMCValuesByID(DTO);

            return result;

        }

        public async ValueTask<CalibrationType> GetCalibrationTypeById(CalibrationType DTO, CallContext context = default)
        {

            var result = await basicsLogic.GetCalibrationTypeByID(DTO);

            return result;

        }
        public async ValueTask<CalibrationType> InsertCMCValue(CalibrationType DTO, CallContext context = default)
        {
            var result = await basicsLogic.InsertCMCValue(DTO);


            return result;
        }


        public async ValueTask<CMCValues> DeleteCMCValue(CMCValues DTO, CallContext context = default)
        {
            var result = await basicsLogic.DeleteCMCValue(DTO);
            return result;
        }

        public async ValueTask<CMCValues> UpdateCMCValue(CMCValues DTO, CallContext context = default)
        {
            var result = await basicsLogic.UpdateCMCValue(DTO);
            return result;


        }
        #endregion
    }
}
