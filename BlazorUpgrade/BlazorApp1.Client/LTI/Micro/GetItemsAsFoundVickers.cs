using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Micro
{
    public class GetItemsAsFoundVickers : ComponentBase, IGetItems
    {
        public async Task<List<GenericCalibration>> GetItems(WorkOrderDetail eq, int CalibrationSubTypeId)
        {

            if (eq == null || !eq.CalibrationTypeID.HasValue || eq.BalanceAndScaleCalibration == null
               || eq.BalanceAndScaleCalibration.GenericCalibration == null
               || eq.BalanceAndScaleCalibration.GenericCalibration.Count == 0
               || eq.BalanceAndScaleCalibration.GenericCalibration.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).FirstOrDefault() == null)
            {
                return null;
            }
            List<GenericCalibration> list3 = new List<GenericCalibration>();


            var items2 =await  new CreateItemsAsFoundVicker().CreateItems(eq, CalibrationSubTypeId);


            var list = eq.BalanceAndScaleCalibration.GenericCalibration
                 .Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList();


            if (items2 != null && items2.Count > 0)
            {
                foreach (var item in items2)
                {

                    var tmp = list.Where(x => x.SequenceID == item.SequenceID).FirstOrDefault();

                    if (tmp != null)
                    {
                        list3.Add(tmp);
                    }
                    else
                    {
                        list3.Add(item);
                    }
                }

                return list3;
            }
            else if (list != null && list.Count > 0)
            {
                return list;
            }

            return list3;
        }

    }
}
