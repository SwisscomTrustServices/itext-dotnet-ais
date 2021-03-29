using System;

namespace AIS.Utils
{
    public static class Utils
    {
        public static string GetStringNotNull(string propertyName, string nonNullableString)
        {
            if (nonNullableString == null)
            {
                throw new Exception($"Invalid configuration. {propertyName} is missing or empty");
            }

            return nonNullableString;
        }

        public static void ValueNotNull(Object value, String errorMessage, Trace trace)
        {
            if (value == null)
            {
                if (trace == null)
                {
                    throw new Exception(errorMessage);
                }
                else
                {
                    throw new Exception(errorMessage + " - " + trace.Id);
                }

            }
        }
    }
}
