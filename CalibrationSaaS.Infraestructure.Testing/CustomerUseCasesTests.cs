using CalibrationSaaS.Application.UseCases;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CalibrationSaaS.Infraestructure.Testing
{
    public class CustomerUseCasesTests
    {
        private readonly CustomerUseCases _sud;
        private readonly Mock<ICustomerRepository> _customerRepoMoc =  new Mock<ICustomerRepository>();
        private readonly Mock<ILogger> _logMoc = new Mock<ILogger>();

        public CustomerUseCasesTests()
        {
            //this._sud = new CustomerUseCases(_customerRepoMoc.Object, _logMoc.Object);
            //this._sud = new CustomerUseCases(_customerRepoMoc.Object);
        }

        //[Fact]
        //public async Task GetCustomer_ValidCall()
        //{
        //    TenantDTO tenant = new TenantDTO
        //    {
        //        TenantID = 1
        //    };

        //    //Arrange
        //    _customerRepoMoc.Setup(x => x.GetCustomers()).ReturnsAsync(GetSampleCustomers());
            
            //var expected = GetSampleCustomers();

            ////Act
            //var actual = (List<Customer>)(await _sud.GetCustomer());

            ////Assert
            //Assert.True(actual != null);
            //Assert.Equal(expected.Count, actual.Count);
            //for(int i =0; i< expected.Count; i++)
            //{
            //    Assert.Equal(expected[i].Name, actual[i].Name);
            //}
               
        //}

        private List<Customer> GetSampleCustomers()
        {
            return new List<Customer> {
                new Customer
                {
                
                    CustomerID = 1,
                    Name = "Springfield inc",
                    PieceOfEquipment = null,
                    TenantId = 1,
                    WorkOrder = null            
                }
            };
        }
    }
}
