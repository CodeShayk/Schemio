using Schemio.Core.Helpers;

namespace Schemio.Core.PathMatchers
{
    public class JPathMatcher : ISchemaPathMatcher
    {
        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths) =>
              // Does the template path contain any of the mapping xpaths?
              inputXPath.IsNotNullOrEmpty()
              && configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower()));
    }
}