using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataService.Core.Data
{
    public static class AttributeHelper
    {
        public static List<string> GetDataNames(Type type, string propertyName)
        {
            var property = type
                           .GetProperty(propertyName)
                           .GetCustomAttributes(false)
                           .Where(x => x.GetType().Name == "DataNamesAttribute")
                           .FirstOrDefault();

            if (property != null)
            {
                return ((DataNamesAttribute)property).ValueNames;
            }
            return new List<string>();
        }
    }
}
