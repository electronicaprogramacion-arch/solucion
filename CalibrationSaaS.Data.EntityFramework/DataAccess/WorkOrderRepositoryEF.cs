using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Helpers;
namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class WorkOrderRepositoryEF : IWorkOrderRepository, IDisposable
    {
        private CalibrationSaaSDBContext context;

        public WorkOrderRepositoryEF(CalibrationSaaSDBContext context)
        {
            this.context = context;
        
        }

        public async Task<WorkOrder> DeleteWorkOrder(int workOrderId)
        {
            WorkOrder result = await context.WorkOrder.FirstOrDefaultAsync(Querys.WorkOrderByID(workOrderId));
            
            context.WorkOrder.Remove(result);
            
            return result;
        }

        public async Task<WorkOrder> GetWorkOrderByID(int workOrderId)
        {
            return await context.WorkOrder.Include(x=>x.WorkOrderDetails).FirstOrDefaultAsync(Querys.WorkOrderByID(workOrderId));
        }





     





        public async Task<IEnumerable<WorkOrder>> GetWorkOrder()
        {
        
            return await context.WorkOrder.OrderBy(x => x.WorkOrderId).ToListAsync();
        }

        
        public async Task<WorkOrder> InsertWokOrder(WorkOrder workOrder)       
        
        {
        
            workOrder.WorkOrderId = NumericExtensions.GetUniqueID();
            

            context.WorkOrder.Add(workOrder);
            
            await context.SaveChangesAsync();


            
            
            //foreach (WorkOrderDetail wod in  workOrder.WorkOrderDetails)
            
            //{
            
            //    wod.WorkOrderDetailID = 0;
            
            //    wod.WorkOderID = workOrder.WorkOrderId;
            
            
            
            //    context.WorkOrderDetail.Add(wod);
            
            //    await context.SaveChangesAsync();
            
            //}


           




            
            return workOrder;
        }

      

        public async Task<WorkOrder> UpdateWorkOrder(WorkOrder workOrder)
        
        {
        
            //context.Entry(newWorkOrder).State = EntityState.Modified;
            
            
            //context.SaveChanges();
            
            context.WorkOrder.Update(workOrder);
            
            //context.Entry(a).State= EntityState.Modified;
            
            await context.SaveChangesAsync();
            
            return workOrder;
        }


        
        public async Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailXWorkOrder(int id)
        
        {
        
            var res = await context.WorkOrderDetail.Where(Querys.WorkOrderDetailByWorkOrderID(id)).ToArrayAsync();
            
            return res;
        }



        
        public async Task<bool> Save()
        
        {
        
            return (await context.SaveChangesAsync()) > 0;
        }



        
        //public async Task<WorkOrder> GetWorByName(string name, int tenant)
        
        //{
        
        //    return await context.Customer.FirstOrDefaultAsync(c => c.CustomerName.Equals(name) && c.TenantId == tenant);
        
        //}




        
        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
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










