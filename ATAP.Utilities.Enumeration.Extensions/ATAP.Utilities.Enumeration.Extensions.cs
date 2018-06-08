using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ATAP.Utilities.Enumeration.Extensions {
 
    public static class Extensions {

		//	
        public interface IAttribute<out T> {
            T Value { get; }
        }

		//
        public static CustomAttributeType GetAttributeValue<CustomAttributeName, CustomAttributeType>(this Enum value) {
            // The enumeration value passed as the parameter to the GetSymbol method call
            var x = value
                // Get the the specific enumeration type
                .GetType()
                // Gets the FieldInfo object for this specific value of the enumeration
                .GetField(value.ToString())
                // If the field info object is not null, get a custom attribute of type T from this specific value of the enumeration
                ?.GetCustomAttributes(typeof(CustomAttributeName), false)
                .FirstOrDefault();
                // If the result is not null, return it as CustomAttributeType, else return the default value for that CustomAttributeType
                if(x == null) {
                    return default(CustomAttributeType);
                }
            IAttribute<CustomAttributeType> z = x as IAttribute<CustomAttributeType>;
            return z.Value;
        }

        // returns the string associated with the [description] attribute of an enumeration value.
        public static string GetDescription(Enum value) {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }

		// Get the enumeration value from the  case-insensitive string representation of the enumeration value
        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {

                return (T)Enum.Parse(typeof(T), value, ignoreCase);
           
        }
    }
}
