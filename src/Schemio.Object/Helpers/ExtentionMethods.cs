namespace Schemio.Data.Core.Helpers
{
    public static class ExtentionMethods
    {
        public static bool IsNotNullOrEmpty(this string value) => !string.IsNullOrEmpty(value);
    }
}