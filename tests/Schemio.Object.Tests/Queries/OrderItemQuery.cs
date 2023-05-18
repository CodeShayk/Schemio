using Schemio.Object.Core;

namespace Schemio.Object.Tests.Queries
{
    internal class OrderItemQuery : BaseQuery<OrderItemParameter, OrderItemResult>
    {
        public override void ResolveRootQueryParameter(IDataContext context)
        {
            // Does not execute as root or level 1 queries.
        }

        public override void ResolveChildQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to order query.
            var order = (OrderResult)parentQueryResult;
            QueryParameter = new OrderItemParameter
            {
                OrderId = order.Id
            };
        }
    }

    internal class OrderItemResult : IQueryResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }

    internal class OrderItemParameter : IQueryParameter
    {
        public int OrderId { get; set; }
    }
}