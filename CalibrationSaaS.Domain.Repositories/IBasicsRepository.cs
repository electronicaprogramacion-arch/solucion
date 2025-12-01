using CalibrationSaaS.Domain.Aggregates.Entities;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public partial interface IBasicsRepository
    {

        Task<ResultSet<CustomSequence>> CustomSequences();
        Task CreateToleranceType(ToleranceType DTO);
        Task<IEnumerable<EquipmentType>> GetEquipmentTypes();
        Task<ICollection<EquipmentTypeGroup>> GetEquipmentTypeGroup();
        Task<EquipmentTypeGroup> CreateEquipmentTypeGroup(EquipmentTypeGroup DTO);
        #region Manufacturer

        Task<Component>GetComponentByRoute(string route);

        Task<Component> DeleteComponent(Component Component);
        Task<ResultSet<Component>> CreateComponents(ICollection<Component> Component);

        Task<ResultSet<Component>> GetAllComponents();

        Task<Manufacturer> CreateManufacturer(Manufacturer DTO);

        Task<ResultSet<Manufacturer>> GetAllManufacturers(Pagination<Manufacturer> pagintion);


        Task<IEnumerable<Manufacturer>> GetManufacturers();



        Task<Manufacturer> DeleteManufacturer(Manufacturer id);

        Task<Manufacturer> UpdateManufacturer(Manufacturer DTO);

        Task<Manufacturer> GetManufacturerXId(int id);

        Task<Manufacturer> GetManufacturerXName(Manufacturer DTO);
        #endregion
        #region Procedure

        //Task<Component> GetComponentByRoute(string route);

        //Task<Component> DeleteComponent(Component Component);
        //Task<ResultSet<Component>> CreateComponents(ICollection<Component> Component);

        //Task<ResultSet<Component>> GetAllComponents();

        Task<Procedure> CreateProcedure(Procedure DTO);

        Task<ResultSet<Procedure>> GetAllProcedures(Pagination<Procedure> pagintion);


        Task<IEnumerable<Procedure>> GetProcedures();



        Task<Procedure> DeleteProcedure(Procedure id);

        Task<Procedure> UpdateProcedure(Procedure DTO);

        Task<Procedure> GetProcedureXId(int id);

        Task<Procedure> GetProcedureXName(Procedure DTO);
        #endregion
        #region Technician
        Task<User> CreateUser(User DTO);

        Task<User> GetUserById(int id);

        Task<User> GetUserByUserName(string id);

        Task<User> GetUserById2(User DTO,bool fromapi=false);
        Task<IEnumerable<User>> GetAllUser();


        Task<IEnumerable<Rol>> GetAllRol();

        Task<Rol> DeleteRole(Rol rol);

        Task<Rol> AddRol(Rol rol);

        Task<ResultSet<User>> GetUserPag(Pagination<User> pagination);

        Task<IEnumerable<Certification>> GetCertifications();

        Task<IEnumerable<ToleranceType>> GetToleranceTypes();

        Task<ResultSet<TechnicianCode>> GetTechnicianCodePag(Pagination<TechnicianCode> pagination);

        Task<TechnicianCode> CreateTechnicianCode(TechnicianCode DTO);

        Task<TechnicianCode> DeleteTechnicianCode(TechnicianCode DTO);

        Task<User> DeleteUser(User id);
        
        Task<User> UpdateUser(User DTO);
        #endregion

        #region Contact
        Task<Contact> CreateContact(Contact DTO);

        Task<IEnumerable<Contact>> GetAllContact();

        Task<Contact> DeleteContact(Contact id);

        Task<Contact> UpdateContact(Contact DTO);
        #endregion

        Task<bool> Save();

        #region EquipmentType


        Task<bool> RemoveNotes<TSource>(int Id, TSource DTO, int Source) where TSource : INote;

        Task<Note> SaveNotes(Note DTO);

         Task<List<Note>> GetNotes(int Id, int Source = 1);

        

        Task<EquipmentType> CreateEquipmentType(EquipmentType DTO);
      

        Task<IEnumerable<EquipmentType>> GetAllEquipmentType();

        Task<ResultSet<EquipmentType>> GetEquipmenTypesPag(Pagination<EquipmentType> Pagination);

        Task<ResultSet<CalibrationType>> GetCalibrationTypesPag(Pagination<CalibrationType> Pagination);


        Task<EquipmentType> DeleteEquipmentType(EquipmentType id);

        Task<EquipmentType> UpdateEquipmentType(EquipmentType DTO);
        Task<EquipmentType> GetEquipmentTypeXId(int? id);
        #endregion
        #region equipmentTemplate

        Task<bool> Exists(EquipmentTemplate EquipmentDTO);
        Task<EquipmentTemplate> CreateEquipment(EquipmentTemplate EquipmentDTO, bool isThread = false,bool validate=false, bool ThrowException=true);
        Task<EquipmentTemplate> UpdateEquipment(EquipmentTemplate EquipmentDTO, bool isThread = false);
        Task<EquipmentTemplate> DeleteEquipment(EquipmentTemplate EquipmentDTO);
        Task<ResultSet<EquipmentTemplate>> GetEquipment(Pagination<EquipmentTemplate> pagination);

        Task<EquipmentTemplate> GetValidEquipment(string model, int? equipmentypegroup, int manufacturer);
        Task<EquipmentTemplate> GetEquipmentByID(EquipmentTemplate EquipmentDTO,string Component= "");

        Task<EquipmentTemplate> GetEquipmentByIDHead(EquipmentTemplate EquipmentDTO);

        //Task<EquipmentTemplate> GetEquipmentXName(EquipmentTemplate EquipmentDTO);


        Task<IEnumerable<TestPointGroup>> GetTespointGroup(EquipmentTemplate EquipmentDTO);

        Task<IEnumerable<TestPoint>> GetTespoint(TestPointGroup DTO);

        Task<IEnumerable<TestPointGroup>> GetTespointByEquipment(EquipmentTemplate EquipmentDTO);

        Task<TestPointGroup> InsertTestPointGroup(TestPointGroup DTO);

        Task<TestPoint> InsertTestPoint(TestPoint DTO);


        Task<ResultSet<EquipmentTypeGroup>> GetEquipmentTypeGroups();

        Task<List<CMCValues>> GetCMCValuesByCalibrationType(CalibrationType DTO);
        Task<CMCValues> GetCMCValuesByID(CMCValues DTO);
        Task<CalibrationType> GetCalibrationTypeByID(CalibrationType DTO);
        Task<CalibrationType> InsertCMCValue(CalibrationType DTO);
        Task<CMCValues> DeleteCMCValue(CMCValues DTO);
        Task<CMCValues> UpdateCMCValue(CMCValues DTO);


        #endregion


        #region Status
        Task<IEnumerable<Status>> GetAllStatus();
        #endregion

        #region Roles
        Task<IEnumerable<Rol>> GetAllRoles();
        #endregion

        #region Address
        Task<Address> CreateAddress(Address DTO);

        Task<IEnumerable<Address>> GetAllAddress();

        Task<Address> DeleteAddress(int id);

        Task<Address> UpdateAddress(Address DTO);
        #endregion

    }


    public partial interface IIdentityRepository
    {

        public Task<bool> Reset(string name);

        public Task<bool> Enable(string name);


        public Task<IEnumerable<User>> ShowRecords();

        public Task<User> ShowRecords(string name);

        public  Task<User> CreateRecord(User user);

        public Task<User> DeleteRecord(User user);

        public Task<Rol> DeleteRol(Rol rol);

        public Task<User> DeleteRecordByID(string name);


    }

    }
