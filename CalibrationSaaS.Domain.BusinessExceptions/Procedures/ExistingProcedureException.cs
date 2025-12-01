using CalibrationSaaS.Domain.Aggregates.Entities;
using System;

namespace CalibrationSaaS.Domain.BusinessExceptions.Procedures
{
    [Serializable]
    public class ExistingProcedureException : Exception
    {
        public string Identity;
        public ExistingProcedureException() { }
        public ExistingProcedureException(string message) : base(message)
        {

        }
        public ExistingProcedureException(string message, Exception inner) : base(message, inner)
        {

        }

        public ExistingProcedureException(string message, Exception inner, Procedure entity) : base(message, inner)
        {
            this.Identity = entity.ProcedureID.ToString();
        }
    }
}
