using Schemio.Object.Core;
using Schemio.Object.Core.Helpers;

namespace Schemio.Object.Pathmatchers
{
    public class JPathMatcher : ISchemaPathMatcher
    {
        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths) =>
          // Does the template jsonpath contain any of the mapping jsonpaths?
          inputXPath.IsNotNullOrEmpty()
          && configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower()));
    }
}