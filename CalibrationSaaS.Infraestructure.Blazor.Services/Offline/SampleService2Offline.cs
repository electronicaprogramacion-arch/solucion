using IndexedDB.Blazor;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
//using Blazored.LocalStorage;
using System.Threading.Tasks;
using CalibrationSaaS.Application.Services;
using Helpers.Controls.ValueObjects;
using Helpers.Controls;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline
{
    public class SampleService2Offline : Application.Services.ISampleService2<CallContext>
    {
        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        private readonly IIndexedDbFactory DbFactory;
#pragma warning disable CS0169 // El campo 'SampleService2Offline.Name' nunca se usa
        private readonly string Name;
#pragma warning restore CS0169 // El campo 'SampleService2Offline.Name' nunca se usa
#pragma warning disable CS0169 // El campo 'SampleService2Offline.Version' nunca se usa
        private readonly int Version;
#pragma warning restore CS0169 // El campo 'SampleService2Offline.Version' nunca se usa


        
        public SampleService2Offline(IIndexedDbFactory dbFactory)
        {
            //this.localStorageService = localStorageService;
            this.DbFactory = dbFactory;
        }


        public async ValueTask<Customer> CreateCustomer(Customer customerDTO, CallContext context)
        {

            //Console.WriteLine("CreateCustomer Offline");

            //await localStorageService.SetItemAsync(customerDTO.Name.ToString(), customerDTO);

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                db.Customer.Add(customerDTO);
                await db.SaveChanges();
                customerDTO = db.Customer.FirstOrDefault();
            }

            return await Task.FromResult(customerDTO);
        }

        public async ValueTask<CustomerResultSet> GetCustomers(TenantDTO tenantID, CallContext context)
        {
            //Console.WriteLine("GetCustomers Offline");
            CustomerResultSet resultSet = new CustomerResultSet();

            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                return await Task.FromResult(new CustomerResultSet { Customers = db.Customer.ToList() });

            }
        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetWOD(Pagination<WorkOrderDetail> pag, CallContext context)
        {

            

            List<WorkOrderDetail> lst;
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                lst = db.WorkOrderDetail.Where(x => x.OfflineStatus == 0 || x.OfflineStatus == 1).ToList();

            }

            ResultSet<WorkOrderDetail> result = new ResultSet<WorkOrderDetail>();

            result.Shown = pag.Show;
            result.CurrentPage = pag.Page;
            result.List = lst;

            return result;//await Task.FromResult(new CustomerResultSet { Customers = db.Customers.ToList() });


        }

        public async ValueTask<WorkOrderDetailResultSet> InsertWOD(ICollection<WorkOrderDetail> pag, CallContext context)
        {
            //Console.WriteLine("GetCustomers Offline");
            WorkOrderDetailResultSet resultSet = new WorkOrderDetailResultSet();
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    foreach (var item in pag)
                    {

                        var c = db.WorkOrderDetail.Where(x => x.WorkOrderDetailID == item.WorkOrderDetailID).FirstOrDefault();
                        if (c == null)
                        {
                            db.WorkOrderDetail.Add(item);
                        }
                        else
                        {
                            //db.WorkOrderDetail.Remove(item);
                            //await db.SaveChanges();

                            //db.WorkOrderDetail.Add(item);
                        }


                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }





            var a = new WorkOrderDetailResultSet
            {
                WorkOrderDetails = pag.ToList()


            };

            return a;
        }

        public async ValueTask<ResultSet<EquipmentType>> LoadEquipmentType(ICollection<EquipmentType> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    db.EquipmentType.Clear();

                    foreach (var item in pag)
                    {

                        //var c = db.EquipmentType.Where(x => x.EquipmentTypeID == item.EquipmentTypeID).FirstOrDefault();
                        //if (c == null)
                        //{
                        db.EquipmentType.Add(item);
                        //}
                        //else
                        //{
                        //    db.EquipmentType.Remove(item);


                        //    db.EquipmentType.Add(item);
                        //}
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;

        }
        public async ValueTask<ResultSet<Manufacturer>> LoadManufacturer(ICollection<Manufacturer> pag, CallContext context)
        {
            try
            {

                //using (var db2 = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                //{
                //    var lstx = db2.WorkOrderDetail.ToList();
                //    var lst2x = db2.Manufacturer.ToList();
                //    var lst3x = db2.EquipmentType.ToList();

                //}

                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    int cont = 0;
                    db.Manufacturer.Clear();

                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            //var c = db.Manufacturer.Where(x => x.ManufacturerID == item.ManufacturerID).FirstOrDefault();
                            //if (c == null)
                            //{
                            db.Manufacturer.Add(item);
                            //}
                            //else
                            //{
                            //    db.Manufacturer.Remove(item);
                            //    //await db.SaveChanges();

                            //    db.Manufacturer.Add(item);
                            //}


                            //Console.WriteLine("item :" + cont.ToString());
                            cont = cont + 1;
                        }


                    }
                    await db.SaveChanges();
                    //using (var db1 = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                    //{
                    //    var lst = db.WorkOrderDetail.ToList();
                    //    var lst2 = db.Manufacturer.ToList();

                    //}



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;

        }

        public async ValueTask<ResultSet<Status>> LoadStatus(ICollection<Status> pag, CallContext context)
        {
            try
            {


                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {

                    db.Status.Clear();
                    //await db.SaveChanges();

                    foreach (var item in pag)
                    {

                        //var c = db.Status.Where(x => x.StatusId == item.StatusId).FirstOrDefault();
                        //if (c == null)
                        //{
                        db.Status.Add(item);
                        //}
                        //else
                        //{
                        //    db.Status.Remove(item);                           

                        //    db.Status.Add(item);
                        //}
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;

        }

        public async ValueTask<ResultSet<Certification>> LoadCertification(ICollection<Certification> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    foreach (var item in pag)
                    {
                        if (item != null)
                        {

                            var c = db.Certification.Where(x => x.CertificationID == item.CertificationID).FirstOrDefault();
                            if (c == null)
                            {
                                db.Certification.Add(item);
                            }
                            else
                            {
                                db.Certification.Remove(item);
                                //await db.SaveChanges();

                                db.Certification.Add(item);
                            }

                        }
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        public async ValueTask<ResultSet<User>> LoadUser(ICollection<User> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {

                    db.User.Clear();

                    foreach (var item in pag)
                    {
                        //if (item != null)
                        //{
                        //    var c = db.User.Where(x => x.UserID == item.UserID).FirstOrDefault();
                        //    if (c == null)
                        //    {
                        db.User.Add(item);
                        //}
                        //else
                        //{
                        //    db.User.Remove(item);                               
                        //    db.User.Add(item);
                        //}                           
                        //}
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async ValueTask<ResultSet<CalibrationType>> LoadCalibrationType(ICollection<CalibrationType> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    foreach (var item in pag)
                    {

                        var c = db.CalibrationType.Where(x => x.CalibrationTypeId == item.CalibrationTypeId).FirstOrDefault();
                        if (c == null)
                        {
                            db.CalibrationType.Add(item);
                        }
                        else
                        {
                            db.CalibrationType.Remove(item);


                            db.CalibrationType.Add(item);
                        }
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async Task<CalibrationType> CreateNormalConfiguration(CalibrationType DTO, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> LoadPOE(ICollection<PieceOfEquipment> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    db.PieceOfEquipment.Clear();

                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            //var c = db.PieceOfEquipment.Where(x => x.PieceOfEquipmentID == item.PieceOfEquipmentID).FirstOrDefault();
                            //if (c == null)
                            //{
                            db.PieceOfEquipment.Add(item);
                            //}
                            //else
                            //{
                            //    db.PieceOfEquipment.Remove(item);


                            //    db.PieceOfEquipment.Add(item);
                            //}                           
                        }
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async ValueTask<ResultSet<UnitOfMeasure>> LoadUOM(ICollection<UnitOfMeasure> pag, CallContext context)
        {


            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    db.UnitOfMeasure.Clear();

                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            //var c = db.UnitOfMeasure.Where(x => x.UnitOfMeasureID == item.UnitOfMeasureID).FirstOrDefault();
                            //if (c == null)
                            //{
                            db.UnitOfMeasure.Add(item);
                            //}
                            //else
                            //{
                            //    db.UnitOfMeasure.Remove(item);                              

                            //    db.UnitOfMeasure.Add(item);
                            //}


                        }
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async ValueTask<ResultSet<UnitOfMeasureType>> LoadUOMType(ICollection<UnitOfMeasureType> pag, CallContext context)
        {


            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            var c = db.UnitOfMeasureType.Where(x => x.Value == item.Value).FirstOrDefault();
                            if (c == null)
                            {
                                db.UnitOfMeasureType.Add(item);
                            }
                            else
                            {
                                db.UnitOfMeasureType.Remove(item);

                                db.UnitOfMeasureType.Add(item);
                            }


                        }
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public async ValueTask<ResultSet<WorkOrder>> LoadWorkOrder(ICollection<WorkOrder> pag, CallContext context)
        {


            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    db.WorkOrder.Clear();
                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            //var c = db.WorkOrder.Where(x => x.WorkOrderId == item.WorkOrderId).FirstOrDefault();
                            //if (c == null)
                            //{
                            db.WorkOrder.Add(item);
                            //}
                            //else
                            //{
                            //    db.WorkOrder.Remove(item);
                            //    //await db.SaveChanges();

                            //    db.WorkOrder.Add(item);
                            //}


                        }
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public async ValueTask<ResultSet<Address>> LoadAddress(ICollection<Address> pag, CallContext context)
        {


            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            var c = db.Address.Where(x => x.AddressId == item.AddressId).FirstOrDefault();
                            if (c == null)
                            {
                                db.Address.Add(item);
                            }
                            else
                            {
                                db.Address.Remove(item);
                                //await db.SaveChanges();

                                db.Address.Add(item);
                            }


                        }
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async Task<bool> Delete(WorkOrderDetail work)
        {
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                db.WorkOrderDetail.Remove(work);
                await db.SaveChanges();
                await db.SaveChanges();
                return true;
            }


        }

        public async ValueTask<ResultSet<CustomerAggregate>> LoadAggregates(ICollection<CustomerAggregate> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            var c = db.CustomerAggregate.Where(x => x.AggregateID == item.AggregateID).FirstOrDefault();
                            if (c == null)
                            {
                                db.CustomerAggregate.Add(item);
                            }
                            else
                            {
                                db.CustomerAggregate.Remove(item);
                                //await db.SaveChanges();

                                db.CustomerAggregate.Add(item);
                            }


                        }
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async ValueTask<ResultSet<Customer>> LoadCustomer(ICollection<Customer> pag,bool clear, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    db.Customer.Clear();


                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            //var c = db.Customer.Where(x => x.CustomerID == item.CustomerID).FirstOrDefault();
                            //if (c == null)
                            //{
                            db.Customer.Add(item);
                            //}
                            //else
                            //{
                            //    db.Customer.Remove(item);
                            //    //await db.SaveChanges();

                            //    db.Customer.Add(item);
                            //}


                        }

                    }

                    await db.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async ValueTask<ResultSet<EquipmentTemplate>> LoadEquipmentTemplate(ICollection<EquipmentTemplate> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    db.EquipmentTemplate.Clear();
                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            //var c = db.EquipmentTemplate.Where(x => x.EquipmentTemplateID == item.EquipmentTemplateID).FirstOrDefault();
                            //if (c == null)
                            //{
                            db.EquipmentTemplate.Add(item);
                            //}
                            //else
                            //{
                            //    db.EquipmentTemplate.Remove(item);
                            //    //await db.SaveChanges();

                            //    db.EquipmentTemplate.Add(item);
                            //}


                        }

                    }

                    await db.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public async Task LoadMany(ICollection<ToleranceType> toltype,ICollection<EquipmentTypeGroup> TestCodes2,ICollection<TestCode> TestCodes, ICollection<Procedure> proced,ICollection<CalibrationType> cal, ICollection<Status> sta, ICollection<EquipmentType> pag, ICollection<Manufacturer> manu, ICollection<UnitOfMeasure> uom, ICollection<UnitOfMeasureType> uomtype)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {
                    db.CalibrationType.Clear();
                    db.Status.Clear();
                    db.EquipmentType.Clear();
                    db.Manufacturer.Clear();
                    db.UnitOfMeasure.Clear();
                    db.UnitOfMeasureType.Clear();
                    await db.SaveChanges();
                    foreach (var item in cal)
                    {

                        //var c = db.EquipmentType.Where(x => x.EquipmentTypeID == item.EquipmentTypeID).FirstOrDefault();
                        //if (c == null)
                        //{
                        db.CalibrationType.Add(item);
                        //}
                        //else
                        //{
                        //    db.EquipmentType.Remove(item);


                        //    db.EquipmentType.Add(item);
                        //}
                    }
                    await db.SaveChanges();
                    foreach (var item in sta)
                    {

                        //var c = db.EquipmentType.Where(x => x.EquipmentTypeID == item.EquipmentTypeID).FirstOrDefault();
                        //if (c == null)
                        //{
                        db.Status.Add(item);
                        //}
                        //else
                        //{
                        //    db.EquipmentType.Remove(item);


                        //    db.EquipmentType.Add(item);
                        //}
                    }
                    await db.SaveChanges();
                    foreach (var item in pag)
                    {

                        //var c = db.EquipmentType.Where(x => x.EquipmentTypeID == item.EquipmentTypeID).FirstOrDefault();
                        //if (c == null)
                        //{
                        db.EquipmentType.Add(item);
                        //}
                        //else
                        //{
                        //    db.EquipmentType.Remove(item);


                        //    db.EquipmentType.Add(item);
                        //}
                    }
                    await db.SaveChanges();

                    foreach (var item in manu)
                    {

                        //var c = db.EquipmentType.Where(x => x.EquipmentTypeID == item.EquipmentTypeID).FirstOrDefault();
                        //if (c == null)
                        //{
                        db.Manufacturer.Add(item);
                        //}
                        //else
                        //{
                        //    db.EquipmentType.Remove(item);


                        //    db.EquipmentType.Add(item);
                        //}
                    }
                    await db.SaveChanges();

                    foreach (var item in uom)
                    {

                        //var c = db.EquipmentType.Where(x => x.EquipmentTypeID == item.EquipmentTypeID).FirstOrDefault();
                        //if (c == null)
                        //{
                        db.UnitOfMeasure.Add(item);
                        //}
                        //else
                        //{
                        //    db.EquipmentType.Remove(item);


                        //    db.EquipmentType.Add(item);
                        //}
                    }
                    await db.SaveChanges();

                    foreach (var item in uomtype)
                    {

                        //var c = db.EquipmentType.Where(x => x.EquipmentTypeID == item.EquipmentTypeID).FirstOrDefault();
                        //if (c == null)
                        //{
                        db.UnitOfMeasureType.Add(item);
                        //}
                        //else
                        //{
                        //    db.EquipmentType.Remove(item);


                        //    db.EquipmentType.Add(item);
                        //}
                    }
                    await db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetWODUploaded(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            List<WorkOrderDetail> lst;
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                lst = db.WorkOrderDetail.Where(x => x.OfflineStatus == 1).ToList();

            }

            ResultSet<WorkOrderDetail> result = new ResultSet<WorkOrderDetail>();

            result.Shown = pag.Show;
            result.CurrentPage = pag.Page;
            result.List = lst;

            return result;//await Task.FromResult(new CustomerResultSet { Customers = db.Customers.ToList() });

        }


        public async Task LoadRol(ICollection<Rol> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
                {

                    db.Roles.Clear();

                    foreach (var item in pag)
                    {
                        //if (item != null)
                        //{
                        //    var c = db.User.Where(x => x.UserID == item.UserID).FirstOrDefault();
                        //    if (c == null)
                        //    {
                        db.Roles.Add(item);
                        //}
                        //else
                        //{
                        //    db.User.Remove(item);                               
                        //    db.User.Add(item);
                        //}                           
                        //}
                    }
                    await db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return null;
        }

        public async Task LoadUser(CurrentUser user , CallContext context)
        {
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {
                db.CurrentUser.Clear();
                await db.SaveChanges();
                db.CurrentUser.Add(user);

                // await Task.Delay(100);
                await db.SaveChanges();
                // await Task.Delay(100);
                //await db.SaveChanges();

                var a = db.CurrentUser.FirstOrDefault();
            
            }
            //throw new NotImplementedException();
        }


        public async Task<CurrentUser> GetUser( CallContext context)
        {
            using (var db = await this.DbFactory.Create<CalibrationSaaSOfflineDB>())
            {

                //if (db.CurrentUser.Count > 0)
                //{
                    var r= await Task.FromResult(db.CurrentUser.SingleOrDefault());
                //}
               return r;


            }

            //throw new NotImplementedException();
        }

        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        ValueTask<ResultSet<Customer>> ISampleService2<CallContext>.GetCustomers(TenantDTO tenantID, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<Address>> LoadAddress(ICollection<Address> pag, bool clear, CallContext context)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderDetail> GetWODById(WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WorkOrderDetail> UpdateOfflineID(WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDatabase()
        {
            throw new NotImplementedException();
        }

        public ValueTask<List<Manufacturer>> GetManufacturerOff()
        {
            throw new NotImplementedException();
        }

        public ValueTask<List<PieceOfEquipment>> GetPoeOff()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRols(CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task ini2()
        {
            throw new NotImplementedException();
        }

        public Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ResultSet<WorkOrder>> LoadUserWorkOrder(ICollection<User_WorkOrder> pag, CallContext context)
        {
            throw new NotImplementedException();
        }

        public Task LoadComponents(ICollection<Helpers.Controls.Component> Components)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Helpers.Controls.Component>> GetComponents()
        {
            throw new NotImplementedException();
        }

        public Task Excecute(string Query)
        {
            throw new NotImplementedException();
        }

        public Task Excecute(string Query, string Parameters = "")
        {
            throw new NotImplementedException();
        }
    }
}
