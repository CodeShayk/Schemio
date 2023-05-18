using Schemio.Object.Core.Helpers;
using System.Linq;

namespace Schemio.Object.Core.XML
{
    public class XPathMatcher : ISchemaPathMatcher
    {
        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths) =>
             // Does the template xpath contain any of the mapping xpaths?
             inputXPath.IsNotNullOrEmpty()
             && configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower()));
    }
}