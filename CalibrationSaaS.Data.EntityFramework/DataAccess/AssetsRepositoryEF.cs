using AutoMapper;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Helpers.Controls.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;
using SqliteWasmHelper;
using Helpers;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class AssetsRepositoryEF<TContext> : IAssetsRepository, IDisposable where TContext : DbContext, ICalibrationSaaSDBContextBase 
    {
       

        private readonly IDbContextFactory<TContext> DbFactory;

        private IConfiguration _configuration;

        public AssetsRepositoryEF(IDbContextFactory<TContext> dbFactory, IConfiguration configuration = null)
        {
            DbFactory = dbFactory;
            _configuration = configuration;
        }


        public async Task<WorkOrder> DeleteWorkOrder(int workOrderId)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            var result = await context.WorkOrder
                    .Include(c => c.WorkOrderDetails)
                    .FirstOrDefaultAsync(sc => sc.WorkOrderId == workOrderId);

            if (result.WorkOrderDetails.Count() > 0)
            {
                throw new Exception("Work Order contains pending Jobs");
            }
           else
            {
                context.WorkOrder.Remove(result);
                await context.SaveChangesAsync();
                return result;
            }
               
           
        }


        public async Task<WorkOrder> GetWorkOrderByIDHeader(int workOrderId, int CurrentStatus)
        {
            await using var context = await DbFactory.CreateDbContextAsync();


            var wo = await context.WorkOrder.AsNoTracking().Where(x => x.WorkOrderId == workOrderId).FirstOrDefaultAsync();

            return wo;

        }


        public async Task<WorkOrder> GetWorkOrderByID(int workOrderId, int CurrentStatus)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            try {
                var a = await context.WorkOrder
                    //.Include(x => x.PieceOfEquipments)
                    //.Include(c => c.WorkOrderDetails)
                    //.ThenInclude(d=>d.PieceOfEquipment)
                    //.Include(x => x.WorkOrderDetail).ThenInclude(x => x.WorkOder)
                    //.Include(x => x.Customer)
                    //.ThenInclude(x=>x.Aggregates)
                    //.ThenInclude(x=>x.Contacts)
                    // .Include(x => x.Customer)
                    //.ThenInclude(x => x.Aggregates)
                    //.ThenInclude(x => x.Addresses)
                    .Include(x=>x.UserWorkOrders)
                    .ThenInclude(x=>x.User)
                    .FirstOrDefaultAsync(sc => sc.WorkOrderId == workOrderId);


                //var a= from b in context.WorkOrder.Include(x=>x.Customer) 
                //       join c in context.WorkOrderDetail on b.WorkOrderId equals c.wo

                var b = await context.Customer.AsNoTracking()
                    .Include(x => x.Aggregates)
                    .ThenInclude(x => x.Contacts)
                    .Include(x => x.Aggregates)
                    .ThenInclude(x => x.Addresses).Where(x => x.CustomerID == a.CustomerId).FirstOrDefaultAsync();

                a.Customer = b;

                List<WorkOrderDetail> c = null;
                if (CurrentStatus==0)
                {
                    c = await context.WorkOrderDetail.AsNoTracking()
                 //.Include(d => d.PieceOfEquipment).ThenInclude(c=>c.EquipmentTemplate).ThenInclude(d=>d.Manufacturer1)
                 //.Include(x => x.CurrentStatus)
                 .Where(x => x.WorkOrderID == workOrderId).ToListAsync();

                    foreach(var wod in c)
                    {
                        var poe = await context.PieceOfEquipment.AsNoTracking().Where(x => x.PieceOfEquipmentID == wod.PieceOfEquipmentId).FirstOrDefaultAsync();
                        if(poe != null)
                        {
                            var et = await context.EquipmentTemplate.AsNoTracking().Where(x => x.EquipmentTemplateID == poe.EquipmentTemplateId).FirstOrDefaultAsync();
                            poe.EquipmentTemplate = et;
                        }
                        else
                        {
//                            Console.WriteLine("---------- Error load poe -----------------------");

                            throw new Exception("Error load POE  ");
                            
                        }
                       
                        wod.PieceOfEquipment = poe;
                    
                    }

                }
                else
                {
                    c = await context.WorkOrderDetail.AsNoTracking()
                      .Include(d => d.PieceOfEquipment).ThenInclude(c => c.EquipmentTemplate).ThenInclude(d => d.Manufacturer1)
                    .Include(x => x.CurrentStatus)
                    .Where(x => x.WorkOrderID == workOrderId && x.CurrentStatusID == CurrentStatus).ToListAsync();
                }
                

                foreach( var item in c)
                {
                    var eto = await context.EquipmentType.AsNoTracking()
                        .Where(x => x.EquipmentTypeGroupID == item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID && x.HasWorkOrderDetail==true).ToListAsync();


                    if (eto.Count == 1)
                    {
                        item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject = eto.FirstOrDefault();
                    }
                    else if(eto.Count > 1)
                    {
                        item.PieceOfEquipment.EquipmentTemplate.AditionalEquipmentTypesJSON = Newtonsoft.Json.JsonConvert.SerializeObject(eto);
                    }
                }
                //if(a?.UserWorkOrders?.Count > 0)
                //{
                //    a.Users
                //}

                a.WorkOrderDetails = c;


                ////YPPP 9515 WeightSet and Standard
                ///
                List<WO_Weight> wO_Weights = new List<WO_Weight>();


                var w = await context.WO_Weight.AsNoTracking().Where(w => w.WorkOrderID == workOrderId).ToArrayAsync();

                //Include(x => x.WeightSet)
                if (w != null && w.Count() > 0)
                {
                    foreach (var w1 in w)
                    {
                        var w12 = await context.WeightSet.AsNoTracking().Where(x => x.WeightSetID == w1.WeightSetID)
                        .AsNoTracking().FirstOrDefaultAsync();
                        w1.WeightSet = w12;
                        if (w12 != null)
                        {
                            w1.WeightSet.Option = w1.Option;
      
                        }
                        else
                        {
//                            Console.WriteLine("-------------------Weight Set not found-------------------------------");
                        }

                        wO_Weights.Add(w1);
                    }
 
                }


                var wst = await context.WO_Standard.AsNoTracking().Where(w => w.WorkOrderID == workOrderId).ToArrayAsync();

                if (wst != null && wst.Count() > 0)
                {
                    a.WO_Standard = wst;
                    int cont = 1;

                    foreach (var w1 in wst)
                    {
                        var w12 = await context.PieceOfEquipment.AsNoTracking().Where(x => x.PieceOfEquipmentID == w1.PieceOfEquipmentID).FirstOrDefaultAsync();

                        if (w12 != null)
                        {
                            w12.TestPointResult = context.GenericCalibrationResult2.AsNoTracking().Where(x => x.ComponentID == w1.PieceOfEquipmentID && x.Component == "PieceOfEquipmentCreate").ToList();
                        }

                        WO_Weight wO_Weight = new WO_Weight();

                        wO_Weight.WeightSetID = cont;
                        wO_Weight.WorkOrderID = workOrderId;
                        wO_Weight.WeightSet = new WeightSet();
                        wO_Weight.WeightSet.Option = w1.Option;
                        wO_Weight.WeightSet.WeightSetID = cont;
                        wO_Weight.WeightSet.PieceOfEquipmentID = w12.PieceOfEquipmentID;
                        wO_Weight.WeightSet.PieceOfEquipment = w12;
                        wO_Weight.WeightSet.PieceOfEquipment.WeightSets = null;
                        wO_Weight.WeightSet.PieceOfEquipment.EquipmentTemplate = null;
                        wO_Weight.WeightSet.WO_Weights = null;
                        wO_Weights.Add(wO_Weight);
                        cont++;
                    }

                }

                a.WO_Weights = wO_Weights;

                //end YPPP 9515 WeightSet and Standard


                return a;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultSet<WorkOrder>> GetWorkOrder(Pagination<WorkOrder> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var filterQuery = Querys.WorkOrderFilter(pagination.Filter);

            var queriable = context.WorkOrder.Include(x=>x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            var simplequery = context.WorkOrder.Include(x => x.Customer); ;

            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<WorkOrder>( pagination, simplequery, filterQuery);

            return result;


        }

        public async Task<WorkOrder> GetWorkOrderByInvoice(WorkOrder DTO)
        {
        	    await using var context = await DbFactory.CreateDbContextAsync();
        	
                var result = await context.WorkOrder.AsNoTracking().Where(x=> !string.IsNullOrEmpty(DTO.Invoice) 
                && x.Invoice==DTO.Invoice).FirstOrDefaultAsync();

               //if(result != null)
               // {
               // throw new AlreadyInUseException();
               // }
                var custoid= await context.Customer.Where(x=> !string.IsNullOrEmpty(DTO.CustomerInvoice) 
                && x.CustomID==DTO.CustomerInvoice)
                .Include(x=>x.Aggregates).ThenInclude(x=>x.Addresses)
                .Include(x=>x.Aggregates).ThenInclude(x=>x.Contacts)
                .FirstOrDefaultAsync();

                int? address = null;
                int? contactid = null;

                if(custoid?.Aggregates?.FirstOrDefault()?.Addresses?.FirstOrDefault() != null)
                {
                    address= custoid.Aggregates.FirstOrDefault().Addresses.FirstOrDefault().AddressId;
                }

                if (custoid?.Aggregates?.FirstOrDefault()?.Contacts?.FirstOrDefault() != null)
                {
                    contactid = custoid.Aggregates.FirstOrDefault().Contacts.FirstOrDefault().ContactID;
                }



            if (custoid != null)
                {
                    DTO.CustomerId = custoid.CustomerID;
                    if (address.HasValue)
                    {
                    DTO.AddressId = address.Value;
                    }
                    if (contactid.HasValue)
                    {
                    DTO.ContactId = contactid.Value;
                    }

                    
                }
            


            return DTO;
        }

        public async Task<ResultSet<WorkOrder>> GetWorkOrderOff(Pagination<WorkOrder> pagination)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            WorkOrder w = pagination.Entity;
            int id = 0;
            if (w.Users != null && w.Users.Count > 0)
            {
               id= w.Users[0].UserID;
            }            

            

            Pagination<WorkOrderDetail> p = new Pagination<WorkOrderDetail>();

                p.ColumnName = pagination.ColumnName;
            p.Filter = pagination.Filter;
            //p.Object = pagination.Object.;

            p.Page = pagination.Page;
            p.Show = pagination.Show;
            p.SortingAscending = pagination.SortingAscending;
            p.Entity = new WorkOrderDetail();
            p.Entity.TechnicianID = id;

            var a = (from WOD in context.WorkOrderDetail.Include(x => x.PieceOfEquipment).ThenInclude(x => x.Customer).Include(x => x.CurrentStatus)
                     join U in context.User_WorkOrder on WOD.WorkOrderID equals U.WorkOrderID
                     //where U.UserID == id
                     select WOD);


            var filterQuery = Querys.WODbyTech(id);

            var queriable = a; // context.WorkOrder.Include(x => x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            var simplequery = a;

            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<WorkOrderDetail>(p, simplequery, filterQuery);

            if (result.List != null)
            {
                foreach (var item in result.List)
                {
                    item.PieceOfEquipment.WorOrderDetails = null;
                }
            }

            var filterQuery2 = Querys.WorkOrderFilter(pagination.Filter);
            List<int> list2 = new List<int>();
          foreach (var iten in result.List)
            {
                //WorkOrder w1 = new WorkOrder();
                //w1.WorkOrderId = iten.WorkOrderID;
                list2.Add(iten.WorkOrderID);
            }




            //var c = await context.PieceOfEquipment.Include(x => x.WeightSets).Where(x => context.WeightSet.Any(c => c.Reference == item.Reference && x.SerialNumber == item.Serial)).ToListAsync();
            //var result = peopleList2.Where(p => !peopleList1.Any(p2 => p2.ID == p.ID));
            var queriable2 =  context.WorkOrder.Include(x=>x.Customer).AsNoTracking().Where(z=> list2.Contains(z.WorkOrderId));//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();
            var simplequery2 = queriable2;

            //ResultSet<WorkOrder> resultSet = new ResultSet<WorkOrder>();

            //resultSet.List = queriable2;
            //resultSet.CurrentPage = result.CurrentPage;
            //resultSet.Shown = result.Shown;
            //resultSet.PageTotal = result.PageTotal;
            pagination.Show = 1000;
            pagination.Page = 1;
            var resultSet = await queriable2.PaginationAndFilterQuery<WorkOrder>(pagination, simplequery2, filterQuery2);

            return resultSet;
            //return result;

            //return null;

        }



        public async Task<WorkOrder> InsertWokOrder(WorkOrder workOrder)
        {

//            Console.WriteLine("InsertWokOrder WORKORDER " + workOrder.WorkOrderId.ToString());

            await using var context = await DbFactory.CreateDbContextAsync();

            bool isWod = false;
            //no se debe generar WOD sin POE
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(workOrder);

            //Console.WriteLine(json);
            //
            if (workOrder.StatusID == 3)
            {
                var wods2 = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderID == workOrder.WorkOrderId).ToListAsync();

                if(wods2?.Count > 0)
                {
                    foreach (var wod33 in wods2)
                    {
                        wod33.CurrentStatusID = 5;
                        wod33.HasBeenCompleted = true;

                        context.Update(wod33);
                    }

                    await context.SaveChangesAsync();
                }
               
            }


            workOrder.Address = null;

            if (workOrder.WorkOrderDetails == null || workOrder.WorkOrderDetails.Count == 0)
            {
                workOrder.WorkOrderDetails = null;

            }
            else
            {
                isWod = true;

                workOrder.WorkOrderDetails.ToList().ForEach(async item =>
               {
                   

                    if (item.PieceOfEquipment != null)
                   {
                       item.PieceOfEquipmentId = item.PieceOfEquipment.PieceOfEquipmentID;
                       item.CalibrationDate = DateTime.Now;
                       item.PieceOfEquipment = null;
                   }
                   item.CurrentStatus = null;
                   item.PossibleNextStatus = null;
                   item.PreviusStatus = null;
                   item.WorkOder = null;
               });

            }

            List<User_WorkOrder> lst = new List<User_WorkOrder>();
            List<User> users = new List<User>();

            var usersserver = await context.User_WorkOrder.AsNoTracking().Where(x => x.WorkOrderID == workOrder.WorkOrderId).ToListAsync();

            if(usersserver.Count > 0)
            {
                foreach (var itu in usersserver)
                {
                    User uuss = null;
                    if(workOrder.Users != null)
                    {
                       uuss = workOrder.Users.Where(x => x.UserID == itu.UserID).FirstOrDefault();
                    }
                    if(uuss == null)
                    {

                        var aa = await context.WorkOrderDetail.AsNoTracking()
                            .Where(x => x.WorkOrderID == workOrder.WorkOrderId && x.TechnicianID.HasValue && x.TechnicianID.Value==itu.UserID).FirstOrDefaultAsync();

                        if (aa == null)
                        {
                            context.User_WorkOrder.Remove(itu);
                        
                        
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            //throw new Exception("Technician in Work Order Detail: " + aa.WorkOrderDetailID );
//                            Console.WriteLine("Technician in Work Order Detail: " + aa.WorkOrderDetailID);
                        }

                        
                    }          
                   
                        
                
                }
            }
            
            if (workOrder.Users != null)
            {

                users = workOrder.Users.ToList(); 

                foreach (var item in workOrder.Users)
                {
                    item.UserWorkOrders = lst;
                }


                workOrder.Users = null;
                workOrder.UserWorkOrders = null;
            }
            //workOrder.PieceOfEquipments = null;
            ///workOrder.WorkOrderId = 0;

            var woo = await context.WorkOrder.AsNoTracking().Where(x => x.WorkOrderId == workOrder.WorkOrderId).FirstOrDefaultAsync();
            workOrder.Users = null;
            workOrder.UserWorkOrders = null;

            List<WorkOrderDetail> wods = null;
               if(workOrder?.WorkOrderDetails?.Count > 0)
            {
                wods= workOrder.WorkOrderDetails.ToList();
            }


            workOrder.WorkOrderDetails = null;

            await SaveWOWeightSet(workOrder);
            workOrder.WO_Weights = null;
            workOrder.WO_Standard = null;

            if (!string.IsNullOrEmpty(workOrder.Invoice))
            {
                var woo2 = await context.WorkOrder.AsNoTracking().Where(x => x.Invoice == workOrder.Invoice).FirstOrDefaultAsync();

                if(woo != null && woo2 != null && woo2.WorkOrderId != woo.WorkOrderId)
                {
                    throw new Exception("Error in Work order Invoice is not correct");
                }
                woo = woo2;
                if(woo != null)
                {
                    workOrder.WorkOrderId = woo.WorkOrderId;
                }
               
            }

            if (workOrder.StatusID.HasValue == false)
            {
                workOrder.StatusID = 1;
            }


            if (woo != null)
                {
                    context.WorkOrder.Update(workOrder);
                }
                else
                {
                    int maxwod = workOrder.WorkOrderId;
                    if (!string.IsNullOrEmpty(workOrder.Invoice) && workOrder.WorkOrderId == 0)
                    {
                    workOrder.WorkOrderId = NumericExtensions.GetUniqueID(workOrder.Invoice);
                    }
                else if(workOrder.WorkOrderId==0)
                    {

                    // Replace the problematic line with the following:
                    var name = _configuration.GetSection("Reports:Customer")?.Value;
                                       
                    int RangeMin = 0;

                    if (name == "LTI")
                    {
                        string RangeMinS = "0";   

                        RangeMin = Convert.ToInt32(RangeMinS);   
                        

                        var ifex = await context.WorkOrder.AsNoTracking().FirstOrDefaultAsync();

                        if (ifex != null)
                        {
                            maxwod = await context.WorkOrder.AsNoTracking().MaxAsync(x => x.WorkOrderId) + RangeMin;
                        }
                        maxwod = maxwod + 1;
                    }

                    }



                workOrder.WorkOrderId = NumericExtensions.GetUniqueID(maxwod);



                context.WorkOrder.Add(workOrder);
                }
           
            await context.SaveChangesAsync();

            if(wods != null)
            {
                IWorkOrderDetailRepository wodrep = new WODRepositoryEF<TContext>(DbFactory);

                foreach (var woditem in wods)
                {
                    context.WorkOrderDetail.Add(woditem);
                    await context.SaveChangesAsync();
                    //await wodrep.ChangeStatus(woditem);
                }


            }


                foreach(var item in users)
                {
                    item.UserID = item.UserID;
                    User_WorkOrder uw = new User_WorkOrder();

                    var uc = usersserver.Where(x => x.UserID == item.UserID && x.WorkOrderID==workOrder.WorkOrderId).FirstOrDefault();

                    if (uc != null)
                    {
                        context.User_WorkOrder.Update(uc);
                    }
                    else
                    {
                       uw.UserID = item.UserID;
                       uw.WorkOrderID = workOrder.WorkOrderId;
                                   
                    context.User_WorkOrder.Add(uw);
                    }
                    
                    await context.SaveChangesAsync();
                    //lst.Add(uw);
                }


                await SaveOffline();

                if (!isWod)
                {
                    return workOrder;
                }
                else
                {
                  var w = await context.WorkOrderDetail.AsNoTracking().Include(x => x.WorkOder).Include(d => d.PieceOfEquipment).Where(x => x.WorkOrderID == workOrder.WorkOrderId).ToArrayAsync();
                                       
                  foreach(var t in w)
                    {
                        t.PieceOfEquipment.WorOrderDetails = null;
                        t.PieceOfEquipment.EquipmentTemplate = null;
                        t.WorkOder.WorkOrderDetails = null;
                    }

                    workOrder.WorkOrderDetails = w;

                    return workOrder;
                }


        }

        async Task SaveWOWeightSet(WorkOrder DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var wo = await context.WorkOrder.Where(x => x.WorkOrderId == DTO.WorkOrderId).AsNoTracking().FirstOrDefaultAsync();

            if (wo == null)
            {
                return;
            }
            var stas = await context.WO_Standard.Where(x => x.WorkOrderID == DTO.WorkOrderId).AsNoTracking().ToArrayAsync();
         
            if (stas != null && stas.Count() > 0)
            {
                context.WO_Standard.RemoveRange(stas);
                await context.SaveChangesAsync();
            }

            if (DTO.WO_Standard != null && DTO.WO_Standard.Count > 0)
            {
                foreach (var www in DTO.WO_Standard)
                {

                    WO_Standard ws = new WO_Standard();
                    ws = www;
                    context.WO_Standard.Add(ws);

                }
                await context.SaveChangesAsync();
            }

            var sss = await context.WO_Weight.AsNoTracking().Where(x => x.WorkOrderID == DTO.WorkOrderId).ToArrayAsync();

            if (sss != null && sss.Count() > 0)
            {
                context.WO_Weight.RemoveRange(sss);
                await context.SaveChangesAsync();
            }

            var existingWeightSetIds = await context.WeightSet.Select(w => w.WeightSetID).ToHashSetAsync();
            if (DTO.WO_Weights != null && DTO.WO_Weights.Count > 0)
            {
                var ww = DTO.WO_Weights.DistinctBy2(x => x.WeightSetID).ToArray();

                foreach (var www in ww)
                {
                    if (!existingWeightSetIds.Contains(www.WeightSetID))
                    {
                        
                        continue;
                    }


                    www.WeightSet = null;
                    www.WorkOrder = null;

                    if (context.WO_Weight != null)
                    {
                        context.WO_Weight.Add(www);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new InvalidOperationException("WO_Weight DbSet is not initialized.");
                    }
                }

            }
        }

        async Task SaveOffline()
        {
            var ty1 = DbFactory.GetType();
            var ty2 = typeof(SqliteWasmDbContextFactory<TContext>);

            if (ty1 == ty2)
            {
//                Console.WriteLine("MakeBackup");
                await using var context = await DbFactory.CreateDbContextAsync();

                var fac = DbFactory as ISqliteWasmDbContextFactory<TContext>;
                await fac.MakeBackup(context);

            }

        }

        public async Task<WorkOrder> UpdateWorkOrder(WorkOrder workOrder)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.WorkOrder.Update(workOrder);
            //context.Entry(a).State= EntityState.Modified;
            await context.SaveChangesAsync();
            return workOrder;
        }


        public async Task<IEnumerable<Address>>  GetAddressByCustomerId(int customerId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            try
            { //context.Customer.Include(c=>c.Aggregates).Where(x=>x.CustomerID== customerId).ToArrayAsync();

                var res = await context.Customer.Include(x => x.Aggregates).
                 ThenInclude(c => c.Addresses).Where(x => x.CustomerID == customerId).FirstOrDefaultAsync();
                
                if (res != null && res.Aggregates.Count > 0)
                {
                    List<Address> lst = new List<Address>();
                    foreach(var item in res.Aggregates)
                    {
                        lst.AddRange(item.Addresses);
                    }

                    return lst;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<IEnumerable<Contact>> GetContactsByCustomerId(int customerId)
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            var res = await context.Customer.Include(x=>x.Aggregates).
                ThenInclude(c=>c.Contacts).Where(x => x.CustomerID == customerId).FirstOrDefaultAsync();
            if(res != null && res.Aggregates.Count > 0)
            {

                List<Contact> lst = new List<Contact>();
                foreach (var item in res.Aggregates)
                {
                    
                    lst.AddRange(item.Contacts);
                }

                return lst;


                //return res.Aggregates.ElementAt(0).Contacts;
            }
            return null;
        }

        public async Task<IEnumerable<PieceOfEquipment>> GetPieceOfEquipmentByCustomerId(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var res = await context.PieceOfEquipment.Where(x => x.CustomerId == id).ToArrayAsync();

            return res;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var res = await context.User.ToArrayAsync();

            return res;
        }
        public async Task<Status> GetDefaultStatus()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var res = await context.Status.Where(X=>X.IsDefault==true).FirstOrDefaultAsync();

            return res;
        }

        public async Task<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByStatus(Pagination<WorkOrderDetailByStatus> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var wod = pagination.Entity;
            var filterQuery = Querys.WorkOrderDetailByStatusFilter(pagination.Entity);
            var queriable = context.WorkOrderDetailByStatus.AsQueryable();
   
            var result = await queriable.PaginationAndFilterQuery<WorkOrderDetailByStatus>( pagination, context.WorkOrderDetailByStatus, filterQuery);

            return result;


           
        }

        public async Task<ResultSet<WorkOrderDetailByStatus>> GetWorkOrderDetailByEquipment(Pagination<WorkOrderDetailByStatus> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var wod = pagination.Entity;
            var filterQuery = Querys.WorkOrderDetailByEquipmentFilter(pagination.Entity);
            var queriable = context.WorkOrderDetailByEquipment
            //.Where(x => (x.StatusId == wod.StatusId || wod.StatusId == 0)
            //// && (x.WorkOrderReceiveDate == wod.WorkOrderReceiveDate || x.WorkOrderReceiveDate == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
             //&& (x.EquipmentTypeID == wod.EquipmentTypeID || x.EquipmentTypeID == 0)
            // && (x.Model.ToUpper().Contains(wod.Model.ToUpper()) || x.Model == null)
            // && (x.Company.ToUpper().Contains(wod.Company.ToUpper()) || x.Company == null)
           // )
            .AsQueryable();

            var queriable1 = context.WorkOrderDetailByEquipment
            .Where(x => (x.StatusId == wod.StatusId || wod.StatusId == 0)
            //// && (x.WorkOrderReceiveDate == wod.WorkOrderReceiveDate || x.WorkOrderReceiveDate == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
             && (x.EquipmentTypeID == wod.EquipmentTypeID || x.EquipmentTypeID == 0)
            // && (x.Model.ToUpper().Contains(wod.Model.ToUpper()) || x.Model == null)
            // && (x.Company.ToUpper().Contains(wod.Company.ToUpper()) || x.Company == null)
            )
            .ToList();



            var result = await queriable.PaginationAndFilterQuery<WorkOrderDetailByStatus>( pagination, context.WorkOrderDetailByStatus, filterQuery);

            return result;
       }

        public async Task<ResultSet<WorkOrderDetailByCustomer>> GetWorkOrderDetailByCustomer(Pagination<WorkOrderDetailByCustomer> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var wod = pagination.Entity;
            var filterQuery = Querys.WorkOrderDetailByCustomerFilter(pagination.Entity);
            var queriable = context.WorkOrderDetailByCustomer
            .AsQueryable();


            var result = await queriable.PaginationAndFilterQuery<WorkOrderDetailByCustomer>( pagination, context.WorkOrderDetailByCustomer, filterQuery);

            return result;
        }

        //public async Task<IEnumerable<User>> GetUserByCustomerId(int customerId)
        //{
        //    //var res = await context.User.Where(x => x == customerId).ToArrayAsync();

        //    //return res;
        //}
        public async Task<bool> Save()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            return (await context.SaveChangesAsync()) > 0;
        }

        public async Task<ICollection<Certificate>> GetCertificateByWod(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var res = await  context.Certificate.Where(x => x.WorkOrderDetailId == id).ToListAsync();

            return res;
        }

        public async Task<WeightSet> DeleteWeightSet(WeightSet id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            //id.Delete = true;
            context.Entry(id).State = EntityState.Deleted;
            await context.SaveChangesAsync();
            return id;
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
       

        #endregion


         public async Task<ICollection<CertificatePoE>> GetCertificateXPoE(PieceOfEquipment DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var result = await context.CertificatePoE.AsNoTracking().Where(x=>x.PieceOfEquipmentID==DTO.PieceOfEquipmentID).ToArrayAsync();

            return result;
        }


        public async Task<ICollection<CalibrationType>> GetCalibrationTypes(PieceOfEquipment DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var result = await context.CalibrationType.AsNoTracking().ToArrayAsync();

            return result;
        }


       


        public async Task<IEnumerable<CalibrationType>> GetCalibrationTypes()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var result = await context.CalibrationType.AsNoTracking().Include(x=>x.CalibrationSubTypes).ToArrayAsync();

            return result;
        }

        public async Task LoadUserWorkOrder(ICollection<User_WorkOrder> pag)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            foreach (var item in pag)
            {
                context.User_WorkOrder.Add(item);
                
            }

            await context.SaveChangesAsync();


            
        }

        public async Task<IEnumerable<WOStatus>> GetStatus()
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var a = await context.WOStatus.AsNoTracking().ToListAsync();

            return a;
        }

    }
}
