using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Security
{
    public class AccesRequirement : IAuthorizationRequirement
    {
        public string Requirement { get; set; }

        public AccesRequirement(string companyDomain,ICollection<Helpers.Controls.Component> _components)
        {
            Requirement = companyDomain;

            Components = _components;
        }


        public ICollection<Helpers.Controls.Component> Components { get; set; }

        public AccesRequirement()
        {
            //Requirement = companyDomain;
        }

    }

    public class AccesRequirement2 : IAuthorizationRequirement
    {
       

    }


    public class TechRequirement : IAuthorizationRequirement
    {
        public string Requirement { get; }

        public TechRequirement(string companyDomain)
        {
            Requirement = companyDomain;
        }
    }
}
