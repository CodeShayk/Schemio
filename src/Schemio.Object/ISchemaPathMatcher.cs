namespace Schemio.Object
{
    public interface ISchemaPathMatcher
    {
        bool IsMatch(string inputPath, ISchemaPaths configuredPaths);
    }
}