using System.Text.Json;
using System.Xml;

namespace Schemio.SQL.Tests
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            return JsonSerializer.Serialize(obj);
        }
    }
}