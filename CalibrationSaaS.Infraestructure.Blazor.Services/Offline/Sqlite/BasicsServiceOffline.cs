using IndexedDB.Blazor;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
//using Blazored.LocalStorage;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;

using static SQLite.SQLite3;
using Helpers.Controls.ValueObjects;
using Helpers.Controls;
using SqliteWasmHelper;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
    public class BasicsServiceOffline<TContext> : Application.Services.IBasicsServices<CallContext>
        where TContext:DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly ISqliteWasmDbContextFactory<TContext> DbFactory;

        public BasicsServiceOffline(ISqliteWasmDbContextFactory<TContext> dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }

        public async ValueTask<Rol> AddRol(Rol rol, CallContext context)
        {

            using (var db = await this.DbFactory.CreateDbContextAsync())
            {

                db.Rol.Add(rol);

                await db.SaveChangesAsync();

                return rol;
            
            }



        }

        public Task<ResultSet<Helpers.Controls.Component>> CreateComponents(ICollection<Helpers.Controls.Component> Component, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Contact> CreateContact(Contact contactDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<EquipmentTemplate> CreateEquipment(EquipmentTemplate DTO, CallContext context)
        {
            var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res=await eq.CreateEquipment(DTO);

            return res;

        }

        public ValueTask<EquipmentType> CreateEquipmentType(EquipmentType equipmentTypeDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Manufacturer> CreateManufacturer(Manufacturer DTO, CallContext context)
        {
             var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            DTO.IsOffline = true;

            var res=await eq.CreateManufacturer(DTO);

            return res;
        }

        public ValueTask<Procedure> CreateProcedure(Procedure procedureDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TechnicianCode> CreateTechnicianCode(TechnicianCode DTO, CallContext _context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<User> CreateUser(User userDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<Helpers.Controls.Component> DeleteComponent(Helpers.Controls.Component Component, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Contact> DeleteContact(Contact contactDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<EquipmentTemplate> DeleteEquipment(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<EquipmentType> DeleteEquipmentType(EquipmentType equipmentTypeDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Manufacturer> DeleteManufacturer(Manufacturer manufacturerDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Procedure> DeleteProcedure(Procedure procedureDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Rol> DeleteRole(Rol rol, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TechnicianCode> DeleteTechnicianCode(TechnicianCode DTO, CallContext _context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<User> DeleteUser(User userDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<User> Enable(User userDTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<ResultSet<Helpers.Controls.Component>> GetAllComponents(CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<ManufacturerResultSet> GetAllManufacturers(CallContext context)
        {
            var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res = await eq.GetAllManufacturers();

            ManufacturerResultSet   mr= new ManufacturerResultSet();

            mr.Manufacturers = res.List;

            return mr;
        }

        public ValueTask<ProcedureResultSet> GetAllProcedures(CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<IEnumerable<Certification>> GetCertifications(CallContext context)
        {
            using (var db = await this.DbFactory.CreateDbContextAsync())
            {
                return await Task.FromResult(db.Certification.ToList());

            }



        }

        public ValueTask<Helpers.Controls.Component> GetComponentByRoute(string route)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ContactResultSet> GetContacts(CallContext context)
        {
            throw new NotImplementedException();
        }
        public string GetComponent(CallContext context)
        {
            var header = context.RequestHeaders.Where(x => x.Key.ToLower() == "component").FirstOrDefault();
            //var user = context.ServerCallContext.GetHttpContext();

            string com = "";

            if (header != null)
            {
                com = header.Value;


            }

            return com;
        }
        public async ValueTask<ResultSet<EquipmentTemplate>> GetEquipment(Pagination<EquipmentTemplate> pagination, CallContext context)
        {
            //var DTO = pagination.Entity;

            //var filterQuery = Querys.WorkOrderFilter(pagination.Filter);
             var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res=await eq.GetEquipment(pagination);

            return res;
            
        }

        public async ValueTask<EquipmentTemplate> GetEquipmentByID(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res=await eq.GetEquipmentByID(EquipmentDTO, GetComponent(context));

            return res;
        }

        public ValueTask<EquipmentType> GetEquipmentTypeByID(EquipmentType DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<EquipmentTypeGroup>> GetEquipmentTypeGroups(CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task<EquipmentTemplate> GetEquipmentXName(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<EquipmentTypeResultSet> GetEquipmenTypes(CallContext context)
        {
              var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res=await eq.GetAllEquipmentType();


            EquipmentTypeResultSet rt = new EquipmentTypeResultSet();

            rt.EquipmentTypes = res.ToList();

            return rt;
        }

        public async  ValueTask<ResultSet<EquipmentType>> GetEquipmenTypesPag(Pagination<EquipmentType> Pagination, CallContext _context)
        {
               var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res=await eq.GetEquipmenTypesPag(Pagination);


            return res;
        }
        public async ValueTask<ResultSet<CalibrationType>> GetCalibrationTypesPag(Pagination<CalibrationType> Pagination, CallContext _context)
        {
            var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res = await eq.GetCalibrationTypesPag(Pagination);


            return res;
        }
        public async ValueTask<ResultSet<Manufacturer>> GetManufacturers(Pagination<Manufacturer> Pagination, CallContext context)
        {
             var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res=await eq.GetAllManufacturers(Pagination);


            return res;
        }

        public Task<Manufacturer> GetManufacturerXName(Manufacturer manufacturerDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<Procedure>> GetProcedures(Pagination<Procedure> Pagination, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task<Procedure> GetProcedureXName(Procedure ProcedureDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<RolResultSet> GetRoles(CallContext context)
        {
            var result = new RolResultSet();

            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{
            //    result.Roles = db.Rol.ToList();
                
            //    return await Task.FromResult(result); 
            
            //}
            var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res=await eq.GetAllRoles();

            result.Roles = res.ToList();
            return result;



        }

        public async ValueTask<StatusResultSet> GetStatus(CallContext context)
        {
            var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var result = await eq.GetAllStatus();

            return new StatusResultSet { Status = result.ToList() };




        }

        public ValueTask<ResultSet<TechnicianCode>> GetTechnicianCodePag(Pagination<TechnicianCode> Pagination, CallContext _context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TestPointResultSet> GetTespoint(TestPointGroup DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TestPointGroupResultSet> GetTespointByEquipment(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TestPointGroupResultSet> GetTespointGroup(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<IEnumerable<ToleranceType>> GetToleranceTypes(CallContext context)
        {
            var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res = await eq.GetToleranceTypes();


            return res;
        }

        public ValueTask<User> GetUserById(User userDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<User> GetUserById2(User userDTO, CallContext context)
        {
            var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res = await eq.GetUserById2(userDTO);

           
            return res;
        }

        public async ValueTask<UserResultSet> GetUsers(CallContext context)
        {
             var eq = new BasicsRepositoryEF<TContext>(DbFactory);

            var res= await eq.GetAllUser();

            return new UserResultSet() { Users = (List<User>)res };

        }

        public ValueTask<ResultSet<User>> GetUsersPag(Pagination<User> Pagination, CallContext _context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TestPointGroup> InsertTestPointGroup(TestPointGroup DTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<User> Reset(User userDTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateContact(Contact contactDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task UpdateManufacturer(Manufacturer manufacturerDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProcedure(Procedure ProcedureDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(User userDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        #region CMCValues
        public async ValueTask<List<CMCValues>> GetCMCValuesByCalibrationType(CalibrationType DTO, CallContext context = default)
        {

            throw new NotImplementedException();

        }

        public async ValueTask<CMCValues> GetCMCValuesById(CMCValues DTO, CallContext context = default)
        {
            throw new NotImplementedException();

        }
        public async ValueTask<CalibrationType> GetCalibrationTypeById(CalibrationType DTO, CallContext context = default)
        {
            var eq = new BasicsRepositoryEF<TContext>(DbFactory);

           var result= await  eq.GetCalibrationTypeByID(DTO);

            return result;

        }
        public async ValueTask<CalibrationType> InsertCMCValue(CalibrationType DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<CMCValues> DeleteCMCValue(CMCValues DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<CMCValues> UpdateCMCValue(CMCValues DTO, CallContext context = default)
        {
            throw new NotImplementedException();

        }
        #endregion
    }
}
