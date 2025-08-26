using Shared.Models.Filters;
using System.Reflection;
using System.Text;

namespace Shared.Util
{
    public static class ToStringCustomConverter
    {
        public static string ConvertFilterToQuery(ProductFilter item)
        {
            var queryStringBuilder = new StringBuilder();
            Type type = item.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(item);

                if (value == null)
                {
                    continue;
                }
                else if (property.PropertyType.IsEnum)
                {
                    value = value.ToString();
                }

                string encodedName = property.Name;
                string encodedValue = value.ToString();

                if (queryStringBuilder.Length > 0)
                {
                    queryStringBuilder.Append("&");
                }

                queryStringBuilder.Append($"{encodedName}={encodedValue}");
            }
            return queryStringBuilder.ToString();
        }
    }
}
