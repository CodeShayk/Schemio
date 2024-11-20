namespace Schemio.Core
{
    public interface ISchemaPathMatcher
    {
        /// <summary>
        /// Determines whether there is a match for given input path vs configured paths for entity's object graph.
        /// </summary>
        /// <param name="inputPath">Input path from IEntityContext.SchemaPaths</param>
        /// <param name="configuredPaths">Configured paths from EntityConfiguration<TEntity></param>
        /// <returns></returns>
        bool IsMatch(string inputPath, ISchemaPaths configuredPaths);
    }
}