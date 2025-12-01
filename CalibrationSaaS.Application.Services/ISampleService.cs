using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using Helpers.Controls;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.Services
{
    [ServiceContract(Name = "CalibrationSaaS.Application.Services.SampleService")]
    public interface ISampleService<T>
    {
        ValueTask<Customer> CreateCustomer(Customer customerDTO, T context);

        ValueTask<CustomerResultSet> GetCustomers(TenantDTO tenantID, T context);

        ValueTask<ResultSet<WorkOrderDetail>> GetWOD(Pagination<WorkOrderDetail> pag, T context);



    }

    [ServiceContract(Name = "CalibrationSaaS.Application.Services.SampleService2")]
    public interface ISampleService2<T>
    {
        //Task SaveChanges();
        Task LoadComponents(ICollection<Helpers.Controls.Component> Components);
        Task<ICollection<Helpers.Controls.Component>> GetComponents();
        Task ini2();

        Task Excecute(string Query,string Parameters="");
        Task<string> GetRols(T context);
        ValueTask<List<PieceOfEquipment>> GetPoeOff();

        ValueTask<List<Manufacturer>> GetManufacturerOff();
        Task DeleteDatabase();
        ValueTask<WorkOrderDetail> UpdateOfflineID(WorkOrderDetail DTO);
        ValueTask<WorkOrderDetail> GetWODById(WorkOrderDetail DTO);
        Task InitializeAsync();
        Task<CurrentUser> GetUser(T context);
        Task LoadUser(CurrentUser user, T context);
        Task LoadRol(ICollection<Rol> pag, T context);
        ValueTask<Customer> CreateCustomer(Customer customerDTO, T context);

        ValueTask<ResultSet<Customer>> GetCustomers(TenantDTO tenantID, T context);

        ValueTask<ResultSet<WorkOrderDetail>> GetWOD(Pagination<WorkOrderDetail> pag, T context);

        ValueTask<ResultSet<WorkOrderDetail>> GetWODUploaded(Pagination<WorkOrderDetail> pag, T context);

        ValueTask<WorkOrderDetailResultSet> InsertWOD(ICollection<WorkOrderDetail> pag, T context);

        ValueTask<ResultSet<EquipmentType>> LoadEquipmentType(ICollection<EquipmentType> pag, T context);

        ValueTask<ResultSet<Manufacturer>> LoadManufacturer(ICollection<Manufacturer> pag, T context);

        ValueTask<ResultSet<User>> LoadUser(ICollection<User> pag, T context);

        ValueTask<ResultSet<Status>> LoadStatus(ICollection<Status> pag, T context);

        ValueTask<ResultSet<Certification>> LoadCertification(ICollection<Certification> pag, T context);


        ValueTask<ResultSet<CalibrationType>> LoadCalibrationType(ICollection<CalibrationType> pag, T context);

        Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination, T context = default);


        Task<CalibrationType> CreateNormalConfiguration(CalibrationType DTO, T context = default);

        ValueTask<ResultSet<PieceOfEquipment>> LoadPOE(ICollection<PieceOfEquipment> pag, T context);

        //ValueTask<ResultSet<UnitOfMeasure>> LoadUOM(ICollection<UnitOfMeasure> pag, T context);


        //ValueTask<ResultSet<UnitOfMeasureType>> LoadUOMType(ICollection<UnitOfMeasureType> pag, T context);

        ValueTask<ResultSet<WorkOrder>> LoadUserWorkOrder(ICollection<User_WorkOrder> pag, T context);

        ValueTask<ResultSet<WorkOrder>> LoadWorkOrder(ICollection<WorkOrder> pag, T context);

        ValueTask<ResultSet<Address>> LoadAddress(ICollection<Address> pag, T context);

        ValueTask<ResultSet<Address>> LoadAddress(ICollection<Address> pag, bool clear, T context);

        ValueTask<ResultSet<CustomerAggregate>> LoadAggregates(ICollection<CustomerAggregate> pag, T context);

        ValueTask<ResultSet<Customer>> LoadCustomer(ICollection<Customer> pag, bool clear, T context);

        ValueTask<ResultSet<EquipmentTemplate>> LoadEquipmentTemplate(ICollection<EquipmentTemplate> pag, T context);

        Task<bool> Delete(WorkOrderDetail work);

        Task LoadMany(ICollection<ToleranceType> toltype,ICollection<EquipmentTypeGroup> etg,ICollection<TestCode> TestCodes, ICollection<Procedure> procedures,ICollection<CalibrationType> cal, ICollection<Status> sta, ICollection<EquipmentType> pag, ICollection<Manufacturer> manu, ICollection<UnitOfMeasure> uom, ICollection<UnitOfMeasureType> uomtype);

    }


}
