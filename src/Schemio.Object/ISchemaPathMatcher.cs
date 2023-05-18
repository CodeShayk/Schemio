namespace Schemio.Object.Core
{
    public interface ISchemaPathMatcher
    {
        bool IsMatch(string inputPath, ISchemaPaths configuredPaths);
    }
}