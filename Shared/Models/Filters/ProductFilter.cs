using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Shared.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Filters
{
    public class ProductFilter
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? OnSale { get; set; } = false;

        public override string ToString()
        {
            var queryStringBuilder = new StringBuilder();
            Type type = this.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(this);

                if (value == null)
                {
                    continue;
                }
                else if (property.PropertyType.IsEnum)
                {
                    value = value.ToString();
                }
                else if (property.PropertyType == typeof(bool?))
                {
                    if ((bool)value == false) continue;
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

        public void PopulateFromUri(string uri)
        {
            Dictionary<string, StringValues> paramDictionary = QueryHelpers.ParseQuery(uri);
            var filterProperties = typeof(LaptopFilter).GetProperties();

            foreach (var property in filterProperties)
            {
                if (paramDictionary.TryGetValue(property.Name, out var stringValue))
                {
                    SetPropertyValue(property, stringValue);
                }
            }
        }

        private void SetPropertyValue(PropertyInfo property, string value)
        {
            // Handle nullable types
            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            // Handle Lists
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var itemType = propertyType.GetGenericArguments()[0];
                // Create a list and populate it
                var list = (IList)Activator.CreateInstance(propertyType);
                foreach (var item in value.Split(',')) // Assumes comma-separated list in query
                {
                    list.Add(Convert.ChangeType(item, itemType));
                }
                property.SetValue(this, list);
            }
            // Handle Enums
            else if (propertyType.IsEnum)
            {
                var enumValue = Enum.Parse(propertyType, value, true);
                property.SetValue(this, enumValue);
            }
            // Handle primitive types
            else
            {
                var convertedValue = Convert.ChangeType(value, propertyType);
                property.SetValue(this, convertedValue);
            }
        }
    }
}
