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
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using CalibrationSaaS.Data.EntityFramework;
using LinqKit;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using SqliteWasmHelper;
using CalibrationSaaS.Application.Services;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;


namespace CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite
{
   


    public class SampleService2Offline<TContext> : Application.Services.ISampleService2<CallContext> 
        where TContext:DbContext, ICalibrationSaaSDBContextBase, ICalibrationSaaSDBContextBaseOff
    {

         private readonly ISqliteWasmDbContextFactory<TContext> DbFactory;

        private bool _hasSynced = false;

        //private readonly ILocalStorageService localStorageService;

        //public SampleServiceOffline(ILocalStorageService localStorageService)

        //private readonly IIndexedDbFactory DbFactory;

        private readonly string Name;

        private readonly int Version;


        // public SampleService2Offline(IDbContextFactory<ContributionDbContext> factory)
        //{
           
        //    _factory = factory;
        //}


        public SampleService2Offline(ISqliteWasmDbContextFactory<TContext> factory)
        {
            //this.localStorageService = localStorageService;


            this.DbFactory = factory;


        }


         public async Task InitializeAsync()
        {
            try
            {
                //#if RELEASE

                //Console.WriteLine("ini database");
                if(DbFactory != null)
                {
                    await using var dbContext = await DbFactory.CreateDbContextAsync();
                    var resul = await dbContext.Database.EnsureCreatedAsync();
                    var i = await dbContext.SaveChangesAsync();
                    await ini2();
                }
             
                //Console.WriteLine("end database");
                //#endif
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    //Console.WriteLine(ex.GetType().Name, ex.InnerException.Message);
                }
                //Console.WriteLine(ex.GetType().Name, ex.Message);
                //Console.WriteLine("InitDatabaseAsync " + ex.Message);
            }

            //await using var db = await DbFactory.CreateDbContextAsync();

          

            // var i= await db.SaveChangesAsync();

            
             
            
        }

        public async Task ini2()
        {
            //DbFactory.ForceUpdate = true;
            await using var db = await DbFactory.CreateDbContextAsync();
            if(db != null)
            {
                await DbFactory.MakeBackup(db);
            }
            
            //DbFactory.ForceUpdate = false;

        }


        public async Task Excecute(string Query,string QueryPar="")
        {
            //DbFactory.ForceUpdate = true;
            await using var db = await DbFactory.CreateDbContextAsync();
            if (db != null)
            {
                FormattableString sqlstringfor = null;

                var lstParameter = new List<object>();

                if (!string.IsNullOrEmpty(QueryPar))
                {
                    var lst = QueryPar.Split(",");

                    foreach (var item in lst)
                    {
                        lstParameter.Add(""+ item + "");
                    }
                }

                    

               var query= FormattableStringFactory.Create(Query, lstParameter.ToArray());

                await db.Database.ExecuteSqlAsync(query);

                await db.SaveChangesAsync();

            }

            //DbFactory.ForceUpdate = false;

        }


        public async ValueTask<Customer> CreateCustomer(Customer customerDTO, CallContext context)
        {

            //Console.WriteLine("CreateCustomer Offline");



            using (var db = await this.DbFactory.CreateDbContextAsync())
            {
                db.Customer.Add(customerDTO);
                await db.SaveChangesAsync();
                customerDTO = db.Customer.FirstOrDefault();
            }

            return await Task.FromResult(customerDTO);



        }

