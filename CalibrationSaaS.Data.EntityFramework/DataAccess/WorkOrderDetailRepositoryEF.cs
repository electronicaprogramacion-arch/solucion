using CalibrationSaaS.Data.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.EntityFramework.DataAccess
{
    public class WorkOrderDetailRepositoryEF //: IWorkOrderDetailRepository
    {

        private CalibrationSaaSDBContext context;

        public WorkOrderDetailRepositoryEF(CalibrationSaaSDBContext context)
        {
            this.context = context;
        }

        public async Task<WorkOrderDetail> GetWorkOrderDetailByID(int workOrderDetailId)
        {
            return await context.WorkOrderDetail.Where(x => x.WorkOrderDetailID == workOrderDetailId)
                .Include(d => d.BalanceAndScaleCalibration)
                .Include(d => d.BalanceAndScaleCalibration.Linearities)
                .Include(d => d.BalanceAndScaleCalibration.Linearities.Select(d1 => d1.BasicCalibrationResult))
                .Include(d => d.BalanceAndScaleCalibration.Linearities.Select(d1 => d1.BasicCalibrationResult.UnitOfMeasure))
                .Include(d => d.BalanceAndScaleCalibration.Linearities.Select(d1 => d1.BasicCalibrationResult.UnitOfMeasure.UncertaintyUnitOfMeasure))
                .Include(d => d.BalanceAndScaleCalibration.Linearities.Select(d1 => d1.WeightSets))
                .Include(d => d.BalanceAndScaleCalibration.Linearities.Select(d1 => d1.WeightSets.Select(d2 => d2.UnitOfMeasure)))
                .Include(d => d.BalanceAndScaleCalibration.Linearities.Select(d1 => d1.WeightSets.Select(d2 => d2.UnitOfMeasure.UncertaintyUnitOfMeasure)))
                .Include(d => d.BalanceAndScaleCalibration.Linearities.Select(d1 => d1.WeightSets.Select(d2 => d2.UncertaintyUnitOfMeasure)))
                .Include(d => d.BalanceAndScaleCalibration.Eccentricity.TestPointResult)
                .Include(d => d.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Select(d1 => d1.UnitOfMeasure))
                .Include(d => d.BalanceAndScaleCalibration.Eccentricity.TestPointResult.Select(d1 => d1.UnitOfMeasure.UncertaintyUnitOfMeasure))
                .Include(d => d.BalanceAndScaleCalibration.Repeatability.TestPointResult.Select(d1 => d1.UnitOfMeasure))
                .Include(d => d.BalanceAndScaleCalibration.Repeatability.TestPointResult.Select(d1 => d1.UnitOfMeasure.UncertaintyUnitOfMeasure)).FirstOrDefaultAsync();            
        }

        public async Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailByWorkOrderID(int WorkOrderId)
        {
            var res = await context.WorkOrderDetail.Where(x => x.WorkOrderID == WorkOrderId).ToArrayAsync();
            return res;
        }

        public void AttachWorkOrderDetail (WorkOrderDetail workOrderDetail)
        {
            context.WorkOrderDetail.Attach(workOrderDetail);
        }

        public void UpdateWorkOrderDetail(WorkOrderDetail workOrderDetail)
        {
            context.WorkOrderDetail.Update(workOrderDetail);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
