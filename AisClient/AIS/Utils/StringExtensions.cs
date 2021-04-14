namespace AIS.Utils
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return value == null || value.Trim().Length == 0;
        }
    }
}
