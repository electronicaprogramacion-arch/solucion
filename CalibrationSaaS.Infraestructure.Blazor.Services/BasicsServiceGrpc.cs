using Blazor.IndexedDB.Framework;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Grpc.Core;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Services
{
    public class BasicsServiceGRPC : IDisposable, IBasicsServices<CallContext>
    {

        private CallContext context;


        private readonly dynamic DbFactory;

        private readonly IBasicsServices<CallContext> service;


        public BasicsServiceGRPC(Func<dynamic, Application.Services.IBasicsServices<CallContext>> _service, dynamic _DbFactory)
        {
            DbFactory = _DbFactory;
            service = _service(DbFactory);
            context = new CallOptions();
        }

        public BasicsServiceGRPC(Func<dynamic, Application.Services.IBasicsServices<CallContext>> _service, dynamic _DbFactory, CallOptions _context)
        {
            DbFactory = _DbFactory;
            service = _service(DbFactory);
            context = _context;
        }



        public BasicsServiceGRPC(IBasicsServices<CallContext> _service)
        {
            service = _service;
            context = new CallOptions();
        }

        public BasicsServiceGRPC(IBasicsServices<CallContext> _service, CallContext _context)
        {
            service = _service;
            context = new CallOptions();
        }

        public BasicsServiceGRPC(IBasicsServices<CallContext> _service, CallOptions _context)
        {
            service = _service;
            context = _context;
        }

        public void Dispose()
        {

        }

        public ValueTask<EquipmentType> CreateEquipmentType(EquipmentType equipmentTypeDTO, CallContext _context = default)
        {
            return service.CreateEquipmentType(equipmentTypeDTO, context);
        }

        public async ValueTask<EquipmentTypeResultSet> GetEquipmenTypes(CallContext _context = default)
        {
            return await service.GetEquipmenTypes(context);
        }

         
        public async ValueTask<ResultSet<EquipmentType>> GetEquipmenTypesPag(Pagination<EquipmentType> Pagination, CallContext _context = default)
        {
            return await service.GetEquipmenTypesPag(Pagination, context);
        }

        public async ValueTask<ResultSet<CalibrationType>> GetCalibrationTypesPag(Pagination<CalibrationType> Pagination, CallContext _context = default)
        {
            return await service.GetCalibrationTypesPag(Pagination, context);
        }

        public async ValueTask<EquipmentType> DeleteEquipmentType(EquipmentType equipmentTypeDTO, CallContext _context = default)
        {
            return await service.DeleteEquipmentType(equipmentTypeDTO, context);
        }

        #region Manufacturer
        public async ValueTask<Manufacturer> CreateManufacturer(Manufacturer manufacturerDTO, CallContext _context = default)
        {
            return await service.CreateManufacturer(manufacturerDTO, context);
        }

        public ValueTask<ResultSet<Manufacturer>> GetManufacturers(Pagination<Manufacturer> Pagination, CallContext _context = default)
        {
            return service.GetManufacturers(Pagination);
        }

        public ValueTask<Manufacturer> DeleteManufacturer(Manufacturer manufacturerDTO, CallContext _context = default)
        {
            return service.DeleteManufacturer(manufacturerDTO, context);
        }

        public Task UpdateManufacturer(Manufacturer manufacturerDTO, CallContext _context = default)
        {
            return service.UpdateManufacturer(manufacturerDTO, context);
        }
        #endregion

        #region Procedure
        public async ValueTask<Procedure> CreateProcedure(Procedure procedureDTO, CallContext _context = default)
        {
            return await service.CreateProcedure(procedureDTO, context);
        }

        public ValueTask<ResultSet<Procedure>> GetProcedures(Pagination<Procedure> Pagination, CallContext _context = default)
        {
            return service.GetProcedures(Pagination);
        }

        public ValueTask<Procedure> DeleteProcedure(Procedure procedureDTO, CallContext _context = default)
        {
            return service.DeleteProcedure(procedureDTO, context);
        }

        public Task UpdateProcedure(Procedure procedureDTO, CallContext _context = default)
        {
            return service.UpdateProcedure(procedureDTO, context);
        }
        public async ValueTask<ProcedureResultSet> GetAllProcedures(CallContext _context = default)
        {
            return await service.GetAllProcedures(context);
        }


        #endregion

        public ValueTask<EquipmentTemplate> CreateEquipment(EquipmentTemplate EquipmentDTO, CallContext _context = default)
        {
            return service.CreateEquipment(EquipmentDTO, context);
        }

        public async ValueTask<ResultSet<EquipmentTemplate>> GetEquipment(Pagination<EquipmentTemplate> pagination, CallContext _context = default)
        {
            return await service.GetEquipment(pagination, context);
        }

        public ValueTask<EquipmentTemplate> DeleteEquipment(EquipmentTemplate EquipmentDTO, CallContext _context = default)
        {
            return service.DeleteEquipment(EquipmentDTO, context);
        }

        public async ValueTask<EquipmentTemplate> GetEquipmentByID(EquipmentTemplate EquipmentDTO, CallContext _context = default)
        {
            var result = await service.GetEquipmentByID(EquipmentDTO, context);
            return result;
        }

        public async Task<EquipmentTemplate> GetEquipmentXName(EquipmentTemplate EquipmentDTO, CallContext _context = default)
        {
            var result = await service.GetEquipmentXName(EquipmentDTO, context);
            return result;
        }



        public async ValueTask<TestPointResultSet> GetTespoint(TestPointGroup DTO, CallContext _context = default)
        {
            return await service.GetTespoint(DTO, context);
        }

        public async ValueTask<TestPointGroupResultSet> GetTespointByEquipment(EquipmentTemplate EquipmentDTO, CallContext _context = default)
        {
            return await service.GetTespointByEquipment(EquipmentDTO, context);
        }

        public async ValueTask<TestPointGroupResultSet> GetTespointGroup(EquipmentTemplate EquipmentDTO, CallContext _context = default)
        {
            return await service.GetTespointGroup(EquipmentDTO, context);
        }

        public async ValueTask<TestPointGroup> InsertTestPointGroup(TestPointGroup DTO, CallContext _context = default)
        {
            return await service.InsertTestPointGroup(DTO, context);
        }

        #region Technicians
        public ValueTask<User> CreateUser(User userDTO, CallContext _context = default)
        {
            var result = service.CreateUser(userDTO, context);
            return result;
        }

        public async ValueTask<User> GetUserById(User userDTO, CallContext _context = default)
        {
            return await service.GetUserById(userDTO, context);
        }
        public async ValueTask<User> GetUserById2(User userDTO, CallContext _context = default)
        {
            var result = await service.GetUserById2(userDTO, context);
            return result;
        }
        public async ValueTask<UserResultSet> GetUsers(CallContext _context = default)
        {
            return await service.GetUsers(context);
        }

        public ValueTask<ResultSet<User>> GetUsersPag(Pagination<User> Pagination, CallContext _context = default)
        {
            return service.GetUsersPag(Pagination, context);
        }


        public ValueTask<User> DeleteUser(User userDTO, CallContext _context = default)
        {
            return service.DeleteUser(userDTO, context);
        }

        public Task UpdateUser(User userDTO, CallContext _context = default)
        {
            return service.UpdateUser(userDTO, context);
        }
        #endregion

        #region Contacts
        public ValueTask<Contact> CreateContact(Contact contactDTO, CallContext _context = default)
        {
            return service.CreateContact(contactDTO, context);
        }

        public ValueTask<ContactResultSet> GetContacts(CallContext _context = default)
        {
            return service.GetContacts(context);
        }

        public ValueTask<Contact> DeleteContact(Contact contactDTO, CallContext _context = default)
        {
            return service.DeleteContact(contactDTO, context);
        }

        public Task UpdateContact(Contact contactDTO, CallContext _context = default)
        {
            return service.UpdateContact(contactDTO, context);
        }
        #endregion

        #region Status
        public ValueTask<StatusResultSet> GetStatus(CallContext _context = default)
        {
            return service.GetStatus(context);
        }
        #endregion

        #region Rol
        public async ValueTask<RolResultSet> GetRoles(CallContext _context = default)
        {
            return await service.GetRoles(context);
        }

        public async ValueTask<ManufacturerResultSet> GetAllManufacturers(CallContext _context = default)
        {
            return await service.GetAllManufacturers(context);
        }

        //public ValueTask<EquipmentType> GetEquipmentTypeByID(EquipmentType DTO)
        //{
        //    return service.GetEquipmentTypeByID(DTO, context);
        //}

        public async ValueTask<EquipmentType> GetEquipmentTypeByID(EquipmentType DTO, CallContext _context = default)
        {
            return await service.GetEquipmentTypeByID(DTO, context);
        }

        public async ValueTask<IEnumerable<Certification>> GetCertifications(CallContext _context = default)
        {
            return await service.GetCertifications(context);
        }

        public async ValueTask<ResultSet<TechnicianCode>> GetTechnicianCodePag(Pagination<TechnicianCode> Pagination, CallContext _context = default)
        {
            var res = await service.GetTechnicianCodePag(Pagination, context);
            return res;
        }

        public async ValueTask<TechnicianCode> CreateTechnicianCode(TechnicianCode DTO, CallContext _context = default)
        {
            return await service.CreateTechnicianCode(DTO, context);
        }

        public async ValueTask<TechnicianCode> DeleteTechnicianCode(TechnicianCode DTO, CallContext _context = default)
        {
            return await service.DeleteTechnicianCode(DTO, context);
        }

        public async ValueTask<Rol> DeleteRole(Rol rol, CallContext context = default)
        {
            return await service.DeleteRole(rol, context);
        }

        public async ValueTask<Rol> AddRol(Rol rol, CallContext _context = default)
        {
            return await service.AddRol(rol, context = default);
        }

        public async ValueTask<IEnumerable<ToleranceType>> GetToleranceTypes(CallContext context=default)
        {
            return await service.GetToleranceTypes( context = default);
        }

        public async ValueTask<User> Enable(User userDTO, CallContext context = default)
        {
            return await service.Enable(userDTO,context = default);
        }

        public async ValueTask<User> Reset(User userDTO, CallContext context = default)
        {
            return await service.Reset(userDTO,context = default);
        }

        public async Task<Manufacturer> GetManufacturerXName(Manufacturer DTO, CallContext context = default)
        {
            return await service.GetManufacturerXName(DTO,  context = default);
        }

        public async Task<Procedure> GetProcedureXName(Procedure ProcedureDTO, CallContext context)
        {
           
                var result = await service.GetProcedureXName(ProcedureDTO, context);
                return result;
            
        }

        public async ValueTask<ResultSet<EquipmentTypeGroup>> GetEquipmentTypeGroups(CallContext context)
        {
            var result = await service.GetEquipmentTypeGroups( context);
            return result;
        }

        //public async Task<Component> DeleteComponent(Component Component, CallContext context = default)
        //{
        //    return await service.DeleteComponent(Component, context);
        //}

        //public async Task<ResultSet<Component>> CreateComponents(ICollection<Component> Component, CallContext context = default)
        //{
        //    return await service.CreateComponents(Component, context);

        //}

        //public async Task<ResultSet<Component>> GetAllComponents(CallContext context = default)
        //{
        //    return await service.GetAllComponents(context);
        //}

        //public async ValueTask<Component> GetComponentByRoute(string route)
        //{
        //    return await service.GetComponentByRoute(route);
        //}
        #endregion

        #region CMCValues
        public async ValueTask<List<CMCValues>> GetCMCValuesByCalibrationType(CalibrationType DTO, CallContext context = default)
        {

            var result = await service.GetCMCValuesByCalibrationType(DTO, context);

            return result;

        }

        public async ValueTask<CMCValues> GetCMCValuesById(CMCValues DTO, CallContext context = default)
        {

            var result = await service.GetCMCValuesById(DTO, context);

            return result;

        }
        public async ValueTask<CalibrationType> GetCalibrationTypeById(CalibrationType DTO, CallContext context = default)
        {

            var result = await service.GetCalibrationTypeById(DTO, context);

            return result;

        }
        public async ValueTask<CalibrationType> InsertCMCValue(CalibrationType DTO, CallContext context = default)
        {
            var result = await service.InsertCMCValue(DTO, context);


            return result;
        }

        public async ValueTask<CMCValues> DeleteCMCValue(CMCValues DTO, CallContext context = default)
        {
            var result = await service.DeleteCMCValue(DTO, context);
            return result;
        }

        public async ValueTask<CMCValues> UpdateCMCValue(CMCValues DTO, CallContext context = default)
        {
            var result = await service.UpdateCMCValue(DTO, context);
            return result;


        }
        #endregion
    }
}
