using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace CalibrationSaaS.Infraestructure.Blazor.GenericMethods
{
    public class GetCapacityBased : ComponentBase, IGetItems<WorkOrderDetail, GenericCalibration2>
    {
        public async Task<List<GenericCalibration2>> GetItems(WorkOrderDetail eq, int CalibrationSubTypeId, int ihneritCalSubtypeId)
        {

            List<GenericCalibration2> ListGC3 = new List<GenericCalibration2>();
            if (eq != null && eq.PieceOfEquipment != null && eq.PieceOfEquipment.TestPointResult != null && eq.PieceOfEquipment.TestPointResult.Count() > 0 && (eq.TestPointResult == null || eq.TestPointResult.Count()==0)  && CalibrationSubTypeId ==1002)
            {
                var listPoe = eq.PieceOfEquipment.TestPointResult;
                
                
                foreach (var itemGC in listPoe.Where(y=>y.CalibrationSubTypeId == 1001))
                {
                    var newItem1 = DeepClone(itemGC);
                    var newItem = newItem1.CreateNew(newItem1); 
                    newItem.CalibrationSubTypeId = CalibrationSubTypeId;
                    newItem.Component = "WorkOrderItem";
                    newItem.TestPointResult.FirstOrDefault().CalibrationSubTypeId = CalibrationSubTypeId;
                    newItem.TestPointResult.FirstOrDefault().ComponentID = eq.WorkOrderDetailID.ToString();
                    newItem.Component = "WorkOrderItem";
                    ListGC3.Add(newItem);
                }

            }
            else
            {
                if (eq == null || eq.GetCalibrationTypeID() == null || eq.BalanceAndScaleCalibration == null
                    || eq.BalanceAndScaleCalibration.TestPointResult == null || eq.BalanceAndScaleCalibration.TestPointResult.Count == 0
                    || eq.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).FirstOrDefault() == null)
                {
                    return null;
                }



                var list = eq.BalanceAndScaleCalibration.TestPointResult
                     .Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList();


               
                foreach (var itemGC in list)
                {
                    if (itemGC.GenericCalibration2 == null)
                    {

                        ListGC3.Add(itemGC.CreateNew(itemGC));
                    }
                    else
                    {
                        itemGC.GenericCalibration2.TestPointResult = new List<GenericCalibrationResult2>();

                        itemGC.GenericCalibration2.TestPointResult.Add(itemGC);
                        ListGC3.Add(itemGC.GenericCalibration2);
                    }
                    //itemGC.GenericCalibration2.BasicCalibrationResult.Add(itemGC);


                }
            }
            return ListGC3;
        }

        public static T DeepClone<T>(T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(json);
        }

    }
}
