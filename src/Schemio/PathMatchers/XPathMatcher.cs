using System.Text.RegularExpressions;
using Schemio.Helpers;

namespace Schemio.PathMatchers
{
    public class XPathMatcher : ISchemaPathMatcher
    {
        private static readonly Regex ancestorRegex = new Regex(@"=ancestor::(?'path'.*?)(/@|\[.*\]/@)", RegexOptions.Compiled);

        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths)
        {
            // Does the template xpath contain any of the mapping xpaths?

            if (inputXPath == null)
                return false;

            if (configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower())))
                return true;

            if (configuredXPaths.Paths.Any(x => inputXPath.Contains("ancestor::")
                    && ancestorRegex.Matches(inputXPath).Select(match => match.Groups["path"].Value).Distinct().Any(match => x.EndsWith(match))))
                return true;

            return false;

            //return !inputXPath.IsNotNullOrEmpty()
            // && configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower())
            //     || inputXPath.Contains("ancestor::")
            //         && ancestorRegex.Matches(inputXPath).Select(match => match.Groups["path"].Value).Distinct().Any(match => x.EndsWith(match)));
        }
    }
}