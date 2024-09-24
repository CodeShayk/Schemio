namespace Schemio.Tests.EntitySetup.Queries
{
    internal class CustomerOrdersQuery : BaseQuery<CustomerParameter, CollectionResult<OrderValue>>, IChildQuery
    {
        public void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Does not execute as child to any query.
        }
    }
}