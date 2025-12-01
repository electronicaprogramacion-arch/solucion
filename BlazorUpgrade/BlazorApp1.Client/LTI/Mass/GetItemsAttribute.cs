using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Interfaces;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.LTI.Mass
{
    public class GetAttributeTestBlock : ComponentBase, IGetItems<PieceOfEquipment,GenericCalibration2>
    {
        

      public async Task<List<GenericCalibration2>> GetItems(PieceOfEquipment eq, int CalibrationSubTypeId, int ihneritCalSubtypeId)
        {

            

            List<GenericCalibration2> list = new List<GenericCalibration2>();
            if(eq != null)
            {
                GenericCalibration2 cal = new GenericCalibration2();

                var result= NewMethod(eq, CalibrationSubTypeId);

                cal.TestPointResult = result;

                if (result == null || result?.Count == 0)
                {
                    
                    return null;
                }

                list.Add(cal);

                return list;
            }

            return null;
        }


        private  List<GenericCalibrationResult2> NewMethod(PieceOfEquipment eq, int CalibrationSubTypeId)
        {

            if (eq?.TestPointResult == null)
            {
                return null;
            }

            var list = eq.TestPointResult.Where(x => x.CalibrationSubTypeId == CalibrationSubTypeId).ToList(); //  new List<GenericCalibrationResult2>();

            //var gen = new GenericCalibrationResult2();

            //gen.SequenceID = 1;

            //gen.ComponentID = eq.PieceOfEquipmentID;

            //var s = gen as IResult2;

            //list.Add(gen);

            return list;
        }


    }
}
