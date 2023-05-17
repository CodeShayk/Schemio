using Schemio.Data.Core.Helpers;
using System.Linq;

namespace Schemio.Data.Core.XML
{
    public class XPathMatcher : ISchemaPathMatcher
    {
        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths) =>
             // Does the template xpath contain any of the mapping xpaths?
             inputXPath.IsNotNullOrEmpty()
             && configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower()));
    }
}