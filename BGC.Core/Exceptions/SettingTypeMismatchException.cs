using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class SettingTypeMismatchException : SettingException
    {
        private static string GetMessage(string name, Type expectedType, Type actualType)
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(name))
            {
                result.Append($"Type mismatch in setting {name}.");
            }
            else
            {
                result.Append($"Type mismatch in the {nameof(Setting.ValueType)} of a setting object and the type of the provided value.");
            }

            if (expectedType != null)
            {
                result.Append($" Expected {expectedType.FullName}.");
            }

            if (actualType != null)
            {
                result.Append($" Actual type was {actualType.FullName}");
            }

            return result.ToString();
        }

        public SettingTypeMismatchException(string name, Type expectedType, Type actualType) :
            base(GetMessage(name, expectedType, actualType))
        {

        }
    }
}
