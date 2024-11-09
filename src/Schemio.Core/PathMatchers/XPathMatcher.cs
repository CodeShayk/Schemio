using System.Text.RegularExpressions;

namespace Schemio.Core.PathMatchers
{
    public class XPathMatcher : ISchemaPathMatcher
    {
        private static readonly Regex ancestorRegex = new Regex(@"=ancestor::(?'path'.*?)(/@|\[.*\]/@)", RegexOptions.Compiled);

        public bool IsMatch(string inputXPath, ISchemaPaths configuredXPaths)
        {
            if (inputXPath == null)
                return false;

            if (configuredXPaths.Paths.Any(x => inputXPath.ToLower().Contains(x.ToLower())))
                return true;

            if (configuredXPaths.Paths.Any(x => inputXPath.Contains("ancestor::")
                    && ancestorRegex.Matches(inputXPath).Select(match => match.Groups["path"].Value).Distinct().Any(match => x.EndsWith(match))))
                return true;

            return false;
        }
    }
}