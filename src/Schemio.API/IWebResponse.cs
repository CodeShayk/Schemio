using Schemio.Core;

namespace Schemio.API
{
    public interface IWebResponse : IQueryResult
    {
        IDictionary<string, string> Headers { get; internal set; }
    }
}