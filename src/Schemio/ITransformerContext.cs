namespace Schemio
{
    /// <summary>
    /// Implement to set transform with data context.
    /// </summary>
    public interface ITransformerContext
    {
        /// <summary>
        /// Implement to set context in transform.
        /// </summary>
        /// <param name="context"></param>
        void SetContext(IDataContext context);
    }
}