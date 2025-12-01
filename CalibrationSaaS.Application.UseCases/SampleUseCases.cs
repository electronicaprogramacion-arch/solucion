using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;
using CalibrationSaaS.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    public class SampleUseCases
    {
        private readonly ICustomerRepository customerRepository;
        // private readonly ILogger _logger;

        //public CustomerUseCases(ICustomerRepository customerRepository, ILogger logger)
        //{
        //    this.customerRepository = customerRepository;
        //    this._logger = logger;
        //}

        public SampleUseCases(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        //A DTO is also a Value Object, however a DTO has an integrity meaning you should not modify its properties while in a Value Object it is accepted.
        public async Task<int> CreateCustomer(CalibrationSaaS.Domain.Aggregates.Entities.Customer customerDTO)
        {
            var doesCustomerExist = await this.customerRepository.GetCustomerByName(customerDTO.Name);
            if (doesCustomerExist != null)
            {
                // throw new ExistingCustomerException("This Customer is already in", null, doesCustomerExist);
                throw new ExistingRecordException<Customer>("This Customer is already in", null, doesCustomerExist);
            }
            customerDTO.CustomerID = 0;
            this.customerRepository.InsertCustomer(customerDTO);
            await this.customerRepository.Save();
            //_logger.LogInformation(string.Format("Customer successfully added: {0} Id: {1}", customerDTO.CustomerName, customerDTO.CustomerID));
            return customerDTO.CustomerID;
        }

        public async Task<IEnumerable<CalibrationSaaS.Domain.Aggregates.Entities.Customer>> GetCustomer(TenantDTO tenantID)
        {
            return null;// await customerRepository.GetCustomers();
        }

        public async Task<IEnumerable<WorkOrderDetail>> GetWorkOrderDetailXIdRep(int id)
        {
            List<WorkOrderDetail> wodList = new List<WorkOrderDetail>();

            return wodList;
            //return await customerRepository.GetCustomers();
        }
    }
}
