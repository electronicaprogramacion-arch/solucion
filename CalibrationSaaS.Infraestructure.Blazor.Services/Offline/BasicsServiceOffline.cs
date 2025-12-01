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
using Helpers.Controls.ValueObjects;
using Helpers.Controls;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline
{
    public class BasicsServiceOffline : Application.Services.IBasicsServices<CallContext>
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly IIndexedDbFactory DbFactory;

        public BasicsServiceOffline(IIndexedDbFactory dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }

        public async ValueTask<Rol> AddRol(Rol rol, CallContext context)
        {

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {

                db.Roles.Add(rol);

                await db.SaveChanges();

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

        public ValueTask<EquipmentTemplate> CreateEquipment(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<EquipmentType> CreateEquipmentType(EquipmentType equipmentTypeDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Manufacturer> CreateManufacturer(Manufacturer manufacturerDTO, CallContext context)
        {
             using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {

                var c = db.Manufacturer.Where(x => x.ManufacturerID == manufacturerDTO.ManufacturerID).FirstOrDefault();

                if (c == null)
                {
                     db.Manufacturer.Add(manufacturerDTO);
                }
                else
                {
                     db.Manufacturer.Remove(manufacturerDTO);


                     db.Manufacturer.Add(manufacturerDTO);
                }
                   

                    await db.SaveChanges();
                }

            return manufacturerDTO;
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

        public ValueTask<ManufacturerResultSet> GetAllManufacturers(CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ProcedureResultSet> GetAllProcedures(CallContext context)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<IEnumerable<Certification>> GetCertifications(CallContext context)
        {
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
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

        public async ValueTask<ResultSet<EquipmentTemplate>> GetEquipment(Pagination<EquipmentTemplate> pagination, CallContext context)
        {
            //var DTO = pagination.Entity;

            //var filterQuery = Querys.WorkOrderFilter(pagination.Filter);

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {

                var filterQuery = Querys.EquipmentTemplateFilter(pagination.Filter);

                //var b = db.EquipmentTemplate.ToList();

                var queriable = db.EquipmentTemplate.AsQueryable();

                var result = await queriable.PaginationAndFilterQueryOff<EquipmentTemplate>(pagination, db.EquipmentTemplate.AsQueryable(), filterQuery);

                return result;
            }
        }

       

        public ValueTask<EquipmentTemplate> GetEquipmentByID(EquipmentTemplate EquipmentDTO, CallContext context)
        {
            throw new NotImplementedException();
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

        public ValueTask<EquipmentTypeResultSet> GetEquipmenTypes(CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<EquipmentType>> GetEquipmenTypesPag(Pagination<EquipmentType> Pagination, CallContext _context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<CalibrationType>> GetCalibrationTypesPag(Pagination<CalibrationType> Pagination, CallContext _context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<Manufacturer>> GetManufacturers(Pagination<Manufacturer> Pagination, CallContext context)
        {
            throw new NotImplementedException();
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

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                result.Roles = db.Roles.ToList();
                
                return await Task.FromResult(result); 
            
            }
        }

        public ValueTask<StatusResultSet> GetStatus(CallContext context)
        {
            throw new NotImplementedException();
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

        public ValueTask<IEnumerable<ToleranceType>> GetToleranceTypes(CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<User> GetUserById(User userDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<User> GetUserById2(User userDTO, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<UserResultSet> GetUsers(CallContext context)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();

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
