using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Domain.Repositories
{
    public interface IEquipmentTemplateRepository
    {
        //Task<Manufacturer> CreateManufacturer(Manufacturer DTO);

        //Task<IEnumerable<Manufacturer>> GetAllManufacturers();

        //Task<Manufacturer> DeleteManufacturer(Manufacturer id);

        //Task UpdateManufacturer(Manufacturer DTO);


        //Task<bool> Save();

        Task<EquipmentTemplate> CreateEquipment(EquipmentTemplate EquipmentDTO);
        Task<EquipmentResultSet> GetEquipment();
        Task<EquipmentTemplate> DeleteEquipment(EquipmentTemplate EquipmentDTO);
       Task<EquipmentTemplate> GetEquipmentByID(EquipmentTemplate EquipmentDTO);

        Task<bool> Save();



    }

   
}
