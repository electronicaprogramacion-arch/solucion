using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.LaserMicrometer.AsFound
{
    public class Get: ComponentBase, IGetItems<WorkOrderDetail,GenericCalibration2>
    {
        public async Task<List<GenericCalibration2>> GetItems(WorkOrderDetail eq, int CalibrationSubTypeId, int ihneritCalSubtypeId)
        {
            if (eq == null || eq.GetCalibrationTypeID() == null || eq.BalanceAndScaleCalibration == null
                 || eq.BalanceAndScaleCalibration.TestPointResult == null || eq.BalanceAndScaleCalibration.TestPointResult.Count == 0
                 || eq.BalanceAndScaleCalibration.TestPointResult.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).FirstOrDefault() == null)
            {
                return null;
            }

            List<GenericCalibrationResult2> list3 = new List<GenericCalibrationResult2>();


            var items2 = await new CalibrationSaaS.Infraestructure.Blazor.LTI.LaserMicrometer.AsFound.Create().CreateItems(eq, CalibrationSubTypeId);


            var list = eq.BalanceAndScaleCalibration.TestPointResult
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
                        list3.AddRange(item.TestPointResult);
                    }
                }


                List<GenericCalibration2> ListGC = new List<GenericCalibration2>();
                foreach (var itemGC in list3)
                {
                    if (itemGC.GenericCalibration2 == null)
                    {
                        itemGC.GenericCalibration2 = new GenericCalibration2();

                        itemGC.GenericCalibration2.TestPointResult = new List<GenericCalibrationResult2>();

                        itemGC.GenericCalibration2.TestPointResult.Add(itemGC);
                    }

                    if (itemGC.GenericCalibration2.TestPointResult == null)
                    {


                        itemGC.GenericCalibration2.TestPointResult = new List<GenericCalibrationResult2>();

                        itemGC.GenericCalibration2.TestPointResult.Add(itemGC);
                    }
                    //itemGC.GenericCalibration2.BasicCalibrationResult.Add(itemGC);
                    ListGC.Add(itemGC.GenericCalibration2);


                }



                return ListGC;
            }
            else if (list != null && list.Count > 0)
            {

                List<GenericCalibration2> ListGC2 = new List<GenericCalibration2>();
                foreach (var itemGC in list)
                {
                    //itemGC.GenericCalibration2.BasicCalibrationResult.Add(itemGC);
                    ListGC2.Add(itemGC.GenericCalibration2);


                }



                return ListGC2;
            }

            List<GenericCalibration2> ListGC3 = new List<GenericCalibration2>();
            foreach (var itemGC in list3)
            {
                //itemGC.GenericCalibration2.BasicCalibrationResult.Add(itemGC);
                ListGC3.Add(itemGC.GenericCalibration2);


            }

            return ListGC3;
        }

        private static List<GenericCalibrationResult2> NewMethod(WorkOrderDetail eq, int CalibrationSubTypeId)
        {
            var list = eq.BalanceAndScaleCalibration.TestPointResult;//  new List<GenericCalibrationResult2>();

            //List < GenericCalibrationResult2 > list2=null;


            //if(list != null)
            //{
            //    list2 = new List<GenericCalibrationResult2>();
            //    foreach (var item in list)
            //    {
            //        list2.AddRange(item.BasicCalibrationResult);
            //    }

            //}


            //var gen = new GenericCalibrationResult2();

            //gen.SequenceID = 1;

            //gen.ComponentID = eq.PieceOfEquipmentID;

            //var s = gen as IResult2;

            //list.Add(gen);
            if (list == null)
            {
                return null;
            }
            return list.Where(x=>x.CalibrationSubTypeId== CalibrationSubTypeId).ToList();
        }


    }
}