        public async ValueTask<ResultSet<Customer>> GetCustomers(TenantDTO tenantID, CallContext context)
        {
            //Console.WriteLine("GetCustomers Offline");
            //CustomerResultSet resultSet = new CustomerResultSet();

            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{

            //    var a = await db.Customer.ToListAsync();

            //    return await Task.FromResult(new CustomerResultSet { Customers = a });

            //}

            ICustomerRepository c = new CustomerRepositoryEF<TContext>(DbFactory);

            Pagination<Customer> cust = new Pagination<Customer>();

            var res = await c.GetCustomers(cust);

            return res;

        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetWOD(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            
           
            IWorkOrderDetailRepository c = new WODRepositoryEF<TContext>(DbFactory);

            var res = await c.GetByTechnicianPag(pag);

            return res;
        }

        public async ValueTask<WorkOrderDetailResultSet> InsertWOD(ICollection<WorkOrderDetail> pag, CallContext context)
        {
            
            WorkOrderDetailResultSet resultSet = new WorkOrderDetailResultSet();


            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);
            bool error = false;
            string errorstr = "";
           
            //using (var db = await this.DbFactory.CreateDbContextAsync())
                //{
                    foreach (var item in pag)
                    {

                try
                {

                    WOD_ParametersTable item2 = new WOD_ParametersTable()
                    {
                        WorkOrderDetailID = item.WorkOrderDetailID
                    };

                    //var result = await wod.GetWOD_Parameter(item2);




                    var wodres = await wod.ChangeStatus(item, "WorkOrderItem", true, true);

                }

                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);

                    errorstr = errorstr + item.WorkOrderDetailID.ToString() + " , ";

                    error = true;

                }

            }

