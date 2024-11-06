using Microsoft.EntityFrameworkCore;
using Schemio.EntityFramework.Tests.Domain;

namespace Schemio.EntityFramework.Tests.EntitySetup.EntitySchemas.Queries
{
    internal class CustomerCommunicationQuery : BaseSQLQuery<CustomerParameter, CommunicationResult>
    {
        public override void ResolveQueryParameter(IDataContext context, IQueryResult parentQueryResult)
        {
            // Execute as child to customer query.
            var customer = (CustomerResult)parentQueryResult;
            QueryParameter = new CustomerParameter
            {
                CustomerId = customer.Id
            };
        }

        public override IEnumerable<IQueryResult> Run(DbContext dbContext)
        {
            return dbContext.Set<Communication>()
                .Where(p => p.Customer.Id == QueryParameter.CustomerId)
                .Select(c => new CommunicationResult
                {
                    Id = c.CommunicationId,
                    AddressId = c.Address.AddressId,
                    Telephone = c.Phone,
                    Email = c.Email,
                    HouseNo = c.Address.HouseNo,
                    City = c.Address.City,
                    Region = c.Address.Region,
                    PostalCode = c.Address.PostalCode,
                    Country = c.Address.Country
                });
            ;
        }
    }
}