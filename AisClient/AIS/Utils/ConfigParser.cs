using System;

namespace AIS.Utils
{
    public static class ConfigParser
    {
        public static string GetStringNotNull(string propertyName, string nonNullableString)
        {
            if (string.IsNullOrEmpty(nonNullableString))
            {
                throw new ArgumentNullException($"Invalid configuration. {propertyName} is missing or empty");
            }

            return nonNullableString;
        }

        public static int GetIntNotNull(string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException($"Invalid configuration. {propertyName} is missing or empty");
            }

            return int.Parse(value);
        }
    }
}
