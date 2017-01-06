using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class DefaultActionAttribute : Attribute
    {
        /// <summary>
        /// Returns the name of the first public instance method declared by a class that has the <see cref="DefaultActionAttribute"/> applied.
        /// If the method has the <see cref="ActionNameAttribute"/> applied, the value specified by that attribute will be returned instead of 
        /// the methods's name.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetDefaultActionName(Type type)
        {
            MethodInfo[] methods = type.ArgumentNotNull().GetValueOrThrow().GetMethods();
            MethodInfo defaultMethod = methods.FirstOrDefault(method => method.GetCustomAttribute<DefaultActionAttribute>() != null) ?? methods.FirstOrDefault();
            return defaultMethod.GetCustomAttribute<ActionNameAttribute>()?.Name ?? defaultMethod?.Name;
        }
    }
}