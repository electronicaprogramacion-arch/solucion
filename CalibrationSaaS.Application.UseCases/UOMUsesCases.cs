using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;
using CalibrationSaaS.Domain.BusinessExceptions.Customer;
using CalibrationSaaS.Domain.Repositories;
using Helpers.Controls.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    public class UOMUseCases
    {
        private readonly IUOMRepository Repository;
        // private readonly ILogger _logger;

        //public CustomerUseCases(ICustomerRepository customerRepository, ILogger logger)
        //{
        //    this.customerRepository = customerRepository;
        //    this._logger = logger;
        //}

        public UOMUseCases(IUOMRepository _Repository)
        {
            this.Repository = _Repository;
        }


        public async Task<UnitOfMeasure> Update(UnitOfMeasure DTO)
        {
            return await Repository.Update(DTO);
        }

            public async  Task<UnitOfMeasure> Create(UnitOfMeasure DTO)
        {
            UnitOfMeasure a;
            if (await Exists(DTO))
            {
                //a = await Repository.Update(DTO);
                throw new UOMExistingException();


            }
            else
            {
                 a = await Repository.Create(DTO);
                

            }


            if(await Repository.Save())
            {
                return a;

            }
            else
            {
                throw new UOMNotSaveException("Not Save") { Entity = DTO};

            }





        }

        public async Task<UnitOfMeasure> Delete(UnitOfMeasure DTO)
        {
            return await Repository.Delete(DTO);
        }

        public async  Task<IEnumerable<UnitOfMeasure>> GetAll()
        {
            var a= await Repository.GetAll();

           return  a;

        }

        public async Task<ResultSet<UnitOfMeasure>> GetAllPag(Pagination<UnitOfMeasure> pagination)
        {
            var a = await Repository.GetAllPag(pagination);

            return a;

        }



        public async Task<IEnumerable<UnitOfMeasure>> GetAllEnabled()
        {
            var a = await Repository.GetAll();

            return a.Where(x => x.IsEnabled == true).ToList();
        }


        public async  Task<UnitOfMeasure> GetByID(UnitOfMeasure DTO)
        {
            return await Repository.GetByID(DTO);
        }

        public async Task<IEnumerable<UnitOfMeasure>> GetByType(UnitOfMeasureType Type)
        {
            return await Repository.GetByType(Type);
        }


        public async Task<bool> Exists(UnitOfMeasure DTO)
        {

            var b = await Repository.GetAll();

            var a =  b.Where(x => x.UnitOfMeasureID == DTO.UnitOfMeasureID
           || x.Name == DTO.Name || x.Abbreviation == DTO.Abbreviation).FirstOrDefault();

            if (a == null)
            {
                return false;
            }
            else
            {
                return true;
            }


        }


        public async  Task<ICollection<UnitOfMeasureType>> GetTypes()
        {
            return await Repository.GetTypes();
        }



        public async Task<UnitOfMeasureType> UpdateType(UnitOfMeasureType DTO)
        {
            return await Repository.UpdateType(DTO);
        }



        public async Task<UnitOfMeasureType> CreateType(UnitOfMeasureType DTO)
        {
            return await Repository.CreateType(DTO);
        }



    }
}
