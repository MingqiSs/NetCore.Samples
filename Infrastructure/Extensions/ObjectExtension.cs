using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Globalization;
namespace Infrastructure.Extensions
{
  public static  class ObjectExtension
    {
        public static T DicToEntity<T>(this Dictionary<string, object> dic)
        {
            return new List<Dictionary<string, object>>() { dic }.DicToList<T>().ToList()[0];
        }
        public static List<T> DicToList<T>(this List<Dictionary<string, object>> dicList)
        {
            return dicList.DicToIEnumerable<T>().ToList();
        }
        public static object DicToList(this List<Dictionary<string, object>> dicList, Type type)
        {
            return typeof(ConvertJsonExtension).GetMethod("DicToList")
               .MakeGenericMethod(new Type[] { type })
               .Invoke(typeof(ConvertJsonExtension), new object[] { dicList });
        }

        public static IEnumerable<T> DicToIEnumerable<T>(this List<Dictionary<string, object>> dicList)
        {
            foreach (Dictionary<string, object> dic in dicList)
            {
                T model = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in model.GetType()
                    .GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                {
                    //
                    var value = dic.FirstOrDefault(x => string.Equals(x.Key, property.Name, StringComparison.OrdinalIgnoreCase)).Value;
                    if (value != null)
                    {
                        property.SetValue(model, value.ToString().ChangeType(property.PropertyType), null);
                    }

                    //if (!dic.TryGetValue(property.Name, out object value)) continue;
                    //property.SetValue(model, value?.ToString().ChangeType(property.PropertyType), null);
                }
                yield return model;
            }
        }

        public static object ChangeType(this object convertibleValue, Type type)
        {
            if (null == convertibleValue) return null;

            try
            {
                if (type == typeof(Guid) || type == typeof(Guid?))
                {
                    string value = convertibleValue.ToString();
                    if (value == "") return null;
                    return Guid.Parse(value);
                }

                if (!type.IsGenericType) return Convert.ChangeType(convertibleValue, type);
                if (type.ToString() == "System.Nullable`1[System.Boolean]" || type.ToString() == "System.Boolean")
                {
                    if (convertibleValue.ToString() == "0")
                        return false;
                    return true;
                }
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(type));
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

       
    }
}
