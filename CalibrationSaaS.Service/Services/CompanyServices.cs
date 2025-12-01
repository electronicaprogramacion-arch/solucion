using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Grpc.Helpers;
using ProtoBuf.Grpc;

namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class CompanyServices : ICompanyServices
    {
        public CompanyServices()
        {
        }

        public async  ValueTask<User> Create(User userDTO)
        {
            return userDTO;
        }

        public async ValueTask<User> Delete(User userDTO)
        {
            return userDTO;
        }

        public async  ValueTask<UserResultSet> GetAll()
        {
            List<User> result = new List<User> {
                new User
                {
                    UserID = 23,
                    Name = "Pedro",
                    LastName="Perz",
                    IsEnabled = true
                },
                 new User
                {
                    UserID = 23,
                    Name = "Jhonn",
                    LastName="Doe",
                    IsEnabled = true
                } };

            return new UserResultSet { Users = result };
        }

        public async ValueTask<User> GetByID(User DTO)
        {
            return new User
            {
                UserID = 23,
                Name = "Jhonn",
                LastName = "Doe",
                IsEnabled = true
            };
        }

        public async ValueTask<UserResultSet> GetUsersByCustomer(Customer CustomerDTO)
        {

            List<Rol> roles = new List<Rol>();

            roles.Add(new Rol { RolID = 1, Name = "admin" });
            roles.Add(new Rol { RolID = 2, Name = "tech" });

            List<User> result = new List<User> {
                new User
                {
                    UserID = 11,
                    Name = "Pedro",
                    LastName="Perz",
                    IsEnabled = true
                },
                 new User
                {
                    UserID = 12,
                    Name = "Jhonn",
                    LastName="Doe",
                    IsEnabled = true,
                    RolesList= roles
                } };

            return new UserResultSet { Users = result };
        }

        public ValueTask<UserResultSet> GetUsersByType()
        {
            throw new NotImplementedException();
        }
    }
}
