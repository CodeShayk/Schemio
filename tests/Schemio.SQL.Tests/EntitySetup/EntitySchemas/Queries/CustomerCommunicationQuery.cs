using System.Data;
using Dapper;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerCommunicationQuery : BaseSQLQuery<CustomerParameter, CommunicationResult>
    {
        protected override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }

        public override IEnumerable<CommunicationResult> Execute(IDbConnection conn)
        {
            return conn.Query<CommunicationResult>(new CommandDefinition
            (
                "select c.CommunicationId as Id, " +
                       "c.Phone as Telephone, " +
                       "c.Email, " +
                       "a.AddressId, " +
                       "a.HouseNo, " +
                       "a.City, " +
                       "a.Region, " +
                       "a.PostCode as PostalCode, " +
                       "a.Country " +
                "from TCommunication c " +
                "left join TAddress a on a.CommunicationId = c.CommunicationId " +
                $"where customerId={QueryParameter.CustomerId}"
           ));
        }
    }
}