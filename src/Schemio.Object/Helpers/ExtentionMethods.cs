namespace Schemio.Object.Helpers
{
    public static class ExtentionMethods
    {
        public static bool IsNotNullOrEmpty(this string value) => !string.IsNullOrEmpty(value);
    }
}