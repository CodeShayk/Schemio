namespace Schemio.Core
{
    public interface IEntityContext
    {
        /// <summary>
        /// Entity schema paths for data retrieval.
        /// </summary>
        public string[] SchemaPaths { get; set; }
    }
}