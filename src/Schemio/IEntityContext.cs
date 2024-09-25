namespace Schemio
{
    public interface IEntityContext
    {
        /// <summary>
        /// Entity XPaths for data retrieval.
        /// </summary>
        public string[] Paths { get; set; }
    }
}