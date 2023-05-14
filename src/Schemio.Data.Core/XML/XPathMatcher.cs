using Schemio.Data.Core.Helpers;
using System.Linq;
using System.Text.RegularExpressions;

namespace Schemio.Data.Core.XML
{
    public class XPathMatcher : ISchemaPathMatcher
    {
        private static readonly Regex ancestorRegex = new Regex(@"=ancestor::(?'path'.*?)(/@|\[.*\]/@)", RegexOptions.Compiled);

        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths) =>
             // Does the template xpath contain any of the mapping xpaths?
             inputXPath.IsNotNullOrEmpty()
             && configuredXPaths.XPath.Any(x => inputXPath.ToLower().Contains(x.ToLower())
                 || inputXPath.Contains("ancestor::")
                     && ancestorRegex.Matches(inputXPath).Select(match => match.Groups["path"].Value).Distinct().Any(match => x.EndsWith(match)));
    }
}