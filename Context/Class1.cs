using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Views;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;


namespace CalibrationSaaS.Data.EntityFramework
{

    public partial class CalibrationSaaSDBContextBase : DbContext 
    {

        public CalibrationSaaSDBContextBase(DbContextOptions<CalibrationSaaSDBContextBase> options)
          : base(options)
        {

        }
        public CalibrationSaaSDBContextBase()
        {

        }


        /// <summary>
        /// Modfied from https://stackoverflow.com/a/18870315
        /// </summary>
        /// <param name="path"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        //public bool PropertyExists(string path, Type t)
        //{
        //    if (string.IsNullOrEmpty(path))
        //        return false;

        //    var pp = path.Split('.');

        //    foreach (var prop in pp)
        //    {
        //        if (int.TryParse(prop, out var result))
        //        {
        //            /* skip array accessors */
        //            continue;
        //        }

        //        var propInfo = t.GetMember(prop)
        //            .Where(p => p is PropertyInfo)
        //            .Cast<PropertyInfo>()
        //            .FirstOrDefault();

        //        if (propInfo != null)
        //        {
        //            t = propInfo.PropertyType;

        //            if (t.GetInterfaces().Contains(typeof(IEnumerable)) && t != typeof(string))
        //            {
        //                t = t.IsGenericType
        //                    ? t.GetGenericArguments()[0]
        //                    : t.GetElementType();

        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}


        //public override EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class
        //{
        //    if (entity == null)
        //    {
        //        throw new System.ArgumentNullException(nameof(entity));
        //    }

        //    var type = entity.GetType();
        //    var et = this.Model.FindEntityType(type);
        //    var key = et.FindPrimaryKey();

        //    var keys = new object[key.Properties.Count];
        //    var x = 0;
        //    foreach (var keyName in key.Properties)
        //    {
        //        var keyProperty = type.GetProperty(keyName.Name, BindingFlags.Public | BindingFlags.Instance);
        //        keys[x++] = keyProperty.GetValue(entity);
        //    }

        //    var originalEntity = Find(type, keys);
        //    if (originalEntity != null && Entry(originalEntity).State == EntityState.Modified)
        //    {
        //        return base.Update(entity);
        //    }
        //    else
        //    {

        //    }

        //    if(originalEntity != null) 
        //    { 
        //    Entry(originalEntity).CurrentValues.SetValues(entity);
        //    }
        //    return Entry((TEntity)originalEntity);
        //}




        





        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        //public static explicit operator CalibrationSaaSDBContextBase(DbContextOptions<EntityFramework.CalibrationSaaSDBContextBase> v)
        //{
        //    throw new NotImplementedException();
        //}
    }




    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("server=.;database=myDb;trusted_connection=true;");
    //}



}
