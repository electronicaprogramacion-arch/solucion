using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace CalibrationSaaS.Infraestructure.Blazor.Helper
{
    public class CustomValidator
    {

        public static T GetAttributeFrom<T>(object instance, string propertyName) where T : Attribute
        {
            var attributeType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            if (property == null) return default(T);
            return (T)property.GetCustomAttributes(attributeType, false).FirstOrDefault();
        }

        public Dictionary<string, string> EmptyValidationDictionary { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> EmptyValidationMessage { get; set; } = new Dictionary<string, string>();

        public string GetErrorMessage(string PropertyName)
        {
            if (EmptyValidationMessage.ContainsKey(PropertyName))
            {
                return EmptyValidationMessage[PropertyName];
            }
            else
            {
                return "";
            }
        }


        public void Ini(object currentEdit)
        {
            EmptyValidationDictionary = currentEdit.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                  .ToDictionary(prop => prop.Name, prop => "valid");


            foreach (KeyValuePair<string, string> item in EmptyValidationDictionary)
            {
                var attributevalue = GetAttributeFrom<RequiredAttribute>(currentEdit, item.Key);
                if (attributevalue != null)
                {
                    EmptyValidationMessage.Add(item.Key, attributevalue.ErrorMessage);

                }
            }

        }

    }



}
