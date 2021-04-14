using System;

namespace AIS.Utils
{
    public static class Validator
    {
        public static void AssertValueNotNull(Object value, String errorMessage, Trace trace)
        {
            if (value == null)
            {
                if (trace == null)
                {
                    throw new ArgumentNullException(errorMessage);
                }

                throw new ArgumentNullException(errorMessage + " - " + trace.Id);

            }
        }

        public static void AssertIntValueBetween(int value, int min, int max, string errorMessage, Trace trace)
        {
            if (value < min || value > max)
            {
                if (trace == null)
                {
                    throw new ArgumentOutOfRangeException(errorMessage);
                }

                throw new ArgumentOutOfRangeException(errorMessage + " - " + trace.Id);

            }
        }

        public static void AssertValueNotEmpty(string value, string errorMessage, Trace trace)
        {
            if (value == null || value.Trim().Length == 0)
            {
                if (trace == null)
                {
                    throw new ArgumentException(errorMessage);
                }
                else
                {
                    throw new ArgumentException(errorMessage + " - " + trace.Id);
                }
            }
        }
    }
}
