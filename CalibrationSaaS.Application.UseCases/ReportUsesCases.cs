using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.BusinessExceptions;
using CalibrationSaaS.Domain.BusinessExceptions.Customer;
using CalibrationSaaS.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Application.UseCases
{
    public class ReportUseCases
    {
        private readonly IReportRepository Repository;
        // private readonly ILogger _logger;

        //public CustomerUseCases(ICustomerRepository customerRepository, ILogger logger)
        //{
        //    this.customerRepository = customerRepository;
        //    this._logger = logger;
        //}

        public ReportUseCases(IReportRepository _Repository)
        {
            this.Repository = _Repository;
        }

       



    }
}
