namespace Schemio.Core
{
    public interface IEntityRequest
    {
        /// <summary>
        /// Entity schema paths for data retrieval.
        /// </summary>
        public string[] SchemaPaths { get; set; }
    }
}