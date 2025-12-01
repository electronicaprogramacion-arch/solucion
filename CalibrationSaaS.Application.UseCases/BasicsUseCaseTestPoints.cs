using CalibrationSaaS.Domain.Aggregates.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
   public   partial class BasicsUseCases
    {


       public async  Task<IEnumerable<TestPointGroup>> GetTespointGroup(EquipmentTemplate EquipmentDTO)
        {
           return await  basicsRepository.GetTespointGroup(EquipmentDTO);

        }

        public async Task<IEnumerable<TestPoint>> GetTespoint(TestPointGroup DTO)
        {
            return await basicsRepository.GetTespoint(DTO);
        }

       public async  Task<IEnumerable<TestPointGroup>> GetTespointByEquipment(EquipmentTemplate EquipmentDTO)
        {

            return await basicsRepository.GetTespointByEquipment(EquipmentDTO);

        }

        public async Task<TestPointGroup> InsertTestPointGroup(TestPointGroup DTO)
        {

            try
            {

                if (DTO != null && DTO.TestPoints != null && DTO.TestPoints.Count > 0)
                {
                    if (DTO.TestPoints != null && DTO.TestPoints.Count > 0)
                    {
                        await basicsRepository.InsertTestPointGroup(DTO);

                    //if (DTO.TestPoints != null && DTO.TestPoints.Count > 0)
                    //{
                    //    foreach (var item in DTO.TestPoints)
                    //    {
                    //        await basicsRepository.InsertTestPoint(item);

                    //    }
                    }

                    await basicsRepository.Save();

                    return DTO;
                }
                else
                {
                    return null;

                }


            }
            catch(Exception ex)
            {
                throw ex;
            }

        }



    }
}
