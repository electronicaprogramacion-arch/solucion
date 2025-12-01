using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Grpc.Helpers
{
    public class ValidatorHelper
    {
        public bool IsValid<T>(T model)
        {
            StringBuilder strValidationResults = new StringBuilder();
            ValidationContext context = new ValidationContext(model, null, null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(model, context, validationResults, true);
            if (!valid)
            {
                foreach (ValidationResult validationResult in validationResults)
                {
                    strValidationResults.AppendLine(validationResult.ErrorMessage);
                }
                throw new CalibrationSaaS.Domain.Aggregates.Querys.InvalidCalSaaSModel(strValidationResults.ToString());
            }
            return valid;
        }

         
    }


    //[Serializable]
    //public class InvalidCalSaaSModel : Exception
    //{
    //    public InvalidCalSaaSModel() { }
    //    public InvalidCalSaaSModel(string message) : base(message)
    //    {

    //    }
    //    public InvalidCalSaaSModel(string message, Exception inner) : base(message, inner)
    //    {

    //    }
    //}
}
