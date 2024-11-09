namespace Schemio.Core.Tests.EntitySetup.Queries
{
    internal class CustomerOrdersQuery : BaseQuery<CustomerParameter, CollectionResult<OrderValue>>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Does not execute as child to any query.
        }
    }
}