using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Domain.Aggregates.Querys;
using DynamicExpressions;
using Helpers.Controls.ValueObjects;
namespace CalibrationSaaS.Infraestructure.EntityFramework.Helpers
{
    public static class QueryableExtensions
    {
        //public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, Pagination pagination)
        //{
        //    return queryable
        //        .Skip((pagination.Page - 1) * pagination.Show)
        //        .Take(pagination.Show);
        //}

        //public static Expression<Func<T, object>> ToMemberOf<T>(this string name) where T : class
        //{


        //    var parameter = Expression.Parameter(typeof(T), "e");
        //    //var propertyOrField = Expression.PropertyOrField(parameter, name);
        //    var propertyOrField = name.Split('.')
        //    .Aggregate((Expression)parameter, Expression.PropertyOrField);
        //    var unaryExpression = Expression.MakeUnary(ExpressionType.Convert, propertyOrField, typeof(object));

        //    return Expression.Lambda<Func<T, object>>(unaryExpression, parameter);
        //}


        //public static async  Task<ResultSet<T>> PaginationAndFilterQuery<T> (this IQueryable<T> MainQuery, Pagination<T> pagination,
        //    IQueryable<T> SimpleQuery, Expression<Func<T, bool>> Filter) where T : class
        //{
        //    string filter = pagination.Filter;
        //    List<T> result = null;
        //    int rows = 0;

        //    var softdelete = QueryableExtensions2.PropertyExists("IsDelete", typeof(T));

        //    Expression<Func<T, bool>> predicate = null;

        //    if (softdelete)
        //    {
        //         predicate = DynamicExpressions.DynamicExpressions.GetPredicate<T>("IsDelete", FilterOperator.Equals, false);
        //    }


        //    if (pagination.Page == 1)
        //    {


        //        if (Filter==null || ((string.IsNullOrEmpty(filter) && pagination.Entity ==null) && pagination.Object==null))
        //        {
        //            try
        //            {
        //                if (softdelete)
        //                {
        //                    if (!string.IsNullOrEmpty(pagination.ColumnName))
        //                    {
        //                        var order2 = pagination.ColumnName.ToMemberOf<T>();
        //                        var contQuery = await SimpleQuery.Where(predicate).OrderBy(order2).Take(pagination.Top).CountAsync(); //Task.FromResult();

        //                        rows = contQuery;
        //                    }
        //                    else
        //                    {
        //                        var contQuery = await SimpleQuery.Where(predicate).Take(pagination.Top).CountAsync(); //Task.FromResult();

        //                        rows = contQuery;
        //                    }
                            
                           
        //                }
        //                else
        //                {
        //                    if (!string.IsNullOrEmpty(pagination.ColumnName))
        //                    {
        //                        var order2 = pagination.ColumnName.ToMemberOf<T>();
        //                        var count = await SimpleQuery.OrderBy(order2).Take(pagination.Top).CountAsync(); //Task.FromResult();
        //                        rows = count;
        //                    }
        //                    else
        //                    {
        //                        var count = await SimpleQuery.Take(pagination.Top).CountAsync(); //Task.FromResult();
        //                        rows = count;
        //                    }
                               
        //                }
        //            }catch(Exception ex)
        //            {
        //                throw new Exception("--------------------------------Error in NO filter implementation, please review--------------------------------------------------------");
        //            }
                    
        //        }
        //        else
        //        {
        //            try
        //            {
        //                if (softdelete)
        //                {

        //                    if (!string.IsNullOrEmpty(pagination.ColumnName))
        //                    {
        //                        var order2 = pagination.ColumnName.ToMemberOf<T>();
        //                        rows = await SimpleQuery
        //                        .Where(Filter).Where(predicate).OrderBy(order2).Take(pagination.Top)
        //                        .CountAsync();
        //                    }
        //                    else
        //                    {
        //                        rows = await SimpleQuery
        //                        .Where(Filter).Where(predicate).Take(pagination.Top)
        //                        .CountAsync();
        //                    }


                            
        //                }
        //                else
        //                {
        //                    if (!string.IsNullOrEmpty(pagination.ColumnName))
        //                    {
        //                        var order2 = pagination.ColumnName.ToMemberOf<T>();
        //                        rows = await SimpleQuery
        //                        .Where(Filter).OrderBy(order2).Take(pagination.Top)
        //                        .CountAsync();
        //                    }
        //                    else
        //                    {
        //                        rows = await SimpleQuery
        //                        .Where(Filter).Take(pagination.Top)
        //                        .CountAsync();
        //                    }

                             

        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("--------------------------------Error in filter implementation, please review--------------------------------------------------------");
        //            }
        //        }
        //    }


        //    if (Filter == null || ((string.IsNullOrEmpty(filter) && pagination.Entity == null) && pagination.Object == null))
        //    {
        //        var queryable = MainQuery; //context.Include(x => x.EquipmentTemplate).Include(d => d.Customer).AsQueryable();
        //        if (!string.IsNullOrEmpty(pagination.ColumnName))
        //        {
        //            var order2 = pagination.ColumnName.ToMemberOf<T>();
        //            if (pagination.SortingAscending)
        //            {
        //                if (softdelete)
        //                {
        //                    queryable = queryable.Where(predicate).OrderBy(order2);
        //                    //queryable = queryable.OrderBy(p => EF.Property<object>(p, pagination.ColumnName));
        //                }
        //                else
        //                {
        //                    queryable = queryable.OrderBy(order2);
        //                    //queryable = queryable.OrderBy(p => EF.Property<object>(p, pagination.ColumnName));
        //                }

        //            }
        //            else
        //            {
        //                if (softdelete)
        //                {
        //                    queryable = queryable.Where(predicate).OrderByDescending(order2);
        //                }
        //                else
        //                {
        //                    queryable = queryable.OrderByDescending(order2);
        //                }
                            
        //                //queryable = queryable.OrderByDescending(p => EF.Property<object>(p, pagination.ColumnName));
        //            }
        //        }
        //        result = await queryable.Paginar(pagination).Take(pagination.Top).ToListAsync();
        //    }
        //    else
        //    {
        //        IQueryable<T> queryable = null;

        //        if (softdelete)
        //        {
        //            queryable = MainQuery
        //            .Where(Filter).Where(predicate)
        //            .AsQueryable();
        //        }
        //        else
        //        {
        //            queryable= MainQuery
        //            .Where(Filter)
        //            .AsQueryable();
        //        }

        //        if (!string.IsNullOrEmpty(pagination.ColumnName))
        //        {
        //            var order2 = pagination.ColumnName.ToMemberOf<T>();
        //            if (pagination.SortingAscending)
        //            {
        //                queryable = queryable.OrderBy(order2);
        //            }
        //            else
        //            {
        //                queryable = queryable.OrderByDescending(order2);
        //            }
        //        }
        //        result = await queryable.Paginar(pagination).Take(pagination.Top).ToListAsync();
        //    }

        //    int total = QueryableExtensions2.GetTotalPages(rows, pagination.Show);

        //    return new ResultSet<T>
        //    {
        //        CurrentPage = pagination.Page,
        //        List = result,
        //        Count = rows,
        //        PageTotal = total,
        //        ColumnName = pagination.ColumnName,
        //        SortingAscending = pagination.SortingAscending,
                
        //    };
        //}

        //public static TOutput SelectSingle<TInput, TOutput>(
        //this TInput obj,
        //Expression<Func<TInput, TOutput>> expression)
        //{
        //    return expression.Compile()(obj);
        //}

    }
}
