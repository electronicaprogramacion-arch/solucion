using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.Helpers;
using Helpers;
using Helpers.Controls.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class UOMRepositoryEF<TContext> : IUOMRepository, IDisposable where TContext : DbContext, ICalibrationSaaSDBContextBase 
    {


        private readonly IDbContextFactory<TContext> DbFactory;


        public UOMRepositoryEF(IDbContextFactory<TContext> dbFactory)
        {
            DbFactory = dbFactory;
        }


        public async Task<UnitOfMeasure> Update(UnitOfMeasure DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            DTO.UncertaintyUnitOfMeasure = null;
            DTO.UnitOfMeasureBase = null;
            DTO.Type = null;
            context.UnitOfMeasure.Update(DTO);
            await context.SaveChangesAsync();
            DTO.UncertaintyUnitOfMeasure = null;
            return DTO;
        }

        public async  Task<UnitOfMeasure> Create(UnitOfMeasure DTO)
        {
            //if (DTO.UnitOfMeasureBase != null)
            //{
            //    context.Entry<UnitOfMeasure>(DTO.UnitOfMeasureBase).State = EntityState.Unchanged;
            //}

            //if (DTO.UncertaintyUnitOfMeasure != null)
            //{
            //    context.Entry<UnitOfMeasure>(DTO.UncertaintyUnitOfMeasure).State = EntityState.Unchanged;
            //}

            //if(DTO.UncertaintyUnitOfMeasure != null)
            //{

            //}
            await using var context = await DbFactory.CreateDbContextAsync();

            DTO.NullableCollection = true;
            var dto1 =(UnitOfMeasure) DTO.CloneObject();

            context.UnitOfMeasure.Add(dto1);
            await context.SaveChangesAsync();
            DTO.NullableCollection = false;
            dto1.NullableCollection = false;
            return DTO;
           
        }

        public async Task<UnitOfMeasure> Delete(UnitOfMeasure DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.Entry(DTO).State=EntityState.Deleted;
            await context.SaveChangesAsync();

            return DTO;
        }


        public async Task<IEnumerable<UnitOfMeasure>> GetHeaderAll()
        {

            await using var context = await DbFactory.CreateDbContextAsync();

            var a = await context.UnitOfMeasure.AsNoTracking().Where(x => x.IsEnabled).ToListAsync();

            return a;
        }

            public async Task<IEnumerable<UnitOfMeasure>> GetAll()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = (from b in context.UnitOfMeasure.AsNoTracking().Include(x => x.UnitOfMeasureBase)
                     join c in context.UnitOfMeasure.AsNoTracking() on (int?)b.UncertaintyUnitOfMeasureID equals c.UnitOfMeasureID
                     into d
                     from pc in d.DefaultIfEmpty()
                     where b.IsEnabled == true
                     select new UnitOfMeasure
                     {
                         UnitOfMeasureID =
                         b.UnitOfMeasureID,
                         UnitOfMeasureBase = b.UnitOfMeasureBase,
                         UncertaintyUnitOfMeasureID = b.UncertaintyUnitOfMeasureID,
                         UncertaintyUnitOfMeasure =  form(pc),
                         Abbreviation = b.Abbreviation,
                         ConversionValue = b.ConversionValue,
                         Description = b.Description,
                         IsEnabled = b.IsEnabled,
                         Name = b.Name,
                         TypeID = b.TypeID,
                         Type=b.Type,
                         NullableCollection=false
                     }).AsNoTracking();



            var result= await a.ToListAsync();

            if(result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    var ty = await context.UnitOfMeasureType.AsNoTracking().Where(x=>x.Value==item.TypeID).FirstOrDefaultAsync();

                    item.Type = ty; 
                }
            }

            return result;
        }



        public async Task<ResultSet<UnitOfMeasure>> GetAllPag(Pagination<UnitOfMeasure> pagination)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = (from b in context.UnitOfMeasure.AsNoTracking().Include(x => x.UnitOfMeasureBase)
                     join c in context.UnitOfMeasure.AsNoTracking() on (int?)b.UncertaintyUnitOfMeasureID equals c.UnitOfMeasureID
                     into d
                     from pc in d.DefaultIfEmpty()
                     where b.IsEnabled == true
                     select new UnitOfMeasure
                     {
                         UnitOfMeasureID =
                         b.UnitOfMeasureID,
                         UnitOfMeasureBase = b.UnitOfMeasureBase,
                         UncertaintyUnitOfMeasureID = b.UncertaintyUnitOfMeasureID,
                         UncertaintyUnitOfMeasure = form(pc),
                         Abbreviation = b.Abbreviation,
                         ConversionValue = b.ConversionValue,
                         Description = b.Description,
                         IsEnabled = b.IsEnabled,
                         Name = b.Name,
                         TypeID = b.TypeID,
                         Type = b.Type,
                         NullableCollection = false


                     }).AsNoTracking().AsQueryable();


            var filterQuery = Querys.UOMFilter(pagination.Filter);

            var queriable = a;//context.PieceOfEquipment.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();

            var simplequery = a;//context.UnitOfMeasure;

            //var result = await QueryableExtensions.PaginationAndFilterQuery<PieceOfEquipment>(exprTree, pagination, context.PieceOfEquipment, queriable);
            var result1 = await queriable.PaginationAndFilterQuery<UnitOfMeasure>( pagination, simplequery, filterQuery);

            var result =  result1.List;

            if (result != null && result.Count > 0)
            {
                foreach (var item in result)
                {
                    var ty = await context.UnitOfMeasureType.AsNoTracking().Where(x => x.Value == item.TypeID).FirstOrDefaultAsync();

                    item.Type = ty;
                }
            }
            result1.List = result;

            return result1;


          
        }


        private static UnitOfMeasure form(UnitOfMeasure obj)
        {

            if (obj == null)
            {
                return null;
            }
            if(obj.UnitOfMeasureBase != null)
            {
                 obj.UnitOfMeasureBase.UnitOfMeasureBase = null;
                 obj.UnitOfMeasureBase.UncertaintyUnitOfMeasure = null;
            }
           
            if(obj.UncertaintyUnitOfMeasure != null)
            {
                obj.UncertaintyUnitOfMeasure.UncertaintyUnitOfMeasure = null;
           
                obj.UncertaintyUnitOfMeasure.UnitOfMeasureBase = null;
            }
            

            return obj;

        }

        public async Task<UnitOfMeasure> GetByID(UnitOfMeasure DTO)
        {
            if (DTO != null)
            {
                await using var context = await DbFactory.CreateDbContextAsync();
                var a = await context.UnitOfMeasure.AsNoTracking().
                    Include(x => x.UnitOfMeasureBase).Where(x => x.IsEnabled == true && x.UnitOfMeasureID == DTO.UnitOfMeasureID).
                    AsNoTracking().FirstOrDefaultAsync();

                if (a != null && a.UncertaintyUnitOfMeasureID != null && a.UncertaintyUnitOfMeasureID > 0)
                {
                    var un = await context.UnitOfMeasure.AsNoTracking().
                            Where(x => x.IsEnabled == true && x.UncertaintyUnitOfMeasureID == a.UncertaintyUnitOfMeasureID).AsNoTracking().FirstOrDefaultAsync();
                    a.UncertaintyUnitOfMeasure = un;

                }
                return a;

            }
            else
            {
                return null;
            }
        }
        public async Task<IEnumerable<UnitOfMeasure>> GetByType(UnitOfMeasureType DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.UnitOfMeasure.Where(x => x.IsEnabled == true && x.TypeID == DTO.Value).AsNoTracking().ToListAsync(); 
            return a;
        }

        public async  Task<ICollection<UnitOfMeasureType>> GetTypes()
        {
            if(DbFactory == null)
            {
                return null;
            }
            await using var context = await DbFactory.CreateDbContextAsync();

            var a = await context.UnitOfMeasureType.AsNoTracking().ToListAsync();
            return a;
        }

        public async Task<UnitOfMeasureType> CreateType(UnitOfMeasureType DTO )
        {

            await using var context = await DbFactory.CreateDbContextAsync();
            try {
                //DTO.Value = 0;

            context.UnitOfMeasureType.Add(DTO);
            await context.SaveChangesAsync();
            return DTO;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UnitOfMeasureType> UpdateType(UnitOfMeasureType DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            context.UnitOfMeasureType.Update(DTO);
            await context.SaveChangesAsync();
            return DTO;

        }




        public async  Task<bool> Save()
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            try
            {
                bool res = (await context.SaveChangesAsync()) > 0;
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> Exists(UnitOfMeasure DTO)
        {
            await using var context = await DbFactory.CreateDbContextAsync();
            var a = await context.UnitOfMeasure.AsNoTracking().Where(x =>  x.UnitOfMeasureID == DTO.UnitOfMeasureID 
            || x.Name==DTO.Name || x.Abbreviation == DTO.Abbreviation).FirstOrDefaultAsync();

            if (a == null)
            {
                return false;
            }
            else
            {
                return true;
            }

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
    }
}
