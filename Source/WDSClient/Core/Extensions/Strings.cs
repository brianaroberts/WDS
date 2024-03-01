using System.Data;
using System.Text.RegularExpressions;

namespace WDS.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsMatch(this string thisString, string matchExpression)
        {
            if (matchExpression.IsNullOrEmpty() || matchExpression == "*")
                return true;
            return Regex.Match(thisString, matchExpression, RegexOptions.IgnoreCase).Success; 
        }

        public static bool IsNullOrEmpty(this string thisString)
        {
            return string.IsNullOrWhiteSpace(thisString);
        }

        public static string IfEmptySet(this string thisString, string setString)
        {
            if (string.IsNullOrWhiteSpace(thisString))
                thisString = setString;

            return thisString;
        }

        public static string ReplaceWith(this string thisString, Dictionary<string, string> replacements)
        {
            thisString = replacements.Aggregate(thisString, (o, p) => o.Replace($"{{{p.Key}}}", p.Value));

            thisString.Replace("{TODAYS_DATE}", DateTime.Now.ToString("mm/dd/yyyy"));
            thisString.Replace("{BLANK}", "");

            //TODO: Add {ENV:Environment variable Name}
            //TODO: Add {TODAYS_DATE:-/+#}

            return thisString;
        }

        public static Dictionary<string, string> ToDictionary(this string thisString, char seperator = ';', char valueSeperator = '=')
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var items = thisString.Split(seperator);
            foreach (var item in items)
            {
                var value = item.Split(valueSeperator);
                result.Add(value[0], (value.Count() > 1) ? value[1] : "");
            }

            return result; 
        }

        public static DataTable FromXml(this string datatable)
        {
            var ds = new DataSet();
            ds.ReadXml(datatable);
            if (ds.Tables.Count == 0)
                return new DataTable();
            else
                return ds.Tables[0];
        }
    }
}
