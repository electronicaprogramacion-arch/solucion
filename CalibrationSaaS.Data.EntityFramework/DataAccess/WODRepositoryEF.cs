using AutoMapper;
using Bogus;
using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.Blazor.Services.Offline.Sqlite;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using Castle.Components.DictionaryAdapter.Xml;
using Helpers;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using LinqKit;
using LinqKit.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;
using SqliteWasmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;
using static System.Net.Mime.MediaTypeNames;
using CalibrationSaaS.Models;
//using PredicateBuilder = CalibrationSaaS.Domain.Aggregates.Querys.PredicateBuilder;



namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class WODRepositoryEF<TContext> : RepositoryEF<TContext>, IWorkOrderDetailRepository, IDisposable where TContext : DbContext, ICalibrationSaaSDBContextBase
    {
        //private CalibrationSaaSDBContext context;

        //public WODRepositoryEF(CalibrationSaaSDBContext _context)
        //{
        //    context = _context;
        //}

        private readonly IDbContextFactory<TContext> DbFactory;
        private readonly Microsoft.Extensions.Logging.ILoggerFactory loggerFactory;


        public WODRepositoryEF(IDbContextFactory<TContext> dbFactory)
        {
            DbFactory = dbFactory;
        }

        public WODRepositoryEF(IDbContextFactory<TContext> dbFactory,Microsoft.Extensions.Logging.ILoggerFactory _loggerFactory)
        {
            DbFactory = dbFactory;
            loggerFactory = _loggerFactory;
        }
        public async Task<WorkOrderDetail> Create(WorkOrderDetail DTO)
        {
            return DTO;
        }

        public async Task Clear() {
            await using var context = await DbFactory.CreateDbContextAsync();

            //context.WorkOrderDetail.Clear(context);
        }


        public async Task<WorkOrderDetail> Delete(WorkOrderDetail DTO,bool Reset=false)
        {
            int line = 0;

            try
            { 
                
                
                
                DTO.CurrentStatus = null;

                await using var context = await DbFactory.CreateDbContextAsync();
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;


                //var bcr = await context.BasicCalibrationResult.Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();



                //var linbcr = await context.Linearity.Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                //var repbcr = await context.Repeatability.Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                //var eccebcr = await context.Eccentricity.Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                //var testgr = await context.TestPointGroup.Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

                //List<TestPoint> lsttest = new List<TestPoint>();
                //foreach (var ityu in testgr)
                //{
                //    var testpoin = await context.TestPoint.Where(x => x.TestPointGroupTestPoitGroupID == ityu.TestPoitGroupID).ToListAsync();

                //    lsttest.AddRange(testpoin);
                //}

                try
                {
                    if (1 == 1)
                    {
                        var csh1 = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.Component == "WorkOrderItem" && x.ComponentID == DTO.WorkOrderDetailID.ToString()).ToListAsync();

                        //var csh2 = await context.CalibrationSubType_Weight.Where(x => x.Component == "WorkOrderItem" && x.ComponentID == DTO.WorkOrderDetailID.ToString()).ToListAsync();


                        var csh3 = await context.GenericCalibrationResult.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                        var csh4 = await context.GenericCalibration.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                        var csh5 = await context.GenericCalibration2.AsNoTracking().Where(x => x.Component == "WorkOrderItem" && x.ComponentID == DTO.WorkOrderDetailID.ToString()).ToListAsync();

                        var csh6 = await context.GenericCalibrationResult2.AsNoTracking().Where(x => x.Component == "WorkOrderItem" && x.ComponentID == DTO.WorkOrderDetailID.ToString()).ToListAsync();


                        var csh7 = await context.Force.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                        var csh8 = await context.ForceResult.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                        var csh9 = await context.Rockwell.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                        var csh10 = await context.RockwellResult.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();


                        if (csh3?.Count > 0)
                        {
                            foreach (var itms in csh3)
                            {
                                itms.GenericCalibration = null;

                            }
                            context.GenericCalibrationResult.RemoveRange(csh3);


                        }

                        if (csh4?.Count > 0)
                        {
                            foreach (var itms in csh4)
                            {
                                itms.BasicCalibrationResult = null;
                                itms.CalibrationSubType_Weights = null;
                                itms.CalibrationResultContributors = null;
                                itms.Standards = null;
                                itms.TestPoint = null;


                            }
                            context.GenericCalibration.RemoveRange(csh4);


                        }


                        if (csh6?.Count > 0)
                        {
                            foreach (var itms in csh6)
                            {

                                itms.GenericCalibration2 = null;


                            }
                            context.GenericCalibrationResult2.RemoveRange(csh6);


                        }

                        if (csh5?.Count > 0)
                        {
                            foreach (var itms in csh5)
                            {

                                itms.CalibrationSubType_Weights = null;
                                itms.CalibrationResultContributors = null;
                                itms.Standards = null;
                                itms.TestPoint = null;


                            }
                            context.GenericCalibration2.RemoveRange(csh5);


                        }

                       

                        if (csh10?.Count > 0)
                        {
                            foreach (var itms in csh10)
                            {

                                itms.Rockwell = null;



                            }
                            context.RockwellResult.RemoveRange(csh10);


                        }

                        if (csh9?.Count > 0)
                        {
                            foreach (var itms in csh9)
                            {
                                itms.BasicCalibrationResult = null;

                                itms.CalibrationSubType_Weights = null;
                                itms.CalibrationResultContributors = null;
                                itms.Standards = null;
                                itms.TestPoint = null;


                            }
                            context.Rockwell.RemoveRange(csh9);


                        }

                        if (csh8?.Count > 0)
                        {
                            foreach (var itms in csh8)
                            {
                                itms.Force = null;




                            }
                            context.ForceResult.RemoveRange(csh8);


                        }

                        if (csh7?.Count > 0)
                        {
                            foreach (var itms in csh7)
                            {
                                itms.BasicCalibrationResult = null;

                                itms.CalibrationSubType_Weights = null;
                                itms.CalibrationResultContributors = null;
                                itms.Standards = null;
                                itms.TestPoint = null;


                            }
                            context.Force.RemoveRange(csh7);


                        }

                        await context.SaveChangesAsync();
                    }
                }
                catch(Exception ex)
                {
//                    Console.WriteLine(ex.Message);
                }
              

               


                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                if (1 == 1)
                {

                    var csh = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

                    context.CalibrationSubType_Weight.RemoveRange(csh);
                    await context.SaveChangesAsync();

                    line = 1;

                    var cer = await context.Certificate.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                    context.Certificate.RemoveRange(cer);
                    await context.SaveChangesAsync();
                    line = 2;

                    var ecc = await context.Eccentricity.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                    context.Eccentricity.RemoveRange(ecc);
                    await context.SaveChangesAsync();

                    line = 3;

                    var rep = await context.Repeatability.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();



                    context.Repeatability.RemoveRange(rep);
                    await context.SaveChangesAsync();

                    line = 4;
                    var linea = await context.Linearity.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                    foreach (var lk in linea)
                    {
                        lk.UnitOfMeasure = null;
                        lk.CalibrationUncertaintyValueUncertaintyUnitOfMeasure = null;
                    }
                    context.Linearity.RemoveRange(linea);
                    await context.SaveChangesAsync();

                    line = 5;


                    var b = await context.BasicCalibrationResult.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                    foreach (var bcl in b)
                    {
                        bcl.UnitOfMeasure = null;

                    }
                    context.BasicCalibrationResult.RemoveRange(b);
                    await context.SaveChangesAsync();
                }

             


                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                line = 6;

                var be = await context.BalanceAndScaleCalibration.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                if(be?.Count > 1)
                {
                    context.BalanceAndScaleCalibration.RemoveRange(be);
                    context.BalanceAndScaleCalibration.Add(be.First());

                }
                else if(be?.Count == 0 && DTO.BalanceAndScaleCalibration != null)
                {
                    BalanceAndScaleCalibration bb = new BalanceAndScaleCalibration();

                    bb.WorkOrderDetailId = DTO.WorkOrderDetailID;
                    
                     bb.CalibrationTypeId = DTO.BalanceAndScaleCalibration.CalibrationTypeId;
                    
                                     

                    context.BalanceAndScaleCalibration.Add(bb);
                }
               
                await context.SaveChangesAsync();
                line = 7;
                //var linea = context.Linearity.Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();
                //var t = await context.TestPoint.Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                var t = await context.TestPointGroup.AsNoTracking().Include(x => x.TestPoints).Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

                foreach (var item in t)
                {
                    foreach (var item2 in item.TestPoints)
                    {
                        item2.TestPointGroup = null;
                        item2.UnitOfMeasurement = null;
                        item2.UnitOfMeasurementOut = null;
                        context.Remove(item2);
                        await context.SaveChangesAsync();
                    }
                }
                line = 8;
                t = await context.TestPointGroup.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

                foreach (var tpg in t)
                {
                    tpg.UnitOfMeasurementOut = null;
                    tpg.OutUnitOfMeasurement = null;
                }

                context.TestPointGroup.RemoveRange(t);
                await context.SaveChangesAsync();
                line = 9;

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var wh = await context.WOD_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

                foreach (var item3 in wh)
                {
                    context.WOD_Weight.Remove(item3);
                    await context.SaveChangesAsync();
                }
                line = 10;

                //WorkDetailHistory wh = new WorkDetailHistory();
                var a = await context.WorkDetailHistory.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

                foreach (var u in a)
                {
                    u.WorkOrderDetail = null;

                }

                //wh.WorkOrderDetailID = DTO.WorkOrderDetailID;
                context.WorkDetailHistory.RemoveRange(a);
                await context.SaveChangesAsync();

                line = 11;


                DTO.BalanceAndScaleCalibration = null;
                DTO.HumidityUOM = null;
                DTO.TemperatureUOM = null;
                DTO.WOD_Weights = null;
                DTO.WOD_TestPoints = null;
                DTO.WorkOder = null;
                DTO.PieceOfEquipment = null;
                DTO.Ranges = null;
                DTO.TestGroups = null;
                DTO.WorkDetailHistorys = null;
                DTO.CurrentStatus = null;
                DTO.PreviusStatus = null;
                if (Reset)
                {
                    

                    var wodd = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).FirstOrDefaultAsync();

                    var local2 = context.Set<WorkOrderDetail>()
                .Local
                .FirstOrDefault(entry => entry.WorkOrderDetailID.Equals(DTO.WorkOrderDetailID));

                    if (local2 != null)
                    {
                        // detach
                        context.Entry(local2).State = EntityState.Detached;
                    }

                    // set Modified flag in your entry
                    //context.Entry(DTO).State = EntityState.Modified;



                    //var record6 = new WorkOrderDetail { WorkOrderDetailID = DTO.WorkOrderDetailID, CurrentStatusID = 1 };
                    //context.Attach(record6);
                    //context.Entry(record6).Property(r => r.CurrentStatusID).IsModified = true;
                    wodd.CurrentStatus = null;
                    wodd.PreviusStatus = null;
                    wodd.CurrentStatusID = 1;
                    wodd.TechnicianID = null;
                    wodd.CalibrationIntervalID = 0;
                    wodd.CalibrationDate = null;
                    wodd.CalibrationCustomDueDate = null;
                    wodd.CalibrationNextDueDate = null;
                    context.WorkOrderDetail.Update(wodd);

                    DTO = wodd;
                }
                else
                {
                    context.WorkOrderDetail.Remove(DTO);
                }
               
                await context.SaveChangesAsync();

                line = 12000;
//                Console.WriteLine("delete success");
                return DTO;

            }
            catch (Exception ex)
            {
//                Console.WriteLine(ex.Message + " " + line);
//                Console.WriteLine(ex?.InnerException?.Message + " " + line);
//                Console.WriteLine(ex?.StackTrace);
                throw new Exception(ex.Message + " " + line);
            }

        }

        //public async Task<IQueryable> GetEntity() 
        //{
        //    await using var context = await DbFactory.CreateDbContextAsync();
        //    Type type = Type.GetType("WorkOrderDetail");//typeof(T);
        //    Type spoofedType = typeof(InternalDbSet<>).MakeGenericType(type);
        //    //instance is only used to spoof the binding
        //    dynamic instance = Activator.CreateInstance(spoofedType, context, spoofedType.Name);
        //    return context.Set<T>();
        //}

        //public IEnumerable<T> SpoofedMethod<T>(DbSet<T> _) where T : class
        //{
        //    return context.Set<T>().Select(e => e);
        //}


        //public CampaignCreative GetCampaignCreativeById(int id, string[] includes)
        //{
        //    await using var context = await DbFactory.CreateDbContextAsync();

        //    var query = db.CampaignCreatives.AsQueryable();
        //        foreach (string include in includes)
        //        {
        //            query = query.Include(include);
        //        }

        //        return query
        //            .AsNoTracking()
        //            .Where(x => x.Id.Equals(id))
        //            .FirstOrDefault();

        //}

        public async Task<ResultSet<T>> FetchFromTable<T>(DbSet<T> _,TContext context,Pagination<T> pagination) where T : class
        {
            //DbSet<T> parameter is not needed - it will throw an Exception

            List<object> lstParameter = null;
            FormattableString sqlstringfor = null;
            string sqlstring = pagination.ReportView.Query;
            List<string> lstwhere = new List<string>();
            if (pagination.ReportView != null && pagination?.ReportView?.Parameters?.Count > 0)
            {
                lstParameter = new List<object>();
                int cont = 0;
                foreach (var item in pagination?.ReportView?.Parameters)
                {
                    //var columnValue = SqliteParameter(item.Name, item.Value);

                    string query = item.ColumnName + " " + item.Operator + " {" + cont + "}"; // + item.Value;

                    lstwhere.Add(query);


                    lstParameter.Add("" + item.Value + "");
                    cont++;
                }

                sqlstring = sqlstring + " where " + string.Join(" or ", lstwhere);

                sqlstringfor = FormattableStringFactory.Create(sqlstring, lstParameter.ToArray());

                string res = sqlstringfor.ToString();

            }


            var a= context.Set<T>().FromSql(sqlstringfor).AsNoTracking().AsQueryable();
            foreach (string include in pagination?.ReportView?.Includes)
            {
                a = a.Include(include);
            }


            var filterQuery = Querys.WorkOrderDetailFilterQuery<T>(null);

            var queriable = a; // context.WorkOrder.Include(x => x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            var simplequery = a;

            var result = await queriable.PaginationAndFilterQuery<T>(pagination, a, filterQuery);




            return result;//context.Set<T>().AsNoTracking().AsQueryable();
        }

        public async Task<T> FetchFromHeader<T>(DbSet<T> _, TContext context, ReportView ReportView) where T : class
        {
            //DbSet<T> parameter is not needed - it will throw an Exception

            List<object> lstParameter = null;
            FormattableString sqlstringfor = null;
            string sqlstring = ReportView.Header.Query;
            List<string> lstwhere = new List<string>();
            if (ReportView != null && ReportView?.Header.Parameters?.Count > 0)
            {
                lstParameter = new List<object>();
                int cont = 0;
                foreach (var item in ReportView?.Header?.Parameters)
                {
                    //var columnValue = SqliteParameter(item.Name, item.Value);

                    string query = item.ColumnName + " " + item.Operator + " {" + cont + "}"; // + item.Value;

                    lstwhere.Add(query);


                    lstParameter.Add("" + item.Value + "");
                    cont++;
                }

                sqlstring = sqlstring + " where " + string.Join(" or ", lstwhere);

                sqlstringfor = FormattableStringFactory.Create(sqlstring, lstParameter.ToArray());

                string res = sqlstringfor.ToString();

            }


            var a =  context.Set<T>().FromSql(sqlstringfor).AsNoTracking().AsQueryable();
            foreach (string include in ReportView?.Header?.Includes)
            {
                a = a.Include(include);
            }

            var b = await a.FirstOrDefaultAsync();

            //var filterQuery = Querys.WorkOrderDetailFilterQuery<T>(null);

            //var queriable = a; // context.WorkOrder.Include(x => x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            //var simplequery = a;

            //pagination.Show = 1;

            //var result = await queriable.PaginationAndFilterQuery<T>(pagination, a, filterQuery);

            return b;//context.Set<T>().AsNoTracking().AsQueryable();
        }

        public async  Task<List<Type>> FetchDbSetTypes()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var properties = context.GetType().GetProperties();
            var dbSets = new List<Type>();
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType && propertyType.Name.ToLower().Contains("dbset"))
                {
                    Type dbSetType = propertyType.GenericTypeArguments[0]; //point of interest here
                    dbSets.Add(dbSetType);
                }
            }
            return dbSets;
        }
        public async Task<ResultSet<WorkOrderDetail>> GetWodsFromQuery(Pagination<WorkOrderDetail> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            

            
            var dblst = await FetchDbSetTypes();

            string hclassname = pagination.ReportView.Header.ClassName;
            if (pagination.ReportView.Header.ClassName.Contains("."))
            {
                var arrr= pagination.ReportView.Header.ClassName.Split(".");
                hclassname = arrr.ElementAtOrDefault(arrr.Length - 1);
            }

             

            var hdbsetType = dblst.Where(x => x.Name.ToLower() == hclassname.ToLower()).FirstOrDefault();

            Type hmyType = typeof(InternalDbSet<>).MakeGenericType(hdbsetType);
            //instance is only used to spoof the binding
            dynamic hinstance = Activator.CreateInstance(hmyType, context, hdbsetType.Name);


            //var entityType = context.GetType().GetProperty("WorkOrderDetail").PropertyType.GetGenericArguments().FirstOrDefault();
            //dynamic instance = Activator.CreateInstance(entityType) ;

            PieceOfEquipment ha = await FetchFromHeader(hinstance, context, pagination.ReportView);
            string json1 = "";
            if (ha != null )
            {
                json1 = Newtonsoft.Json.JsonConvert.SerializeObject(ha);


            }


            //////////////////////////////////////////////////////////////////////////////

            string classname = pagination.ReportView.ClassName;
            if (pagination.ReportView.ClassName.Contains("."))
            {
                var arrr = pagination.ReportView.ClassName.Split(".");
                classname = arrr.ElementAtOrDefault(arrr.Length - 1);
            }

            var dbsetType = dblst.Where(x => x.Name.ToLower() == classname.ToLower()).FirstOrDefault();

            Type myType = typeof(InternalDbSet<>).MakeGenericType(dbsetType);
            //instance is only used to spoof the binding
            dynamic instance = Activator.CreateInstance(myType, context, dbsetType.Name);


            //var entityType = context.GetType().GetProperty("WorkOrderDetail").PropertyType.GetGenericArguments().FirstOrDefault();
            //dynamic instance = Activator.CreateInstance(entityType) ;

            ResultSet<WorkOrderDetail> a= await FetchFromTable(instance,context, pagination);

            if (!string.IsNullOrEmpty(pagination?.ReportView?.Header?.ClassName))
            {
                a.HeaderJSON = json1;
                a.HeadeClassName = pagination.ReportView.Header.ClassName;
            }
            

            //foreach (var item in a.List)
            //{
            //    item.PieceOfEquipment.WorOrderDetails = null;
            //}


            //foreach (string include in pagination?.ReportView?.Includes)
            //{
            //    a = a.Include(include);
            //}

            //var filterQuery = Querys.WorkOrderDetailFilterQuery(pagination.Object);

            //var queriable = a; // context.WorkOrder.Include(x => x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            //var simplequery = a;

            //var result = await queriable.PaginationAndFilterQuery<WorkOrderDetail>(pagination, a, filterQuery);

            return a;

        }



        public async Task<ResultSet<WorkOrderDetail>> GetWods(Pagination<WorkOrderDetail> pagination)
        {


        //    MapperConfiguration config = new(cfg =>
        //cfg.CreateMap<WorkOrderDetail, WorkOrderDetail>().ReverseMap(), null);

            //    IMapper mapper = new Mapper(config);

            //UserEntity entity = new UserEntity("username", "mail@mail.com");

            //UserDto dto = mapper.Map<UserDto>(entity);



            await using var context = await DbFactory.CreateDbContextAsync();



            var a = (from wd in context.WorkOrderDetail
                     join wo in context.WorkOrder on wd.WorkOrderID equals wo.WorkOrderId
                     join poe in context.PieceOfEquipment on wd.PieceOfEquipmentId equals poe.PieceOfEquipmentID
                     join et in context.EquipmentTemplate on poe.EquipmentTemplateId equals et.EquipmentTemplateID
                     join cus in context.Customer on poe.CustomerId equals cus.CustomerID
                     where wd.CurrentStatusID < 4
                     select new WorkOrderDetail()
                     {
                         WorkOrderDetailID = wd.WorkOrderDetailID
                     ,
                         WorkOrderID = wd.WorkOrderID
                     ,
                         CurrentStatusID = wd.CurrentStatusID
                     ,
                         WorkOder = wo
                     ,
                         PieceOfEquipment = new PieceOfEquipment()
                         { SerialNumber = poe.SerialNumber, PieceOfEquipmentID = poe.PieceOfEquipmentID, EquipmentTemplate = et, CustomerId = poe.CustomerId, Customer = cus },
                         TechnicianID = wd.TechnicianID,
                         TestCodeID = wd.TestCodeID,
                         WorkOrderDetailUserID = wd.WorkOrderDetailUserID

                     }

                       ).AsQueryable();



            IList<int> ids = null;
            if (!string.IsNullOrEmpty(pagination?.Object?.EntityFilter?.TestCode?.Code))
            {
                var test = await context.TestCode.AsNoTracking()
                    .Where(x => x.Code.ToLower().Contains(pagination.Object.EntityFilter.TestCode.Code.ToLower())).ToListAsync();

                ids = new List<int>();

                foreach (var id in test)
                {
                    ids.Add(id.TestCodeID);
                }

                a = a.Join(ids, e => e.TestCodeID, id => id, (e, id) => e).AsQueryable();
            }
            Expression<Func<WorkOrderDetail, bool>> filterQuery = null;

            //var predicate1 = CalibrationSaaS.Domain.Aggregates.Querys.PredicateBuilder.False<WorkOrderDetail>();



            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                string filter = pagination.Filter;

                var numeric = filter.IsInt();

                pagination.Object.Advanced = true;

                WorkOrderDetail eq = new WorkOrderDetail();

                eq.PieceOfEquipment = new PieceOfEquipment();

                eq.PieceOfEquipment.EquipmentTemplate = new EquipmentTemplate();

                eq.CurrentStatus = new Domain.Aggregates.Entities.Status();

                eq.PieceOfEquipment.Customer = new Customer();

                eq.TestCode = new TestCode();

                eq.WorkOder = new WorkOrder();

                var entity = (WorkOrderDetail)eq.CloneObject();

                entity.PieceOfEquipment.EquipmentTemplate.Model = filter;

                pagination.Object.EntityFilter = entity;

                var filterQuery2 = Querys.WorkOrderDetailFilter(pagination.Object);

                //a= a.Where(filterQuery2);

                entity = (WorkOrderDetail)eq.CloneObject();

                entity.PieceOfEquipment.SerialNumber = filter;

                pagination.Object.EntityFilter = entity;

                var filterQuery3 = Querys.WorkOrderDetailFilter(pagination.Object);

                entity = (WorkOrderDetail)eq.CloneObject();

                entity.PieceOfEquipment.PieceOfEquipmentID = filter;

                pagination.Object.EntityFilter = entity;

                var filterQuery4 = Querys.WorkOrderDetailFilter(pagination.Object);

                entity = (WorkOrderDetail)eq.CloneObject();

                entity.WorkOder.PurchaseOrder = filter;

                pagination.Object.EntityFilter = entity;

                var filterQuery5 = Querys.WorkOrderDetailFilter(pagination.Object);

                entity = (WorkOrderDetail)eq.CloneObject();

                entity.PieceOfEquipment.Customer.Name = filter;

                pagination.Object.EntityFilter = entity;

                var filterQuery9 = Querys.WorkOrderDetailFilter(pagination.Object);

                Expression<Func<WorkOrderDetail, bool>> filterQuery8 = null;

                if (numeric)
                {
                    entity = (WorkOrderDetail)eq.CloneObject();

                    entity.WorkOder.WorkOrderId = Convert.ToInt32(filter);

                    pagination.Object.EntityFilter = entity;

                    var filterQuery6 = Querys.WorkOrderDetailFilter(pagination.Object);

                    entity = (WorkOrderDetail)eq.CloneObject();

                    entity.WorkOrderDetailID = Convert.ToInt32(filter);

                    pagination.Object.EntityFilter = entity;

                    var filterQuery7 = Querys.WorkOrderDetailFilter(pagination.Object);

                    filterQuery8 = filterQuery6.Or(filterQuery7);

                }
                if (filterQuery8 != null)
                {
                    a = a.Where(filterQuery2.Or(filterQuery3).Or(filterQuery4).Or(filterQuery5).Or(filterQuery8).Or(filterQuery9));
                }
                else
                {
                    a = a.Where(filterQuery2.Or(filterQuery3).Or(filterQuery4).Or(filterQuery5).Or(filterQuery9));
                }

            }
            else
            {
                filterQuery = Querys.WorkOrderDetailFilter(pagination.Object);// Querys.WorkOrderFilter(1);
            }




            pagination.ColumnName = "WorkOder.ScheduledDate";

            var queriable = a; // context.WorkOrder.Include(x => x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            var simplequery = a;

            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<WorkOrderDetail>(pagination, simplequery, filterQuery);


            foreach (var item in result.List)
            {
                var eto = await context.EquipmentType.AsNoTracking()
                    .Where(x => item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID.HasValue == true && x.EquipmentTypeGroupID == item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID && x.HasWorkOrderDetail == true).ToListAsync();


                if (eto.Count == 1)
                {
                    item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject = eto.FirstOrDefault();
                }
                else if (eto.Count > 1)
                {
                    item.PieceOfEquipment.EquipmentTemplate.AditionalEquipmentTypesJSON = Newtonsoft.Json.JsonConvert.SerializeObject(eto);
                }

                //var wod = await context.Customer.AsNoTracking().Where(x => x.CustomerID == item.WorkOder.CustomerId).FirstOrDefaultAsync();

                //item.WorkOder.Customer = wod;

                var user = await context.User.AsNoTracking().Where(x => x.UserID == item.TechnicianID).FirstOrDefaultAsync();

                item.Technician = user;

                item.PieceOfEquipment.WorOrderDetails = null;

                item.WorkOder.WorkOrderDetails = null;


                var testcode = await context.TestCode.AsNoTracking().Where(x => item.TestCodeID.HasValue == true && x.TestCodeID == item.TestCodeID).FirstOrDefaultAsync();

                item.TestCode = testcode;

            }

            return result;
        }




        public async Task<IEnumerable<WorkOrderDetail>> GetAll()
        {
            return null;
        }

        public async Task<IEnumerable<WorkOrderDetail>> GetAllEnabled()
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<int>> GetChartPie()
        {

            await using var context = await DbFactory.CreateDbContextAsync();


            var list = new List<int>();

            var s = await GetStatus();


            foreach (var item in s.Where(x => x.IsLast == false).ToList())
            {
                var a = await context.WorkOrderDetail.Where(x => x.CurrentStatusID == item.StatusId).CountAsync();
                list.Add(a);
            }
            return list;
        }



        public async Task<IEnumerable<int>> GetTotals()
        {
            var list = new List<int>();
            try
            {

                await using var context = await DbFactory.CreateDbContextAsync();
                //var s = await GetStatus();

                var date = DateTime.Now.AddDays(-30).Date;

                //var tw = await context.WorkOrderDetail.AsNoTracking().Where(x => x.CalibrationDate.HasValue && x.CalibrationDate > date && x.CalibrationDate < DateTime.Now.Date).CountAsync();

                var tw = await (from w in context.WorkOrder
                                join wd in context.WorkOrderDetail on w.WorkOrderId equals wd.WorkOrderID
                                where w.WorkOrderDate >= date && w.WorkOrderDate <= DateTime.Now.AddDays(1).Date
                                select w).CountAsync();



                var date2 = DateTime.Now.AddDays(30).Date;

                var pdd = await context.PieceOfEquipment.AsNoTracking().Where(x => x.DueDate >= DateTime.Now.AddDays(1).Date && x.DueDate <= date2).CountAsync();

                list.Add(tw);

                list.Add(pdd);

                var a = await GetChartPie();

                int sum = 0;


                foreach (var item in a)
                {
                    sum = sum + item;
                }
                list.Add(sum);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(0);
                list.Add(0);
                list.Add(0);
                return list;
            }

        }

        public async Task<IEnumerable<KeyValueDate>> GetWOCountPerDay()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var list = new List<KeyValueDate>();

            //var listt = _Service.GetDataByMonth(start, end).GroupBy(x => x.Start.Date).Select(grp => new { Date = grp.Key, Count = grp.Count() });

            //var listt2 = await context.WorkOrderDetail.AsNoTracking().Where(x => x.CurrentStatusID < 4).CountAsync();

            //var k1 = new KeyValueDate();
            //k1.Key = new DateTime(1900,1,1);
            //k1.Value = listt2;
            //list.Add(k1);

            var listt = context.WorkOrderDetail.AsNoTracking().Include(x => x.WorkOder).Where(x => x.CurrentStatusID < 4 && x.WorkOder.ScheduledDate.HasValue
            && x.WorkOder.ScheduledDate.Value.Date >= DateTime.Now.AddDays(-365).Date
            && x.WorkOder.ScheduledDate.Value.Date <= DateTime.Now.AddDays(6).Date)
            .GroupBy(x => x.WorkOder.ScheduledDate.Value.Date).Select(grp => new { Date = grp.Key, Count = grp.Count() });

            listt.ForEach(async item =>
            {
                var k = new KeyValueDate();
                k.Key = item.Date;
                k.Value = item.Count;
                list.Add(k);
            });


            return list;
        }



        public async Task<IEnumerable<KeyValueDate>> GetWODCountPerDay()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var list = new List<KeyValueDate>();
            //var listt = _Service.GetDataByMonth(start, end).GroupBy(x => x.Start.Date).Select(grp => new { Date = grp.Key, Count = grp.Count() });
            var listt = context.WorkOrderDetail.Where(x => x.CalibrationDate.HasValue
            && x.CalibrationDate.Value.Date >= DateTime.Now.AddDays(-30).Date
            && x.CalibrationDate.Value.Date <= DateTime.Now.AddDays(1).Date)
            .GroupBy(x => x.CalibrationDate.Value.Date).Select(grp => new { Date = grp.Key, Count = grp.Count() });

            listt.ForEach(async item =>
            {
                var k = new KeyValueDate();
                k.Key = item.Date;
                k.Value = item.Count;
                list.Add(k);
            });


            return list;
        }


        public async Task<ICollection<WorkOrderDetail>> GetByTechnician(User DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();


            var a = await (from WOD in context.WorkOrder
                           join U in context.User_WorkOrder on WOD.WorkOrderId equals U.WorkOrderID
                           where U.UserID == DTO.UserID
                           select WOD).ToListAsync();

            //var a = await (from WOD in context.WorkOrderDetail                          
            //               where U.UserID == DTO.UserID
            //               select WOD).ToListAsync();

            return (ICollection<WorkOrderDetail>)a;

        }


        public async Task<ResultSet<WorkOrderDetail>> GetByTechnicianPag(Pagination<WorkOrderDetail> pagination)
        {


            ResultSet<WorkOrderDetail> result = new ResultSet<WorkOrderDetail>();

            WorkOrderDetail w = pagination.Entity;
            int id = 0;
            int status = 0;
            if (w != null)
            {
                id = w.TechnicianID.Value;

                status = w.CurrentStatusID;
            }

            else
            {
                return new ResultSet<WorkOrderDetail>();
            }



            await using var context = await DbFactory.CreateDbContextAsync();

            var a = (from WOD in context.WorkOrderDetail.Include(x => x.PieceOfEquipment).ThenInclude(x => x.Customer).Include(x => x.CurrentStatus)
                     join U in context.User_WorkOrder on WOD.WorkOrderID equals U.WorkOrderID
                     //where U.UserID == id
                     select WOD);

            if (status > 0)
            {
                a = (from WOD in context.WorkOrderDetail
                     .Include(x => x.PieceOfEquipment).ThenInclude(x => x.Customer)
                     .Include(x => x.CurrentStatus)
                     from U in context.User_WorkOrder
                     where
                     WOD.CurrentStatusID == status &&
                     U.UserID == id
                     //&& (WOD.TechnicianID == id || WOD.TechnicianID == null)
                     &&
                     U.WorkOrderID == WOD.WorkOrderID
                     select WOD);

            }
            ///Code for empty wo
            if (status == 0)
            {
                //a = (from WOD in context.WorkOrderDetail
                //     .Include(x => x.PieceOfEquipment).ThenInclude(x => x.Customer)
                //     .Include(x => x.CurrentStatus)
                //     from U in context.User_WorkOrder
                //     where
                //     WOD.CurrentStatusID == status &&
                //     U.UserID == id
                //     //&& (WOD.TechnicianID == id || WOD.TechnicianID == null)
                //     &&
                //     U.WorkOrderID == WOD.WorkOrderID
                //     select WOD);


               var  b = (from WO in context.WorkOrder
                    .Include(x => x.Customer)
                    
                     from U in context.User_WorkOrder
                     where
                     
                     U.UserID == id
                     //&& (WOD.TechnicianID == id || WOD.TechnicianID == null)
                     &&
                     U.WorkOrderID == WO.WorkOrderId
                     select WO);


                Pagination<WorkOrder> pagwo = new Pagination<WorkOrder>();

                pagwo.Show = 1000;


                var filterQuery1 = (Expression<Func<WorkOrder, bool>>)default; // Querys.WODbyTech2(id);

                var queriable1 = b; // context.WorkOrder.Include(x => x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

                var simplequery1 = b;

                //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
                var result1 = await queriable1.PaginationAndFilterQuery<WorkOrder>(pagwo, simplequery1, filterQuery1);

                if(result1 != null && result1.List.Count > 0)
                {
                    List<WorkOrderDetail> wods = new List<WorkOrderDetail>();
                    foreach (var item in result1.List)
                    {

                        WorkOrderDetail itm = new WorkOrderDetail();

                        itm.PieceOfEquipment = new PieceOfEquipment();

                        itm.PieceOfEquipment.EquipmentTemplate = new EquipmentTemplate();

                        itm.PieceOfEquipment.CustomerId = item.CustomerId;
                        itm.PieceOfEquipment.Customer = item.Customer;
                        itm.WorkOrderDetailID = 0;
                        itm.WorkOrderID = item.WorkOrderId;
                        itm.TechnicianID = id;
                        itm.WorkOder = item;


                        wods.Add(itm);

                    }

                    result.List = wods;
                    return result;

                }




            }


            var filterQuery = Querys.WODbyTech2(id);

            var queriable = a; // context.WorkOrder.Include(x => x.Customer);//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            var simplequery = a;

            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
             result = await queriable.PaginationAndFilterQuery<WorkOrderDetail>(pagination, simplequery, filterQuery);



            if (result.List != null)
            {
                result.List = result.List.DistinctBy(x => x.WorkOrderDetailID).ToList();

                var calibrationtypes = await context.CalibrationType.AsNoTracking().ToListAsync();

                var equiptypeg = await context.EquipmentTypeGroup.Include(x=>x.EquipmentTypes).AsNoTracking().ToListAsync();

                foreach (var item in result.List)
                {
                    if (item.PieceOfEquipment != null)
                    {
                        item.PieceOfEquipment.WorOrderDetails = null;

                        item.PieceOfEquipment.EquipmentTemplate = await context.EquipmentTemplate.AsNoTracking().Where(x => x.EquipmentTemplateID == item.PieceOfEquipment.EquipmentTemplateId).FirstOrDefaultAsync();

                        var ct = item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject;
                        if (ct != null)
                        {
                            var calt = calibrationtypes.Where(x => ct != null && x.CalibrationTypeId == ct.CalibrationTypeID).FirstOrDefault();
                       
                            ct.CalibrationType = calt;
                        }
                        else
                        {
                            var etypes2 = equiptypeg.Where(x => x.EquipmentTypeGroupID == item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID).ToList();

                            ICollection<EquipmentType> etypes = null;

                            if (etypes2 != null && etypes2.Count > 0)
                            {
                                etypes=etypes2.First().EquipmentTypes;

                                foreach (var etyper in etypes)
                                {
                                    var calt = calibrationtypes.Where(x => x.CalibrationTypeId == etyper.CalibrationTypeID).FirstOrDefault();

                                    etyper.CalibrationType = calt;
                                }
                                item.PieceOfEquipment.EquipmentTemplate.AditionalEquipmentTypes = etypes;
                            }
                            else
                            {
//                                Console.WriteLine("------ EquipmentypeGroupID: " + item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID);
                            }

                        }
                        

                    }

                }
            }

            foreach (var ith in result.List)
            {
                //ith.PieceOfEquipment.Customer.PieceOfEquipment = null;

                ////ith.PieceOfEquipment = null;
                //if (ith?.WorkOder != null)
                //{
                //    ith.WorkOder.WorkOrderDetails = null;
                //}

                //ith.CurrentStatus.WorkOrderDetails = null;

                if(ith?.PieceOfEquipment?.EquipmentTemplate != null)
                {
                    //ith.PieceOfEquipment.EquipmentTemplate.AditionalEquipmentTypes = null;
                    //ith.PieceOfEquipment.EquipmentTemplate.PieceOfEquipments = null;
                    ith.PieceOfEquipment.Indicator = null;
                }


            }
            return result;



        }



       

        public async Task<WorkOrderDetail> UpdateOfflineID(WorkOrderDetail DTO)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            var wod = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).FirstOrDefaultAsync();

            wod.OfflineStatus = 1;

            await context.SaveChangesAsync();

            return wod;

        }


        public async Task<WorkOrderDetail> GetByID(WorkOrderDetail DTO)
        {



            await using var context = await DbFactory.CreateDbContextAsync();


            using (WorkOrderDetail a = await context.WorkOrderDetail.AsNoTracking().Where(Querys.WorkOrderDetailByID(DTO))    //.Include(d => d.Technician)
            .Include(x => x.EquipmentCondition)

            //.Include(y=>y.TestCode)
            //.Include(x => x.TestGroups).ThenInclude(x => x.TestPoints)

            .FirstOrDefaultAsync())
            {
                if (a == null)
                {
                    return null;
                }


                //await context.Entry(a).Collection(p => p.TestGroups).LoadAsync();

                var tp = await context.TestPointGroup.AsNoTracking().Include(x => x.TestPoints)
                    .Where(x => x.WorkOrderDetailID.HasValue && x.WorkOrderDetailID.Value == DTO.WorkOrderDetailID).ToArrayAsync();

                if (tp.Count() > 0)
                {
                    a.TestGroups = tp;
                }



                if (a != null && a.TechnicianID.HasValue)
                {
                    var te = await context.User.Where(x => x.UserID == a.TechnicianID.Value).FirstOrDefaultAsync();
                    a.Technician = te;
                }


                var w = await context.WorkOrder.AsNoTracking().Where(x => x.WorkOrderId == a.WorkOrderID)
                .Include(r => r.UserWorkOrders).ThenInclude(u => u.User).FirstOrDefaultAsync();

                var cus = await context.Customer.AsNoTracking().Where(x => x.CustomerID == w.CustomerId)
                    .Include(x => x.Aggregates)
                    .ThenInclude(x => x.Addresses).FirstOrDefaultAsync();


                var PoeRepository = new PieceOfEquipmentRepositoryEF<TContext>(DbFactory);

                var poe = await PoeRepository.GetPieceOfEquipmentByID(a.PieceOfEquipmentId);

                w.Customer = cus;

                a.WorkOder = w;

                a.PieceOfEquipment = poe;

                var ran = await context.RangeTolerance.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

                a.Ranges = ran;

                var cert = await context.Certificate.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).OrderByDescending(x => x.Version).FirstOrDefaultAsync();

                a.Certificate = cert;


                if (a.TestCodeID.HasValue)
                {
                    var TestCode = await context.TestCode.AsNoTracking().Where(x => x.TestCodeID == a.TestCodeID).FirstOrDefaultAsync();
                    var Procedure = await context.Procedure.AsNoTracking().FirstOrDefaultAsync(y => y.ProcedureID == TestCode.ProcedureID);
                    TestCode.Procedure = Procedure;
                    a.TestCode = TestCode;
                }
                var env = await context.ExternalCondition.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                a.EnviromentCondition = env;


                var proce = await context.WOD_Procedure.AsNoTracking().Include(x => x.Procedure).Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

                a.WOD_Procedure = proce;

                if ((a != null && a?.EquipmentCondition == null) || a?.EquipmentCondition?.Count == 0)
                {
                    a.EquipmentCondition = (ICollection<EquipmentCondition>)Querys.DefaultEquipmentConditionForBalance();
                }


                //Find default status if not exits
                if (a.CurrentStatusID == 0)//a != null && a.CurrentStatus == null ||
                {

                    List<Status> statuslist = (List<Status>)await GetStatus();

                    var defaultstatus = statuslist.Find(x => x.IsDefault == true);

                    a.CurrentStatus = defaultstatus;
                }

                //Load Notes
                if (a != null)
                {
                    a.NotesWOD = await GetNotes(a.WorkOrderDetailID, 1);
                }



                if (a?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeGroupID.HasValue == true
                    && (a.CurrentStatusID >= 2 || a.ParentID.HasValue))
                {

                    //var equipmentTypeObject = await GetAllEquipmentType(a.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID.Value);
                    //var eqGroup = equipmentTypeObject.AsQueryable().Where(x => x.EquipmentTypeGroupID == a.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID && x.HasWorkOrderDetail == true).FirstOrDefault();

                    var eqGroup = await GetEquipmentTypeForWOD(a.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID.Value);

                    //Load calibration
                    var wod = await GetWorkOrderDetailByID(a.WorkOrderDetailID, eqGroup.DynamicConfiguration || eqGroup.DynamicConfiguration2, a.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationTypeID);//(wod?.BalanceAndScaleCalibration?.HasLinearity == true && wod?.BalanceAndScaleCalibration?.Linearities.Count() > 0)

                    if (wod != null)
                    {
                        a.BalanceAndScaleCalibration = wod.BalanceAndScaleCalibration;
                        a.WOD_Weights = wod.WOD_Weights;
                        a.CalibrationSubType_Standards = wod.CalibrationSubType_Standards;
                    }

                    if (eqGroup.IsBalance != true && a.BalanceAndScaleCalibration?.GenericCalibration == null)
                    {

                        //Error in offline mode
                        try
                        {
                            var genericCalibration = await context.GenericCalibration.AsNoTracking().Include(x => x.BasicCalibrationResult).AsNoTracking().Where(x => x.WorkOrderDetailId == a.WorkOrderDetailID).ToListAsync();
                            var genericCalibrationResult = await context.GenericCalibrationResult.AsNoTracking().Include(x => x.GenericCalibration).AsNoTracking().Where(x => x.WorkOrderDetailId == a.WorkOrderDetailID).ToListAsync();


                            a.BalanceAndScaleCalibration.GenericCalibration = genericCalibration;
                        }
                        catch (Exception ex)
                        {
//                            Console.WriteLine("Error loading Generic Calibration");

                        }
                    }
                    if(wod?.WOD_Weights != null && wod?.WOD_Weights?.Count > 0)
                    {
                        a.WOD_Weights = wod.WOD_Weights.DistinctBy(x => x.WeightSetID).ToArray();
                    }               


                    if (a?.PieceOfEquipment != null && wod != null && wod.PieceOfEquipment != null && wod.PieceOfEquipment.Uncertainty != null)
                    {
                        a.PieceOfEquipment.Uncertainty = wod.PieceOfEquipment.Uncertainty;

                    }
                    if (a?.PieceOfEquipment?.EquipmentTemplate != null && wod != null && wod.PieceOfEquipment != null && wod.PieceOfEquipment.Uncertainty != null)
                    {

                        a.PieceOfEquipment.EquipmentTemplate.Uncertainty = wod.PieceOfEquipment.EquipmentTemplate.Uncertainty;

                    }
                }

                if (a.CurrentStatusID == 0)
                {
                    a.CurrentStatusID = 1;
                }


                if(a.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject != null)
                {
                   // a.PieceOfEquipment.EquipmentTemplate._equipmentTypeObject = a.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject;
                    //a.PieceOfEquipment.EquipmentTemplate.AditionalEquipmentTypes = null;
                    //a.PieceOfEquipment.EquipmentTemplate.AditionalEquipmentTypesJSON = "";
                }

                return a;

            }


        }
        //9503
        public async Task<WorkOrderDetail> GetByIDPreviousCalibration(WorkOrderDetail DTO)
        {
            //use GetByid to find complete calibration
            // replace just BalanceAndScaleCalibration property
            var calTypeActual = DTO.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject.CalibrationTypeID;
            var newWod = DTO;

            await using var context = await DbFactory.CreateDbContextAsync();
            var wodCompleteId = (
                                    from wd in context.WorkOrderDetail
                                    join poe in context.PieceOfEquipment on wd.PieceOfEquipmentId equals poe.PieceOfEquipmentID
                                    join et in context.EquipmentTemplate on poe.EquipmentTemplateId equals et.EquipmentTemplateID
                                    join etg in context.EquipmentTypeGroup on et.EquipmentTypeGroupID equals etg.EquipmentTypeGroupID
                                    join ety in context.EquipmentType on etg.EquipmentTypeGroupID equals ety.EquipmentTypeGroupID
                                    where wd.CurrentStatusID == 4
                                          && ety.CalibrationTypeID == calTypeActual
                                          && wd.StatusDate == (
                                              from wdInner in context.WorkOrderDetail
                                              join poeInner in context.PieceOfEquipment on wdInner.PieceOfEquipmentId equals poeInner.PieceOfEquipmentID
                                              join etInner in context.EquipmentTemplate on poeInner.EquipmentTemplateId equals etInner.EquipmentTemplateID
                                              join etgInner in context.EquipmentTypeGroup on etInner.EquipmentTypeGroupID equals etgInner.EquipmentTypeGroupID
                                              join etyInner in context.EquipmentType on etgInner.EquipmentTypeGroupID equals etyInner.EquipmentTypeGroupID
                                              where wdInner.CurrentStatusID == 4
                                                    && etyInner.CalibrationTypeID == calTypeActual
                                                    && wdInner.PieceOfEquipmentId == wd.PieceOfEquipmentId
                                              select wdInner.StatusDate
                                          ).Max()
                                    select new WorkOrderDetail()
                                    {
                                        WorkOrderDetailID = wd.WorkOrderDetailID
                                    }
                                ).FirstOrDefault();
            if (wodCompleteId != null && wodCompleteId.WorkOrderDetailID != null && wodCompleteId.WorkOrderDetailID != 0)
            {
                WorkOrderDetail wodComplete = await GetByID(wodCompleteId);

                var genericCalibrationResult2 = wodComplete.BalanceAndScaleCalibration.TestPointResult.ToList();

                foreach (var item in genericCalibrationResult2)
                {
                    item.GenericCalibration2.ComponentID = newWod.WorkOrderDetailID.ToString();

                }

                genericCalibrationResult2.ForEach(testPoint => testPoint.ComponentID = newWod.WorkOrderDetailID.ToString());

                wodComplete.BalanceAndScaleCalibration.WorkOrderDetailId = newWod.WorkOrderDetailID;
                wodComplete.BalanceAndScaleCalibration.TestPointResult = genericCalibrationResult2;

                newWod.BalanceAndScaleCalibration = wodComplete.BalanceAndScaleCalibration;
                newWod.WorkOrderDetailIdPrevious = wodComplete.WorkOrderDetailID;

            }

            return newWod;
        }
        public async Task<IEnumerable<EquipmentType>> GetAllEquipmentType(int EquipmentTypeGroup)
        {


            await using var context = await DbFactory.CreateDbContextAsync();


            var eqType = await context.EquipmentType.AsNoTracking()
                .Include(b => b.CalibrationType).AsNoTracking()
                .Include(c => c.CalibrationType.CalibrationSubTypes).AsTracking().
                 Where(x => x.IsEnabled == true && x.EquipmentTypeGroupID.HasValue && x.EquipmentTypeGroupID == EquipmentTypeGroup).OrderBy(x => x.Name).ToListAsync();
            //var PieceOfEquipmentRepository = new PieceOfEquipmentRepositoryEF(context);

            foreach (var eq in eqType)
            {

                if (eq.EquipmentTypeGroupID.HasValue)
                {
                    var etg = await context.EquipmentTypeGroup.AsNoTracking().Where(x => x.EquipmentTypeGroupID == eq.EquipmentTypeGroupID.Value).FirstOrDefaultAsync();

                    eq.EquipmentTypeGroup = etg;
                }


            }



            return eqType;

        }


        public async Task<EquipmentType> GetEquipmentTypeForWOD(int EquipmentTypeGroup)
        {


            await using var context = await DbFactory.CreateDbContextAsync();


            var eqType = await context.EquipmentType.AsNoTracking()
                 //.Include(b => b.CalibrationType).AsNoTracking()
                 //.Include(c => c.CalibrationType.CalibrationSubTypes).AsTracking()
                 .Where(x => x.IsEnabled == true && x.EquipmentTypeGroupID.HasValue && x.EquipmentTypeGroupID == EquipmentTypeGroup).OrderBy(x => x.Name).ToListAsync();

            if (eqType.Count > 1)
            {

                var etrt= eqType.Where(x => x.HasWorkOrderDetail == true).ToList();

                if(etrt.Count ==1)
                {
                    return etrt.ElementAt(0);
                }
                else if(etrt.Count > 0)
                {
                    return etrt.ElementAt(0);
                }                

                //throw new Exception("Multiple Equipment Type Configured");
            }
            if (eqType.Count == 0)
            {
                throw new Exception("No WOD Configured, please review with admin");
            }
            else
            {


                return eqType.ElementAtOrDefault(0);
            }

            //var PieceOfEquipmentRepository = new PieceOfEquipmentRepositoryEF(context);

            //foreach (var eq in eqType)
            //{

            //    if (eq.EquipmentTypeGroupID.HasValue)
            //    {
            //        var etg = await context.EquipmentTypeGroup.AsNoTracking().Where(x => x.EquipmentTypeGroupID == eq.EquipmentTypeGroupID.Value).FirstOrDefaultAsync();

            //        eq.EquipmentTypeGroup = etg;
            //    }


            //}



        }



        public async Task<IEnumerable<WorkDetailHistory>> GetHistory(WorkOrderDetail DTO)
        
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.WorkDetailHistory.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

            return a;

        }


        public async Task<WorkOrderDetail> GetHeaderById(WorkOrderDetail DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var a = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).FirstOrDefaultAsync();

            return a;

        }




        public Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailXWorkOrder(WorkOrder DTO)
        {
            throw new NotImplementedException();
        }


        private async Task SaveWODWeights(TContext context, WorkOrderDetail DTO, Dictionary<string, WeightSet> weightSetBackup = null)
        {

            //if(DTO?.PieceOfEquipment?.EquipmentTemplate?.EquipmentTypeObject?.UseWorkOrderDetailStandard== true)
            //{

            //await using var context = await DbFactory.CreateDbContextAsync();

            var wod = await context.WorkOrderDetail.Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).AsNoTracking().FirstOrDefaultAsync();

            if (wod == null)
            {
                return;
            }

            var stas = await context.WOD_Standard.Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).AsNoTracking().ToArrayAsync();


            if (stas != null && stas.Count() > 0)
            {
                context.WOD_Standard.RemoveRange(stas);
                await context.SaveChangesAsync();
            }


            if (DTO.WOD_Weights != null && DTO.WOD_Weights.Count > 0)
            {
                // Convert to List to ensure it's mutable and avoid "Collection was of a fixed size" error
                var wodWeightsList = DTO.WOD_Weights.Where(x => x.WeightSet.PieceOfEquipment != null).ToList();

                foreach (var www in wodWeightsList)
                {



                    WOD_Standard ws = new WOD_Standard();

                    ws.WorkOrderDetailID = DTO.WorkOrderDetailID;
                    ws.PieceOfEquipmentID = www.WeightSet.PieceOfEquipmentID;
                    ws.Option = www.WeightSet.Option;
                    context.WOD_Standard.Add(ws);
                    await context.SaveChangesAsync();


                }
               
            }



            //}
            //else
            //{

            var sss = await context.WOD_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToArrayAsync();

            if (sss != null && sss.Count() > 0)
            {
                context.WOD_Weight.RemoveRange(sss);
                await context.SaveChangesAsync();
            }
            if (DTO.WOD_Weights != null && DTO.WOD_Weights.Count > 0)
            {
                var ww = DTO.WOD_Weights.DistinctBy2(x => x.WeightSetID).ToArray();

                foreach (var www in ww.Where(x => x.WeightSet.PieceOfEquipment == null))
                {
                    www.Option = www.WeightSet.Option;

                    // Guardar el WeightSet en el diccionario antes de ponerlo en null
                    // Crear una copia desconectada para evitar problemas de tracking
                    if (weightSetBackup != null && www.WeightSet != null)
                    {
                        string key = $"{www.WorkOrderDetailID}_{www.WeightSetID}";
                        if (!weightSetBackup.ContainsKey(key))
                        {
                            // Clonar el WeightSet para crear una copia desconectada
                            var clonedWeightSet = (WeightSet)www.WeightSet.CloneObject();

                            // Limpiar referencias de navegación para evitar conflictos de tracking
                            // Estas se recargarán más tarde desde la base de datos con AsNoTracking
                            clonedWeightSet.UnitOfMeasure = null;
                            clonedWeightSet.UncertaintyUnitOfMeasure = null;
                            clonedWeightSet.PieceOfEquipment = null;
                            clonedWeightSet.WOD_Weights = null;
                            clonedWeightSet.WO_Weights = null;
                            clonedWeightSet.CalibrationSubType_Weights = null;

                            weightSetBackup[key] = clonedWeightSet;
                        }
                    }

                    www.WeightSet = null;
                    www.WorkOrderDetail = null;

                    if (www.WorkOrderDetailID > 0 && www.WeightSetID > 0)
                    {
                        try
                        {
                            context.WOD_Weight.Add(www);
                            await context.SaveChangesAsync();
                        }
                        catch(Exception ex)
                        {

                        }

                    }

                }

            }

            //}





        }

        private Force FormatForce(Force item)
        {

            item.Uncertainty = item.Uncertainty.ValidDouble();
            item.Tolerance = item.Tolerance.ValidDouble();

            item.BasicCalibrationResult.Uncertanty = item.BasicCalibrationResult.Uncertanty.ValidDouble();
            item.BasicCalibrationResult.Error = item.BasicCalibrationResult.Error.ValidDouble();
            item.BasicCalibrationResult.ErrorPer = item.BasicCalibrationResult.ErrorPer.ValidDouble();
            item.BasicCalibrationResult.ErrorpRun1 = item.BasicCalibrationResult.ErrorpRun1.ValidDouble();
            item.BasicCalibrationResult.ErrorRun1 = item.BasicCalibrationResult.ErrorRun1.ValidDouble();
            item.BasicCalibrationResult.ErrorRun2 = item.BasicCalibrationResult.ErrorRun2.ValidDouble();
            item.BasicCalibrationResult.ErrorRun3 = item.BasicCalibrationResult.ErrorRun3.ValidDouble();
            item.BasicCalibrationResult.ErrorRun4 = item.BasicCalibrationResult.ErrorRun4.ValidDouble();
            item.BasicCalibrationResult.IncludeASTM = item.BasicCalibrationResult.IncludeASTM.ValidDouble();
            item.BasicCalibrationResult.MaxErrorp = item.BasicCalibrationResult.MaxErrorp.ValidDouble();
            item.BasicCalibrationResult.RelativeIndicationError = item.BasicCalibrationResult.RelativeIndicationError.ValidDouble();
            item.BasicCalibrationResult.RelativeIndicationErrorR1 = item.BasicCalibrationResult.RelativeIndicationErrorR1.ValidDouble();
            item.BasicCalibrationResult.RelativeIndicationErrorR2 = item.BasicCalibrationResult.RelativeIndicationErrorR2.ValidDouble();
            item.BasicCalibrationResult.RelativeIndicationErrorR3 = item.BasicCalibrationResult.RelativeIndicationErrorR3.ValidDouble();
            item.BasicCalibrationResult.RelativeIndicationErrorR4 = item.BasicCalibrationResult.RelativeIndicationErrorR4.ValidDouble();
            item.BasicCalibrationResult.RelativeRepeatabilityError = item.BasicCalibrationResult.RelativeRepeatabilityError.ValidDouble();
            item.BasicCalibrationResult.RelativeResolution = item.BasicCalibrationResult.RelativeResolution.ValidDouble();
            item.BasicCalibrationResult.Repeatability = item.BasicCalibrationResult.Repeatability.ValidDouble();
            item.BasicCalibrationResult.RepeatabilityUncertainty = item.BasicCalibrationResult.RepeatabilityUncertainty.ValidDouble();
            item.BasicCalibrationResult.Resolution = item.BasicCalibrationResult.Resolution.ValidDouble();
            item.BasicCalibrationResult.RUN1 = item.BasicCalibrationResult.RUN1.ValidDouble();
            item.BasicCalibrationResult.RUN2 = item.BasicCalibrationResult.RUN2.ValidDouble();
            item.BasicCalibrationResult.RUN3 = item.BasicCalibrationResult.RUN3.ValidDouble();
            item.BasicCalibrationResult.RUN4 = item.BasicCalibrationResult.RUN4.ValidDouble();


            return item;
        }


        private GenericCalibration FormatGenericCalibration(GenericCalibration item)
        {
            return item;

        }

        public GenericCalibration2 FormatGenericCalibration2(GenericCalibration2 item)
        {
            return item;

        }

        private Rockwell FormatRocwell(Rockwell item)
        {

            item.Uncertainty = item.Uncertainty.ValidDouble();
            item.Tolerance = item.Tolerance.ValidDouble();

            item.BasicCalibrationResult.Uncertanty = item.BasicCalibrationResult.Uncertanty.ValidDouble();
            item.BasicCalibrationResult.Error = item.BasicCalibrationResult.Error.ValidDouble();
            item.BasicCalibrationResult.Average = item.BasicCalibrationResult.Average.ValidDouble();
            item.BasicCalibrationResult.Test1 = item.BasicCalibrationResult.Test1.ValidDouble();
            item.BasicCalibrationResult.Test2 = item.BasicCalibrationResult.Test2.ValidDouble();
            item.BasicCalibrationResult.Test3 = item.BasicCalibrationResult.Test3.ValidDouble();
            item.BasicCalibrationResult.Test4 = item.BasicCalibrationResult.Test4.ValidDouble();
            item.BasicCalibrationResult.Test5 = item.BasicCalibrationResult.Test5.ValidDouble();



            return item;
        }

        public async Task SaveForces(int id, ICollection<Force> Forces)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            if (Forces == null)
            {
                return; ;
            }

            Forces = Forces.DistinctBy(book => new { book.WorkOrderDetailId, book.SequenceID, book.CalibrationSubTypeId }).ToArray();




            var lin = Forces.ToList();
            foreach (var item in lin)
            {
                item.BasicCalibrationResult.ComponentID = id.ToString();
                if (item.CalibrationSubType_Weights != null)
                {

                    await SaveSubType(item.CalibrationSubTypeId, 3);

                    //var sss = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == item.WorkOrderDetailId
                    // && x.SecuenceID == item.SequenceID && x.CalibrationSubTypeID == item.CalibrationSubTypeId).ToListAsync();



                    //if (sss != null && sss.Any())
                    //{
                    //    //    context.CalibrationSubType_Weight.RemoveRange(sss);
                    //    //    await context.SaveChangesAsync();
                    //    foreach (var record in sss)
                    //    {
                    //        context.CalibrationSubType_Weight.Remove(record);
                    //        Console.WriteLine($"Deleting record with WeightSetID: " + record.WeightSetID); // Ajusta según tu modelo
                    //    }
                    //    await context.SaveChangesAsync();
                    //}

                    context.ChangeTracker.Clear();
                    foreach (var we in item.CalibrationSubType_Weights)
                    {

                        ////
                        ///
                        var sss = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == we.WorkOrderDetailID
                        && x.SecuenceID == we.SecuenceID && x.CalibrationSubTypeID == we.CalibrationSubTypeID).ToListAsync();
                        if (sss != null && sss.Any())
                        {

                            foreach (var record in sss)
                            {
                                context.CalibrationSubType_Weight.Remove(record);
//                                Console.WriteLine($"Deleting record with WeightSetID: " + record.WeightSetID);
                            }
                            await context.SaveChangesAsync();
                        }
                        /// 
                        /// 
                        ////

                        we.ComponentID = id.ToString();
                        we.Component = "WorkOrderItem";

                        //we.CalibrationSubType = c;
                        context.CalibrationSubType_Weight.Add(we);

                        await context.SaveChangesAsync();

                    }

                    item.CalibrationSubType_Weights = null;
                }
                item.WeightSets = null;
            }




            var a = await context.Force.AsNoTracking().Where(x => x.WorkOrderDetailId == id).ToListAsync();
            var aa = await context.ForceResult.AsNoTracking().Where(x => x.WorkOrderDetailId == id).ToListAsync();

            //int calib = 0;

            var groupByLastNamesQuery =
    from student in Forces
    group student by student.CalibrationSubTypeId into newGroup
    orderby newGroup.Key
    select newGroup;

            foreach (var nameGroup in groupByLastNamesQuery)
            {
//                Console.WriteLine($"Key: {nameGroup.Key}");
                //foreach (var student in nameGroup)
                //{
                //    Console.WriteLine($"\t{student.LastName}, {student.FirstName}");
                //}
                int cont = 1;
                foreach (var item in nameGroup)
                {
                    try
                    {

                        //if (item.CalibrationSubTypeId != calib)
                        //{
                        //    calib = item.CalibrationSubTypeId;
                        //    cont = 1;
                        //}


                        var b = a.Where(x => x.SequenceID == item.SequenceID && x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();

                        var bb = aa.Where(x => x.SequenceID == item.BasicCalibrationResult.SequenceID && x.CalibrationSubTypeId == item.BasicCalibrationResult.CalibrationSubTypeId).FirstOrDefault();


                        item.BasicCalibrationResult.ComponentID = id.ToString();
                        item.BasicCalibrationResult.Position = cont;
                        item.BasicCalibrationResult.WorkOrderDetailId = id;
                        item.WorkOrderDetailId = id;
                        item.TestPointID = 0;
                        item.BasicCalibrationResult.Force = null;

                        FormatForce(item);

                        cont++;

                        if (b == null && bb == null)
                        {
//                            Console.WriteLine(item.BasicCalibrationResult.CalibrationSubTypeId);

                            context.ForceResult.Add(item.BasicCalibrationResult);
                            await context.SaveChangesAsync();
                            item.BasicCalibrationResult = null;
                            context.Force.Add(item);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            if (bb == null)
                            {
                                context.ForceResult.Add(item.BasicCalibrationResult);
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                context.ForceResult.Update(item.BasicCalibrationResult);
                                await context.SaveChangesAsync();
                            }
                            if (b == null)
                            {

                                item.BasicCalibrationResult = null;
                                context.Force.Add(item);
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                item.BasicCalibrationResult = null;
                                context.Force.Update(item);
                                await context.SaveChangesAsync();
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }


            }




            //////////////////////////////////////// DELETE
            var lines = a;//await context.Force.AsNoTracking().Where(x => x.WorkOrderDetailId == id).ToListAsync();

            var resultlines = lines.Where(p => !Forces.Any(p2 => p2.SequenceID == p.SequenceID
            && p2.CalibrationSubTypeId == p.CalibrationSubTypeId && p2.WorkOrderDetailId == p.WorkOrderDetailId));

            foreach (var ity in resultlines)
            {


                context.Force.Remove(ity);
                await context.SaveChangesAsync();

                var cc = await context.ForceResult.AsNoTracking().Where(x => x.WorkOrderDetailId == id && x.SequenceID == ity.SequenceID && x.CalibrationSubTypeId == ity.CalibrationSubTypeId).FirstOrDefaultAsync();

                if (cc != null)
                {
                    //ity.BasicCalibrationResult.Force = null;
                    context.ForceResult.Remove(cc);
                    await context.SaveChangesAsync();

                }
            }

            ////////////////////////////////////////////////////////////////////


            lines = null;
            resultlines = null;

        }

        public async Task SaveExternalConditions(int id, ICollection<ExternalCondition> items)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var ExCon = await context.ExternalCondition.AsNoTracking().Where(x => x.WorkOrderDetailId == id).ToListAsync();

            // Convert to List to ensure it's mutable and avoid "Collection was of a fixed size" error
            var itemsList = items.ToList();

            foreach (var item in itemsList)
            {
                var exc = ExCon.Where(x => x.ExternalConditionId == item.ExternalConditionId).FirstOrDefault();
                item.WorkOrderDetailId = id;
                if (exc != null)
                {
                    context.ExternalCondition.Update(item);
                }
                else
                {
                    item.ExternalConditionId = NumericExtensions.GetUniqueID(item.ExternalConditionId);

                    context.ExternalCondition.Add(item);
                }

                await context.SaveChangesAsync();
            }


        }




        public async Task SaveCalibrationTypes2<TType, TResult>(string Component, string id, ICollection<TResult> Forces, Func<TType, TType> FormatFunction,BalanceAndScaleCalibration bc= null)
           where TType : class, IGenericCalibrationSubTypeCollection<TResult>, IResultComp, IResultTesPointGroup
            where TResult : class, IResult2, IResultComp, IResultGen2, IUpdated, ISelect, IAggregate, IResultTesPointGroup
        {


            await using var context = await DbFactory.CreateDbContextAsync();

            //TContext context;
            //if (_context1 == null)
            //{
                
            //    context = context1;
            //}
            //else
            //{
            //    context = _context1;
            //}

            //await using var context = await DbFactory.CreateDbContextAsync();

           


            if (Forces == null)
            {
                return;
            }

            var bcc = await context.BalanceAndScaleCalibration.AsNoTracking().Where(x => x.WorkOrderDetailId.ToString() == id).FirstOrDefaultAsync();


            if (bcc == null && bc != null)
            {
                context.BalanceAndScaleCalibration.Add(bc);
                await context.SaveChangesAsync();

            }

            Forces = Forces.DistinctBy(book => new { book.ComponentID, book.SequenceID, book.CalibrationSubTypeId, book.GroupName }).ToArray();


            var lin234 = Forces.ToList();

            List<GenericCalibration2> lin = new List<GenericCalibration2>();

            foreach (var itemtype in lin234)
            {


                ///if (itemtype.GenericCalibration2 != null && itemtype.GenericCalibration2.BasicCalibrationResult != null)

                if (itemtype.GenericCalibration2 != null)
                {
                    lin.Add(itemtype.GenericCalibration2);
                }



            }

            foreach (var item in lin.Where(x => x.WeightSets != null && x.WeightSets.Count > 0))
            {
                if (item.WeightSets != null && item.WeightSets.Count > 0)
                {
                    List<CalibrationSubType_Weight> csw = new List<CalibrationSubType_Weight>();

                    foreach (var w in item.WeightSets)  //.Where(x => x.PieceOfEquipment == null)
                    {

                        if (csw.Where(x => x.WeightSetID == w.WeightSetID && x.SecuenceID == item.SequenceID).FirstOrDefault() == null)
                        {
                            CalibrationSubType_Weight ww = new CalibrationSubType_Weight();

                            ww.CalibrationSubTypeID = item.CalibrationSubTypeId;
                            ww.SecuenceID = item.SequenceID;
                            //ww.WorkOrderDetailID = id;
                            ww.WeightSetID = w.WeightSetID;
                            ww.Component = item.Component;
                            ww.ComponentID = id;
                            csw.Add(ww);
                        }

                    }

                    item.CalibrationSubType_Weights = csw;
                    item.WeightSets = null;
                }

            }

            //var lin = Forces.ToList();

            if (lin != null && lin.Count > 0)
            {
                foreach (var item in lin)
                {

                    await SaveSubType(item.CalibrationSubTypeId);

                    var sss = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.SecuenceID == item.SequenceID
                    && x.CalibrationSubTypeID == item.CalibrationSubTypeId && x.ComponentID == id && x.Component == item.Component).ToArrayAsync();
                    if (sss != null && sss.Count() > 0)
                    {
                        context.CalibrationSubType_Standard.RemoveRange(sss);
                        await context.SaveChangesAsync();
                    }

                    if (item.Standards != null && item.Standards.Count > 0)
                    {
                        foreach (var we in item.Standards)
                        {
                            //we.WorkOrderDetailID = Convert.ToInt32(id);
                            we.Component = item.Component;
                            we.ComponentID = id;
                            we.SecuenceID = item.SequenceID;
                            we.CalibrationSubTypeID = item.CalibrationSubTypeId;
                            //we.CalibrationSubType = c;
                            context.CalibrationSubType_Standard.Add(we);
                            await context.SaveChangesAsync();

                        }
                    }
                    item.Standards = null;

                    //item.wei = null;
                }

                //var sss2 = await context.CalibrationSubType_Standard.AsNoTracking().Where(x =>  && x.ComponentID == id).ToArrayAsync();


            }

            if (lin != null && lin.Count > 0)
            {
                foreach (var item in lin)
                {

                    await SaveSubType(item.CalibrationSubTypeId);

                    var sss = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.SecuenceID == item.SequenceID
                    && x.CalibrationSubTypeID == item.CalibrationSubTypeId && x.ComponentID == id && x.Component == item.Component).ToArrayAsync();
                    if (sss != null && sss.Count() > 0)
                    {
                        context.CalibrationSubType_Weight.RemoveRange(sss);
                        await context.SaveChangesAsync();
                    }

                    if (item.CalibrationSubType_Weights != null && item.CalibrationSubType_Weights.Count > 0)
                    {
                        foreach (var we in item.CalibrationSubType_Weights)
                        {

                            //we.CalibrationSubType = c;
                            context.CalibrationSubType_Weight.Add(we);
                            await context.SaveChangesAsync();

                        }
                    }


                    item.CalibrationSubType_Weights = null;

                    item.WeightSets = null;
                }
            }



            var a = await context.Set<TType>().AsNoTracking().Where(x => x.ComponentID == id && x.Component == Component).ToListAsync();
            var aa = await context.Set<TResult>().AsNoTracking().Where(x => x.ComponentID == id && x.Component == Component).ToListAsync();

            var groupByLastNamesQuery =
            from student in Forces
            group student by student.CalibrationSubTypeId into newGroup
            orderby newGroup.Key
            select newGroup;

            foreach (var nameGroup in groupByLastNamesQuery)
            {
//                Console.WriteLine($"Key: {nameGroup.Key}");
                //foreach (var student in nameGroup)
                //{
                //    Console.WriteLine($"\t{student.LastName}, {student.FirstName}");
                //}
                int cont = 1;
                //foreach (var item in nameGroup)
                //{



                //}

                foreach (var item in nameGroup)
                {
                    //try
                    //{
                        var b = a.Where(x => x.SequenceID == item.SequenceID && x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();

                        var bb = aa.Where(x => x.SequenceID == item.SequenceID && x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();

                        if (item.Position >= 0)
                        {
                            item.Position = cont;
                        }

                        item.ComponentID = id;
                        item.Component = Component;


                        //if (item.SequenceID == 0)
                        //{
                            item.SequenceID = cont;
                        //}

                        if (item.Updated != null && item.Updated < 10000)
                        {
                            item.Updated = null;
                        }

                        if (item.GenericCalibration2 != null)
                        {

                            item.GenericCalibration2.Component = item.Component;
                            item.GenericCalibration2.ComponentID = item.ComponentID;
                            item.GenericCalibration2.SequenceID = item.SequenceID;
                            item.GenericCalibration2.CalibrationSubTypeId = item.CalibrationSubTypeId;
                            item.GenericCalibration2.TestPoint = null;
                            item.GenericCalibration2.TestPointID = null;
                            //item.BasicCalibrationResult.Rockwell = null;
                            item.GenericCalibration2.WeightSets = null;
                            item.GenericCalibration2.Standards = null;

                            if (item.GenericCalibration2.Standards != null && item.GenericCalibration2.Standards.Count > 0)
                            {
                                foreach (var item4 in item.GenericCalibration2.Standards)
                                {
                                    item4.ComponentID = id;

                                    var sdall = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.CalibrationSubTypeID == item.CalibrationSubTypeId
                                        && x.ComponentID == id && x.SecuenceID == item.SequenceID).FirstOrDefaultAsync();


                                    CalibrationSubType_Standard sdall2 = null;
                                    if (sdall != null)
                                    {

                                        sdall2 = item.GenericCalibration2.Standards.FirstOrDefault(x => x.PieceOfEquipmentID == sdall.PieceOfEquipmentID);
                                    }


                                    if (sdall2 == null && sdall != null)
                                    {
                                        context.Add(item4);
                                        await context.SaveChangesAsync();
                                        context.Remove(sdall);
                                        await context.SaveChangesAsync();

                                    }
                                    else
                                    if (sdall2 == null && sdall == null)
                                    {
                                        context.Add(item4);
                                        await context.SaveChangesAsync();
                                    }

                                }



                            }

                            if (item.GenericCalibration2.Standards == null && 1 == 2)
                            {
                                var sdallx = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.CalibrationSubTypeID == item.CalibrationSubTypeId
                                        && x.Component == id && x.ComponentID == id && x.SecuenceID == item.SequenceID).ToListAsync();


                                if (sdallx.Count > 0)
                                {
                                    context.RemoveRange(sdallx);
                                    await context.SaveChangesAsync();
                                }


                            }

                        }
                        if (item?.GenericCalibration2?.Standards != null)
                        {
                            item.GenericCalibration2.Standards = null;
                        }


                        if (b == null && bb == null)
                        {
//                            Console.WriteLine(item.CalibrationSubTypeId);
                            var bcr = item;
                            //item.GenericCalibration2 = null;
                            if (item.GenericCalibration2 != null)
                            {
                                item.GenericCalibration2.TestPointResult = null;

                            var bexis = a.Where(x => x.ComponentID == id && x.Component == Component && x.SequenceID == item.SequenceID && x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();



                            if (bexis == null)
                            {
                                context.Add(item.GenericCalibration2);
                            }
                            else
                            {
                                context.Update(item.GenericCalibration2);

                            }


                          
                                await context.SaveChangesAsync();

                            }
                            bcr.GenericCalibration2 = null;

                        var bexis2 = aa.Where(x => x.ComponentID == id && x.Component == Component && x.SequenceID == item.SequenceID && x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();



                        if (bexis2 == null)
                        {
                            context.Add(bcr);
                        }
                        else
                        {
                            context.Update(bcr);

                        }

                        
                            await context.SaveChangesAsync();
                            if (!string.IsNullOrEmpty(bcr.Aggregate))
                            {
                                GenericCalibrationResult2Aggregate agg = new GenericCalibrationResult2Aggregate();


                                agg.SequenceID = bcr.SequenceID;
                                agg.Component = bcr.Component;
                                agg.ComponentID = bcr.ComponentID;
                                agg.CalibrationSubTypeId = bcr.CalibrationSubTypeId;
                                agg.Type = bcr.Component;
                                agg.JSON = bcr.Aggregate;

                                context.Add(agg);
                                await context.SaveChangesAsync();

                            }


                        }
                        else
                        {
                            if (item.GenericCalibration2 != null && b == null)
                            {
                                item.GenericCalibration2.TestPointResult = null;

                                context.Add(item.GenericCalibration2);
                                await context.SaveChangesAsync();

                            }
                            else if (item.GenericCalibration2 != null)
                            {

                            item.GenericCalibration2.TestPointResult = null;

                            var bexis= a.Where(x => x.ComponentID == id && x.Component == Component && x.SequenceID == item.SequenceID && x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();

                            

                            if(bexis == null)
                            {                                
                                context.Add(item.GenericCalibration2);
                            }
                            else
                            {
                                context.Update(item.GenericCalibration2);
                               
                            }

                            await context.SaveChangesAsync();
                        }
                            if (bb == null)
                            {

                                item.GenericCalibration2 = null;

                            var bexis2 = aa.Where(x => x.ComponentID == id && x.Component == Component && x.SequenceID == item.SequenceID && x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();

                            if(bexis2 == null)
                                {
                                    context.Add(item);
                                }
                                else
                                {
                                    context.Update(item);
                                }

                                await context.SaveChangesAsync();

                                if (!string.IsNullOrEmpty(item.Aggregate))
                                {
                                    GenericCalibrationResult2Aggregate agg = new GenericCalibrationResult2Aggregate();


                                    agg.SequenceID = item.SequenceID;
                                    agg.Component = item.Component;
                                    agg.ComponentID = item.ComponentID;
                                    agg.CalibrationSubTypeId = item.CalibrationSubTypeId;
                                    agg.Type = item.Component;
                                    
                                    agg.JSON = item.Aggregate;

                                    context.Add(agg);
                                    await context.SaveChangesAsync();

                                }

                            }
                            else
                            {
                                item.GenericCalibration2 = null;

                            var bexis2 = aa.Where(x => x.ComponentID == id && x.Component == Component && x.SequenceID == item.SequenceID && x.CalibrationSubTypeId == item.CalibrationSubTypeId).FirstOrDefault();

                            if (bexis2 == null)
                            {
                                context.Add(item);
                            }
                            else
                            {
                                context.Update(item);
                            }

                            await context.SaveChangesAsync();

                                if (!string.IsNullOrEmpty(item.Aggregate))
                                {
                                    GenericCalibrationResult2Aggregate agg = new GenericCalibrationResult2Aggregate();


                                    agg.SequenceID = item.SequenceID;
                                    agg.Component = item.Component;
                                    agg.ComponentID = item.ComponentID;
                                    agg.CalibrationSubTypeId = item.CalibrationSubTypeId;
                                    agg.Type = item.Component;
                                    agg.JSON = item.Aggregate;

                                    context.Update(agg);
                                    await context.SaveChangesAsync();

                                }

                            }
                        }
                        cont++;

                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                }


            }



            //////////////////////////////////////// DELETE
            var lines = aa;//await context.Force.AsNoTracking().Where(x => x.WorkOrderDetailId == id).ToListAsync();

            var resultlines = lines.Where(p => !Forces.Any(p2 => p2.SequenceID == p.SequenceID
            && p2.CalibrationSubTypeId == p.CalibrationSubTypeId && p2.Component == p.Component && p2.ComponentID == p2.ComponentID));

            foreach (var ity in resultlines)
            {

                var cc = await context.Set<TType>().AsNoTracking().Where(x => x.ComponentID == id && x.Component == Component && x.SequenceID == ity.SequenceID && x.CalibrationSubTypeId == ity.CalibrationSubTypeId).FirstOrDefaultAsync();

                context.Remove(ity);
                await context.SaveChangesAsync();

                if (cc != null)
                {
                    //ity.BasicCalibrationResult.Force = null;
                    context.Remove(cc);
                    await context.SaveChangesAsync();

                }




            }

            ////////////////////////////////////////////////////////////////////


            //////Aggregattes
            ///




            lines = null;
            resultlines = null;

        }


        public async Task<List<TType>> GetCalibrationType2<TType, TResult>(string workOrderDetailId, string Component)
          where TType : class, IGenericCalibrationSubTypeCollection<TResult>, ICalibrationSubType2, IGenericCalibrationStandard where TResult : class, IResult2, IResultComp, IResultGen2
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            IQueryable<TType> query = context.Set<TType>().AsNoTracking().Where(x => x.ComponentID == workOrderDetailId && x.Component == Component);

            var forces = await query.ToListAsync();

            if (forces != null && forces?.Count() > 0)
            {

                var ws = await context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).Where(x => x.ComponentID == workOrderDetailId).ToListAsync();


                foreach (var wer in forces)
                {

                    var forcesres = await context.Set<TResult>().AsNoTracking().Where(x => x.ComponentID == workOrderDetailId
                    && x.SequenceID == wer.SequenceID && x.CalibrationSubTypeId == wer.CalibrationSubTypeId && x.Component == Component).ToListAsync();

                    wer.TestPointResult = forcesres;

                    wer.WeightSets = await GetCalibrationSubTypes_WeightSets(wer, workOrderDetailId, Component);


                    var stan = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.ComponentID == workOrderDetailId
                    && x.SecuenceID == wer.SequenceID && x.CalibrationSubTypeID == wer.CalibrationSubTypeId && x.Component == wer.Component).ToArrayAsync();

                    wer.Standards = stan;



                }

                return forces.ToList();


            }
            else
            {
                IQueryable<TResult> query2 = context.Set<TResult>().AsNoTracking().Where(x => x.ComponentID == workOrderDetailId && x.Component == Component);

                var forces2 = await query2.ToListAsync();



            }
            return null;


        }

        public async Task<List<TResult>> GetCalibrationType22<TType, TResult>(string workOrderDetailId, string Component)
         where TType : class, IGenericCalibrationSubTypeCollection<TResult>, ICalibrationSubType2, IGenericCalibrationStandard, IResultTesPointGroup
            where TResult : class, IResult2, IResultComp, IResultGen2, IUpdated, ISelect, IResultTesPointGroup
        {
            await using var context = await DbFactory.CreateDbContextAsync();


            IQueryable<TResult> query = context.Set<TResult>().AsNoTracking().Where(x => x.ComponentID == workOrderDetailId && x.Component == Component);

            var forces = await query.ToListAsync();

            if (forces != null && forces?.Count() > 0)
            {

                var ws = await context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).Where(x => x.ComponentID == workOrderDetailId && x.Component == Component).ToListAsync();


                foreach (var wer in forces)
                {

                    var forcesres = await context.Set<TType>().AsNoTracking().Where(x => x.ComponentID == workOrderDetailId
                    && x.SequenceID == wer.SequenceID && x.CalibrationSubTypeId == wer.CalibrationSubTypeId && x.Component == Component).FirstOrDefaultAsync();

                    wer.GenericCalibration2 = forcesres as GenericCalibration2;

                    if (forcesres != null)
                    {
                        wer.GenericCalibration2.WeightSets = await GetCalibrationSubTypes_WeightSets(wer.GenericCalibration2, workOrderDetailId, Component);

                        var stan = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.ComponentID == workOrderDetailId
                   && x.SecuenceID == wer.SequenceID && x.CalibrationSubTypeID == wer.CalibrationSubTypeId && x.Component == wer.Component).ToArrayAsync();

                        wer.GenericCalibration2.Standards = stan;
                    }

                }


                if (forces != null)
                {


                    foreach (var item in forces)
                    {
                        if (item?.GenericCalibration2?.TestPointResult != null)
                        {
                            item.GenericCalibration2.TestPointResult = null;

                        }

                        //item.GenericCalibration2.WeightSets = null;

                        //item.GenericCalibration2.Standards= null;   

                        //item.GenericCalibration2.CalibrationSubType_Weights= null;  

                        //if (item.GenericCalibration2.WeightSets.Count > 0)
                        //{

                        //}

                    }





                }


                return forces.ToList();


            }
            else
            {
                IQueryable<TResult> query2 = context.Set<TResult>().AsNoTracking().Where(x => x.ComponentID == workOrderDetailId && x.Component == Component);

                var forces2 = await query2.ToListAsync();

                return forces2;

            }
            return null;


        }




        public async Task SaveCalibrationTypes<TType, TResult>(int id, ICollection<TType> Forces, Func<TType, TType> FormatFunction)
            where TType : class, IGenericCalibrationSubType<TResult> where TResult : class, IResult2, IUpdated
        {

            await using var context = await DbFactory.CreateDbContextAsync();


            if (Forces == null)
            {
                return;
            }

            Forces = Forces.DistinctBy(book => new { book.WorkOrderDetailId, book.SequenceID, book.CalibrationSubTypeId }).ToArray();

            var lin = Forces.ToList();

            //foreach (var item in lin.Where(x => x.WeightSets != null && x.CalibrationSubType_Weights == null))
            //{
            //    foreach (var item3 in item.WeightSets)
            //    {

            //        var wes = await context.WeightSet.AsNoTracking().Where(x => x.WeightSetID == item3.WeightSetID).FirstOrDefaultAsync();

            //        var csw = new CalibrationSubType_Weight();

            //        csw.WorkOrderDetailID = id;
            //        csw.SecuenceID = item.SequenceID;
            //        csw.CalibrationSubTypeID = item.SequenceID;



            //    }








            //}

            foreach (var item in lin.Where(x => x.WeightSets != null && x.WeightSets.Count > 0))
            {
                if (item.WeightSets != null && item.WeightSets.Count > 0)
                {
                    List<CalibrationSubType_Weight> csw = new List<CalibrationSubType_Weight>();

                    foreach (var w in item.WeightSets)
                    {
                        CalibrationSubType_Weight ww = new CalibrationSubType_Weight();

                        ww.CalibrationSubTypeID = item.CalibrationSubTypeId;
                        ww.SecuenceID = item.SequenceID;
                        ww.WorkOrderDetailID = id;
                        ww.WeightSetID = w.WeightSetID;
                        ww.Component = "WorkOrderItem";
                        ww.ComponentID = id.ToString();
                        if (w.PieceOfEquipment == null)
                        {
                            csw.Add(ww);
                        }

                    }

                    item.CalibrationSubType_Weights = csw;
                    item.WeightSets = null;
                }

            }
            try
            {
                //&& x.CalibrationSubType_Weights.Count > 0 && (x.Standards==null || (x.Standards !=null && x.Standards.Count==0)))
                foreach (var item in lin.Where(x => x.CalibrationSubType_Weights != null))
                {

                    await SaveSubType(item.CalibrationSubTypeId);

                    var sss = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == item.WorkOrderDetailId
                     && x.SecuenceID == item.SequenceID && x.CalibrationSubTypeID == item.CalibrationSubTypeId).ToArrayAsync();
                    if (sss != null && sss.Count() > 0)
                    {
                        context.CalibrationSubType_Weight.RemoveRange(sss);
                        await context.SaveChangesAsync();
                    }
                    //TODO review this code to validate
                    foreach (var we in item.CalibrationSubType_Weights)
                    {
                        //var weigth = await context.WeightSet.AsNoTracking().Where(x => x.WeightSetID == we.WeightSetID).FirstOrDefaultAsync();

                        //if (weigth == null)
                        //{
                            //we.CalibrationSubType = c;
                            context.CalibrationSubType_Weight.Add(we);
                            await context.SaveChangesAsync();
                        //}
                    }

                    item.CalibrationSubType_Weights = null;

                    item.WeightSets = null;
                }
            }
            catch (Exception ex)
            {
//                Console.WriteLine(ex.Message);
            }



            var a = await context.Set<TType>().AsNoTracking().Where(x => x.WorkOrderDetailId == id).ToListAsync();
            var aa = await context.Set<TResult>().AsNoTracking().Where(x => x.WorkOrderDetailId == id).ToListAsync();
            int cont = 0;
            foreach (var item2w in Forces.ToList())
            {
                try
                {
                    var b = await context.GenericCalibration.AsNoTracking().Where(x => x.SequenceID == item2w.SequenceID && x.CalibrationSubTypeId == item2w.CalibrationSubTypeId && x.WorkOrderDetailId == id).FirstOrDefaultAsync();

                    var bb = await context.GenericCalibrationResult.AsNoTracking().Where(x => x.SequenceID == item2w.BasicCalibrationResult.SequenceID && x.CalibrationSubTypeId == item2w.BasicCalibrationResult.CalibrationSubTypeId && x.WorkOrderDetailId == id).FirstOrDefaultAsync();


                    item2w.BasicCalibrationResult.Position = cont;
                    item2w.BasicCalibrationResult.WorkOrderDetailId = id;
                    item2w.WorkOrderDetailId = id;
                    item2w.TestPoint = null;
                    item2w.TestPointID = null;
                    //item2w.BasicCalibrationResult.Rockwell = null;
                    item2w.WeightSets = null;

                    item2w.CalibrationSubType_Weights = null;

                    if (item2w.Standards != null && item2w.Standards.Count > 0)
                    {
                        foreach (var item4 in item2w.Standards)
                        {
                            item4.WorkOrderDetailID = id;

                            var sdall = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.CalibrationSubTypeID == item2w.CalibrationSubTypeId
                                && x.WorkOrderDetailID == id && x.SecuenceID == item2w.SequenceID).FirstOrDefaultAsync();


                            CalibrationSubType_Standard sdall2 = null;
                            if (sdall != null)
                            {

                                sdall2 = item2w.Standards.FirstOrDefault(x => x.PieceOfEquipmentID == sdall.PieceOfEquipmentID);
                            }


                            if (sdall2 == null && sdall != null)
                            {
                                context.Add(item4);
                                await context.SaveChangesAsync();
                                context.Remove(sdall);
                                await context.SaveChangesAsync();

                            }
                            else
                            if (sdall2 == null && sdall == null)
                            {
                                context.Add(item4);
                                await context.SaveChangesAsync();
                            }

                        }



                    }

                    if (item2w.Standards == null)
                    {
                        var sdallx = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.CalibrationSubTypeID == item2w.CalibrationSubTypeId
                                && x.WorkOrderDetailID == id && x.SecuenceID == item2w.SequenceID).ToListAsync();


                        if (sdallx.Count > 0)
                        {
                            context.RemoveRange(sdallx);
                            await context.SaveChangesAsync();
                        }


                    }

                    item2w.Standards = null;
                    item2w.CalibrationSubType_Weights = null;
                    item2w.Standards = null;

                    FormatFunction(item2w);
                    if (item2w?.Standards?.Count > 0)
                    {
                        item2w.Standards.Clear();
                    }
                    if (item2w?.CalibrationSubType_Weights?.Count > 0)
                    {
                        item2w.CalibrationSubType_Weights.Clear();
                    }


                    if (item2w?.BasicCalibrationResult.Updated != null && item2w?.BasicCalibrationResult?.Updated < 10000)
                    {
                        item2w.BasicCalibrationResult.Updated = null;
                    }

                    if (b == null && bb == null)
                    {
//                        Console.WriteLine(item2w.BasicCalibrationResult.CalibrationSubTypeId);
                        var bcr = item2w.BasicCalibrationResult;
                        item2w.BasicCalibrationResult = null;
                        context.Add(item2w);
                        await context.SaveChangesAsync();


                        context.Add(bcr);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        if (bb == null)
                        {
                            context.Add(item2w.BasicCalibrationResult);
                            await context.SaveChangesAsync();
                        }
                        else
                        {

                            var csd = item2w.BasicCalibrationResult;
                            context.Update(csd);
                            await context.SaveChangesAsync();
                        }
                        if (b == null)
                        {

                            item2w.BasicCalibrationResult = null;
                            context.Add(item2w);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            item2w.BasicCalibrationResult = null;
                            context.Update(item2w);
                            await context.SaveChangesAsync();
                        }
                    }
                    cont++;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            foreach (var item4 in lin.Where(x => x.Standards != null))
            {
                var sdall = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.CalibrationSubTypeID == item4.CalibrationSubTypeId
                && x.WorkOrderDetailID == id && x.SecuenceID == item4.SequenceID).FirstOrDefaultAsync();

                var sdall2 = item4.Standards.Where(x => x.PieceOfEquipmentID == sdall.PieceOfEquipmentID).FirstOrDefault();

                if (sdall2 == null && sdall != null)
                {
                    context.Add(item4);
                    await context.SaveChangesAsync();
                    context.Remove(sdall);
                    await context.SaveChangesAsync();

                }
                else
                if (sdall2 == null && sdall == null)
                {
                    context.Add(item4);
                    await context.SaveChangesAsync();
                }


            }



            var lines = a;//await context.Force.AsNoTracking().Where(x => x.WorkOrderDetailId == id).T
            //////////////////////////////////////// DELETEoListAsync();

            var resultlines = lines.Where(p => !Forces.Any(p2 => p2.SequenceID == p.SequenceID
            && p2.CalibrationSubTypeId == p.CalibrationSubTypeId && p2.WorkOrderDetailId == p.WorkOrderDetailId));

            foreach (var ity in resultlines)
            {

                var cc = await context.Set<TResult>().AsNoTracking().Where(x => x.WorkOrderDetailId == id && x.SequenceID == ity.SequenceID && x.CalibrationSubTypeId == ity.CalibrationSubTypeId).FirstOrDefaultAsync();

                if (cc != null)
                {
                    //ity.BasicCalibrationResult.Force = null;
                    context.Remove(cc);
                    await context.SaveChangesAsync();

                }

                context.Remove(ity);
                await context.SaveChangesAsync();


            }

            ////////////////////////////////////////////////////////////////////


            lines = null;
            resultlines = null;

        }


        public async Task SaveItemsWeights(List<ICalibrationSubType> lin)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            foreach (var item in lin)
            {
                if (item.CalibrationSubType_Weights != null)
                {

                    await SaveSubType(item.CalibrationSubTypeId);

                    var sss = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == item.WorkOrderDetailId
                     && x.SecuenceID == item.SequenceID && x.CalibrationSubTypeID == item.CalibrationSubTypeId).ToArrayAsync();
                    if (sss != null && sss.Count() > 0)
                    {
                        context.CalibrationSubType_Weight.RemoveRange(sss);
                        await context.SaveChangesAsync();
                    }
                    foreach (var we in item.CalibrationSubType_Weights)
                    {

                        //we.CalibrationSubType = c;
                        context.CalibrationSubType_Weight.Add(we);
                        await context.SaveChangesAsync();

                    }

                    item.CalibrationSubType_Weights = null;
                }
            }



        }

        public async Task<WorkOrderDetail> ChangeStatus(WorkOrderDetail DTO, string Component = "WorkOrderItem", bool forceUpdate = false, bool CustomId = false)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            int line = 0;

            if (string.IsNullOrEmpty(Component))
            {
                Component = "WorkOrderItem";
            }
            
        
            ///Cancel
            if (DTO.OnlyChngeStatus)
            {

                var wodcna = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).FirstOrDefaultAsync();

                wodcna.CurrentStatusID = DTO.CurrentStatusID;

                if (wodcna.CurrentStatusID == 5)
                {
                    wodcna.HasBeenCompleted = true;
                }
                else
                {
                    wodcna.HasBeenCompleted = false;

                }
                wodcna.CurrentStatus = null;

                context.Update(wodcna);
                await context.SaveChangesAsync();

                return DTO;
            }

            if (DTO.WorkOrderDetailID == 0)
            {
                return DTO;
            }


            if (DTO?.BalanceAndScaleCalibration?.Linearities != null && DTO?.BalanceAndScaleCalibration?.Linearities?.Count() > 0)
            {
                DTO.BalanceAndScaleCalibration.Linearities.ToList().ForEach(item =>
                {
                    if (item.BasicCalibrationResult != null)
                    {
                        item.BalanceAndScaleCalibration = null;
                        item.BasicCalibrationResult.UnitOfMeasure = null;
                        item.UnitOfMeasure = null;

                        // Convert WeightSets to CalibrationSubType_Weights if not already converted
                        // ALWAYS convert if WeightSets exists (even if empty) to support clearing weights
                        if (item.WeightSets != null)
                        {
                            List<CalibrationSubType_Weight> lstcsw = new List<CalibrationSubType_Weight>();

                            foreach (var item2 in item.WeightSets)
                            {
                                var csw = new CalibrationSubType_Weight();

                                csw.CalibrationSubTypeID = item.CalibrationSubTypeId;
                                csw.SecuenceID = item.SequenceID;
                                csw.WorkOrderDetailID = DTO.WorkOrderDetailID;
                                csw.WeightSetID = item2.WeightSetID;
                                csw.Component = Component;
                                csw.ComponentID = DTO.WorkOrderDetailID.ToString();
                                lstcsw.Add(csw);
                            }

                            // Set CalibrationSubType_Weights (will be empty list if no weights selected)
                            // This triggers deletion of old weights in the database
                            item.CalibrationSubType_Weights = lstcsw;
                        }

                        item.WeightSets = null;
                    }
                }
              );
            }


            if (DTO?.BalanceAndScaleCalibration?.Repeatability != null)
            {

                if (DTO?.BalanceAndScaleCalibration?.Repeatability?.CalibrationSubType_Weights == null
                    && DTO?.BalanceAndScaleCalibration?.Repeatability?.WeightSets?.Count > 0)
                {
                    List<CalibrationSubType_Weight> lstcsw = new List<CalibrationSubType_Weight>();
                    foreach (var item2 in DTO?.BalanceAndScaleCalibration?.Repeatability?.WeightSets)
                    {
                        var csw = new CalibrationSubType_Weight();

                        csw.CalibrationSubTypeID = DTO.BalanceAndScaleCalibration.Repeatability.CalibrationSubTypeId;
                        csw.SecuenceID = DTO.BalanceAndScaleCalibration.Repeatability.SequenceID;
                        csw.WorkOrderDetailID = DTO.WorkOrderDetailID;
                        csw.WeightSetID = item2.WeightSetID;
                        csw.Component = Component;
                        csw.ComponentID = DTO.WorkOrderDetailID.ToString();
                        lstcsw.Add(csw);
                    }

                    DTO.BalanceAndScaleCalibration.Repeatability.CalibrationSubType_Weights = lstcsw;
                }


                DTO.BalanceAndScaleCalibration.Repeatability.WeightSets = null;
                DTO.BalanceAndScaleCalibration.Repeatability.BalanceAndScaleCalibration = null;

                DTO.BalanceAndScaleCalibration.Repeatability?.TestPointResult?.ToList().ForEach(item =>
                {

                    item.UnitOfMeasure = null;

                }
                  );
            }


            if (DTO?.BalanceAndScaleCalibration?.Eccentricity != null)
            {

                DTO.BalanceAndScaleCalibration.Eccentricity.BalanceAndScaleCalibration = null;

                if (DTO?.BalanceAndScaleCalibration?.Eccentricity?.CalibrationSubType_Weights == null
                    && DTO?.BalanceAndScaleCalibration?.Eccentricity?.WeightSets?.Count > 0)
                {
                    List<CalibrationSubType_Weight> lstcsw = new List<CalibrationSubType_Weight>();
                    foreach (var item2 in DTO?.BalanceAndScaleCalibration?.Eccentricity?.WeightSets)
                    {
                        var csw = new CalibrationSubType_Weight();

                        csw.CalibrationSubTypeID = DTO.BalanceAndScaleCalibration.Eccentricity.CalibrationSubTypeId;
                        csw.SecuenceID = DTO.BalanceAndScaleCalibration.Eccentricity.SequenceID;
                        csw.WorkOrderDetailID = DTO.WorkOrderDetailID;
                        csw.WeightSetID = item2.WeightSetID;
                        csw.Component = Component;
                        csw.ComponentID = DTO.WorkOrderDetailID.ToString();
                        lstcsw.Add(csw);
                    }

                    DTO.BalanceAndScaleCalibration.Eccentricity.CalibrationSubType_Weights = lstcsw;
                }




                DTO.BalanceAndScaleCalibration.Eccentricity.WeightSets = null;

                DTO.BalanceAndScaleCalibration.Eccentricity?.TestPointResult?.ToList().ForEach(item =>
                {

                    item.UnitOfMeasure = null;

                }
               );
            }


            //ahora se crean testpoints nuevos
            //if (DTO?.BalanceAndScaleCalibration?.Eccentricity?.WorkOrderDetailId == 0 || DTO.BalanceAndScaleCalibration?.Eccentricity?.TestPointID == 0)
            //{
            //    DTO.BalanceAndScaleCalibration.Eccentricity = null;
            //}
            //if (DTO?.BalanceAndScaleCalibration?.Repeatability?.WorkOrderDetailId == 0 || DTO?.BalanceAndScaleCalibration?.Repeatability?.TestPointID == 0)
            //{
            //    DTO.BalanceAndScaleCalibration.Repeatability = null;
            //}



            var bas = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID)
                .Include(x => x.BalanceAndScaleCalibration).AsNoTracking().FirstOrDefaultAsync();



            //DTO.PieceOfEquipment = null;

            //TODO: code for offline
            if (bas == null)
            {
                WorkOrderDetail wod = new WorkOrderDetail();

                wod.PieceOfEquipmentId = DTO.PieceOfEquipmentId;
                //wod.SerialNumber = poe.SerialNumber;
                // _wo.CalibrationType = eq.CalibrationType;//Result.CalibrationType;
                //wod.DueDate = poe.DueDate;

                wod.IsAccredited = DTO.IsAccredited;
                //wod.WorkOder = _wo;
                wod.CurrentStatusID = 1;

                wod.WorkOrderDetailID = DTO.WorkOrderDetailID;// Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2) + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());

                //wod.CalibrationTypeID = DTO.CalibrationTypeID;

                wod.WorkOrderID = DTO.WorkOrderID;

                DTO.OfflineID = wod.WorkOrderDetailID.ToString();

                wod.CurrentStatus = null;

                context.WorkOrderDetail.Add(wod);

                await context.SaveChangesAsync();

                bas = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID)
                .Include(x => x.BalanceAndScaleCalibration).AsNoTracking().FirstOrDefaultAsync();

                //DTO.WorkOrderDetailID = wod.WorkOrderDetailID;

            }
            // Diccionario para guardar los WeightSets antes de que se pongan en null
            Dictionary<string, WeightSet> weightSetBackup = new Dictionary<string, WeightSet>();
            await SaveWODWeights(context, DTO, weightSetBackup);
            DTO.WOD_Weights = null;
            if (DTO?.BalanceAndScaleCalibration?.GenericCalibration == null)
            {
                if (bas != null && DTO.BalanceAndScaleCalibration==null && bas?.BalanceAndScaleCalibration == null && !forceUpdate )
                {
                    try
                    {
                        line = 1;
                        //BalanceAndScaleCalibration bc = new BalanceAndScaleCalibration();
                        var bc = DTO.BalanceAndScaleCalibration;


                        if (bc?.Eccentricity != null && bc?.Eccentricity?.BalanceAndScaleCalibration != null)
                        {
                            bc.Eccentricity.BalanceAndScaleCalibration = null;
                        }

                        if (bc?.Repeatability != null && bc?.Repeatability?.BalanceAndScaleCalibration != null)
                        {
                            bc.Repeatability.BalanceAndScaleCalibration = null;
                        }


                        /////////////yp
                        ///

                        ///////////////
                        TestPointGroup trr = null;
                        List<TestPointGroup> lll = null;
                        List<TestPointGroup> tepin = null;
                        if (bc is not null && bc?.Linearities is not null && bc?.Linearities?.Count > 0)
                        {

                            line = 12;
                            tepin = await context.TestPointGroup.AsNoTracking().Include(x => x.TestPoints)
                               .Where(x => x.WorkOrderDetailID.HasValue && x.WorkOrderDetailID == DTO.WorkOrderDetailID).AsNoTracking().ToListAsync();



                            if (DTO?.TestGroups != null && DTO?.TestGroups?.Count() > 0)
                            {
                                trr = DTO.TestGroups.ElementAtOrDefault(0);

                                trr.UnitOfMeasurementOut = null;

                                trr.TestPoints = null;


                            }
                            var tpg11 = tepin.Where(x => x.TestPoitGroupID == trr.TestPoitGroupID).FirstOrDefault();

                            if (trr != null && (tepin?.Count == 0 || tpg11 == null))
                            {
                                trr.TestPoitGroupID = NumericExtensions.GetUniqueID(trr.TestPoitGroupID);
                                context.TestPointGroup.Add(trr);
                            }
                            else
                            {
                                context.TestPointGroup.Update(trr);
                            }


                            await context.SaveChangesAsync();

                            DTO.TestGroups = null;
                            lll = tepin;
                            //var lll = await context.TestPointGroup.AsNoTracking().Include(x => x.TestPoints)
                            //    .Where(x => x.WorkOrderDetailID.HasValue && x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();


                            line = 13;
//                            Console.WriteLine("------------------------ Save wod " + DTO.WorkOrderDetailID.ToString());


                            var saveline = false;

                            foreach (var il in bc?.Linearities)
                            {
                                //context.ChangeTracker.;
                                if (il.TestPoint is not null)
                                {
                                    saveline = true;
                                    il.TestPoint.UnitOfMeasurement = null;
                                    il.TestPoint.UnitOfMeasurementOut = null;
                                    il.TestPoint.TestPointGroupTestPoitGroupID = trr.TestPoitGroupID;
                                    il.TestPoint.TestPointGroup = null;
                                    il.TestPoint.WOD_TestPoints = null;
                                    //var lllesta = lll.Where(x => x.TestPoitGroupID == trr.TestPoitGroupID)
                                    //    .Select(x => x.TestPoints.Select(x => x.TestPointID == il.TestPointID)).FirstOrDefault();

                                    var lllesta = lll.Where(x => x.TestPoitGroupID == trr.TestPoitGroupID).Where(x => x.TestPoints.Any(x => x.TestPointID == il.TestPointID)).FirstOrDefault();
                                    //.Select(x => x.TestPoints.Select(x => x.TestPointID == il.TestPointID)).FirstOrDefault();
                                    il.TestPoint.WOD_TestPoints = null;
                                    if (lllesta == null)
                                    {
                                        il.TestPoint.TestPointID = NumericExtensions.GetUniqueID(il.TestPoint.TestPointID);
                                        il.TestPointID = il.TestPoint.TestPointID;
                                        context.TestPoint.Add(il.TestPoint);
                                    }
                                    else
                                    {


                                        context.TestPoint.Update(il.TestPoint);
                                    }


                                    await Task.Delay(20);




                                    if (saveline)
                                    {
                                        var resul = await context.SaveChangesAsync();
                                    }


                                    il.TestPointID = il.TestPoint.TestPointID;
                                }



                                il.TestPoint = null;

                                il.UnitOfMeasure = null;
                                il.CalibrationUncertaintyValueUncertaintyUnitOfMeasure = null;


                                if (il != null && il?.BalanceAndScaleCalibration != null)
                                {
                                    il.BalanceAndScaleCalibration = null;
                                }



                            }


                        }

                        line = 14;
                        if (bc is not null && bc?.Eccentricity != null)

                        {

                            bc.Eccentricity.TestPoint = null;
                            var saveline = false;



                            if (bc.Eccentricity.TestPoint is not null)
                            {
                                saveline = true;
                                bc.Eccentricity.TestPoint.UnitOfMeasurement = null;
                                bc.Eccentricity.TestPoint.UnitOfMeasurementOut = null;
                                bc.Eccentricity.TestPoint.TestPointGroupTestPoitGroupID = trr.TestPoitGroupID;

                                int? unitOfMeasureID = null;

                                if (DTO?.PieceOfEquipment != null)
                                {
                                    unitOfMeasureID = DTO.PieceOfEquipment.UnitOfMeasureID;
                                }

                                if (bc.Eccentricity.TestPoint.UnitOfMeasurementID == 0 && bc.Eccentricity.TestPoint.UnitOfMeasurementOutID > 0)
                                {
                                    bc.Eccentricity.TestPoint.UnitOfMeasurementID = bc.Eccentricity.TestPoint.UnitOfMeasurementOutID;
                                }

                                if (bc.Eccentricity.TestPoint.UnitOfMeasurementID == 0 && bc.Eccentricity.TestPoint.UnitOfMeasurementOutID == 0 && unitOfMeasureID.HasValue && unitOfMeasureID.Value > 0)
                                {
                                    bc.Eccentricity.TestPoint.UnitOfMeasurementID = unitOfMeasureID.Value;
                                    bc.Eccentricity.TestPoint.UnitOfMeasurementOutID = unitOfMeasureID.Value;

                                }
                                //var lllesta = lll.Where(x => x.TestPoitGroupID == trr.TestPoitGroupID)
                                //    .Select(x => x.TestPoints.Select(x => x.TestPointID == il.TestPointID)).FirstOrDefault();

                                var lllesta = lll.Where(x => x.TestPoitGroupID == trr.TestPoitGroupID).Where(x => x.TestPoints.Any(x => x.TestPointID == bc.Eccentricity.TestPointID)).FirstOrDefault();
                                //.Select(x => x.TestPoints.Select(x => x.TestPointID == il.TestPointID)).FirstOrDefault();

                                if (lllesta == null)
                                {
                                    bc.Eccentricity.TestPoint.TestPointID = (int)NumericExtensions.GetUniqueID(bc.Eccentricity.TestPointID);
                                    bc.Eccentricity.TestPointID = bc.Eccentricity.TestPointID;
                                    context.TestPoint.Add(bc.Eccentricity.TestPoint);
                                }
                                else
                                {


                                    context.TestPoint.Update(bc.Eccentricity.TestPoint);
                                }


                                await Task.Delay(20);




                                if (saveline)
                                {
                                    var resul = await context.SaveChangesAsync();
                                }


                                bc.Eccentricity.TestPointID = bc.Eccentricity.TestPoint.TestPointID;
                            }


                            bc.Eccentricity.TestPoint = null;

                        }
                        line = 15;
                        if (bc != null && bc?.Repeatability != null)
                        {

                            bc.Repeatability.TestPoint = null;

                            if (bc.Repeatability.TestPoint != null)
                            {

                                //bc.Repeatability.TestPoint.UnitOfMeasurement = null;
                                //bc.Repeatability.TestPoint.UnitOfMeasurementOut = null;

                                if (bc.Repeatability.TestPoint != null)
                                {


                                    int? unitOfMeasureID = null;

                                    if (DTO?.PieceOfEquipment != null)
                                    {
                                        unitOfMeasureID = DTO.PieceOfEquipment.UnitOfMeasureID;
                                    }

                                    if (bc.Repeatability.TestPoint.UnitOfMeasurementID == 0 && bc.Repeatability.TestPoint.UnitOfMeasurementOutID > 0)
                                    {
                                        bc.Repeatability.TestPoint.UnitOfMeasurementID = bc.Repeatability.TestPoint.UnitOfMeasurementOutID;
                                    }

                                    if (bc.Repeatability.TestPoint.UnitOfMeasurementID == 0 && bc.Repeatability.TestPoint.UnitOfMeasurementOutID == 0 && unitOfMeasureID.HasValue && unitOfMeasureID.Value > 0)
                                    {
                                        bc.Repeatability.TestPoint.UnitOfMeasurementID = unitOfMeasureID.Value;
                                        bc.Repeatability.TestPoint.UnitOfMeasurementOutID = unitOfMeasureID.Value;

                                    }

                                    var testy = bc.Repeatability.TestPoint;

                                    testy.TestPointID = NumericExtensions.GetUniqueID(testy.TestPointID);
                                    context.TestPoint.Add(testy);

                                    await context.SaveChangesAsync();

                                    bc.Repeatability.TestPointID = testy.TestPointID;
                                }
                                bc.Repeatability.TestPoint = null;
                            }
                            else
                            {
                                bc.Repeatability.TestPointID = null;
                            }

                            bc.Repeatability.TestPoint = null;

                        }
                        line = 16;

                        if (bc is not null)
                        {
                            var bcds = await context.BalanceAndScaleCalibration.Where(x => x.WorkOrderDetailId == bc.WorkOrderDetailId && x.CalibrationTypeId == bc.CalibrationTypeId).AsNoTracking().FirstOrDefaultAsync();

                            if (bcds == null)
                            {
                                bc.Eccentricity = null;
                                bc.Linearities = null;
                                bc.Repeatability = null;
                                bc.Rockwells = null;
                                bc.WorkOrderDetail = null;
                                bc.TestPointResult = null;
                                bc.Tensions = null;
                                bc.GenericCalibration = null;

                                context.BalanceAndScaleCalibration.Add(bc);
                                line = 17;
                                var resu = await context.SaveChangesAsync();

                            }

                            line = 18;
                        }



                    }
                    catch (Exception ex)
                    {
//                        Console.WriteLine(ex?.InnerException?.Message);
//                        Console.WriteLine(ex?.StackTrace);
//                        Console.WriteLine("------------------------ error wod " + line);
                        throw ex;
                    }

                }
                else
                {

                    try
                    {
                        line = 2;


                        Eccentricity ecc = null;
                        Repeatability rep = null;
                        if (DTO?.BalanceAndScaleCalibration != null)
                        {
                            ecc = DTO.BalanceAndScaleCalibration.Eccentricity;
                            rep = DTO.BalanceAndScaleCalibration.Repeatability;
                        }


                        if (DTO?.BalanceAndScaleCalibration?.Linearities != null && DTO?.BalanceAndScaleCalibration?.Linearities?.Count > 0)
                        {
                            List<Linearity> lin = DTO.BalanceAndScaleCalibration.Linearities.ToList();
                            foreach (var item in lin)
                            {
                                if (item.CalibrationSubType_Weights != null)
                                {
                                    await SaveSubType(item.CalibrationSubTypeId);

                                    var sss = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == item.WorkOrderDetailId
                                     && x.SecuenceID == item.SequenceID && x.CalibrationSubTypeID == item.CalibrationSubTypeId).ToArrayAsync();

                                    if (sss != null && sss.Count() > 0)
                                    {
                                        context.CalibrationSubType_Weight.RemoveRange(sss);
                                        await context.SaveChangesAsync();
                                    }

                                    foreach (var we in item.CalibrationSubType_Weights)
                                    {
                                        //we.CalibrationSubType = c;
                                        context.CalibrationSubType_Weight.Add(we);
                                        await context.SaveChangesAsync();
                                    }

                                    item.CalibrationSubType_Weights = null;
                                }
                            }
                        }

                        //await SaveWODWeights(DTO);

                        //DTO.WOD_Weights = null;
                        line = 3;

                        if (rep != null && rep?.CalibrationSubType_Weights != null)
                        {
                            await SaveSubType(rep.CalibrationSubTypeId);


                            var sss = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == rep.WorkOrderDetailId
                                  && x.CalibrationSubTypeID == rep.CalibrationSubTypeId).ToArrayAsync();
                            if (sss != null && sss.Count() > 0)
                            {
                                context.CalibrationSubType_Weight.RemoveRange(sss);
                                await context.SaveChangesAsync();
                            }

                            foreach (var we in rep.CalibrationSubType_Weights)
                            {
                                context.CalibrationSubType_Weight.Add(we);
                                await context.SaveChangesAsync();
                            }
                            rep.CalibrationSubType_Weights = null;
                        }
                        line = 4;
                        if (ecc != null && ecc?.CalibrationSubType_Weights != null)
                        {
                            await SaveSubType(ecc.CalibrationSubTypeId);

                            var sss = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.WorkOrderDetailID == ecc.WorkOrderDetailId
                                  && x.CalibrationSubTypeID == ecc.CalibrationSubTypeId).ToArrayAsync();
                            if (sss != null && sss.Count() > 0)
                            {
                                context.CalibrationSubType_Weight.RemoveRange(sss);
                                await context.SaveChangesAsync();
                            }
                            foreach (var we in ecc.CalibrationSubType_Weights)
                            {
                                context.CalibrationSubType_Weight.Add(we);
                                await context.SaveChangesAsync();
                            }
                            ecc.CalibrationSubType_Weights = null;
                        }
                        line = 5;
                        //context.Entry(DTO).State = EntityState.Unchanged;
                        //context.Entry(DTO).State = EntityState.Detached;
                        BalanceAndScaleCalibration bc = new BalanceAndScaleCalibration();
                        if (DTO?.BalanceAndScaleCalibration != null)
                        {
                            DTO.BalanceAndScaleCalibration.Repeatability = null;
                            DTO.BalanceAndScaleCalibration.Eccentricity = null;
                        }

                        bc = DTO.BalanceAndScaleCalibration;
                        ////////////////////////////////////////////////////////////////////////////////
                        ///yp 11142024
                        if (DTO?.TestGroups != null && DTO?.TestGroups?.Count > 0)
                        {
                            var trr = DTO.TestGroups.ElementAtOrDefault(0);

                            var tepin = await context.TestPointGroup.AsNoTracking()
                            .Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID && x.TestPoitGroupID == trr.TestPoitGroupID).FirstOrDefaultAsync();



                            trr.UnitOfMeasurementOut = null;
                            trr.OutUnitOfMeasurement = null;
                            trr.TestPoints = null;

                            if (tepin == null)
                            {
                                trr.TestPoitGroupID = NumericExtensions.GetUniqueID(trr.TestPoitGroupID);
                                context.TestPointGroup.Add(trr);
                            }
                            else
                            {
                                context.TestPointGroup.Update(trr);
                            }

                            await context.SaveChangesAsync();

                            //TODO delete other tpg
                            var tepin2 = await context.TestPointGroup.AsNoTracking()
                            .Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).ToListAsync();

                            //foreach (var testpg in tepin2)
                            //{

                            //    if (testpg.TestPoitGroupID != trr.TestPoitGroupID)
                            //    {

                            //        var testpoi = await context.TestPoint.AsNoTracking()
                            //.Where(x => x.TestPointGroupTestPoitGroupID == testpg.TestPoitGroupID).ToListAsync();

                            //        if (testpoi.Count > 0)
                            //        {
                            //            foreach (var tep in testpoi)
                            //            {
                            //                var linear = await context.Linearity.AsNoTracking().Where(x => x.TestPointID == tep.TestPointID).FirstOrDefaultAsync();
                            //                if (linear != null)
                            //                {
                            //                    context.Linearity.RemoveRange(linear);
                            //                    await context.SaveChangesAsync();
                            //                }
                            //                tep.UnitOfMeasurement = null;
                            //                tep.UnitOfMeasurementOut = null;

                            //            }



                            //            context.TestPoint.RemoveRange(testpoi);
                            //            await context.SaveChangesAsync();

                            //        }

                            //        context.TestPointGroup.Remove(testpg);

                            //        await context.SaveChangesAsync();

                            //    }


                            //}

                            /////////////////7
                            // Use execution strategy to handle retries instead of user-initiated transaction
                            var strategy = context.Database.CreateExecutionStrategy();
                            await strategy.ExecuteAsync(async () =>
                            {
                                using var transaction = await context.Database.BeginTransactionAsync();
                                try
                                {
                                    foreach (var testpg in tepin2)
                                    {
                                        if (testpg.TestPoitGroupID != trr.TestPoitGroupID)
                                        {
                                            var testpoi = await context.TestPoint
                                                .Where(x => x.TestPointGroupTestPoitGroupID == testpg.TestPoitGroupID)
                                                .ToListAsync();

                                            if (testpoi.Count > 0)
                                            {
                                                var testPointIds = testpoi.Select(t => t.TestPointID).ToList();

                                                // Delete dependent entities first to avoid foreign key constraint violations

                                                // 1. Delete Eccentricity records that reference TestPoints
                                                var eccList = await context.Eccentricity
                                                    .Where(e => testPointIds.Contains((int)e.TestPointID))
                                                    .ToListAsync();

                                                if (eccList?.Count > 0)
                                                {
                                                    context.Eccentricity.RemoveRange(eccList);
                                                    await context.SaveChangesAsync();
                                                }

                                                // 2. Delete Linearity records that reference TestPoints
                                                var linearList = await context.Linearity
                                                    .Where(l => testPointIds.Contains((int)l.TestPointID))
                                                    .ToListAsync();

                                                if (linearList.Count > 0)
                                                {
                                                    context.Linearity.RemoveRange(linearList);
                                                    await context.SaveChangesAsync();
                                                }

                                                // 3. Delete Repeatability records that reference TestPoints
                                                var repeatabilityList = await context.Repeatability
                                                    .Where(r => testPointIds.Contains((int)r.TestPointID))
                                                    .ToListAsync();

                                                if (repeatabilityList.Count > 0)
                                                {
                                                    context.Repeatability.RemoveRange(repeatabilityList);
                                                    await context.SaveChangesAsync();
                                                }

                                                // 4. Delete WOD_TestPoint records that reference TestPoints
                                                var wodTestPointList = await context.WOD_TestPoint
                                                    .Where(wtp => testPointIds.Contains(wtp.TestPointID))
                                                    .ToListAsync();

                                                if (wodTestPointList.Count > 0)
                                                {
                                                    context.WOD_TestPoint.RemoveRange(wodTestPointList);
                                                    await context.SaveChangesAsync();
                                                }

                                                // 5. Clear navigation properties and delete TestPoints
                                                foreach (var tep in testpoi)
                                                {
                                                    tep.UnitOfMeasurement = null;
                                                    tep.UnitOfMeasurementOut = null;
                                                }

                                                context.TestPoint.RemoveRange(testpoi);
                                                await context.SaveChangesAsync();
                                            }

                                            // 6. Finally delete the TestPointGroup
                                            context.TestPointGroup.Remove(testpg);
                                            await context.SaveChangesAsync();
                                        }
                                    }

                                    await transaction.CommitAsync();
                                }
                                catch (Exception)
                                {
                                    await transaction.RollbackAsync();
                                    throw;
                                }
                            });

                            ////////////////////////////////////////



                            if (bc != null)
                            {

                                var be = await context.BalanceAndScaleCalibration.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                                if (be?.Count > 1)
                                {
                                    context.BalanceAndScaleCalibration.RemoveRange(be);
                                    context.BalanceAndScaleCalibration.Add(be.First());

                                }
                                else if (be?.Count == 0)
                                {
                                    BalanceAndScaleCalibration bb = new BalanceAndScaleCalibration();

                                    bb.WorkOrderDetailId = DTO.WorkOrderDetailID;
                                    bb.CalibrationTypeId = DTO.BalanceAndScaleCalibration.CalibrationTypeId;

                                    context.BalanceAndScaleCalibration.Add(bb);
                                }

                                await context.SaveChangesAsync();


                                if (bc != null && bc.Linearities != null)
                                {
                                    line = 6;




                                    DTO.TestGroups = null;


                                    var lines = await context.Linearity.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                                    var resultlines = lines.Where(p => !bc.Linearities.Any(p2 => p2.SequenceID == p.SequenceID
                                    && p2.CalibrationSubTypeId == p.CalibrationSubTypeId
                                    && p2.WorkOrderDetailId == p.WorkOrderDetailId)).ToList();

                                    var bcrt = await context.BasicCalibrationResult.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                                    foreach (var ity in resultlines)
                                    {


                                        context.Linearity.Remove(ity);
                                        await context.SaveChangesAsync();

                                        var bcr1 = bcrt.Where(x => x.SequenceID == ity.SequenceID && x.CalibrationSubTypeId == ity.CalibrationSubTypeId).FirstOrDefault();

                                        if (bcr1 != null)
                                        {
                                            //ity.BasicCalibrationResult.Linearity = null;
                                            context.BasicCalibrationResult.Remove(bcr1);
                                            await context.SaveChangesAsync();

                                        }
                                    }

                                    foreach (var item in bc.Linearities)
                                    {

                                        item.TestPoint.UnitOfMeasurement = null;
                                        item.TestPoint.UnitOfMeasurementOut = null;
                                        item.TestPoint.TestPointGroupTestPoitGroupID = trr.TestPoitGroupID;

                                        var lll = await context.TestPoint.AsNoTracking().Where(x => x.TestPointID == item.TestPointID).FirstOrDefaultAsync();

                                        if (lll == null)
                                        {
                                            item.TestPoint.TestPointID = NumericExtensions.GetUniqueID(item.TestPoint.TestPointID);
                                            context.TestPoint.Add(item.TestPoint);

                                        }
                                        else
                                        {
                                            context.TestPoint.Update(item.TestPoint);
                                        }


                                        var resul = await context.SaveChangesAsync();

                                        item.TestPointID = item.TestPoint.TestPointID;

                                        item.TestPoint = null;

                                        item.UnitOfMeasure = null;
                                        item.CalibrationUncertaintyValueUncertaintyUnitOfMeasure = null;
                                        //context.Entry(item).State = EntityState.Modified;
                                        item.BasicCalibrationResult.Linearity = null;
                                        //item.BasicCalibrationResult.UnitOfMeasureID =
                                        item.BasicCalibrationResult.UnitOfMeasure = null;

                                        var bcr = await context.BasicCalibrationResult.AsNoTracking().Where(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId
                                          && x.WorkOrderDetailId == item.WorkOrderDetailId
                                          && x.SequenceID == item.SequenceID).FirstOrDefaultAsync();




                                        if (bcr == null)
                                        {
                                            context.BasicCalibrationResult.Add(item.BasicCalibrationResult);

                                        }
                                        else
                                        {
                                            context.BasicCalibrationResult.Update(item.BasicCalibrationResult);
                                        }

                                        await context.SaveChangesAsync();

                                        item.BasicCalibrationResult = null;

                                        var linear = await context.Linearity.AsNoTracking().Where(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId
                                        && x.WorkOrderDetailId == item.WorkOrderDetailId && x.SequenceID == item.SequenceID).FirstOrDefaultAsync();




                                        if (linear == null)
                                        {
                                            context.Linearity.Add(item);
                                        }
                                        else
                                        {
                                            context.Linearity.Update(item);
                                        }


                                        var rp = await context.SaveChangesAsync();

                                        if (rp == 0)
                                        {
                                            throw new Exception("Linearity no update");
                                        }



                                    }
                                }

                            }



                            if(bc != null && bc.Linearities != null)
                            {
                                bc.Linearities = null;
                            }

                           

                            if (rep != null && bc != null)
                            {
                                bc.Repeatability = rep;
                            }



                            ///repeat//////////////////////////////////////////////////////////////////////////////

                            if (bc != null && bc?.Repeatability != null && bc.Repeatability?.TestPointResult != null
                                && bc.Repeatability?.TestPointResult?.Count > 0)
                            //&& bc?.Repeatability?.TestPointResult.Count > 0)
                            {
                                line = 7;
                                var bcr1 = await context.BasicCalibrationResult.AsNoTracking().Where(x => x.CalibrationSubTypeId
                              == new Repeatability().CalibrationSubTypeId
                             && x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                                if (bc?.Repeatability?.TestPointResult != null && bc?.Repeatability?.TestPointResult?.Count > 0)
                                {

                                    //var elem1 = bc.Repeatability.TestPointResult.ElementAtOrDefault(0);



                                    foreach (var item in bc?.Repeatability?.TestPointResult)
                                    {

                                        var bcr = bcr1.Where(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId
                                    && x.WorkOrderDetailId == item.WorkOrderDetailId
                                    && x.SequenceID == item.SequenceID).FirstOrDefault();

                                        item.WorkOrderDetailId = DTO.WorkOrderDetailID;

                                        if (bcr == null)
                                        {
                                            context.BasicCalibrationResult.Add(item);

                                        }
                                        else
                                        {
                                            context.BasicCalibrationResult.Update(item);
                                        }

                                        await context.SaveChangesAsync();
                                    }

                                }


                                foreach (var item in bcr1)
                                {
                                    BasicCalibrationResult bcr = null;
                                    if (bc?.Repeatability?.TestPointResult != null)
                                    {
                                        bcr = bc?.Repeatability?.TestPointResult.Where(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId
                                    && x.WorkOrderDetailId == item.WorkOrderDetailId
                                    && x.SequenceID == item.SequenceID).FirstOrDefault();

                                    }

                                    if (bcr == null)
                                    {
                                        context.BasicCalibrationResult.Remove(item);
                                        await context.SaveChangesAsync();
                                    }

                                }


                                //TODO: Comment on test added by JP
                                bc.Repeatability.TestPointResult = null;


                                bc.Repeatability.WorkOrderDetailId = DTO.WorkOrderDetailID;
                                bc.Repeatability.TestPoint = null;
                                ///Added by JP
                                if (bc?.Repeatability?.TestPoint != null)
                                {

                                    var testpo = await context.TestPoint.AsNoTracking().Where(x => bc.Repeatability.TestPointID.HasValue && x.TestPointID == bc.Repeatability.TestPointID.Value).FirstOrDefaultAsync();

                                    bc.Repeatability.TestPoint.TestPointID = bc.Repeatability.TestPointID.Value;
                                    bc.Repeatability.TestPoint.UnitOfMeasurement = null;
                                    bc.Repeatability.TestPoint.UnitOfMeasurementOut = null;
                                    bc.Repeatability.TestPoint.TestPointGroup = null;
                                    bc.Repeatability.TestPoint.WOD_TestPoints = null;

                                    int? unitOfMeasureID = null;

                                    if (DTO?.PieceOfEquipment != null)
                                    {
                                        unitOfMeasureID = DTO.PieceOfEquipment.UnitOfMeasureID;
                                    }

                                    if (bc.Repeatability.TestPoint.UnitOfMeasurementID == 0 && bc.Repeatability.TestPoint.UnitOfMeasurementOutID > 0)
                                    {
                                        bc.Repeatability.TestPoint.UnitOfMeasurementID = bc.Repeatability.TestPoint.UnitOfMeasurementOutID;
                                    }

                                    if (bc.Repeatability.TestPoint.UnitOfMeasurementID == 0 && bc.Repeatability.TestPoint.UnitOfMeasurementOutID == 0 && unitOfMeasureID.HasValue && unitOfMeasureID.Value > 0)
                                    {
                                        bc.Repeatability.TestPoint.UnitOfMeasurementID = unitOfMeasureID.Value;
                                        bc.Repeatability.TestPoint.UnitOfMeasurementOutID = unitOfMeasureID.Value;

                                    }

                                    if (testpo == null || !bc.Repeatability.TestPointID.HasValue)
                                    {
                                        bc.Repeatability.TestPoint.TestPointID = NumericExtensions.GetUniqueID(bc.Repeatability.TestPoint.TestPointID);
                                        context.TestPoint.Add(bc.Repeatability.TestPoint);

                                    }
                                    else
                                    {
                                        context.TestPoint.Update(bc.Repeatability.TestPoint);
                                    }
                                    await context.SaveChangesAsync();
                                    bc.Repeatability.TestPointID = bc.Repeatability.TestPoint.TestPointID;
                                }
                                // End Added


                                bc.Repeatability.TestPoint = null;
                                bc.Repeatability.TestPointID = null;
                                //bc.Repeatability.TestPoint.UnitOfMeasurement = null;
                                //bc.Repeatability.TestPoint.UnitOfMeasurementOut = null;

                                var r = await context.Repeatability.AsNoTracking()
                              .Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID && x.CalibrationSubTypeId == rep.CalibrationSubTypeId).FirstOrDefaultAsync();
                                if (r == null)
                                {
                                    context.Repeatability.Add(bc.Repeatability);
                                }
                                else
                                {
                                    context.Repeatability.Update(bc.Repeatability);
                                }

                                await context.SaveChangesAsync();
                            }
                            //////////////////////////////////////////////////////////////////

                            //Eccentricity/////////////////////////////////////////////////////////////////
                            if (bc != null)
                            {
                                bc.Eccentricity = ecc;
                            }

                            if (bc?.Eccentricity != null && bc?.Eccentricity != null && bc.Eccentricity?.TestPointResult != null
                                && bc.Eccentricity?.TestPointResult?.Count > 0)
                            {
                                line = 8;

                                var bcr1 = await context.BasicCalibrationResult.AsNoTracking().Where(x => x.CalibrationSubTypeId
                              == new Eccentricity().CalibrationSubTypeId
                             && x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();


                                if (bc?.Eccentricity?.TestPointResult != null && bc?.Eccentricity?.TestPointResult?.Count > 0)
                                {
                                    var elem1 = bc.Eccentricity.TestPointResult.ElementAtOrDefault(0);


                                    foreach (var item in bc?.Eccentricity?.TestPointResult)
                                    {
                                        var bcr = bcr1.Where(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId
                                    && x.WorkOrderDetailId == item.WorkOrderDetailId
                                    && x.SequenceID == item.SequenceID).FirstOrDefault();


                                        if (bcr == null)
                                        {
                                            context.BasicCalibrationResult.Add(item);

                                        }
                                        else
                                        {
                                            context.BasicCalibrationResult.Update(item);
                                        }

                                        await context.SaveChangesAsync();
                                    }
                                }

                                //Eccentricity


                                foreach (var item in bcr1)
                                {
                                    BasicCalibrationResult bcr = null;
                                    if (bc?.Eccentricity?.TestPointResult != null)
                                    {
                                        bcr = bc?.Eccentricity?.TestPointResult.Where(x => x.CalibrationSubTypeId == item.CalibrationSubTypeId
                                   && x.WorkOrderDetailId == item.WorkOrderDetailId
                                   && x.SequenceID == item.SequenceID).FirstOrDefault();

                                        if (bcr != null)
                                        {

                                        }
                                    }

                                    if (bcr == null)
                                    {
                                        context.BasicCalibrationResult.Remove(item);
                                        await context.SaveChangesAsync();
                                    }

                                }

                                //bc.Eccentricity.TestPointResult = null;


                                if (bc.Eccentricity?.TestPoint != null || (bc.Eccentricity?.TestPoint == null && bc.Eccentricity?.TestPointResult != null && bc.Eccentricity?.TestPointResult.Count() > 0))
                                {

                                    bc.Eccentricity.TestPointResult = null;
                                    var testpo = await context.TestPoint.AsNoTracking().Where(x => bc.Eccentricity.TestPointID.HasValue && x.TestPointID == bc.Eccentricity.TestPointID.Value).FirstOrDefaultAsync();

                                    if (bc.Eccentricity?.TestPoint == null)
                                    {
                                        bc.Eccentricity.TestPoint = new TestPoint();
                                    }

                                    int? unitOfMeasureID=null;

                                    if (DTO?.PieceOfEquipment != null)
                                    {
                                        unitOfMeasureID = DTO.PieceOfEquipment.UnitOfMeasureID;
                                    }
                                     

                                    if (bc.Eccentricity.TestPoint.UnitOfMeasurementID == 0 && bc.Eccentricity.TestPoint.UnitOfMeasurementOutID > 0)
                                    {
                                        bc.Eccentricity.TestPoint.UnitOfMeasurementID = bc.Eccentricity.TestPoint.UnitOfMeasurementOutID;
                                    }

                                    if (bc.Eccentricity.TestPoint.UnitOfMeasurementID == 0 && bc.Eccentricity.TestPoint.UnitOfMeasurementOutID == 0 && unitOfMeasureID.HasValue && unitOfMeasureID.Value > 0)
                                    {
                                        bc.Eccentricity.TestPoint.UnitOfMeasurementID = unitOfMeasureID.Value;
                                        bc.Eccentricity.TestPoint.UnitOfMeasurementOutID = unitOfMeasureID.Value;

                                    }

                                    bc.Eccentricity.TestPoint.UnitOfMeasurement = null;
                                    bc.Eccentricity.TestPoint.UnitOfMeasurementOut = null;
                                    bc.Eccentricity.TestPoint.TestPointGroup = null;
                                    bc.Eccentricity.TestPoint.WOD_TestPoints = null;
                                    bc.Eccentricity.TestPoint.TestPointGroupTestPoitGroupID = trr.TestPoitGroupID;
                                    bc.Eccentricity.TestPoint.Repeatabilities = null;
                                    bc.Eccentricity.TestPoint.Eccentricities = null;

                                    if (testpo == null || bc.Eccentricity.TestPoint.TestPointID == null || bc.Eccentricity.TestPoint.TestPointID == 0)
                                    {
                                        bc.Eccentricity.TestPoint.TestPointID = NumericExtensions.GetUniqueID(bc.Eccentricity.TestPoint.TestPointID);



                                        context.TestPoint.Add(bc.Eccentricity.TestPoint);

                                    }
                                    else
                                    {
                                        context.TestPoint.Update(bc.Eccentricity.TestPoint);
                                    }
                                    await context.SaveChangesAsync();
                                    bc.Eccentricity.TestPointID = bc.Eccentricity.TestPoint.TestPointID;
                                }


                                bc.Eccentricity.TestPoint = null;

                                bc.Eccentricity.WorkOrderDetailId = DTO.WorkOrderDetailID;

                                var e = await context.Eccentricity.AsNoTracking()
                               .Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID && x.CalibrationSubTypeId == ecc.CalibrationSubTypeId).FirstOrDefaultAsync();
                                if (e == null)
                                {
                                    context.Eccentricity.Add(bc.Eccentricity);

                                }
                                else
                                {
                                    context.Eccentricity.Update(bc.Eccentricity);

                                }

                                await context.SaveChangesAsync();




                            }


                            ////////////////////////////////////////////////////////////////

                            ///////////////////////////////////////////////////////Ranges
                            ///
                            if (DTO?.Ranges != null)
                            {

                                // Convert to List to ensure it's mutable and avoid "Collection was of a fixed size" error
                                var rangesList = DTO.Ranges.ToList();

                                foreach (var rt in rangesList)
                                {
                                    var rrr = await context.RangeTolerance.AsNoTracking().Where(x => x.RangeToleranceID == rt.RangeToleranceID).FirstOrDefaultAsync();

                                    if (rrr == null)
                                    {
                                        context.RangeTolerance.Add(rt);
                                    }
                                    else
                                    {
                                        context.RangeTolerance.Update(rt);
                                    }

                                    await context.SaveChangesAsync();

                                }

                            }

                            var bas2 = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID)
                 .Include(x => x.BalanceAndScaleCalibration).AsNoTracking().FirstOrDefaultAsync();

                            if (bas2.BalanceAndScaleCalibration == null)
                            {

                                if (bc is not null)
                                {
                                    var bcds = await context.BalanceAndScaleCalibration.Where(x => x.WorkOrderDetailId == bc.WorkOrderDetailId && x.CalibrationTypeId == bc.CalibrationTypeId).AsNoTracking().FirstOrDefaultAsync();

                                    if (bcds == null)
                                    {
                                        bc.Eccentricity = null;
                                        bc.Linearities = null;
                                        bc.Repeatability = null;
                                        bc.Rockwells = null;
                                        bc.WorkOrderDetail = null;
                                        bc.TestPointResult = null;
                                        bc.Tensions = null;
                                        bc.GenericCalibration = null;


                                        context.BalanceAndScaleCalibration.Add(bc);
                                        line = 17;
                                        var resu22 = await context.SaveChangesAsync();
                                    }
                                    line = 18;
                                }

                            }


                            ///

                            var resu = await context.SaveChangesAsync();

                            if (resu > 0 && DTO.CurrentStatusID > 2)
                            {

                            }
                        }

                        ///////////////////////////////////////////////////////////////////////////////
                    }
                    catch (Exception ex)
                    {

//                        Console.WriteLine("-----------------chanstatus error:" + ex.Message + " " + line.ToString() + " " + DTO.WorkOrderDetailID);
//                        Console.WriteLine(ex?.InnerException?.Message);
//                        Console.WriteLine(ex?.StackTrace);
                        throw ex;
                    }



                }

                if (DTO?.BalanceAndScaleCalibration?.Forces != null && DTO?.BalanceAndScaleCalibration?.Forces?.Count > 0)
                {
                    await SaveForces(DTO.WorkOrderDetailID, DTO.BalanceAndScaleCalibration.Forces);
                }
            }
            if (DTO?.BalanceAndScaleCalibration?.Rockwells != null && DTO?.BalanceAndScaleCalibration?.Rockwells?.Count > 0)
            {
                await SaveCalibrationTypes<Rockwell, RockwellResult>(DTO.WorkOrderDetailID, DTO.BalanceAndScaleCalibration.Rockwells, FormatRocwell);
            }

            if (DTO?.BalanceAndScaleCalibration?.GenericCalibration != null && DTO?.BalanceAndScaleCalibration?.GenericCalibration?.Count > 0)
            {
                await SaveCalibrationTypes<GenericCalibration, GenericCalibrationResult>(DTO.WorkOrderDetailID, DTO.BalanceAndScaleCalibration.GenericCalibration, FormatGenericCalibration);

                //foreach (var itemcal in DTO.BalanceAndScaleCalibration.GenericCalibration)
                //{
                //await SaveItemsWeights(DTO.BalanceAndScaleCalibration.GenericCalibration as List<ICalibrationSubType>);
                //}
            }
            /////////ERROR YENNY
            //int subType = 0;
            //if (DTO?.BalanceAndScaleCalibration?.TestPointResult != null)
            //    subType = DTO.BalanceAndScaleCalibration.TestPointResult.FirstOrDefault().CalibrationSubTypeId;

            if (DTO?.BalanceAndScaleCalibration?.TestPointResult != null && DTO?.BalanceAndScaleCalibration?.TestPointResult?.Count() > 0)/*&& subType >= 507 && subType <= 518)  TODO YP*/
            {
                // Convert to List to ensure it's mutable and avoid "Collection was of a fixed size" error
                var testPointResultList = DTO.BalanceAndScaleCalibration.TestPointResult.ToList();

                foreach (var item in testPointResultList)
                {
                    item.Updated = DateTime.Now.Ticks;
                }
            }

            BalanceAndScaleCalibration bcss = new BalanceAndScaleCalibration();

            if (DTO?.BalanceAndScaleCalibration?.TestPointResult != null && DTO?.BalanceAndScaleCalibration?.TestPointResult?.Count > 0)
            {

                var groupedCalibrations = DTO.BalanceAndScaleCalibration.TestPointResult
                    .GroupBy(item => item.CalibrationSubTypeId)
                    .Where(group => group.Any(item => item.Updated != 0))
                    .SelectMany(group => group)
                    .ToList();


                ICollection<GenericCalibrationResult2> genericCalibrationResult2s = new List<GenericCalibrationResult2>();

                foreach (var item in groupedCalibrations)
                {
                    genericCalibrationResult2s.Add(item);
                }

                if (genericCalibrationResult2s.Count == 0)
                {
                    var fas = DTO.BalanceAndScaleCalibration.TestPointResult
                   .Where(x => x.GenericCalibration2 != null && ((x.GenericCalibration2.WeightSets != null && x.GenericCalibration2.WeightSets.Count > 0)
                   || (x.GenericCalibration2.Standards != null && x.GenericCalibration2.Standards.Count > 0))).FirstOrDefault();

                    if (fas != null)
                    {
                        genericCalibrationResult2s = DTO.BalanceAndScaleCalibration.TestPointResult;
                    }
                }
               

                bcss =(BalanceAndScaleCalibration) DTO.BalanceAndScaleCalibration.CloneObject();

                bcss.TestPointResult = null;
                bcss.Tensions = null;
                bcss.WorkOrderDetail = null;

                if(DTO.CurrentStatusID > 1)
                {
                    var selectedValues = genericCalibrationResult2s.DistinctBy(x => x.GroupName).Where(x => x.GroupName != "TestPointResult");
                    List<string> lss = new List<string>();

                    foreach (var sty in selectedValues)
                    {
                        lss.Add(sty.GroupName);
                    }
                    List<string> lss1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(DTO.JsonTestPointsGroups ?? "[]");

                    if (lss.Count > lss1.Count)
                    {
                        DTO.JsonTestPointsGroups = Newtonsoft.Json.JsonConvert.SerializeObject(lss);
                    }

                }



                //DTO.JsonTestPointsGroups = System.Text.Json.JsonSerializer.Serialize(lss);

                //await SaveCalibrationTypes2<GenericCalibration2, GenericCalibrationResult2>(Component, DTO.WorkOrderDetailID.ToString(), DTO.BalanceAndScaleCalibration.GenericCalibration2, FormatGenericCalibration2);


                



                await SaveCalibrationTypes2<GenericCalibration2, GenericCalibrationResult2>(Component, DTO.WorkOrderDetailID.ToString(), genericCalibrationResult2s, FormatGenericCalibration2, bcss);

               
            }

   
            if (DTO?.EnviromentCondition != null && DTO?.EnviromentCondition?.Count > 0)
            {
                await SaveExternalConditions(DTO.WorkOrderDetailID, DTO.EnviromentCondition);
            }
            //return DTO;


            DTO.WorkOder = null;
            DTO.CurrentStatus = null;
            DTO.PreviusStatus = null;
            DTO.PieceOfEquipment = null;
            //DTO.BalanceAndScaleCalibration = null;
            DTO.Technician = null;
            DTO.TestGroups = null;



            try
            {
                if (DTO?.EquipmentCondition != null && DTO?.EquipmentCondition?.Count > 0)
                {

                    var ecs = await context.EquipmentCondition.AsNoTracking().Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToListAsync();

                    // Convert to List to ensure it's mutable and avoid "Collection was of a fixed size" error
                    var equipmentConditionList = DTO.EquipmentCondition.ToList();

                    foreach (var item in equipmentConditionList)
                    {
                        var ecs1 = ecs.Where(x => x.EquipmentConditionId == item.EquipmentConditionId).FirstOrDefault();


                        item.WorkOrderDetailId = DTO.WorkOrderDetailID;

                        if (ecs1 == null)
                        {


                            item.EquipmentConditionId = NumericExtensions.GetUniqueID(item.EquipmentConditionId);



                            context.EquipmentCondition.Add(item);
                        }
                        else
                        {
                            context.EquipmentCondition.Update(item);
                        }

                        await context.SaveChangesAsync();
                    }


                }

                DTO.NullableCollections = true;

                //context.Entry(DTO).State = EntityState.Detached;

                var local = context.Set<CalibrationSubType>()
                .Local
                .FirstOrDefault(entry => entry.CalibrationSubTypeId.Equals(1));

                if (local != null)
                {
                    // detach
                    context.Entry(local).State = EntityState.Detached;
                }


                var local2 = context.Set<WorkOrderDetail>()
               .Local
               .FirstOrDefault(entry => entry.WorkOrderDetailID.Equals(DTO.WorkOrderDetailID));

                if (local2 != null)
                {
                    // detach
                    context.Entry(local2).State = EntityState.Detached;
                }

                // set Modified flag in your entry
                context.Entry(DTO).State = EntityState.Modified;

                // save 
                //_context.SaveChanges();

                //context.WorkOrderDetail.Update(DTO);

                await context.SaveChangesAsync();


                //DTO.NullableCollections = false;

                if (DTO?.CurrentStatusID == 4)
                {
                    var tech = context.User.Where(x => x.UserID == DTO.TechnicianID).FirstOrDefault();
                    var techname = tech.Name + " " + tech.LastName;// DTO.Technician.Name + " " + DTO.Technician.LastName;

                    var poe = await context.PieceOfEquipment.AsNoTracking().Where(x => x.PieceOfEquipmentID == DTO.PieceOfEquipmentId).FirstOrDefaultAsync();

                    var tc = "";

                    if (!string.IsNullOrEmpty(DTO.TechnicianComment))
                    {
                        tc = DateTime.Now.ToShortDateString().Replace("/", "-") + " " + techname + " - ";//+  we?.Technician?.Email + " " +  + Environment.NewLine;
                        var stc = "";// + Environment.NewLine;

                        if (!DTO.TechnicianComment.Contains(tc))
                        {
                            tc = stc + tc + DTO.TechnicianComment + Environment.NewLine
                                + " ____________________________ ";
                            //DTO.TechnicianComment = tc.Trim();
                        }
                    }


                    if (DTO?.CurrentStatusID == 4 && !string.IsNullOrEmpty(poe.Notes))
                    {


                        if (!string.IsNullOrEmpty(DTO.TechnicianComment) && !poe.Notes.Trim().Contains(DTO.TechnicianComment.Trim()))
                        {
                            poe.Notes = poe.Notes.Trim() + Environment.NewLine + tc.Trim(); // DTO.TechnicianComment.Trim();

                        }

                    }
                    else if (DTO?.CurrentStatusID == 4)
                    {
                        poe.Notes = tc.Trim() + Environment.NewLine;
                    }
                    if (DTO.CalibrationCustomDueDate.HasValue)
                    {
                        poe.DueDate = DTO.CalibrationCustomDueDate.Value;
                    }

                    context.PieceOfEquipment.Update(poe);
                    await context.SaveChangesAsync();
                }

                var arrls = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderID == DTO.WorkOrderID).ToListAsync();

                if(arrls.Count== arrls.Where(x => x.CurrentStatusID == 4).Count())
                {
                    var wotemp = await context.WorkOrder.AsNoTracking().Where(x => x.WorkOrderId == DTO.WorkOrderID).FirstOrDefaultAsync();

                    wotemp.StatusID = 3;

                    context.Update(wotemp);

                    await context.SaveChangesAsync();
                }


                await SaveOffline(DbFactory);

                /////////
                /// GET Contributors tto Calculate Uncertainy
                // 
                if (DTO?.BalanceAndScaleCalibration?.TestPointResult != null && DTO?.BalanceAndScaleCalibration?.TestPointResult?.Count > 0)
                {
                    
                    UncertaintyLogicDynamic uc = new UncertaintyLogicDynamic();
             
                    var dtoWeights = await GetByID(DTO);

                    if (dtoWeights?.WOD_Weights?.Count() > 0)
                    {
                        foreach (var item in dtoWeights.WOD_Weights)
                        {
                           
                            if (item.WeightSet != null)
                            {
                                var re = item.WeightSet;
                              
                                if (!string.IsNullOrEmpty(re.PieceOfEquipmentID))
                                {
                                    PieceOfEquipment poews = await context.PieceOfEquipment
                                                                 .AsNoTracking()
                                                                 .FirstOrDefaultAsync(x => x.PieceOfEquipmentID == re.PieceOfEquipmentID);
                                    re.PieceOfEquipment = poews;

                                    var uncertainties = await context.Uncertainty.AsNoTracking().Where(x => !string.IsNullOrEmpty(x.PieceOfEquipmentID) && x.PieceOfEquipmentID == re.PieceOfEquipmentID).ToListAsync();


                                    foreach (var uo in uncertainties)
                                    {
                                        UnitOfMeasure uom = new UnitOfMeasure
                                        {
                                            UnitOfMeasureID = uo.UnitOfMeasureID
                                        };

                                        var uom_ = await context.UnitOfMeasure.AsNoTracking().Where(x => x.UnitOfMeasureID == uo.UnitOfMeasureID).FirstOrDefaultAsync();

                                        uo.UnitOfMeasure = uom_;
                                    }

                                    re.PieceOfEquipment.Uncertainty = uncertainties;
                                }
                                  
                            }

                        }
                    }
                    var uncert = uc.GetContributtors(dtoWeights);

                    
                    DTO = dtoWeights;

                    var testPointResultList = DTO.TestPointResult.ToList();
                    var testPointUpdate = testPointResultList;
                    
                    foreach (var item in testPointUpdate)
                    {
                        try
                        {
                            item.GenericCalibration2 = null;
                            context.Update(item);
                            await context.SaveChangesAsync();
                        }
                        catch
                        {

                        }

                    }


                }
                /////////
                /////


                return DTO;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DTO.NullableCollections = false;
            }



        }





        /// <summary>
        /// YPPP
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        public async Task<WorkDetailHistory> ChangeStatusComplete(WorkDetailHistory DTO)
        {


            await using var context = await DbFactory.CreateDbContextAsync();


            var a = await context.WorkDetailHistory.Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).OrderByDescending(a => a.Date).FirstOrDefaultAsync();
            context.WorkDetailHistory.Update(DTO);
            await context.SaveChangesAsync();


            return DTO;
        }

        public async Task<bool> SaveSubType(int Id, int CalibrationType = 1)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var cst = await context.CalibrationSubType.AsNoTracking().Where(x => x.CalibrationSubTypeId == Id).FirstOrDefaultAsync();
            if (cst == null)
            {
                CalibrationSubType c = new CalibrationSubType();
                c.CalibrationSubTypeId = Id;
                c.CalibrationTypeId = CalibrationType;
                context.CalibrationSubType.Add(c);
                await context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<Status>> GetStatus()
        {
            if(DbFactory == null)
            {
                return null;
            }   

            await using var context = await DbFactory.CreateDbContextAsync();

            var a = await context.Status.AsNoTracking().ToListAsync();

            return a;
        }


        public async Task<Status> SaveStatus(Status sta)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            context.Status.Add(sta);

            await context.SaveChangesAsync();

            return sta;

        }

        //public WorkOrderDetailRepositoryEF(CalibrationSaaSDBContext context)
        //{
        //    this.context = context;
        //}

        public async Task<WorkOrderDetail> GetWorkOrderDetailByID(int workOrderDetailId, bool Isgeneric = false, int? calibrationtype=null)
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            WorkOrderDetail wod = new WorkOrderDetail();
            wod.WorkOrderDetailID = workOrderDetailId;

            var a = await context.WorkOrderDetail.AsNoTracking()
            //.Include(x=>x.TestCode)
            //.Where(Querys.WorkOrderDetailByID(workOrderDetailId)).FirstOrDefaultAsync();
            .Where(x => x.WorkOrderDetailID == workOrderDetailId).FirstOrDefaultAsync();

            //var bal = await context.Set<BalanceAndScaleCalibration>().Where(x => x.WorkOrderDetailId == a.WorkOrderDetailID).FirstOrDefaultAsync();

            //if (bal != null)
            //{
            //    a.BalanceAndScaleCalibration = (BalanceAndScaleCalibration)bal.CloneObject();
            //}

            //if (a.BalanceAndScaleCalibration == null)
            //{
            //    return a;
            //}

            var bal = await context.BalanceAndScaleCalibration.AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId).ToListAsync();

            if (bal?.Count > 1)
            {
                context.BalanceAndScaleCalibration.RemoveRange(bal);
                context.BalanceAndScaleCalibration.Add(bal.First());

                await context.SaveChangesAsync();

            }
            else if (bal?.Count == 0 && calibrationtype.HasValue)
            {
                BalanceAndScaleCalibration bb = new BalanceAndScaleCalibration();

                bb.WorkOrderDetailId = workOrderDetailId;
                bb.CalibrationTypeId = calibrationtype.Value;

                context.BalanceAndScaleCalibration.Add(bb);

                await context.SaveChangesAsync();
            }


            a.BalanceAndScaleCalibration = await context.BalanceAndScaleCalibration.AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId).FirstOrDefaultAsync();

            //.IncludeFilter(c=>c.BalanceAndScaleCalibration).IncludeFilter(x=>x.BalanceAndScaleCalibration.Linearities.Select)

            List<Linearity> l = null;

            Eccentricity b = null;

            Repeatability c = null;

            //if (a.TestCodeID.HasValue)
            //{
            //    var test = await context.TestCode.AsNoTracking().Where(x => x.TestCodeID == a.TestCodeID).FirstOrDefaultAsync();
            //    a.TestCode = test;
            //}



            var uomslist2 = await context.UnitOfMeasure.AsNoTracking().Where(x => x.IsEnabled == true).ToListAsync();

            //context.ChangeTracker.Clear();
            var uomslist = uomslist2.ToList();


            if (!Isgeneric)
            {
                l = await context.Linearity


                 //.Include(d => d.BasicCalibrationResult)
                 ////.ThenInclude(d => d.UnitOfMeasure)

                 //.Include(d => d.TestPoint)
                 ////.ThenInclude(d => d.UnitOfMeasurement)

                 //.Include(d => d.BasicCalibrationResult)
                 ////.ThenInclude(d => d.UnitOfMeasure)
                 ////.ThenInclude(d => (d as UnitOfMeasure).UncertaintyUnitOfMeasure)



                 //.Include(d => d.WeightSets)
                 //.ThenInclude(d => d.UnitOfMeasure)
                 // .ThenInclude(d => d.UncertaintyUnitOfMeasure)
                 .AsNoTracking()
                 .Where(x => x.WorkOrderDetailId == workOrderDetailId)
                 .ToListAsync();


                //var un=context.UnitOfMeasure.Where(x=>x.UnitOfMeasureID==l.ba)
                var bcr = await context.BasicCalibrationResult.AsNoTracking().Where(x => x.WorkOrderDetailId == a.WorkOrderDetailID
                && x.CalibrationSubTypeId == new Linearity().CalibrationSubTypeId).ToListAsync();

                var testpointbcr = await context.TestPointGroup.AsNoTracking().Where(x => x.WorkOrderDetailID == a.WorkOrderDetailID).FirstOrDefaultAsync();

                TestPoint[] testpointlistt = null;
                if (testpointbcr != null)
                    testpointlistt = await context.TestPoint.AsNoTracking().Where(x => x.TestPointGroupTestPoitGroupID == testpointbcr.TestPoitGroupID).ToArrayAsync();


                var wesitlist = await context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).Where(x => x.WorkOrderDetailID == a.WorkOrderDetailID
                && x.CalibrationSubTypeID == new Linearity().CalibrationSubTypeId).ToArrayAsync();

                foreach (var linearity23 in l)
                {

                    var weee = wesitlist.Where(x => x.SecuenceID == linearity23.SequenceID).ToList();
                    List<WeightSet> wsss = new List<WeightSet>();
                    foreach (var wess in weee)
                    {
                        wsss.Add(wess.WeightSet);
                    }

                    linearity23.WeightSets = wsss;
                    if (testpointlistt != null)
                        linearity23.TestPoint = testpointlistt.Where(x => x.TestPointID == linearity23.TestPointID).FirstOrDefault();

                    //linearity23.BasicCalibrationResult = bcr.Where(x => x.SequenceID == linearity23.SequenceID).FirstOrDefault();

                    linearity23.BasicCalibrationResult = bcr.Where(x => x.SequenceID == linearity23.SequenceID).FirstOrDefault();


                    var uoms = uomslist
                        .Where(x => x.UnitOfMeasureID == linearity23.BasicCalibrationResult.UnitOfMeasureID).FirstOrDefault();

                    uoms.UnitOfMeasureBase = null;
                    uoms.UncertaintyUnitOfMeasure = null;


                    linearity23.BasicCalibrationResult.UnitOfMeasure = (UnitOfMeasure)uoms.CloneObject();

                    linearity23.BasicCalibrationResult.UnitOfMeasure.UncertaintyUnitOfMeasure = null;

                    var uoms1 = uomslist
                       .Where(x => linearity23.TestPoint != null && x.UnitOfMeasureID == linearity23.TestPoint.UnitOfMeasurementID).FirstOrDefault();


                    linearity23.TestPoint.UnitOfMeasurement = (UnitOfMeasure)uoms1.CloneObject();

                    linearity23.TestPoint.UnitOfMeasurement.UncertaintyUnitOfMeasure = null;

                    if (linearity23?.BasicCalibrationResult?.UnitOfMeasure?.UncertaintyUnitOfMeasure != null)
                    {

                        var uomd22 = uomslist
                       .Where(x => linearity23.BasicCalibrationResult != null && linearity23.BasicCalibrationResult.UnitOfMeasure != null && x.UnitOfMeasureID == linearity23.BasicCalibrationResult.UnitOfMeasure.UncertaintyUnitOfMeasure.UnitOfMeasureID).FirstOrDefault();


                        linearity23.BasicCalibrationResult.UnitOfMeasure.UncertaintyUnitOfMeasure = (UnitOfMeasure)uomd22.CloneObject();
                        linearity23.BasicCalibrationResult.UnitOfMeasure.UncertaintyUnitOfMeasure.UncertaintyUnitOfMeasure = null;
                        linearity23.BasicCalibrationResult.UnitOfMeasure.UncertaintyUnitOfMeasure.UnitOfMeasureBase = null;

                    }



                    if (linearity23?.WeightSets?.Count > 0)
                    {
                        foreach (var wes in linearity23.WeightSets)
                        {

                            var uomwes = uomslist
                             .Where(x => x.UnitOfMeasureID == wes.UnitOfMeasureID).FirstOrDefault();

                            wes.UnitOfMeasure = (UnitOfMeasure)uomwes.CloneObject();

                            var wesuoms = uomslist
                            .Where(x => wes.UncertaintyUnitOfMeasureId.HasValue && x.UnitOfMeasureID == wes.UncertaintyUnitOfMeasureId).FirstOrDefault();


                            wes.UncertaintyUnitOfMeasure = (UnitOfMeasure)wesuoms.CloneObject();
                            wes.UncertaintyUnitOfMeasure.UncertaintyUnitOfMeasure = null;
                            wes.UncertaintyUnitOfMeasure.UnitOfMeasureBase = null;


                        }
                    }




                }


                b = await context.Eccentricity.AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId)
                       .Include(d => d.TestPoint)
                       .Include(d => d.TestPointResult)
                       //.ThenInclude(d => d.UnitOfMeasure)
                       .Include(d => d.TestPointResult)
                        //.ThenInclude(d => d.UnitOfMeasure)
                        //.ThenInclude(d => d.UncertaintyUnitOfMeasure)
                        .AsNoTracking()
                         .FirstOrDefaultAsync();
                if (b?.TestPointResult?.Count > 0)
                {
                    foreach (var b23 in b.TestPointResult)
                    {

                        var b23uom = uomslist
                               .Where(x => x.UnitOfMeasureID == b23.UnitOfMeasureID).FirstOrDefault();

                        b23.UnitOfMeasure = (UnitOfMeasure)b23uom.CloneObject();
                        b23.UnitOfMeasure.UnitOfMeasureBase = null;
                        b23.UnitOfMeasure.UncertaintyUnitOfMeasure = null;


                        var b23uom2 = uomslist
                              .Where(x => b23.UnitOfMeasure.UncertaintyUnitOfMeasureID.HasValue && x.UnitOfMeasureID == b23.UnitOfMeasure.UncertaintyUnitOfMeasureID).FirstOrDefault();


                        b23.UnitOfMeasure.UncertaintyUnitOfMeasure = (UnitOfMeasure)b23uom2?.CloneObject();

                        if (b23.UnitOfMeasure?.UncertaintyUnitOfMeasure?.UncertaintyUnitOfMeasure != null)
                        {
                            b23.UnitOfMeasure.UncertaintyUnitOfMeasure.UncertaintyUnitOfMeasure = null;
                        }

                        if (b23.UnitOfMeasure?.UncertaintyUnitOfMeasure?.UnitOfMeasureBase != null)
                        {
                            b23.UnitOfMeasure.UncertaintyUnitOfMeasure.UnitOfMeasureBase = null;
                        }
                    }
                }








                c = await context.Repeatability.AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId).AsNoTracking()
                        .Include(d => d.TestPoint)
                        .FirstOrDefaultAsync();


                var ctpr = await context.BasicCalibrationResult.AsNoTracking().Where(x => x.WorkOrderDetailId
                == workOrderDetailId && x.CalibrationSubTypeId == new Repeatability().CalibrationSubTypeId).AsNoTracking()
                       .ToListAsync();
                if (c != null)
                {
                    c.TestPointResult = ctpr;
                }



                //.Include(d => d.TestPoint)
                //.Include(d => d.TestPoint)
                //.Include(d => d.TestPoint)
                //.Include(d => d.TestPointResult)
                //.ThenInclude(d => d.UnitOfMeasure)
                //.ThenInclude(d => d.UncertaintyUnitOfMeasure)

                //.Include(d => d.WeightSets)
                //.ThenInclude(d => d.UnitOfMeasure)
                //.AsNoTracking()
                // .FirstOrDefaultAsync();


                //.Include(d => d.TestPoint)
                //           .Include(d => d.TestPointResult)
                //           //.ThenInclude(d => d.UnitOfMeasure)
                //           //.ThenInclude(d => d.UncertaintyUnitOfMeasure)

                //           //.Include(d => d.WeightSets)
                //           //.ThenInclude(d => d.UnitOfMeasure)
                //           .AsNoTracking()
                //            .FirstOrDefaultAsync();




                // var wesitlist2 = await context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).Where(x => x.WorkOrderDetailID == a.WorkOrderDetailID
                //&& x.CalibrationSubTypeID == new Repeatability().CalibrationSubTypeId).ToArrayAsync();

                if (c?.TestPointResult?.Count > 0)
                {
                    c.TestPointResult = ctpr;
                    foreach (var c23 in c.TestPointResult)
                    {


                        var c23uom = uomslist
                            .Where(x => x.UnitOfMeasureID == c23.UnitOfMeasureID).FirstOrDefault();

                        c23.UnitOfMeasure = (UnitOfMeasure)c23uom.CloneObject();

                        c23.UnitOfMeasure.UnitOfMeasureBase = null;
                        c23.UnitOfMeasure.UncertaintyUnitOfMeasure = null;


                        var c23uom2 = uomslist
                            .Where(x => c23.UnitOfMeasure.UncertaintyUnitOfMeasureID.HasValue && x.UnitOfMeasureID == c23.UnitOfMeasure.UncertaintyUnitOfMeasureID).FirstOrDefault();



                        c23.UnitOfMeasure.UncertaintyUnitOfMeasure = (UnitOfMeasure)c23uom2.CloneObject();
                        c23.UnitOfMeasure.UncertaintyUnitOfMeasure.UncertaintyUnitOfMeasure = null;
                        c23.UnitOfMeasure.UncertaintyUnitOfMeasure.UnitOfMeasureBase = null;



                    }
                }

                if (c?.WeightSets?.Count > 0)
                {
                    foreach (var wesc in c.WeightSets)
                    {

                        var wescuom = uomslist
                        .Where(x => x.UnitOfMeasureID == wesc.UnitOfMeasureID).FirstOrDefault();

                        wesc.UnitOfMeasure = (UnitOfMeasure)wescuom.CloneObject();
                        wesc.UnitOfMeasure.UncertaintyUnitOfMeasure = null;

                        var wescuom1 = uomslist
                        .Where(x => wesc.UncertaintyUnitOfMeasureId.HasValue && x.UnitOfMeasureID == wesc.UncertaintyUnitOfMeasureId).FirstOrDefault();

                        wesc.UncertaintyUnitOfMeasure = (UnitOfMeasure)wescuom1.CloneObject();
                    }
                }

            }






            var ws = await context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).Where(x => x.WorkOrderDetailID == workOrderDetailId).ToListAsync();


            if (!Isgeneric && ws?.Count > 0)
            {
                int re = new Repeatability().CalibrationSubTypeId;

                var wss = ws.Where(x => x.CalibrationSubTypeID == re).ToList();
                if (c != null && wss != null && wss.Count > 0)
                {
                    c.WeightSets = new List<WeightSet>();
                    foreach (var wer in wss)
                    {
                        c.WeightSets.Add(wer.WeightSet);
                    }
                }

                int ecc = new Eccentricity().CalibrationSubTypeId;

                var wsecc = ws.Where(x => x.CalibrationSubTypeID == ecc).ToList();
                if (b != null && wsecc != null && wsecc.Count > 0)
                {
                    b.WeightSets = new List<WeightSet>();
                    foreach (var wer in wsecc)
                    {
                        b.WeightSets.Add(wer.WeightSet);
                    }
                }


                int lin = new Linearity().CalibrationSubTypeId;

                var wslin = ws.Where(x => x.CalibrationSubTypeID == lin).ToList();
                if (l != null && wslin != null && wslin.Count > 0)
                {

                    foreach (var wer in l)
                    {
                        var weil = wslin.Where(x => x.SecuenceID == wer.SequenceID).ToList();
                        if (weil != null && weil.Count > 0)
                        {
                            wer.WeightSets = new List<WeightSet>();
                            foreach (var wer1 in weil)
                            {
                                wer.WeightSets.Add(wer1.WeightSet);
                                

                            }
                          
                        }
                    }
                }

              
              
               
            }









            List<WOD_Weight> wOD_Weights = new List<WOD_Weight>();


            var w = await context.WOD_Weight.AsNoTracking().Where(w => w.WorkOrderDetailID == workOrderDetailId).ToArrayAsync();

            //Include(x => x.WeightSet)
            if (w != null && w.Count() > 0)
            {
                foreach (var w1 in w)
                {
                    var w12 = await context.WeightSet.AsNoTracking().Where(x => x.WeightSetID == w1.WeightSetID)
                  //     .Include(d => d.UnitOfMeasure)
                  //.ThenInclude(d => d.UncertaintyUnitOfMeasure)
                  .AsNoTracking().FirstOrDefaultAsync();
                    w1.WeightSet = w12;
                    if (w12 != null)
                    {
                        w1.WeightSet.Option = w1.Option;
                        var w1uom = uomslist.Where(x => x.UnitOfMeasureID == w12.UnitOfMeasureID).FirstOrDefault();

                        w12.UnitOfMeasure = (UnitOfMeasure)w1uom.CloneObject();

                        if (w12.UncertaintyUnitOfMeasureId == null)
                        {
                            w12.UncertaintyUnitOfMeasureId = w12.UnitOfMeasureID;
                        }

                        var w1uom1 = uomslist.Where(x => x.UnitOfMeasureID == w12.UncertaintyUnitOfMeasureId).FirstOrDefault();

                        w12.UncertaintyUnitOfMeasure = (UnitOfMeasure)w1uom1.CloneObject();



                    }
                    else
                    {
//                        Console.WriteLine("-------------------Weight Set not found-------------------------------");
                    }




                    wOD_Weights.Add(w1);
                }
                //a.WOD_Weights = w;
                //w=await context.WOD_Weight.AsNoTracking().Include(x => x.WeightSet).Where(w => w.WorkOrderDetailID == workOrderDetailId).ToArrayAsync();
            }


            var wst = await context.WOD_Standard.AsNoTracking().Where(w => w.WorkOrderDetailID == workOrderDetailId).ToArrayAsync();

            //Include(x => x.WeightSet)
            if (wst != null && wst.Count() > 0)
            {
                a.WOD_Standard = wst;
                int cont = 1;


                foreach (var w1 in wst)
                {
                    var w12 = await context.PieceOfEquipment.AsNoTracking().Where(x => x.PieceOfEquipmentID == w1.PieceOfEquipmentID).FirstOrDefaultAsync();

                    if (w12 != null)
                    {
                        w12.TestPointResult = context.GenericCalibrationResult2.AsNoTracking().Where(x => x.ComponentID == w1.PieceOfEquipmentID && x.Component == "PieceOfEquipmentCreate").ToList();
                    }

                    WOD_Weight wOD_Weight = new WOD_Weight();

                    wOD_Weight.WeightSetID = cont;
                    wOD_Weight.WorkOrderDetailID = workOrderDetailId;
                    wOD_Weight.WeightSet = new WeightSet();
                    wOD_Weight.WeightSet.Option = w1.Option;
                    wOD_Weight.WeightSet.WeightSetID = cont;
                    wOD_Weight.WeightSet.PieceOfEquipmentID = w12.PieceOfEquipmentID;
                    wOD_Weight.WeightSet.PieceOfEquipment = w12;
                    wOD_Weight.WeightSet.PieceOfEquipment.WeightSets = null;
                    wOD_Weight.WeightSet.PieceOfEquipment.EquipmentTemplate = null;
                    wOD_Weight.WeightSet.WOD_Weights = null;
                    wOD_Weights.Add(wOD_Weight);
                    cont++;
                }

                //if (a.WOD_Weights==null)
                //{
                //    a.WOD_Weights= new List<WOD_Weight>();  
                //}

                //foreach (var wwitem in wOD_Weights)
                //{
                //    a.WOD_Weights.Append(wwitem);
                //}

                //w=await context.WOD_Weight.AsNoTracking().Include(x => x.WeightSet).Where(w => w.WorkOrderDetailID == workOrderDetailId).ToArrayAsync();
            }

            a.WOD_Weights = wOD_Weights;

            a.BalanceAndScaleCalibration.Repeatability = c;

            a.BalanceAndScaleCalibration.Eccentricity = b;


            a.BalanceAndScaleCalibration.Linearities = l;

            var etyLevel3 = await context.EquipmentType.AsNoTracking().Where(x => x.CalibrationTypeID == a.BalanceAndScaleCalibration.CalibrationTypeId && x.JSONConfiguration != null).FirstOrDefaultAsync();
            var isBalanceAdvance  = false;

            if (etyLevel3 != null && GetJSONConfigurationArray(etyLevel3)?.FirstOrDefault()?.Value == "Balance")
            {
                isBalanceAdvance = true;
            }

            if (a.BalanceAndScaleCalibration.CalibrationTypeId == 1 || a.BalanceAndScaleCalibration.CalibrationTypeId == 2 || isBalanceAdvance ==true)
            {

                Isgeneric = true;
            }


            if (Isgeneric && (l == null || l.Count() == 0))
            {
                //for bitterman
                try
                {
                   
                        a.BalanceAndScaleCalibration.TestPointResult = await GetCalibrationType22<GenericCalibration2, GenericCalibrationResult2>(workOrderDetailId.ToString(), "WorkOrderItem");

                    var res = a.BalanceAndScaleCalibration.TestPointResult;

                    foreach (var rest in res.Where(x=>x.GenericCalibration2?.WeightSets?.Count() > 0))
                    {
                        var weigsets = rest?.GenericCalibration2?.WeightSets;

                        if (weigsets != null && weigsets.Count() > 0)
                        {

                            foreach (var re in weigsets.ToList())
                            {

                                PieceOfEquipment poews = await context.PieceOfEquipment
                                                             .AsNoTracking()
                                                             .FirstOrDefaultAsync(x => x.PieceOfEquipmentID == re.PieceOfEquipmentID);
                                re.PieceOfEquipment = poews;

                                var uncertainties = await context.Uncertainty.AsNoTracking().Where(x => !string.IsNullOrEmpty(x.PieceOfEquipmentID) && x.PieceOfEquipmentID == re.PieceOfEquipmentID).ToListAsync();
                               

                                foreach (var uo in uncertainties)
                                {
                                    UnitOfMeasure uom = new UnitOfMeasure
                                    {
                                        UnitOfMeasureID = uo.UnitOfMeasureID
                                    };

                                    var uom_ = await context.UnitOfMeasure.AsNoTracking().Where(x => x.UnitOfMeasureID == uo.UnitOfMeasureID).FirstOrDefaultAsync();

                                    uo.UnitOfMeasure = uom_;
                                }

                                re.PieceOfEquipment.Uncertainty = uncertainties;
                            }

                        }



                    }



                    try
                    {
                        a.BalanceAndScaleCalibration.Forces = await GetForces(workOrderDetailId);

                        a.BalanceAndScaleCalibration.Rockwells = await GetCalibrationType<Rockwell, RockwellResult>(workOrderDetailId);


                        a.BalanceAndScaleCalibration.GenericCalibration = await GetCalibrationType<GenericCalibration, GenericCalibrationResult>(workOrderDetailId);



                     }
                    catch (Exception ex)
                    {

                    }
                   
                }
                catch (Exception)
                {

                }






            }






            var env = await context.ExternalCondition.AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId).ToListAsync();

            a.EnviromentCondition = env;

            //EquipmentType equipmentType = new EquipmentType();
            var stRock = await context.CalibrationSubType_Standard.AsNoTracking().Include(d => d.Standard).ThenInclude(x => x.EquipmentTemplate).Where(w => w.WorkOrderDetailID == workOrderDetailId).ToArrayAsync();
            var wod1 = await context.WorkOrderDetail.AsNoTracking().Include(x => x.PieceOfEquipment).ThenInclude(b => b.EquipmentTemplate).Where(w => w.WorkOrderDetailID == workOrderDetailId).FirstOrDefaultAsync();

            //if (a.TestCode != null)
            //{
            //    //equipmentType = await context.EquipmentType.AsNoTracking().Where(x => x.CalibrationTypeID == a.TestCode.CalibrationTypeID && x.EquipmentTypeGroupID == wod1.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID && x.HasStandard == false).FirstOrDefaultAsync();
            //}


            a.PieceOfEquipment = wod1.PieceOfEquipment;


            if (stRock != null)
            {
                a.CalibrationSubType_Standards = stRock;
            }


            if (a?.BalanceAndScaleCalibration?.TestPointResult != null && a?.BalanceAndScaleCalibration?.TestPointResult?.Count > 0 && 1 == 2)
            {
                foreach (var item in a?.BalanceAndScaleCalibration?.TestPointResult)
                {
                    if (item.GenericCalibration2 != null)
                    {
                        var sss = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.SecuenceID == item.SequenceID && x.CalibrationSubTypeID == item.CalibrationSubTypeId).ToArrayAsync();


                        //WOD_Weight wOD_Weight = new WOD_Weight();

                        //wOD_Weight.WeightSetID = cont;
                        //wOD_Weight.WorkOrderDetailID = workOrderDetailId;
                        //wOD_Weight.WeightSet = new WeightSet();
                        //wOD_Weight.WeightSet.Option = w1.Option;
                        //wOD_Weight.WeightSet.WeightSetID = cont;
                        //wOD_Weight.WeightSet.PieceOfEquipmentID = w12.PieceOfEquipmentID;
                        //wOD_Weight.WeightSet.PieceOfEquipment = w12;
                        //wOD_Weight.WeightSet.PieceOfEquipment.WeightSets = null;
                        //wOD_Weight.WeightSet.PieceOfEquipment.EquipmentTemplate = null;
                        //wOD_Weight.WeightSet.WOD_Weights = null;

                        foreach (var itemst in sss)
                        {

                            var poesq = await context.PieceOfEquipment.AsNoTracking().Where(x => x.PieceOfEquipmentID == itemst.PieceOfEquipmentID).FirstOrDefaultAsync();

                            itemst.Standard = poesq;

                        }

                        item.GenericCalibration2.Standards = sss;


                        var sss1 = await context.CalibrationSubType_Weight.AsNoTracking().Where(x => x.SecuenceID == item.SequenceID && x.CalibrationSubTypeID == item.CalibrationSubTypeId).ToArrayAsync();


                        foreach (var itemstw in sss1)
                        {

                            var sss1w = await context.WeightSet.AsNoTracking().Where(x => x.WeightSetID == itemstw.WeightSetID).FirstOrDefaultAsync();

                            itemstw.WeightSet = sss1w;
                        }


                        item.GenericCalibration2.CalibrationSubType_Weights = sss1;





                    }



                }


            }

            var uncert = await context.Uncertainty.AsNoTracking().Where(x => x.PieceOfEquipmentID == a.PieceOfEquipmentId).Include(x => x.UnitOfMeasure).ToListAsync();

            //if(uncert != null && uncert.Count > 0)
            //{

            //    foreach (var uncv in uncert)
            //    {

            //    }

            //}


            if (a.PieceOfEquipment == null)
            {
                //a.PieceOfEquipment = new PieceOfEquipment();

                var piece = await context.PieceOfEquipment.AsNoTracking().Where(x => x.PieceOfEquipmentID == a.PieceOfEquipmentId).FirstOrDefaultAsync();
                a.PieceOfEquipment = piece;
                var equi = await context.EquipmentTemplate.AsNoTracking().Where(x => x.EquipmentTemplateID == piece.EquipmentTemplateId).FirstOrDefaultAsync();
                a.PieceOfEquipment.EquipmentTemplate = equi;
            }

            a.PieceOfEquipment.Uncertainty = uncert;

            a.PieceOfEquipment.EquipmentTemplate.Uncertainty = await context.Uncertainty
            .AsNoTracking().Where(x => x.EquipmentTemplateID == a.PieceOfEquipment
            .EquipmentTemplate
            .EquipmentTemplateID).Include(x => x.UnitOfMeasure).ToListAsync();




            return a;

        }

        public async Task<List<Force>> GetForces(int workOrderDetailId)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            var forces = await context.Force.AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId).ToArrayAsync();



            if (forces != null && forces?.Count() > 0)
            {

                var ws = await context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).Where(x => x.WorkOrderDetailID == workOrderDetailId).ToListAsync();


                foreach (var wer in forces)
                {

                    var forcesres = await context.ForceResult.AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId
                    && x.SequenceID == wer.SequenceID && x.CalibrationSubTypeId == wer.CalibrationSubTypeId).FirstOrDefaultAsync();

                    wer.BasicCalibrationResult = forcesres;

                    wer.WeightSets = await GetCalibrationSubTypes_WeightSets(wer, workOrderDetailId);


                }

                return forces.OrderBy(x => x.BasicCalibrationResult.Position).ToList();

                //a.BalanceAndScaleCalibration.Forces = forces;
            }

            return null;


        }

        //public async Task SaveCalibrationTypes<TType, TResult>(int id, ICollection<TType> Forces, Func<TType, TType> FormatFunction)
        //   where TType : class, IGenericCalibrationSubType<TResult> where TResult : class, IResult






        public async Task<List<TType>> GetCalibrationType<TType, TResult>(int workOrderDetailId)
        where TType : class, IGenericCalibrationSubType<TResult> where TResult : class, IResult2
        {

            try
            {
                await using var context = await DbFactory.CreateDbContextAsync();

                IQueryable<TType> query = context.Set<TType>().AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId);

                var forces = await query.ToListAsync();

                if (forces != null && forces?.Count() > 0)
                {

                    var ws = await context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).Where(x => x.WorkOrderDetailID == workOrderDetailId).ToListAsync();


                    foreach (var wer in forces)
                    {

                        var forcesres = await context.Set<TResult>().AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId
                        && x.SequenceID == wer.SequenceID && x.CalibrationSubTypeId == wer.CalibrationSubTypeId).FirstOrDefaultAsync();

                        wer.BasicCalibrationResult = forcesres;

                        wer.WeightSets = await GetCalibrationSubTypes_WeightSets(wer, workOrderDetailId);


                        var stan = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.WorkOrderDetailID == workOrderDetailId
                        && x.SecuenceID == wer.SequenceID && x.CalibrationSubTypeID == wer.CalibrationSubTypeId).ToArrayAsync();

                        wer.Standards = stan;



                    }

                    return forces.OrderBy(x => x.BasicCalibrationResult.Position).ToList();


                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }






        public async Task<List<TType>> GetCalibrationDynamicType<TType, TResult>(int workOrderDetailId)
            where TType : class, IGenericCalibrationSubType<TResult> where TResult : GenericCalibrationResult, IResult2
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            IQueryable<TType> query = context.Set<TType>().AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId);

            var forces = await query.ToListAsync();

            if (forces != null && forces?.Count() > 0)
            {

                var ws = await context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).Where(x => x.WorkOrderDetailID == workOrderDetailId).ToListAsync();


                foreach (var wer in forces)
                {

                    var forcesres = await context.Set<TResult>().AsNoTracking().Where(x => x.WorkOrderDetailId == workOrderDetailId
                    && x.SequenceID == wer.SequenceID && x.CalibrationSubTypeId == wer.CalibrationSubTypeId).FirstOrDefaultAsync();

                    wer.BasicCalibrationResult = forcesres;

                    wer.WeightSets = await GetCalibrationSubTypes_WeightSets(wer, workOrderDetailId);


                    var stan = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.WorkOrderDetailID == workOrderDetailId
                    && x.SecuenceID == wer.SequenceID && x.CalibrationSubTypeID == wer.CalibrationSubTypeId).ToArrayAsync();

                    wer.Standards = stan;


                    //var obj = forcesres.Object.ToDynamic();

                    //wer.BasicCalibrationResult = obj;



                }

                return forces.ToList();


            }

            return null;


        }

        public async Task<List<WeightSet>> GetCalibrationSubTypes_WeightSets(ICalibrationSubType wer, int workOrderDetailId)
        {


            await using var context = await DbFactory.CreateDbContextAsync();

            var ws = await context.CalibrationSubType_Weight.AsNoTracking()
                      .Include(x => x.WeightSet)
                      .ThenInclude(c => c.UnitOfMeasure)
                      //.ThenInclude(e => (e as UnitOfMeasure).UncertaintyUnitOfMeasure)
                      .Where(x => x.WorkOrderDetailID == workOrderDetailId)
                      .ToListAsync();

            //TODO: Yenny, check the commented line of UncertaintyUnitOfMeasure, it may be necessary

            var wsfor = ws.Where(x => x.CalibrationSubTypeID == wer.CalibrationSubTypeId
            && x.WorkOrderDetailID == wer.WorkOrderDetailId
            && wer.SequenceID == x.SecuenceID).ToList();

            var WeightSets = new List<WeightSet>();
            foreach (var wer1 in wsfor)
            {
                WeightSets.Add(wer1.WeightSet);
            }

            return WeightSets;

        }

        public async Task<List<WeightSet>> GetCalibrationSubTypes_WeightSets(ICalibrationSubType2 wer, string workOrderDetailId, string Component)
        {


            await using var context = await DbFactory.CreateDbContextAsync();
            var ws = await context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).Where(x => x.ComponentID == workOrderDetailId && x.Component == Component).ToListAsync();


            var wsfor = ws.Where(x => x.CalibrationSubTypeID == wer.CalibrationSubTypeId
            && x.ComponentID == wer.ComponentID
            && wer.SequenceID == x.SecuenceID).ToList();

            var WeightSets = new List<WeightSet>();
            foreach (var wer1 in wsfor)
            {
                WeightSets.Add(wer1.WeightSet);
            }

            return WeightSets;

        }

        //public void GetConfiguredWeights(int WorkOrderDetailID, ref BalanceAndScaleCalibration bc)
        //{

        //    //await using var context = await DbFactory.CreateDbContextAsync();

        //    try
        //    {

        //        if (bc == null)
        //        {
        //            return;
        //        }



        //        var ws = context.CalibrationSubType_Weight.AsNoTracking().Include(x => x.WeightSet).ThenInclude(x => x.UnitOfMeasure).Where(x => x.WorkOrderDetailID == WorkOrderDetailID).ToList();
        //        Repeatability c = bc.Repeatability;
        //        if (bc.Repeatability != null)
        //        {


        //            int re = c.CalibrationSubTypeId;

        //            var wss = ws.Where(x => x.CalibrationSubTypeID == re).ToList();
        //            if (wss != null && wss.Count > 0)
        //            {
        //                c.WeightSets = new List<WeightSet>();
        //                foreach (var wer in wss)
        //                {
        //                    c.WeightSets.Add(wer.WeightSet);
        //                }
        //            }
        //        }

        //        Eccentricity b = bc.Eccentricity;
        //        if (bc?.Eccentricity != null)
        //        {


        //            int ecc = b.CalibrationSubTypeId;

        //            var wsecc = ws.Where(x => x.CalibrationSubTypeID == ecc).ToList();
        //            if (wsecc != null && wsecc.Count > 0)
        //            {
        //                b.WeightSets = new List<WeightSet>();
        //                foreach (var wer in wsecc)
        //                {
        //                    b.WeightSets.Add(wer.WeightSet);
        //                }
        //            }
        //        }

        //        var l = bc.Linearities;

        //        if (bc.Linearities != null)
        //        {


        //            int lin = new Linearity().CalibrationSubTypeId;

        //            var wslin = ws.Where(x => x.CalibrationSubTypeID == lin).ToList();
        //            if (wslin != null && wslin.Count > 0)
        //            {

        //                foreach (var wer in l)
        //                {
        //                    var weil = wslin.Where(x => x.SecuenceID == wer.SequenceID).ToList();
        //                    if (weil != null && weil.Count > 0)
        //                    {
        //                        wer.WeightSets = new List<WeightSet>();
        //                        foreach (var wer1 in weil)
        //                        {
        //                            wer.WeightSets.Add(wer1.WeightSet);
        //                        }
        //                    }


        //                }
        //            }
        //        }




        //        var f = bc.Forces;
        //        if (f != null)
        //        {
        //            int forc = new Linearity().CalibrationSubTypeId;

        //            IQueryable<CalibrationSubType_Weight> query = ws.AsQueryable();

        //            var wsfoc = new List<CalibrationSubType_Weight>();

        //            foreach (var iy in CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetLTICalibrationSubTypes())
        //            {
        //                int subtype = iy;

        //                var a = ws.Where(p => p.CalibrationSubTypeID == subtype).ToList();
        //                foreach (var tt in a)
        //                {
        //                    wsfoc.Add(tt);
        //                }

        //            }

        //            //var predicate = PredicateBuilder.New<CalibrationSubType_Weight>();
        //            //foreach (var keyword in CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetLTICalibrationSubTypes())
        //            //{
        //            //int temp = keyword;
        //            //predicate = predicate.Or (p => p.CalibrationSubTypeID ==temp);
        //            //}







        //            //wsfoc = query.Where(predicate).ToList();


        //            if (wsfoc != null && wsfoc.Count > 0)
        //            {

        //                foreach (var wer in f)
        //                {
        //                    var weil = wsfoc.Where(x => x.SecuenceID == wer.SequenceID).ToList();
        //                    if (weil != null && weil.Count > 0)
        //                    {
        //                        wer.WeightSets = new List<WeightSet>();
        //                        foreach (var wer1 in weil)
        //                        {
        //                            wer.WeightSets.Add(wer1.WeightSet);
        //                        }
        //                    }


        //                }
        //            }
        //        }
        //        bc.Forces = f;
        //        bc.Repeatability = c;
        //        bc.Eccentricity = b;
        //        bc.Linearities = l;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}



        public async Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailByWorkOrderID(int WorkOrderId)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            //var res = await context.WorkOrderDetail
            //    .Include(x => x.PieceOfEquipment)
                   
                
                  
            //        .Where(x => x.WorkOrderID == WorkOrderId)
            //    .ToArrayAsync();

            var res = await (from wod in context.WorkOrderDetail
                             join poes in context.PieceOfEquipment on wod.PieceOfEquipmentId equals poes.PieceOfEquipmentID
                             join et in context.EquipmentTemplate on poes.EquipmentTemplateId equals et.EquipmentTemplateID
                             join etg in context.EquipmentTypeGroup on et.EquipmentTypeGroupID equals etg.EquipmentTypeGroupID
                             join ety in context.EquipmentType on etg.EquipmentTypeGroupID equals ety.EquipmentTypeGroupID
                             join man in context.Manufacturer on et.ManufacturerID equals man.ManufacturerID
                             where wod.WorkOrderID == WorkOrderId
                             select new WorkOrderDetail()
                             {
                                 WorkOrderDetailID = wod.WorkOrderDetailID,
                                 PieceOfEquipment = new PieceOfEquipment()
                                 {
                                     SerialNumber = poes.SerialNumber,
                                     PieceOfEquipmentID = poes.PieceOfEquipmentID,
                                     EquipmentTemplate = new EquipmentTemplate()
                                     {
                                         Model = et.Model,
                                         EquipmentTypeObject = new EquipmentType
                                         {
                                             EquipmentTypeID = ety.EquipmentTypeID,
                                             EquipmentTypeGroupID = ety.EquipmentTypeGroupID,
                                             CalibrationTypeID = ety.CalibrationTypeID,

                                         }
                                     }
                                 }
                                 
                                 
                             }
                             ).ToListAsync();

            return res;




        }

        public async Task AttachWorkOrderDetail(WorkOrderDetail workOrderDetail)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.WorkOrderDetail.Attach(workOrderDetail);
        }

        public async Task<WorkOrderDetail> UpdateWorkOrderDetail(WorkOrderDetail workOrderDetail)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            try
            {
                var result = context.WorkOrderDetail.Update(workOrderDetail);
                await context.SaveChangesAsync();
                return workOrderDetail;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task Save()
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            context.SaveChanges();
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


        public async Task<bool> CreateHistory(WorkDetailHistory DTO)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            try
            {


                DTO.WorkOrderDetail = null;
                context.WorkDetailHistory.Add(DTO);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public async Task<Certificate> CreateCertificate(Certificate DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            try
            {
                context.Certificate.Add(DTO);
                await context.SaveChangesAsync();
                return DTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        public async Task<Certificate> GetCertificate(WorkOrderDetail DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            Certificate resultVersion = new Certificate();
            var result = context.Certificate.Where(x => x.WorkOrderDetailId == DTO.WorkOrderDetailID).ToList().OrderByDescending(x => x.Version).FirstOrDefault();

            if (result != null)

                return result;
            else
                return resultVersion;

        }


        public async Task<ResultSet<TestCode>> GetTestCodes(Pagination<TestCode> pagination)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            var filterQuery = Querys.TestCodeFilter(pagination.Filter);

            var queriable = context.TestCode.AsNoTracking()

                //.Include(x => x.UnitOfMeasure)
                //.Include(x => x.Procedure)
                //.Include(x => x.Notes)
                .AsQueryable();
            //context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            var simplequery = context.TestCode.AsNoTracking()
                //.Include(x => x.UnitOfMeasure)
                //.Include(x => x.Procedure)
                //.Include(x => x.Notes)
                .AsQueryable();

            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result = await queriable.PaginationAndFilterQuery<TestCode>(pagination, simplequery, filterQuery);

            var uoms = await context.UnitOfMeasure.AsNoTracking().Where(x => x.IsEnabled).ToListAsync();

            var caty = await context.CalibrationType.AsNoTracking().ToListAsync();

            var proc = await context.Procedure.AsNoTracking().ToListAsync();

            // PERFORMANCE FIX: Fetch all Notes in a single query instead of N+1 queries
            var testCodeIds = result.List.Select(x => x.TestCodeID).ToList();
            var allNotes = await context.Note.AsNoTracking()
                .Where(x => testCodeIds.Contains(x.TestCodeID.Value))
                .ToListAsync();
            var notesByTestCodeId = allNotes.GroupBy(x => x.TestCodeID.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            try
            {
                foreach (var item in result.List)
                {
                    CalibrationType ct = null;

                    if (item.CalibrationTypeID.HasValue)
                    {
                        ct = caty
                        .Where(x => x.CalibrationTypeId == item.CalibrationTypeID).FirstOrDefault();

                        if (ct != null)
                        {
                            item.CalibrationType = ct;
                        }
                    }

                    if (item.UnitOfMeasureID.HasValue)
                    {
                        item.UnitOfMeasure = item.UnitOfMeasureID.GetUoM(uoms);
                    }
                    if (item.ProcedureID.HasValue)
                    {
                        item.Procedure = proc.Where(x => x.ProcedureID == item.ProcedureID.Value).FirstOrDefault();
                    }

                    // PERFORMANCE FIX: Use pre-fetched Notes instead of individual queries
                    item.Notes = notesByTestCodeId.TryGetValue(item.TestCodeID, out var notes) ? notes : new List<Note>();

                }

            }
            catch (Exception ex) { var messsage = ex.Message; }

            //result.List = FormatTestCode(result.List).ToList();

            return result;

        }


        private IEnumerable<TestCode> FormatTestCode(IEnumerable<TestCode> list)
        {

            foreach (var item in list)
            {
                item.UnitOfMeasure.UnitOfMeasureBase = null;
                item.UnitOfMeasure.UncertaintyUnitOfMeasure = null;

                yield return item;
            }



        }

        public async Task<TestCode> CreateTestCode(TestCode item)
        {
            await using var context = await DbFactory.CreateDbContextAsync();


            if (item.UnitOfMeasure != null)
            {
                item.UnitOfMeasure = null;
                //item.UnitOfMeasure.UncertaintyUnitOfMeasure = null;

            }




            var a = await context.TestCode.AsNoTracking().Where(x => x.TestCodeID == item.TestCodeID).FirstOrDefaultAsync();



            if (item.Procedure != null)
            {
                context.Entry(item.Procedure).State = EntityState.Detached;
            }
            item.NullableCollections = true;

            if (a == null)
            {

                item.TestCodeID = NumericExtensions.GetUniqueID(item.TestCodeID);


                context.TestCode.Add(item);
            }
            else
            {
                context.TestCode.Update(item);
            }

            await context.SaveChangesAsync();




            var sg = await context.Note.AsNoTracking().Where(x => x.TestCodeID == item.TestCodeID).ToListAsync();


            if (item.Notes != null)
            {
                foreach (var note in item?.Notes)
                {
                    var notal = sg.Where(x => x.NoteId == note.NoteId).FirstOrDefault();

                    if (notal == null)
                    {
                        context.Note.Add(note);
                    }
                    else
                    {
                        context.Note.Update(note);
                    }

                    await context.SaveChangesAsync();
                }
            }


            return item;


        }

        public async Task<TestCode> GetTestCodeByID(int item)
        {
            await using var context = await DbFactory.CreateDbContextAsync();

            var a = await context.TestCode.AsNoTracking().Where(x => x.TestCodeID == item).FirstOrDefaultAsync();

            if (a != null)
            {
                a.Notes = await GetNotesTC(a.TestCodeID, 2);
            }
            return a;


        }

        public async Task<TestCode> GetTestCodeXName(TestCode item)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.TestCode.AsNoTracking().Where(x => x.Code == item.Code).FirstOrDefaultAsync();
            return a;


        }

        public async Task<TestCode> DeleteTestCode(TestCode item)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await GetTestCodeByID(item.TestCodeID);

            if (a != null)
            {
                context.TestCode.Remove(a);
                await context.SaveChangesAsync();

            }

            return item;

        }

        //NoteWOD
        public async Task<NoteWOD> SaveNotes(NoteWOD DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var note = await context.NoteWOD.AsNoTracking().Where(x => x.NoteWODId == DTO.NoteWODId).FirstOrDefaultAsync();

            if (note != null)
            {

                context.NoteWOD.Update(DTO);

            }
            else
            {
                context.NoteWOD.Add(DTO);
            }

            await context.SaveChangesAsync();

            return null;



        }


        public async Task<List<NoteWOD>> GetNotes(int Id, int Source = 1)

        {
            IQueryable<NoteWOD> query = null;

            await using var context = await DbFactory.CreateDbContextAsync();

            if (Source == 1)
            {
                query = context.Set<NoteWOD>().AsNoTracking().Where(x => x.WorkOrderDetailId == Id);
            }


            var notes = await query.ToListAsync();



            return notes;

        }
        public async Task<List<Note>> GetNotesTC(int Id, int Source = 1)

        {
            await using var context = await DbFactory.CreateDbContextAsync();

            IQueryable<Note> query = null;


            if (Source == 2)
            {
                query = context.Set<Note>().AsNoTracking().Where(x => x.TestCodeID == Id);
            }

            if (Source == 1)
            {
                query = context.Set<Note>().AsNoTracking().Where(x => x.EquipmnetTypeId == Id);
            }


            var forces = await query.ToListAsync();



            return forces;

        }
        public async Task<bool> RemoveNotes<TSource>(int Id, TSource DTO) where TSource : INoteWOD
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            var notes = await context.NoteWOD.AsNoTracking().Where(x => x.WorkOrderDetailId == Id).ToListAsync();

            if (notes.Count > 0 && (DTO.NotesWOD == null || DTO?.NotesWOD?.Count == 0))
            {
                context.RemoveRange(notes);
                await context.SaveChangesAsync();
            }
            if (notes.Count > 0 && DTO?.NotesWOD?.Count > 0)
            {

                foreach (var item in notes)
                {
                    var notetmp = DTO.NotesWOD.Where(x => x.NoteWODId == item.NoteWODId).FirstOrDefault();

                    if (notetmp == null)
                    {
                        context.Remove(item);
                        await context.SaveChangesAsync();
                    }


                }



            }




            return true;

        }

        public async Task<ICollection<CalibrationSubType_Standard>> GetCalibrationSubType_StandardByWodI(int id)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var standards = await context.CalibrationSubType_Standard.AsNoTracking().Where(x => x.WorkOrderDetailID == id).ToListAsync();
            return standards;
        }



        public async Task<List<CalibrationSubType>> GetCalibrationSubType()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var calibrationSubTypes = await context.CalibrationSubType.AsNoTracking().Include(x => x.CalibrationSubTypeView).AsNoTracking().ToListAsync();
            
            
            foreach (var item in calibrationSubTypes)
            {
                if(item?.CalibrationSubTypeView?.CalibrationSubType != null)
                {
                    item.CalibrationSubTypeView.CalibrationSubType = null;
                }
                
                
            }   

            return calibrationSubTypes;
        }


     
        public Task<InstrumentThread> GetInstrumentThread(WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }

        public Task<ResultsTable> GetResultsTable(WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }

        public async Task<WOD_ParametersTable> GetWOD_Parameter(WOD_ParametersTable DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            WOD_ParametersTable wodp = new WOD_ParametersTable();
            var result = await context.WOD_ParametersTable.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).FirstOrDefaultAsync();

            if (result != null)
            {
                return result;
            }
            else
            {
                return wodp;
            }
        }

        public async Task<WOD_ParametersTable> SaveWOD_Parameter(WOD_ParametersTable DTO)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            try
            {
                var result = await GetWOD_Parameter(DTO);

                if (result != null && result.WorkOrderDetailID != 0)
                {

                    context.WOD_ParametersTable.Update(DTO);

                }
                else
                {
                    context.WOD_ParametersTable.Add(DTO);
                }

                await context.SaveChangesAsync();

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Task<WorkOrderDetail> CalculateValuesByID(WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteDatabase()
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            var tableNames = context.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .ToList();
            //SET FOREIGN_KEY_CHECKS = 0;
            foreach (var tableName in tableNames)
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = OFF; DELETE FROM " + tableName);
                }


            }

            //context.Database.ExecuteSqlRaw($"SET FOREIGN_KEY_CHECKS = 1;");

        }

        //public async Task<WorkOrderDetail> UpdateOfflineID(WorkOrderDetail DTO)
        //{

        //    await using var context = await DbFactory.CreateDbContextAsync();

        //    var wod = await context.WorkOrderDetail.AsNoTracking().Where(x => x.WorkOrderDetailID == DTO.WorkOrderDetailID).FirstOrDefaultAsync();

        //    wod.OfflineStatus = 1;

        //    await context.SaveChangesAsync();

        //    return wod;

        //}

        public async Task SaveConfiguredWeights(int WorkOrderDetailID, BalanceAndScaleCalibration bc)
        {
            await using var context = await DbFactory.CreateDbContextAsync();


            if (bc?.Linearities?.Count > 0)
            {
                foreach (var item in bc.Linearities)
                {

                    if (item.WeightSets.Count > 0)
                    {
                        foreach (var item2 in item.WeightSets)
                        {
                            CalibrationSubType_Weight cw = new CalibrationSubType_Weight();
                            cw.CalibrationSubTypeID = item.CalibrationSubTypeId;
                            cw.SecuenceID = item.SequenceID;
                            cw.WorkOrderDetailID = WorkOrderDetailID;
                            cw.WeightSetID = item2.WeightSetID;

                            context.CalibrationSubType_Weight.Add(cw);

                            await context.SaveChangesAsync();
                        }
                    }


                }

            }

            if (bc?.Repeatability != null)
            {


                if (bc?.Repeatability.WeightSets.Count > 0)
                {
                    foreach (var item2 in bc?.Repeatability.WeightSets)
                    {
                        CalibrationSubType_Weight cw = new CalibrationSubType_Weight();
                        cw.CalibrationSubTypeID = bc.Repeatability.CalibrationSubTypeId;
                        cw.SecuenceID = bc.Repeatability.SequenceID;
                        cw.WorkOrderDetailID = WorkOrderDetailID;
                        cw.WeightSetID = item2.WeightSetID;

                        context.CalibrationSubType_Weight.Add(cw);

                        await context.SaveChangesAsync();
                    }
                }




            }

            if (bc?.Eccentricity != null)
            {


                if (bc?.Eccentricity.WeightSets.Count > 0)
                {
                    foreach (var item2 in bc?.Repeatability.WeightSets)
                    {
                        CalibrationSubType_Weight cw = new CalibrationSubType_Weight();
                        cw.CalibrationSubTypeID = bc.Eccentricity.CalibrationSubTypeId;
                        cw.SecuenceID = bc.Eccentricity.SequenceID;
                        cw.WorkOrderDetailID = WorkOrderDetailID;
                        cw.WeightSetID = item2.WeightSetID;

                        context.CalibrationSubType_Weight.Add(cw);

                        await context.SaveChangesAsync();
                    }
                }




            }


        }


        /// <summary>
        /// this method call the bcr tespoint in wod ths have assing weights
        /// </summary>
        /// <param name="WorkOrderDetailID"></param>
        /// <param name="bc"></param>
        /// <returns></returns>
        public async Task<BalanceAndScaleCalibration> GetConfiguredWeights(int WorkOrderDetailID, BalanceAndScaleCalibration bc)
        {

            //BalanceAndScaleCalibration bc = new BalanceAndScaleCalibration();

            if (bc == null)
            {
                return bc;
            }

            await using var context = await DbFactory.CreateDbContextAsync();

            try
            {



                var ws = context.CalibrationSubType_Weight.AsNoTracking()
                    //.Include(x => x.WeightSet)
                    //.ThenInclude(x => x.UnitOfMeasure)
                    .Where(x => x.WorkOrderDetailID == WorkOrderDetailID).ToList();


                var uoms = await context.UnitOfMeasure.AsNoTracking().Where(x => x.IsEnabled == true).ToListAsync();

                foreach (var item in ws)
                {
                    var wt = await context.WeightSet.AsNoTracking().Where(x => x.WeightSetID == item.WeightSetID).FirstOrDefaultAsync();

                    wt.UnitOfMeasure = wt.UnitOfMeasureID.GetUoM(uoms);

                    item.WeightSet = wt;



                }



                Repeatability c = bc.Repeatability;
                if (bc.Repeatability != null)
                {


                    int re = c.CalibrationSubTypeId;

                    var wss = ws.Where(x => x.CalibrationSubTypeID == re).ToList();
                    if (wss != null && wss.Count > 0)
                    {
                        c.WeightSets = new List<WeightSet>();
                        foreach (var wer in wss)
                        {
                            c.WeightSets.Add(wer.WeightSet);
                        }
                    }
                }

                Eccentricity b = bc.Eccentricity;
                if (bc?.Eccentricity != null)
                {


                    int ecc = b.CalibrationSubTypeId;

                    var wsecc = ws.Where(x => x.CalibrationSubTypeID == ecc).ToList();
                    if (wsecc != null && wsecc.Count > 0)
                    {
                        b.WeightSets = new List<WeightSet>();
                        foreach (var wer in wsecc)
                        {
                            b.WeightSets.Add(wer.WeightSet);
                        }
                    }
                }

                var l = bc.Linearities;

                if (bc.Linearities != null)
                {


                    int lin = new Linearity().CalibrationSubTypeId;

                    var wslin = ws.Where(x => x.CalibrationSubTypeID == lin).ToList();
                    if (wslin != null && wslin.Count > 0)
                    {

                        foreach (var wer in l)
                        {
                            var weil = wslin.Where(x => x.SecuenceID == wer.SequenceID).ToList();
                            if (weil != null && weil.Count > 0)
                            {
                                wer.WeightSets = new List<WeightSet>();
                                foreach (var wer1 in weil)
                                {
                                    wer.WeightSets.Add(wer1.WeightSet);
                                }
                            }


                        }
                    }
                }




                var f = bc.Forces;
                if (f != null)
                {
                    int forc = new Linearity().CalibrationSubTypeId;

                    IQueryable<CalibrationSubType_Weight> query = ws.AsQueryable();

                    var wsfoc = new List<CalibrationSubType_Weight>();

                    foreach (var iy in CalibrationSaaS.Domain.Aggregates.Shared.LTILogic.GetLTICalibrationSubTypes())
                    {
                        int subtype = iy;

                        var a = ws.Where(p => p.CalibrationSubTypeID == subtype).ToList();
                        foreach (var tt in a)
                        {
                            wsfoc.Add(tt);
                        }

                    }



                    if (wsfoc != null && wsfoc.Count > 0)
                    {

                        foreach (var wer in f)
                        {
                            var weil = wsfoc.Where(x => x.SecuenceID == wer.SequenceID).ToList();
                            if (weil != null && weil.Count > 0)
                            {
                                wer.WeightSets = new List<WeightSet>();
                                foreach (var wer1 in weil)
                                {
                                    wer.WeightSets.Add(wer1.WeightSet);
                                }
                            }


                        }
                    }
                }
                bc.Forces = f;
                bc.Repeatability = c;
                bc.Eccentricity = b;
                bc.Linearities = l;
                return bc;
            } 
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<ChildrenView>> GetChildrenView(int workOrderId)
        {

            var res = await GetWorkOrderDetailChildren(workOrderId);

            List<ChildrenView> lstcv = new List<ChildrenView>();
            foreach (var item in res)
            {
                ChildrenView cv = new ChildrenView();

                //List<> list = new List<GenericCalibrationResult2>();

                var key = item.PieceOfEquipment.PieceOfEquipmentID + " - " + item.PieceOfEquipment.SerialNumber;


                //list.AddRange(item.TestPointResult);

                cv.ID = key;
                cv.TestPointResult = item.TestPointResult;

                lstcv.Add(cv);

            }

            return lstcv;

        }


        public async Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailChildren(int workOrderId)
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            var c = await context.WorkOrderDetail.AsNoTracking()
               .Include(d => d.PieceOfEquipment).ThenInclude(c => c.EquipmentTemplate).ThenInclude(d => d.Manufacturer1)
             .Include(x => x.CurrentStatus)
             .Where(x =>  x.ParentID == workOrderId).ToListAsync();



            foreach (var item in c)
            {
                var eto = await context.EquipmentType.AsNoTracking()
                    .Where(x => x.EquipmentTypeGroupID == item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeGroupID && x.HasWorkOrderDetail == true).ToListAsync();


                if (eto.Count == 1)
                {
                    item.PieceOfEquipment.EquipmentTemplate.EquipmentTypeObject = eto.FirstOrDefault();
                }
                else if (eto.Count > 1)
                {
                    item.PieceOfEquipment.EquipmentTemplate.AditionalEquipmentTypesJSON = Newtonsoft.Json.JsonConvert.SerializeObject(eto);
                }

                var testpoints = await context.GenericCalibrationResult2.AsNoTracking().Where(x=>x.ComponentID == item.WorkOrderDetailID.ToString()).ToListAsync(); //(x => x.CalibrationSubTypeId == item.Cal && x.ComponentID == item.).FirstOrDefaultAsync();
                
                if (item.BalanceAndScaleCalibration == null)
                {
                    item.BalanceAndScaleCalibration = new BalanceAndScaleCalibration();
                }
                if (item.BalanceAndScaleCalibration.TestPointResult == null)
                {
                    item.BalanceAndScaleCalibration.TestPointResult = new List<GenericCalibrationResult2>();
                }
                foreach (var test in testpoints)
                {
                    
                    GenericCalibrationResult2 gcr = new GenericCalibrationResult2();
                    gcr = test;
                    item.BalanceAndScaleCalibration.TestPointResult.Add(gcr);
                }
                    
            }




            return c;
        }

        public List<KeyValueOption> GetJSONConfigurationArray(EquipmentType eto)
        {


            if (!string.IsNullOrEmpty(eto?.JSONConfiguration))
            {

                var nvc = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValueOption>>(eto.JSONConfiguration);
                return nvc;

            }

            return new List<KeyValueOption>();
        }

       
    }
}
