namespace Schemio.Tests.EntitySetup.Queries
{
    internal class CustomerOrdersQuery : BaseChildQuery<CustomerParameter, CollectionResult<OrderValue>>
    {
        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Does not execute as child to any query.
        }
    }
}