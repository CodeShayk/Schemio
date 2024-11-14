using System.Text.Json;

namespace Schemio.Core.Helpers
{
    public static class JsonExtensions
    {
        public static string? ToJson(this object value) => value != null ? JsonSerializer.Serialize(value) : null;

        public static object? ToObject(this string value, Type type) => !string.IsNullOrEmpty(value) ? JsonSerializer.Deserialize(value, type) : null;
    }
}