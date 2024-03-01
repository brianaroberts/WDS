using System.Data;
using System.Reflection;
using WDS.Core.Extensions;
using WDS.Models;
using static WDSClient.Models.WDSEnums;

namespace DataService.Core.Data
{
    public static class PropertyMapHelper
    {
        public static void Map(Type type, DataRow row, PropertyInfo prop, object entity)
        {
            List<string> columnNames = AttributeHelper.GetDataNames(type, prop.Name);

            foreach (var columnName in columnNames)
            {
                if (!String.IsNullOrWhiteSpace(columnName)
                    && row.Table.Columns.Contains(columnName))
                {
                    var propertyValue = row[columnName];
                    if (propertyValue != DBNull.Value)
                    {
                        ParsePrimitive(prop, entity, row[columnName]);
                        break;
                    }
                }
            }
        }

        private static void ParsePrimitive(PropertyInfo prop, object entity, object value)
        {
            Type type = prop.PropertyType; 
            // TODO: Test and include all primitive types
            switch (prop.PropertyType.Name.ToString().ToLower())
            {               
                case "boolean":
                    bool.TryParse(value.ToString(), out bool boolValue); 
                    prop.SetValue(entity, boolValue, null);
                    break;
                case "int32":
                case "int64":
                    prop.SetValue(entity, TryParseInt(value.ToString().Trim()), null);
                    break;
                case "string":
                    prop.SetValue(entity, value.ToString().Trim(), null);
                    break;
                case "calltypes":
                    prop.SetValue(entity, 
                        TryParseEnum<CallTypes>(value.ToString().Trim()), 
                        null);
                    break;
                case "dataformattypes":
                    prop.SetValue(entity,
                        TryParseEnum<DataFormatTypes>(value.ToString().Trim()),
                        null);
                    break;
                case "datadetailtypes":
                    prop.SetValue(entity,
                        TryParseEnum<DataDetailTypes>(value.ToString().Trim()),
                        null);
                    break;
                case "connectiontypes":
                    prop.SetValue(entity,
                        TryParseEnum<ConnectionTypes>(value.ToString().Trim()),
                        null);
                    break;
                default:
                    break;
            }
        }

        private static int? TryParseInt(string value)
        {
            var i = 0;
            if (!int.TryParse(value, out i))
            {
                return null;
            }
            return i;
        }

        private static object TryParseEnum<T>(string value) where T : struct, IConvertible
        {
            if (value.IsNullOrEmpty()) return null;

            var i = TryParseInt(value);
            if (!i.HasValue)
            {
                object setValue;
                Enum.TryParse(typeof(T), value, true, out setValue);
                return (T)(object)setValue;
            }

            return (T)(object)i.Value;
        }
    }
}
