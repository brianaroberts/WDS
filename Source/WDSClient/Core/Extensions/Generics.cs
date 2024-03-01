using System.Collections.Generic;

namespace WDS.Core.Extensions
{
    public static class GenericExtensions
    {
        public static string ReplaceKeysWithValuesInString<TKey, TValue>(this Dictionary<TKey, TValue> dict, string needsUpdated)
        {
            if (dict != null && !needsUpdated.IsNullOrEmpty())
            {
                foreach (TKey key in dict.Keys)
                    needsUpdated = needsUpdated.Replace($"{{{key.ToString()}}}", dict[key]?.ToString());
            }

            return needsUpdated;
        }

        public static string ToString<TKey, TValue>(this Dictionary<TKey, TValue> dict, string seperator)
        {
            var returnValue = string.Empty;
            if (dict != null)
            {
                foreach (TKey key in dict.Keys)
                    returnValue = returnValue + $"{key}={dict[key]?.ToString()}{seperator}";
            }


            return returnValue;
        }

        public static string GetAValue<TKey, TValue>(this Dictionary<TKey, TValue> thisDict, TKey key, string valueIfNull)
        {
            if (thisDict.ContainsKey(key))
                return thisDict[key].ToString(); 
            else
                return valueIfNull;
        }

        public static string GetKeys<TKey, TValue>(this Dictionary<TKey, TValue> thisDict, string seperator)
        {
            var returnValue = "";

            if(thisDict != null)
            {
                foreach (TKey key in thisDict.Keys)
                {
                    if (returnValue.IsNullOrEmpty())
                        returnValue = $"{key.ToString()}";
                    else
                        returnValue = $"{returnValue},{key.ToString()}";
                }
            }

            return returnValue; 
        }

        public static string GetValues<TKey, TValue>(this Dictionary<TKey, TValue> thisDict, string seperator)
        {
            var returnValue = "";

            if (thisDict != null)
            {
                foreach (TValue value in thisDict.Values)
                {
                    var val = value == null ? "null" : value.ToString();

                    if (returnValue.IsNullOrEmpty())
                        returnValue = $"{val}";
                    else
                        returnValue = $"{returnValue}{seperator}{val.ToString()}";
                }
            }

            return returnValue;
        }
    }
}
