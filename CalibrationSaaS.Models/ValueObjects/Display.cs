using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.ValueObjects
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomDisplayAttribute : Attribute
    {
        public CustomDisplayAttribute(params string[] permission)
        {
            Permission = permission;
        }

        public string[] Permission { get; }

        public CustomDisplayAttribute(object clase)
        {

        }

        public Type Condition { get; set; }


        public string Name { get; set; }


        public void test  (object obj)
            {


            }

    }

    public class CustomDisplay2Attribute : CustomDisplayAttribute
    {
        public CustomDisplay2Attribute(params string[] permission)
        {
            Permission = permission;
        }

        public new string[] Permission { get; }

        public CustomDisplay2Attribute(object clase)
        {

        }

    }
    /// <summary>
    /// This attribute will check the Max Length of Properties/fields
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = true)]
    public class ValidRule : Attribute
    {
        public ValidRule(dynamic role)
        {
            Role = role;
        }
        public dynamic Role { get; private set; }

    }

     /// <summary>
    /// This attribute will check the Max Length of Properties/fields
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = true)]
    public class IsVisible : Attribute
    {
        public IsVisible(bool _IsVisible)
        {
            Role = _IsVisible;
        }
        public bool   Role { get;  set; }

    }

}
