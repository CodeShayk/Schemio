using Schemio.API.Tests.EntitySetup.QueryResults;
using Schemio.Core;
using Schemio.Core.Helpers;

namespace Schemio.API.Tests.EntitySetup.WebApis
{
    internal class OrderItemsWebQuery : WebQuery<CollectionResult<OrderItemResult>>
    {
        public OrderItemsWebQuery() : base(Endpoints.BaseAddress)
        {
        }

        protected override Func<Uri> GetQuery(IDataContext context, IQueryResult parentApiResult)
        {
            // Execute as nested api to order parent api taking OrderResult to resolve api parameter.
            var orders = (CollectionResult<OrderResult>)parentApiResult;
            var customerContext = (CustomerRequest)context.Request;

            return () => new Uri(string.Format(Endpoints.BaseAddress + Endpoints.OrderItems, customerContext.CustomerId, orders.Select(o => o.OrderId).ToCSV()), UriKind.Absolute);
        }
    }
}