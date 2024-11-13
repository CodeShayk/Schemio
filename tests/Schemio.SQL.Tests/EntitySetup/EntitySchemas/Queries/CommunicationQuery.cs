using System.Data;
using Dapper;
using Schemio.Core;

namespace Schemio.SQL.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CommunicationQuery : BaseSQLQuery<CommunicationResult>
    {
        protected override Func<IDbConnection, Task<CommunicationResult>> GetQuery(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;

            return connection => connection.QueryFirstOrDefaultAsync<CommunicationResult>(new CommandDefinition
            (
                "select c.CommunicationId as ContactId, " +
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
                $"where customerId={customer.Id}"
           ));
        }
    }
}