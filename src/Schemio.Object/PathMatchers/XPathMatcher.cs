using Schemio.Object.Helpers;
using System.Text.RegularExpressions;

namespace Schemio.Object.PathMatchers
{
    public class XPathMatcher : ISchemaPathMatcher
    {
        private static readonly Regex ancestorRegex = new Regex(@"=ancestor::(?'path'.*?)(/@|\[.*\]/@)", RegexOptions.Compiled);

        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths) =>
             // Does the template xpath contain any of the mapping xpaths?
             inputXPath.IsNotNullOrEmpty()
             && configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower())
                 || inputXPath.Contains("ancestor::")
                     && ancestorRegex.Matches(inputXPath).Select(match => match.Groups["path"].Value).Distinct().Any(match => x.EndsWith(match)));
    }
}