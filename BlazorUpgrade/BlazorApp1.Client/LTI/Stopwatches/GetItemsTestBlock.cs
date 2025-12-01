using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Stopwatches
{
    public class GetItemsTestBlock : ComponentBase, IGetItems<PieceOfEquipment,GenericCalibration2>
    {
        

      public async Task<List<GenericCalibration2>> GetItems(PieceOfEquipment eq, int CalibrationSubTypeId, int ihneritCalSubtypeId)
        {
            List<GenericCalibration2> list = new List<GenericCalibration2>();
            if(eq != null)
            {
                GenericCalibration2 cal = new GenericCalibration2();

                var result= NewMethod(eq);

                cal.TestPointResult = result;

                if (result == null)
                {
                    return null;
                }

                list.Add(cal);

                return list;
            }

            return null;
        }


        private static List<GenericCalibrationResult2> NewMethod(PieceOfEquipment eq)
        {
            var list = eq.TestPointResult;

          

            return list;
        }


    }
}
