using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection;

namespace Kantin.Handler
{
    public class SwaggerFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context?.Type == null)
                return;

            var ignoredProperties = context.Type.GetProperties()
               .Where(t => t.GetCustomAttribute<JsonIgnoreAttribute>() != null);

            foreach (var ignoredProperty in ignoredProperties)
            {
                var propertiesToIgnore = schema.Properties.Where(p => p.Key.Equals(ignoredProperty.Name, StringComparison.OrdinalIgnoreCase));
                foreach (var propertyToIgnore in propertiesToIgnore)
                    schema.Properties.Remove(propertyToIgnore);
            }
        }
    }
}
