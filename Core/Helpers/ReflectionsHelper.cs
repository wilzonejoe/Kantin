using System;
using System.Linq;
using System.Reflection;

namespace Core.Helpers
{
    public class ReflectionsHelper
    {
        public static T FindValueFromObject<T, R>(R objectToEvaluate, string nameOfVariable)
        {
            var typeInfo = typeof(R).GetTypeInfo();
            var declaredProperties = typeInfo.DeclaredProperties.ToList();
            var fieldInfo = declaredProperties.FirstOrDefault(d => d.Name == nameOfVariable);
            
            if(fieldInfo == null)
                return default;

            var value = fieldInfo.GetValue(objectToEvaluate);

            if (value == null)
                return default;

            return (T)value;
        }
    }
}
