using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerCommunicationQuery : BaseSingleResultQuery<CustomerParameter, CommunicationResult>
    {
        public override void ResolveParameterInChildMode(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }

        public override Func<IDbConnection, IQueryResult> GetQuery()
        {
            return (cnn) =>
                cnn.QuerySingleOrDefault<CommunicationResult>($"select * from TCommunication c " +
                "join TAddress a on a.CommunicationId = c.CommunicationId " +
                $"where customerId={QueryParameter.CustomerId}");
        }
    }
}