            if (error)
            {
                //Console.WriteLine(errorstr);
                throw new Exception("Error in next wod " + errorstr);
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
                using (var db = await this.DbFactory.CreateDbContextAsync())
                {
                    await db.EquipmentType.Clear(db);

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
                    await db.SaveChangesAsync();
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

                //using (var db2 = await this.DbFactory.CreateDbContextAsync())
                //{
                //    var lstx = db2.WorkOrderDetail.ToList();
                //    var lst2x = db2.Manufacturer.ToList();
                //    var lst3x = db2.EquipmentType.ToList();

                //}

                using (var db = await this.DbFactory.CreateDbContextAsync())
                {
                    int cont = 0;

                    await db.Manufacturer.Clear(db);

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
                            //    //await db.SaveChangesAsync();

                            //    db.Manufacturer.Add(item);
                            //}


                            //Console.WriteLine("item :" + cont.ToString());
                            cont = cont + 1;
                        }


                    }
                    await db.SaveChangesAsync();
                    //using (var db1 = await this.DbFactory.CreateDbContextAsync())
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


                using (var db = await this.DbFactory.CreateDbContextAsync())
                {

                    await db.Status.Clear(db);
                    //await db.SaveChangesAsync();

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
                    await db.SaveChangesAsync();
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
                using (var db = await this.DbFactory.CreateDbContextAsync())
                {

                    await db.Certification.Clear();

                    foreach (var item in pag)
                    {
                        //if (item != null)
                        //{

                        //    var c = db.Certification.Where(x => x.CertificationID == item.CertificationID).FirstOrDefault();
                        //    if (c == null)
                        //    {
                        //        db.Certification.Add(item);
                        //    }
                        //    else
                        //    {
                        //        db.Certification.Remove(item);
                        //        //await db.SaveChangesAsync();

                        //        db.Certification.Add(item);
                        //    }

                        //}

                        db.Certification.Add(item);

                    }
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }


        public async Task SaveConfiguredWeights(int WorkOrderDetailID, BalanceAndScaleCalibration bc)
        {

            IWorkOrderDetailRepository wod = new WODRepositoryEF<TContext>(DbFactory);

            await wod.SaveConfiguredWeights(WorkOrderDetailID,bc);


        }


            public async ValueTask<ResultSet<User>> LoadUser(ICollection<User> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.CreateDbContextAsync())
                {

                    await db.User.Clear(db);

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
                    await db.SaveChangesAsync();
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
                using (var db = await this.DbFactory.CreateDbContextAsync())
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
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public async ValueTask LoadAddress(ICollection<Address> pag)
        { 
            var poes = new CustomerRepositoryEF<TContext>(DbFactory);

            //await poes.Clear();

            foreach (var item in pag)
            {

                await poes.InserAddress(item);
            
            }
        
            
        }

        public async Task<ResultSet<PieceOfEquipment>> GetAllWeightSetsPag(Pagination<PieceOfEquipment> pagination, CallContext context = default)
        {



            IPieceOfEquipmentRepository wod = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var a = await wod.GetAllWeightSetsPag(pagination);

            return a;

        }

       public async  Task<CalibrationType> CreateNormalConfiguration(CalibrationType DTO, CallContext context = default)
        {
            var poes = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

            var res= await poes.CreateNormalConfiguration(DTO);

            return null;
        }

        public async ValueTask<ResultSet<PieceOfEquipment>> LoadPOE(ICollection<PieceOfEquipment> pag, CallContext context)
        {
           
                  var poes = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

                    var eq11 = new BasicsRepositoryEF<TContext>(DbFactory);
            //await poes.Clear();

            foreach (var item in pag)
                    {

                bool etexits = false;
                        if (item != null)
                        {

                        if (item.EquipmentTemplate == null)
                        {
                            EquipmentTemplate et = new EquipmentTemplate();

                            et.EquipmentTemplateID = item.EquipmentTemplateId;

                            var eqt = eq11.GetEquipmentByID(et);

                            if (eqt == null)
                            {
                            throw new Exception("ET not configured");
                            }
                            else
                            {
                              etexits = true;
                            }
                            
                        }



                         if (!etexits)
                         {
                            var res = await eq11.CreateEquipment(item.EquipmentTemplate, false, true, false);
                         }
                           


                           var resd=  await poes.InsertPieceOfEquipment(item, "PieceOfEquipmentCreate", false,true);

                            //Console.WriteLine("POE insert: " + resd.PieceOfEquipmentID);
                                                 
                        }
                    }
              
            
            return null;
        }

        //public async ValueTask<ResultSet<UnitOfMeasure>> LoadUOM(ICollection<UnitOfMeasure> pag, CallContext context)
        //{


        //    try
        //    {

        //        var uom = new UOMRepositoryEF<TContext>(DbFactory);

        //        //using (var db = await this.DbFactory.CreateDbContextAsync())
        //        //{
        //        //    db.UnitOfMeasure.Clear(db);

        //            foreach (var item in pag)
        //            {
        //                if (item != null)
        //                {
        //                //var c = db.UnitOfMeasure.Where(x => x.UnitOfMeasureID == item.UnitOfMeasureID).FirstOrDefault();
        //                //if (c == null)
        //                //{
        //                //db.UnitOfMeasure.Add(item);
        //                //}
        //                //else
        //                //{
        //                //    db.UnitOfMeasure.Remove(item);                              

        //                //    db.UnitOfMeasure.Add(item);
        //                //}
        //                await uom.Create(item);

        //                }
        //             }
        //        //    await db.SaveChangesAsync();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return null;
        //}

        //public async ValueTask<ResultSet<UnitOfMeasureType>> LoadUOMType(ICollection<UnitOfMeasureType> pag, CallContext context)
        //{


        //    try
        //    {
        //        using (var db = await this.DbFactory.CreateDbContextAsync())
        //        {
        //            foreach (var item in pag)
        //            {
        //                if (item != null)
        //                {
        //                    var c = db.UnitOfMeasureType.Where(x => x.Value == item.Value).FirstOrDefault();
        //                    if (c == null)
        //                    {
        //                        db.UnitOfMeasureType.Add(item);
        //                    }
        //                    else
        //                    {
        //                        db.UnitOfMeasureType.Remove(item);

        //                        db.UnitOfMeasureType.Add(item);
        //                    }


        //                }
        //            }
        //            await db.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return null;
        //}
       

            public async ValueTask<ResultSet<WorkOrder>> LoadWorkOrder(ICollection<WorkOrder> pag, CallContext context)
        {


            try
            {
                //TODO
                var assets = new AssetsRepositoryEF<TContext>(DbFactory);
                
                //using (var db = await this.DbFactory.CreateDbContextAsync())
                //{
                //    db.WorkOrder.Clear(db);

                //await assets.ClearWorkOrder();
                    
                foreach (var item in pag)
                    {
                        if (item != null)
                        {
                        //var c = db.WorkOrder.Where(x => x.WorkOrderId == item.WorkOrderId).FirstOrDefault();
                        //if (c == null)
                        //{
                        //db.WorkOrder.Add(item);
                        //}
                        //else
                        //{
                        //    db.WorkOrder.Remove(item);
                        //    //await db.SaveChangesAsync();

                        //    db.WorkOrder.Add(item);
                        //}
                        
                        await assets.InsertWokOrder(item);

                        }
                    }
                    //await db.SaveChangesAsync();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        //public async ValueTask<ResultSet<Address>> LoadAddress(ICollection<Address> pag, CallContext context)
        //{


        //    try
        //    {
        //        using (var db = await this.DbFactory.CreateDbContextAsync())
        //        {
        //            foreach (var item in pag)
        //            {
        //                if (item != null)
        //                {
        //                    var c = await db.Address.Where(x => x.AddressId == item.AddressId).FirstOrDefaultAsync();
        //                    if (c == null)
        //                    {
        //                        db.Address.Add(item);
        //                    }
        //                    else
        //                    {
        //                        db.Address.Remove(item);
        //                        //await db.SaveChangesAsync();

        //                        db.Address.Add(item);
        //                    }


        //                }
        //            }
        //            await db.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return null;
        //}

        public async Task<bool> Delete(WorkOrderDetail work)
        {
            //using (var db = await this.DbFactory.CreateDbContextAsync())
            //{
            //    db.WorkOrderDetail.Remove(work);
            //    await db.SaveChangesAsync();
            //    await db.SaveChangesAsync();
            //    return true;
            //}

             var wodservice = new WODRepositoryEF<TContext>(DbFactory);

            await wodservice.Delete(work);

            return true;

        }

        public async ValueTask<ResultSet<CustomerAggregate>> LoadAggregates(ICollection<CustomerAggregate> pag, CallContext context)
        {
            try
            {
                using (var db = await this.DbFactory.CreateDbContextAsync())
                {
                    foreach (var item in pag)
                    {
                        if (item != null)
                        {
                            var c = await db.CustomerAggregates.Where(x => x.AggregateID == item.AggregateID).FirstOrDefaultAsync();
                            if (c == null)
                            {
                                db.CustomerAggregates.Add(item);
                            }
                            else
                            {
                                db.CustomerAggregates.Remove(item);
                                //await db.SaveChangesAsync();

                                db.CustomerAggregates.Add(item);
                            }


                        }
                    }
                    await db.SaveChangesAsync();
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


                var customerservice = new CustomerRepositoryEF<TContext>(DbFactory);
                
                if (clear)
                {
                    await customerservice.Clear();
                }
                

                pag.ForEach(async item =>
                {

                   
                        await customerservice.InsertCustomer(item);
                    
                    
                    
                });




            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


          public async ValueTask<ResultSet<Address>> LoadAddress(ICollection<Address> pag,bool clear, CallContext context)
        {
            try
            {


                var customerservice = new CustomerRepositoryEF<TContext>(DbFactory);
                
                if (clear)
                {
                    await customerservice.Clear();
                }
                

                pag.ForEach(async item =>
                {

                   
                        await customerservice.InserAddress(item);
                    
                    
                    
                });




            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        public async ValueTask<ResultSet<EquipmentTemplate>> LoadEquipmentTemplate(ICollection<EquipmentTemplate> pag, CallContext context)
        {

            var eq11 = new BasicsRepositoryEF<TContext>(DbFactory);
            foreach (var item in pag)
                    {
                        if (item != null)
                        {
                        

                        var res =await eq11.CreateEquipment(item,false,true,false);

                        if (res == null)
                        {
                        throw new Exception("Error Load ET");
                        }


                        }

                    }

            return null;

           
        }


        public async Task LoadMany(ICollection<ToleranceType> toltype,ICollection<EquipmentTypeGroup> eqtg,ICollection<TestCode> TestCodes, ICollection<Procedure> procedures, ICollection<CalibrationType> cal, ICollection<Status> sta, ICollection<EquipmentType> pag, ICollection<Manufacturer> manu, ICollection<UnitOfMeasure> uom, ICollection<UnitOfMeasureType> uomtype)
        {
            try
            {
                var uomservice = new UOMRepositoryEF<TContext>(DbFactory);

                var basicservice = new BasicsRepositoryEF<TContext>(DbFactory);

                var wODRepositoryEF  = new WODRepositoryEF<TContext>(DbFactory);

                //await uomservice.Clear();

                //await uomservice.ClearTypes();                


                var types = await uomservice.GetTypes();

                uomtype.ToList().ForEach(async item =>
                    {

                        await uomservice.CreateType(item);
                    
                    });
                   


                 
                 //var types2 = await uomservice.GetTypes();

               
                

                uom.ToList().ForEach(async item =>
                    {

                        await uomservice.Create(item);
                    
                    });


                var uomuom = await uomservice.GetHeaderAll();

                if (uomuom?.Count() == 0)
                {
                    throw new Exception("UoM No Save");
                }

                manu.ToList().ForEach(async item =>
                    {

                        await basicservice.CreateManufacturer(item);
                
                    });


                foreach (var item in sta)
                {

                    //var c = db.EquipmentType.Where(x => x.EquipmentTypeID == item.EquipmentTypeID).FirstOrDefault();
                    //if (c == null)
                    //{
                    await basicservice.CreateStatus(item);
                    //}
                    //else
                    //{
                    //    db.EquipmentType.Remove(item);


                    //    db.EquipmentType.Add(item);
                    //}
                }


                foreach (var item in eqtg.DistinctBy(x=>x.EquipmentTypeGroupID))
                {

                    await basicservice.CreateEquipmentTypeGroup(item);
                 
                }


               var lsty=  await basicservice.GetEquipmentTypeGroup();

                if (lsty.Count < eqtg.DistinctBy(x => x.EquipmentTypeGroupID).Count())
                {
                    throw new Exception("------------------------------------No Load EquipmentTypegroup------------------------------------------------");
                }


              
                foreach (var item in cal)
                {


                    await basicservice.CreateCalibrationType(item);

                }


                foreach (var item in procedures)
                {
                    await basicservice.CreateProcedure(item);
                }


                var proc = await basicservice.GetAllProcedures(new Pagination<Procedure>());

                if(procedures?.Count != proc.List.Count)
                {
                    throw new Exception("Error Load Procedures");
                }

                foreach (var item in TestCodes)
                {
                    await wODRepositoryEF.CreateTestCode(item);
                }

                var tcs = await wODRepositoryEF.GetTestCodes(new Pagination<TestCode>());

                if (TestCodes?.Count != tcs?.List?.Count)
                {
                    throw new Exception("Error Load TestCodes:" + TestCodes?.Count + " ---  " + tcs?.List?.Count);
                }


                pag.ToList().ForEach(async item =>
                {



                    await basicservice.CreateEquipmentType(item);


                });


                var ettt = await basicservice.GetEquipmentTypes();


                if(ettt.Count() < pag.Count)
                {
                    throw new Exception("--------------------------------------------No Load EquipmentType-----------------------------");
                }


                toltype.ToList().ForEach(async item =>
                {



                    await basicservice.CreateToleranceType(item);


                });


                await ini2();

              
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.WriteLine(ex?.InnerException?.Message);
                //Console.WriteLine(ex?.StackTrace);

                throw new Exception("LoadMany ", ex);
            }



        }

        public async ValueTask<ResultSet<WorkOrderDetail>> GetWODUploaded(Pagination<WorkOrderDetail> pag, CallContext context)
        {
            List<WorkOrderDetail> lst;
            using (var db = await this.DbFactory.CreateDbContextAsync())
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
                using (var db = await this.DbFactory.CreateDbContextAsync())
                {

                    await db.Rol.Clear(db);

                    foreach (var item in pag)
                    {
                        //if (item != null)
                        //{
                        //    var c = db.User.Where(x => x.UserID == item.UserID).FirstOrDefault();
                        //    if (c == null)
                        //    {
                        db.Rol.Add(item);
                        //}
                        //else
                        //{
                        //    db.User.Remove(item);                               
                        //    db.User.Add(item);
                        //}                           
                        //}
                    }
                    await db.SaveChangesAsync();
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
            using (var db = await this.DbFactory.CreateDbContextAsync())
            {
                var existingUser = await db.CurrentUser.FindAsync(user.CurrentUserID);
                if (existingUser == null)
                {
                    db.CurrentUser.Add(user);
                }
                await db.SaveChangesAsync();
            }
        }


        public async Task LoadComponents(ICollection<Helpers.Controls.Component> Components)
        {
            using (var db = await this.DbFactory.CreateDbContextAsync())
            {
                foreach(var item in Components)
                {
                    db.Component.Add(item);

                    await db.SaveChangesAsync();
                }
               
                   
            }
        }


        public async Task<ICollection<Helpers.Controls.Component>> GetComponents()
        {
            using (var db = await this.DbFactory.CreateDbContextAsync())
            {
                var com = await db.Component.ToArrayAsync();

                return com;
            }
        }


        public async Task<CurrentUser> GetUser( CallContext context)
        {

            if (this.DbFactory == null)
                return null;

            using (var db = await this.DbFactory.CreateDbContextAsync())
            {

                //if (db.CurrentUser.Count > 0)
                //{
                    var r= await db.CurrentUser.AsNoTracking().Include(x=>x.Claims).FirstOrDefaultAsync();
                //}
               return r;


            }

            //throw new NotImplementedException();
        }

        public async Task<string> GetRols(CallContext context)
        {

            IBasicsRepository wod = new BasicsRepositoryEF<TContext>(DbFactory);

            var roles = await wod.GetAllRoles();
            var s = "";
            foreach (var item in roles)
            {
                s = s + item.Name + ",";
            }

            if (!string.IsNullOrEmpty(s))
            {
                s = s.Substring(0, s.LastIndexOf(","));
            }


            return s;

        }

       

        public ValueTask<ResultSet<Address>> LoadAddress(ICollection<Address> pag, CallContext context)
        {
            throw new NotImplementedException();
        }


        public async ValueTask<WorkOrderDetail> GetWODById(WorkOrderDetail DTO)
        {
            IWorkOrderDetailRepository wod= new WODRepositoryEF<TContext>(DbFactory);

            var res = await wod.GetByID(DTO);


            return res;

        }


         public async ValueTask<List<Manufacturer>> GetManufacturerOff()
        {
            IBasicsRepository wod= new BasicsRepositoryEF<TContext>(DbFactory);

            Pagination<Manufacturer> pagination = new Pagination<Manufacturer>();

            var res = await wod.GetAllManufacturers(pagination);

            res.List = res.List.Where(x => x.IsOffline == true).ToList();

            return res.List;

        }

        public async Task DeleteDatabase()
        {

             IWorkOrderDetailRepository wod= new WODRepositoryEF<TContext>(DbFactory);

             await wod.DeleteDatabase();


        }
        

         public async ValueTask<WorkOrderDetail> UpdateOfflineID(WorkOrderDetail DTO)
        {
            IWorkOrderDetailRepository wod= new WODRepositoryEF<TContext>(DbFactory);

            var res = await wod.UpdateOfflineID(DTO);

            return res;

        }

        public ValueTask<List<PieceOfEquipment>> GetPoeOff()
        {
            throw new NotImplementedException();
        }



        public async ValueTask<ResultSet<WorkOrder>> LoadUserWorkOrder(ICollection<User_WorkOrder> pag, CallContext context)
        {
            var assets = new AssetsRepositoryEF<TContext>(DbFactory);


            await assets.LoadUserWorkOrder(pag);

            return null;
        }
    }
}
